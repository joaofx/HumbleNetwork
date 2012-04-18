namespace HumbleNetwork
{
    using System.Net.Sockets;
    using Streams;

    /// <summary>
    /// TODO: message framing has resposability to create stream?
    /// </summary>
    public class MessageFraming
    {
        public const string DefaultDelimiter = "\n\r";

        public static IHumbleStream Create(
            Framing framing, 
            TcpClient tcpClient, 
            string delimiter)
        {
            return framing == Framing.LengthPrefixed ?
                (IHumbleStream)new FixedLengthStream(tcpClient) :
                new DelimitedStream(tcpClient, delimiter);
        }
    }
}