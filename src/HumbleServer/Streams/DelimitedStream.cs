namespace HumbleServer.Streams
{
    using System.Net.Sockets;

    public class DelimitedStream : HumbleStreamBase
    {
        private readonly ChunkMessageBuffer buffer;
        private const int BufferSize = 2048;
        private const string Delimiter = "\n\r";

        public DelimitedStream(NetworkStream stream) : base(stream)
        {
            this.buffer = new ChunkMessageBuffer(Delimiter);
        }

        public override void Send(string message)
        {
            this.SendMessage(message + Delimiter);
        }

        public override string Receive()
        {
            while (this.buffer.HasCompleteMessage == false)
            {
                this.buffer.Append(this.ReceiveMessage(BufferSize, true));
            }

            return this.buffer.GetMessage();
        }
    }
}