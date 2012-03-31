namespace HumbleNetwork
{
    using System.Net.Sockets;
    using Streams;

    /// <summary>
    /// TODO: host search dns
    /// TODO: send command
    /// TODO: close or disconnect method
    /// </summary>
    public class HumbleClient : IHumbleClient
    {
        private readonly MessageFraming messageFraming;
        private readonly TcpClient tcpClient = new TcpClient();
        private IHumbleStream stream;

        public HumbleClient()
            : this(MessageFraming.LengthPrefixing)
        {
        }

        public HumbleClient(MessageFraming messageFraming)
        {
            this.messageFraming = messageFraming;
        }

        public int ReceiveTimeOut
        {
            get { return this.stream.NetworkStream.ReadTimeout; }
            set { this.stream.NetworkStream.ReadTimeout = value; }
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
            this.tcpClient.Connect(host, port);
            this.CreateStream();
            return this;
        }

        private void CreateStream()
        {
            //// TODO: refactor, not elegant
            this.stream = this.messageFraming == MessageFraming.LengthPrefixing ?
                (IHumbleStream)new FixedLengthStream(this.tcpClient) :
                new DelimitedStream(this.tcpClient);
        }

        /// <summary>
        /// There's no method to really know if the client is connected on the server.
        /// So I'm using this workaround that I found in some sites on internet
        /// </summary>
        /// <returns></returns>
        public bool IsItReallyConnected()
        {
            if (this.tcpClient.Connected == false)
            {
                return false;
            }

            if (this.tcpClient.Available > 0)
            {
                return false;
            }

            var blockingState = this.tcpClient.Client.Blocking;
            try
            {
                var tmp = new byte[1];

                this.tcpClient.Client.Blocking = false;
                var received = this.tcpClient.Client.Receive(tmp, 0, 0);
                return received > 0;
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                this.tcpClient.Client.Blocking = blockingState;
            }
        }

        /// <summary>
        /// TODO: good practices on disconnect
        /// </summary>
        public void Dispose()
        {
            this.tcpClient.Close();
        }
    }
}