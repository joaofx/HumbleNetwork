namespace HumbleNetwork.Streams
{
    using System.Net.Sockets;
    using System.Text;

    public abstract class HumbleStreamBase : IHumbleStream
    {
        protected readonly NetworkStream stream;
        private readonly TcpClient client;
        private readonly StringBuilder buffer = new StringBuilder();

        protected HumbleStreamBase(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
        }

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

        public abstract string Receive();

        protected abstract void CustomSend(byte[] data);

        public void Send(string message)
        {
            var data = StreamEncoding.GetBytes(message);
            this.CustomSend(data);
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

        public void Close()
        {
            this.stream.Close();
            this.TcpClient.Close();
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

        protected void SendMessage(byte[] message)
        {
            this.stream.Write(message, 0, message.Length);
        }

        protected void AppendBuffer(string data)
        {
            this.buffer.Append(data);
        }
    }
}