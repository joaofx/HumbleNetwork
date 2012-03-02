namespace HumbleServer
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using Streams;

    public class Session : IDisposable
    {
        private readonly NetworkServer server;
        private readonly TcpClient client;

        public Session(NetworkServer server, TcpClient client)
        {
            this.server = server;
            this.client = client;
        }

        public void ProcessCommand()
        {
            ExecuteHandlingExceptions(() =>
            {
                var stream = this.CreateStream();

                try
                {
                    var commandName = stream.Receive().ToLower();
                    var command = this.server.GetCommand(commandName);
                    command.Execute(client, stream);
                }
                catch (Exception exception)
                {
                    this.server.ExceptionHandler().Execute(client, stream, exception);
                }

                this.ProcessCommand();
            });
        }

        private IHumbleStream CreateStream()
        {
            if (this.server.MessageFraming == MessageFraming.Delimiters)
            {
                return new DelimitedStream(this.client.GetStream());
            }

            return new FixedLengthStream(this.client.GetStream());
        }

        private void ExecuteHandlingExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (IOException)
            {
                Dispose();
            }
            catch (SocketException)
            {
                Dispose();
            }
            catch (ObjectDisposedException)
            {
                Dispose();
            }
        }

		public void Dispose()
		{
			this.client.Close();
			GC.SuppressFinalize(this);
		}

		~Session()
		{
			Dispose();
		}
    }
}