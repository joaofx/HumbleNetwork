namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    internal class PingCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("PONG");
        }
    }
}