namespace HumbleNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Session : IDisposable
    {
        private readonly HumbleServer server;
        private readonly TcpClient client;
        ////private readonly IList<Session> sessions;
        private readonly IHumbleStream stream;

        public Session(HumbleServer server, TcpClient client, Framing framing, string delimiter)
        {
            this.stream = MessageFraming.Create(framing, client, delimiter);
            this.server = server;
            this.client = client;
            ////this.sessions = sessions;
            ////this.sessions.Add(this);
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
            ////this.sessions.Remove(this);
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