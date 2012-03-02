namespace HumbleServer.Commands
{
    using System.Net.Sockets;
    using Streams;

    internal class DefaultUnknowCommandHandler : IUnknowCommandHandler
    {
        public void Execute(TcpClient client, IHumbleStream stream)
        {
            stream.Send("UNKN");
        }
    }
}