namespace HumbleServer
{
    using System.Net.Sockets;
    using Streams;

    /// <summary>
    /// TODO: host procurar dns
    /// </summary>
    public class NetworkClient
    {
        private readonly TcpClient tcpClient = new TcpClient();
        private FixedLengthStream stream;

        public int ReceiveTimeOut
        {
            get { return this.stream.NetworkStream.ReadTimeout; }
            set { this.stream.NetworkStream.ReadTimeout = value; }
        }

        public NetworkClient Send(string data)
        {
            this.stream.Send(data);
            return this;
        }

        public string Receive()
        {
            return this.stream.Receive();
        }

        public NetworkClient Connect(string host, int port)
        {
            this.tcpClient.Connect(host, port);
            this.stream = new FixedLengthStream(this.tcpClient.GetStream());
            return this;
        }
    }
}