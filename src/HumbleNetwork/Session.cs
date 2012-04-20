namespace HumbleNetwork
{
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Session : IDisposable
    {
        private readonly HumbleServer server;
        private readonly TcpClient client;
        private readonly IHumbleStream stream;

        public Session(HumbleServer server, TcpClient client, Framing framing, string delimiter)
        {
            this.stream = MessageFraming.Create(framing, client, delimiter);
            this.server = server;
            this.client = client;
        }

        ~Session()
        {
            this.Dispose();
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

                    try
                    {
                        command.Execute(stream);
                    }
                    catch (Exception exception)
                    {
                        this.server.ExceptionHandler().Execute(stream, exception);
                    }

                    return true;
                });
            });

            commandProcessed.ContinueWith(antecedent =>
            {
                if (antecedent.Result)
                {
                    this.ProcessNextCommand();
                }
            });
        }

        public void Dispose()
        {
	        this.client.Close();
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