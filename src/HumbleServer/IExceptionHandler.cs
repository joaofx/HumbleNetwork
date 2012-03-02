namespace HumbleServer
{
    using System;
    using System.Net.Sockets;
    using Streams;

    public interface IExceptionHandler
    {
        void Execute(TcpClient client, IHumbleStream stream, Exception exception);
    }
}