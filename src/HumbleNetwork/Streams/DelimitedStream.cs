namespace HumbleNetwork.Streams
{
    using System.IO;
    using System.Net.Sockets;

    public class DelimitedStream : HumbleStreamBase
    {
        private readonly string delimiter;
        private readonly byte[] delimiterBytes;

        public DelimitedStream(TcpClient client, string delimiter) : base(client)
        {
            this.delimiter = delimiter;
            this.delimiterBytes = StreamEncoding.GetBytes(this.delimiter);
        }

        public override void Send(string message)
        {
            this.SendMessage(message + this.delimiter);
        }

        public override string Receive()
        {
            if (this.ThereIsDataInBuffer)
            {
                return this.GetFromBuffer(this.BufferSize);
            }

            return this.ReceiveMessage();
        }

        protected string ReceiveMessage()
        {
            var buffer = new MemoryStream();
            int @byte;
            bool possibleDelimiterIsComing;
            var possibleDelimiterCount = 0;

            while ((@byte = stream.ReadByte()) != -1)
            {
                buffer.WriteByte((byte)@byte);

                if (@byte == this.delimiterBytes[possibleDelimiterCount])
                {
                    possibleDelimiterIsComing = true;
                    possibleDelimiterCount++;
                }
                else
                {
                    possibleDelimiterIsComing = false;
                    possibleDelimiterCount = 0;
                }

                if (possibleDelimiterIsComing && this.delimiterBytes.Length == possibleDelimiterCount)
                {
                    // complete message received, return removing the delimiters
                    return StreamEncoding.GetString(
                        buffer.GetBuffer(), 
                        0, 
                        (int)(buffer.Length - this.delimiterBytes.Length));
                }
            }

            // return incomplete message
            return StreamEncoding.GetString(buffer.GetBuffer());
        }
    }
}