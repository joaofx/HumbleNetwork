namespace HumbleServer
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using Streams;

    /// <summary>
    /// TODO: host procurar dns
    /// </summary>
    public class NetworkClient
    {
        private readonly TcpClient tcpClient = new TcpClient();
        private FixedLengthStream stream;

        public NetworkClient Send(string data)
        {
            this.stream.Send(data);

            ////var buffer = Encoding.ASCII.GetBytes(data);
            ////this.stream.Write(buffer, 0, buffer.Length);

            return this;
        }

        public string Receive()
        {
            return this.stream.Receive();

            ////var buffer = new byte[2048];
            ////this.stream.Read(buffer, 0, buffer.Length);
            ////var data = Encoding.ASCII.GetString(buffer);
            ////return data.Replace("\0", String.Empty);
        }

        public NetworkClient Connect(string host, int port)
        {
            this.tcpClient.Connect(host, port);
            this.stream = new FixedLengthStream(this.tcpClient.GetStream());
            return this;
        }
    }
}