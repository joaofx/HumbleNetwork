namespace HumbleServer.Tests.Commands
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    public class WaitCommand : BaseCommand
    {
        public override void Execute()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            var data = Encoding.ASCII.GetBytes("1");
            this.socket.Send(data, 0, data.Length, SocketFlags.None);
        }
    }
}