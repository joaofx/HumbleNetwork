namespace HumbleServer.Handlers
{
    using System;
    using System.Net.Sockets;
    using HumbleServer.Streams;

    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void Execute(TcpClient client, IHumbleStream stream, Exception exception)
        {
            var message = exception.GetType().Name + ": " + exception.Message;
            stream.Send(message);
        }
    }
}