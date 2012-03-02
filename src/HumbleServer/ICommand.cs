namespace HumbleServer
{
    using System.Net.Sockets;
    using Streams;

    public interface ICommand
    {
        void Execute(TcpClient client, IHumbleStream stream);
    }
}