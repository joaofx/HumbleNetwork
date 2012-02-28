namespace HumbleServer.Streams
{
    public interface IHumbleStream
    {
        void Send(string message);

        string Receive();
    }
}