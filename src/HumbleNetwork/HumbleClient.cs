namespace HumbleNetwork
{
    using System.Net.Sockets;

    public class HumbleClient : IHumbleClient
    {
        private readonly Framing framing;
        private readonly string delimiter;
        private readonly TcpClient tcpClient = new TcpClient();
        private IHumbleStream stream;

        /// <summary>
        /// Create a instance of HumbleClient
        /// </summary>
        /// <param name="framing">Type of framing</param>
        /// <param name="delimiter">Demiliter character if framing is Framing.LengthPrefixed</param>
        /// <param name="receiveTimeOut">Receive timeout in miliseconds</param>
        /// <param name="sendTimeOut">Send timeout in miliseconds</param>
        public HumbleClient(
            Framing framing = Framing.LengthPrefixed, 
            string delimiter = MessageFraming.DefaultDelimiter,
            int receiveTimeOut = -1,
            int sendTimeOut = -1)
        {
            this.framing = framing;
            this.delimiter = delimiter;
            this.ReceiveTimeOut = receiveTimeOut;
            this.SendTimeOut = sendTimeOut;
        }

        public int ReceiveTimeOut
        {
            get;
            protected set;
        }

        public int SendTimeOut
        {
            get;
            protected set;
        }

        public HumbleClient Send(string data)
        {
            this.stream.Send(data);
            return this;
        }

        public string Receive()
        {
            return this.stream.Receive();
        }

        public HumbleClient Connect(string host, int port)
        {
            if (this.tcpClient.Connected == false)
            {
                this.tcpClient.Connect(host, port);
            }

            this.CreateStream();
            return this;
        }

        /// <summary>
        /// TODO: good practices on disconnect
        /// </summary>
        public void Dispose()
        {
            this.tcpClient.Close();
        }

        private void CreateStream()
        {
            this.stream = MessageFraming.Create(this.framing, this.tcpClient, this.delimiter);
            this.stream.NetworkStream.WriteTimeout = this.SendTimeOut;
            this.stream.NetworkStream.ReadTimeout = this.ReceiveTimeOut;
        }
    }
}