namespace HumbleNetwork.Streams
{
    using System;
    using System.Collections.Generic;

    public class ChunkMessageBuffer
    {
        private readonly string demiliter;
        private readonly Queue<string> messages = new Queue<string>();
        private string buffer = string.Empty;

        public ChunkMessageBuffer(string demiliter)
        {
            this.demiliter = demiliter;
            this.SplitMessages();
        }

        public bool HasCompleteMessage
        {
            get
            {
                return this.messages.Count > 0;
            }
        }

        public void Append(string data)
        {
            this.buffer += data;
            this.SplitMessages();
        }

        public string GetMessage()
        {
            try
            {
                return this.messages.Dequeue();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void SplitMessages()
        {
            while (true)
            {
                var delimiterIndex = this.buffer.IndexOf(this.demiliter, System.StringComparison.Ordinal);

                if (delimiterIndex == -1)
                {
                    return;
                }

                var message = this.buffer.Substring(0, delimiterIndex);

                this.buffer = this.buffer.Remove(0, delimiterIndex + this.demiliter.Length);
                this.messages.Enqueue(message);
            }
        }
    }
}