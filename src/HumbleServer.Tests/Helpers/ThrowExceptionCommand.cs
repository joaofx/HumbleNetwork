namespace HumbleServer.Tests.Helpers
{
    using System;
    using System.Net.Sockets;
    using HumbleServer.Streams;

    public class ThrowExceptionCommand : ICommand
    {
        public void Execute(TcpClient client, IHumbleStream stream)
        {
            throw new InvalidOperationException("An exception was thrown");
        }
    }
}