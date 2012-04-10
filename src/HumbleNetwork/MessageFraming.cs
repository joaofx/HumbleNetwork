namespace HumbleNetwork
{
    using System.Net.Sockets;
    using Streams;

    public class MessageFraming
    {
        public static string Delimiter = "\n\r";

        public static IHumbleStream Create(MessageFramingTypes messageFramingType, TcpClient tcpClient)
        {
            return messageFramingType == MessageFramingTypes.LengthPrefixing ?
                (IHumbleStream)new FixedLengthStream(tcpClient) :
                new DelimitedStream(tcpClient);
        }
    }
}