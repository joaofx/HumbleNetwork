namespace HumbleServer.Tests.Helpers
{
    using System.Net.Sockets;
    using HumbleServer.Streams;

    public class EchoCommand : ICommand
    {
        public void Execute(TcpClient client, IHumbleStream stream)
        {
            var message = stream.Receive();
            stream.Send(message);
        }
    }
}