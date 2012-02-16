namespace HumbleServer.Tests.Commands
{
    using System.Net.Sockets;
    using System.Text;

    public class CustomUnknowCommand : BaseCommand
    {
        public override void Execute()
        {
            var sendBuffer = Encoding.ASCII.GetBytes("CustomUnknow");
            this.socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
        }
    }
}