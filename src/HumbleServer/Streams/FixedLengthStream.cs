namespace HumbleServer.Streams
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    public class FixedLengthStream : IHumbleStream
    {
        private readonly NetworkStream stream;

        public FixedLengthStream(NetworkStream stream)
        {
            this.stream = stream;
        }

        public void Send(string message)
        {
            // send length
            var lengthBytes = BitConverter.GetBytes(message.Length);
            this.stream.Write(lengthBytes, 0, lengthBytes.Length);

            // send message
            var messageBytes = Encoding.ASCII.GetBytes(message);
            this.stream.Write(messageBytes, 0, messageBytes.Length);
        }

        public string Receive()
        {
            // receive length
            var lengthBytes = new byte[4];
            this.stream.Read(lengthBytes, 0, lengthBytes.Length);
            var length = BitConverter.ToInt32(lengthBytes, 0);

            // receive message
            var messageBytes = new byte[length];
            var currentBufferIndex = 0;
            var bytesRead = -1;

            while (bytesRead != 0 && currentBufferIndex < messageBytes.Length)
            {
                bytesRead = this.stream.Read(messageBytes, currentBufferIndex, messageBytes.Length - currentBufferIndex);
                currentBufferIndex += bytesRead;
            }
            
            return Encoding.ASCII.GetString(messageBytes);
        }
    }
}