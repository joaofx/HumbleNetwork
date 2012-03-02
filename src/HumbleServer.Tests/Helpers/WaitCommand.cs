namespace HumbleServer.Tests.Helpers
{
    using System;
    using System.Net.Sockets;
    using System.Threading;
    using HumbleServer.Streams;

    public class WaitCommand : ICommand
    {
        public void Execute(TcpClient client, IHumbleStream stream)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            stream.Send("1");
        }
    }
}