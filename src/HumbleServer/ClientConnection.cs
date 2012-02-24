namespace HumbleServer
{
    using System;
    using System.Net.Sockets;
    using Streams;

    public class ClientConnection
    {
        private readonly NetworkServer server;
        private readonly TcpClient client;
        private readonly FixedLengthStream stream;

        public ClientConnection(NetworkServer server, TcpClient client, NetworkStream networkStream)
        {
            this.server = server;
            this.client = client;
            this.stream = new FixedLengthStream(networkStream);
        }

        public void ProcessNextCommand()
        {


            ////this.ProcessNextCommand();
        }
    }
}