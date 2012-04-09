namespace HumbleNetwork
{
    using System.Net.Sockets;

    public interface IHumbleStream
    {
        void Send(string message);

        string Receive();

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
    }
}