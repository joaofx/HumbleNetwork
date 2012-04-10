namespace HumbleNetwork
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using HumbleNetwork.Streams;

    public class Session : IDisposable
    {
        private readonly HumbleServer server;
        private readonly TcpClient client;

        public Session(HumbleServer server, TcpClient client)
        {
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
                var stream = this.CreateStream();

                var commandName = stream.Receive(4).ToLower();
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
	        GC.SuppressFinalize(this);
        }

        private IHumbleStream CreateStream()
        {
            if (this.server.MessageFramingTypes == MessageFramingTypes.Delimiters)
            {
                return new DelimitedStream(this.client);
            }

            return new FixedLengthStream(this.client);
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