namespace HumbleNetwork.Streams
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;

    public class DelimitedStream : HumbleStreamBase
    {
        private readonly string delimiter;
        private readonly byte[] delimiterBytes;

        public DelimitedStream(TcpClient client, string delimiter) : base(client)
        {
            this.delimiter = delimiter;
            this.delimiterBytes = Encoding.UTF8.GetBytes(this.delimiter);
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

        public override void ReceiveCommand(Action<string, IHumbleStream> processCommandAction)
        {
        }

        protected string ReceiveMessage()
        {
            var buffer = new MemoryStream();
            int @byte;
            bool possibleDelimiterIsComing;
            var possibleDelimiterCount = 0;

            ////var messageBytes = new byte[1024];
            ////var currentBufferIndex = 0;
            ////var bytesRead = -1;

            ////while (bytesRead != 0 && currentBufferIndex < messageBytes.Length)
            ////{
            ////    bytesRead = this.stream.Read(
            ////        messageBytes,
            ////        currentBufferIndex,
            ////        messageBytes.Length - currentBufferIndex);

            ////    currentBufferIndex += bytesRead;
            ////}
            
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