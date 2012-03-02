namespace HumbleServer.Streams
{
    using System;
    using System.Net.Sockets;

    public class FixedLengthStream : HumbleStreamBase
    {
        public FixedLengthStream(NetworkStream stream) : base(stream)
        {
        }

        public override void Send(string message)
        {
            this.SendLength(message);
            this.SendMessage(message);
        }

        private void SendLength(string message)
        {
            var lengthBytes = BitConverter.GetBytes(message.Length);
            this.stream.Write(lengthBytes, 0, lengthBytes.Length);
        }

        public override string Receive()
        {
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
    }
}