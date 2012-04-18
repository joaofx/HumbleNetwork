namespace HumbleNetwork
{
    using System;
    using System.Net.Sockets;

    public interface IHumbleStream
    {
        void Send(string message);

        string Receive();

        string Receive(int length);

        /// <summary>
        /// TODO: encapsulate
        /// </summary>
        NetworkStream NetworkStream
        {
            get;
        }

        TcpClient TcpClient
        {
            get;
        }

        void ReceiveCommand(Action<string, IHumbleStream> processCommandAction);
    }
}