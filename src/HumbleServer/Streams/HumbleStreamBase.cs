namespace HumbleNetwork.Streams
{
    using System.Net.Sockets;
    using System.Text;

    public abstract class HumbleStreamBase : IHumbleStream
    {
        protected readonly NetworkStream stream;
        private readonly TcpClient client;

        public abstract void Send(string message);

        public abstract string Receive();

        public NetworkStream NetworkStream
        {
            get
            {
                return this.stream;
            }
        }

        public TcpClient TcpClient
        {
            get
            {
                return this.client;
            }
        }

        protected HumbleStreamBase(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
        }

        protected void SendMessage(string message)
        {
            var messageBytes = Encoding.Default.GetBytes(message);
            this.stream.Write(messageBytes, 0, messageBytes.Length);
        }

        protected string ReceiveMessage(int length, bool checkDataAvailable = false)
        {
            var messageBytes = new byte[length];
            var currentBufferIndex = 0;
            var bytesRead = -1;

            while (bytesRead != 0 && currentBufferIndex < messageBytes.Length)
            {
                bytesRead = this.stream.Read(
                    messageBytes, 
                    currentBufferIndex, 
                    messageBytes.Length - currentBufferIndex);

                currentBufferIndex += bytesRead;

                if (checkDataAvailable && this.stream.DataAvailable == false)
                {
                    break;
                }
            }

            return Encoding.Default.GetString(messageBytes);
        }
    }
}