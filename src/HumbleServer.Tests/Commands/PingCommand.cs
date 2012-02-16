namespace HumbleServer.Tests.Commands
{
    using System.Net.Sockets;
    using System.Text;

    public class PingCommand : BaseCommand
    {
        public override void Execute()
        {
            var sendBuffer = Encoding.ASCII.GetBytes("PONG");
            this.socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
        }
    }
}