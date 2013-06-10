namespace HumbleNetwork.Streams
{
    using System.Net.Sockets;

    public class FixedLengthStream : HumbleStreamBase
    {
        public FixedLengthStream(TcpClient client)
            : base(client)
        {
        }

        protected override void CustomSend(byte[] data)
        {
            this.SendLength(data);
            this.SendMessage(data);
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
            var length = StreamEncoding.GetInt32(lengthBytes);
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

            return StreamEncoding.GetString(messageBytes);
        }

        private void SendLength(byte[] message)
        {
            var lengthBytes = StreamEncoding.GetBytes(message.Length);
            this.stream.Write(lengthBytes, 0, lengthBytes.Length);
        }
    }
}