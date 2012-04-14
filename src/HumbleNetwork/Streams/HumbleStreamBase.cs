namespace HumbleNetwork.Streams
{
    using System.Net.Sockets;
    using System.Text;

    public abstract class HumbleStreamBase : IHumbleStream
    {
        protected readonly NetworkStream stream;
        private readonly TcpClient client;
        private readonly StringBuilder buffer = new StringBuilder();

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

        protected int BufferSize
        {
            get
            {
                return this.buffer.Length;
            }
        }

        protected bool ThereIsDataInBuffer
        {
            get
            {
                return this.buffer.Length > 0;
            }
        }

        public string Receive(int length)
        {
            if (this.ThereIsDataInBuffer == false)
            {
                this.AppendBuffer(this.Receive());
            }

            if (this.ThereIsDataInBuffer)
            {
                return this.GetFromBuffer(length);
            }

            return string.Empty;
        }

        protected string GetFromBuffer(int length = 0)
        {
            if (length == 0)
            {
                length = this.buffer.Length;
            }

            var tosend = this.buffer.ToString(0, length);
            this.buffer.Remove(0, length);
            return tosend;
        }

        protected void SendMessage(string message)
        {
            var messageBytes = Encoding.Default.GetBytes(message);
            this.stream.Write(messageBytes, 0, messageBytes.Length);
        }

        protected void AppendBuffer(string data)
        {
            this.buffer.Append(data);
        }
    }
}