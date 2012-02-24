namespace HumbleServer
{
    using System.Net.Sockets;
    using Streams;

    public interface ICommand
    {
        void Execute();
        void SetContext(TcpClient client, IHumbleStream stream);
    }
}