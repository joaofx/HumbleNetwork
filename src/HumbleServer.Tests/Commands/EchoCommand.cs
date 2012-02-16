namespace HumbleServer.Tests.Commands
{
    using System.Net.Sockets;
    using System.Text;

    public class EchoCommand : BaseCommand
    {
        public override void Execute()
        {
            var buffer = new byte[2048];
            this.socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            var data = Encoding.ASCII.GetString(buffer);
            var sendBuffer = Encoding.ASCII.GetBytes(data);
            this.socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
        }
    }
}