namespace HumbleNetwork
{
    using System.Net.Sockets;

    public class HumbleClient : IHumbleClient
    {
        private readonly Framing _framing;
        private readonly string _delimiter;
        private TcpClient _tcpClient = new TcpClient();
        private IHumbleStream _stream;

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
            _framing = framing;
            _delimiter = delimiter;
            ReceiveTimeOut = receiveTimeOut;
            SendTimeOut = sendTimeOut;
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
            _stream.Send(data);
            return this;
        }

        public string Receive()
        {
            return _stream.Receive();
        }

        public HumbleClient Connect(string host, int port)
        {
            if (_tcpClient.Connected == false)
            {
                try
                {
                    _tcpClient.Connect(host, port);
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode == 10056)
                    {
                        _tcpClient.Close();
                        _tcpClient = new TcpClient();
                        _tcpClient.Connect(host, port);
                    }
                }
            }

            CreateStream();
            return this;
        }

        /// <summary>
        /// TODO: good practices on disconnect
        /// </summary>
        public void Dispose()
        {
            _tcpClient.Close();
        }

        private void CreateStream()
        {
            _stream = MessageFraming.Create(_framing, _tcpClient, _delimiter);
            _stream.NetworkStream.WriteTimeout = SendTimeOut;
            _stream.NetworkStream.ReadTimeout = ReceiveTimeOut;
        }
    }
}