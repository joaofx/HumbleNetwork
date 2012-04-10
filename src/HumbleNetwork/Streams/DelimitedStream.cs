namespace HumbleNetwork.Streams
{
    using System.Net.Sockets;

    public class DelimitedStream : HumbleStreamBase
    {
        private readonly ChunkMessageBuffer chunkedMessage;
        private const int ChunkedBufferSize = 2048;

        public DelimitedStream(TcpClient client) : base(client)
        {
            this.chunkedMessage = new ChunkMessageBuffer(MessageFraming.Delimiter);
        }

        public override void Send(string message)
        {
            this.SendMessage(message + MessageFraming.Delimiter);
        }

        public override string Receive()
        {
            if (this.ThereIsDataInBuffer)
            {
                return this.GetFromBuffer(this.BufferSize);
            }

            while (this.chunkedMessage.HasCompleteMessage == false)
            {
                this.chunkedMessage.Append(this.ReceiveMessage(ChunkedBufferSize, true));
            }

            return this.chunkedMessage.GetMessage();
        }
    }
}