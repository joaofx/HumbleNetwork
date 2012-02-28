namespace HumbleServer
{
    using System.Net.Sockets;
    using Streams;

    public abstract class CommandBase : ICommand
    {
        protected TcpClient client;
        protected IHumbleStream stream;

        public abstract void Execute();

        public void SetContext(TcpClient client, IHumbleStream stream)
        {
            this.client = client;
            this.stream = stream;
        }
    }
}