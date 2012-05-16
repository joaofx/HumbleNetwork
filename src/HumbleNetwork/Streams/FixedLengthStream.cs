namespace HumbleNetwork.Streams
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    public class FixedLengthStream : HumbleStreamBase
    {
        public FixedLengthStream(TcpClient client)
            : base(client)
        {
        }

        public override void Send(string message)
        {
            this.SendLength(message);
            this.SendMessage(message);
        }

        public override string Receive()
        {
            if (this.ThereIsDataInBuffer)
            {
                return this.GetFromBuffer(this.BufferSize);
            }

            var length = this.ReceiveLength();
            return this.ReceiveMessage(length);
        }

        private int ReceiveLength()
        {
            var lengthBytes = new byte[4];
            this.stream.Read(lengthBytes, 0, lengthBytes.Length);
            var length = BitConverter.ToInt32(lengthBytes, 0);
            return length;
        }

        private string ReceiveMessage(int length)
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
            }

            return Encoding.Default.GetString(messageBytes);
        }

        private void SendLength(string message)
        {
            var lengthBytes = BitConverter.GetBytes(message.Length);
            this.stream.Write(lengthBytes, 0, lengthBytes.Length);
        }
    }
}