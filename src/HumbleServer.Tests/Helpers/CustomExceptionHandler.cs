namespace HumbleServer.Tests.Helpers
{
    using System;
    using System.Net.Sockets;
    using HumbleServer.Streams;

    public class CustomExceptionHandler : IExceptionHandler
    {
        public void Execute(TcpClient client, IHumbleStream stream, Exception exception)
        {
            stream.Send("CustomExceptionHandler: " + exception.GetType().Name);
        }
    }
}