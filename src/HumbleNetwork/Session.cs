namespace HumbleNetwork
{
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Session : IDisposable
    {
        private readonly HumbleServer server;
        private readonly TcpClient client;
        private readonly Sessions sessions;
        private readonly IHumbleStream stream;

        public Session(Sessions sessions, HumbleServer server, TcpClient client, Framing framing, string delimiter)
        {
            this.stream = MessageFraming.Create(framing, client, delimiter);
            this.server = server;
            this.client = client;
            this.sessions = sessions;
        }

        public void ProcessNextCommand()
        {
            var taskExecute = Task<string>.Factory.StartNew(
                () => this.ExecuteHandlingExceptions(() => this.stream.Receive(4)));

            var commandProcessed = taskExecute.ContinueWith(antecedent =>
            {
                if (antecedent.Status == TaskStatus.Faulted)
                {
                    return false;
                }

                return this.ExecuteHandlingExceptions(() =>
                {
                    var commandName = antecedent.Result.ToLower();

                    if (string.IsNullOrEmpty(commandName))
                    {
                        return false;
                    }

                    var command = this.server.GetCommand(commandName);
                    this.ExecuteCommand(command);

                    return true;
                });
            });

            this.ContinueProcessingNextCommand(commandProcessed);
        }

        public void Dispose()
        {
            this.sessions.Disposed(this);
            this.stream.Close();
            this.client.Close();
        }

        private void ContinueProcessingNextCommand(Task<bool> commandProcessed)
        {
            commandProcessed.ContinueWith(antecedent =>
            {
                if (antecedent.Result)
                {
                    this.ProcessNextCommand();
                }
            });
        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute(this.stream);
            }
            catch (Exception exception)
            {
                this.server.ExceptionHandler().Execute(this.stream, exception);
            }
        }

        private T ExecuteHandlingExceptions<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                this.Dispose();
                return default(T);
            }
        }
    }
}