namespace HumbleNetwork.Streams
{
    using System.Net.Sockets;

    public class DelimitedStream : HumbleStreamBase
    {
        public static string Delimiter = "\n\r";
        private readonly ChunkMessageBuffer buffer;
        private const int BufferSize = 2048;

        public DelimitedStream(TcpClient client)
            : base(client)
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