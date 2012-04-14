namespace HumbleNetwork.Streams
{
    using System.IO;
    using System.Net.Sockets;
    using System.Text;

    public class DelimitedStream : HumbleStreamBase
    {
        private readonly byte[] delimiterBytes;

        public DelimitedStream(TcpClient client) : base(client)
        {
            this.delimiterBytes = Encoding.UTF8.GetBytes(MessageFraming.Delimiter);
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
                    return Encoding.Default.GetString(
                        buffer.GetBuffer(), 
                        0, 
                        (int) (buffer.Length - this.delimiterBytes.Length));
                }
            }

            // return incomplete message
            return Encoding.Default.GetString(buffer.GetBuffer());
        }
    }
}