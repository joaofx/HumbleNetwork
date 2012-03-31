namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    public class PingCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("PONG");
        }
    }
}