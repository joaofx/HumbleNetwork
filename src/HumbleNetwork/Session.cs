namespace HumbleNetwork
{
    using System;
    using System.IO;
    using System.Net.Sockets;

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

        public void ProcessCommand()
        {
            this.ExecuteHandlingExceptions(() =>
            {
                var commandName = this.stream.Receive(4).ToLower();

                if (string.IsNullOrEmpty(commandName))
                {
                    if (this.client.IsItReallyConnected() == false)
                    {
                        return;
                    }
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

                this.ProcessCommand();
            });
        }

        public void Dispose()
        {
	        this.client.Close();
        }

        private void ExecuteHandlingExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (IOException)
            {
                this.Dispose();
            }
            catch (SocketException)
            {
                this.Dispose();
            }
            catch (ObjectDisposedException)
            {
                this.Dispose();
            }
        }
    }
}