namespace HumbleServer.Tests.Helpers
{
    using System.Net.Sockets;
    using HumbleServer.Streams;

    public class CustomUnknowCommandHandler : IUnknowCommandHandler
    {
        public void Execute(TcpClient client, IHumbleStream stream)
        {
            stream.Send("CustomUnknowCommandHandler");
        }
    }
}