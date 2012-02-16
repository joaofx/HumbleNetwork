namespace HumbleServer
{
    using System.Net.Sockets;
    using System.Text;

    internal class UnknowCommand : BaseCommand
    {
        public override void Execute()
        {
            var sendBuffer = Encoding.ASCII.GetBytes("UNKN");
            this.socket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
        }
    }
}