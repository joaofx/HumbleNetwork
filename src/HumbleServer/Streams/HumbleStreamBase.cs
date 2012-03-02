namespace HumbleServer.Streams
{
    using System.Net.Sockets;
    using System.Text;

    public abstract class HumbleStreamBase : IHumbleStream
    {
        protected readonly NetworkStream stream;

        public abstract void Send(string message);

        public abstract string Receive();

        public NetworkStream NetworkStream
        {
            get
            {
                return this.stream;
            }
        }

        protected HumbleStreamBase(NetworkStream stream)
        {
            this.stream = stream;
        }

        protected void SendMessage(string message)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);
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

            return Encoding.ASCII.GetString(messageBytes);
        }
    }
}