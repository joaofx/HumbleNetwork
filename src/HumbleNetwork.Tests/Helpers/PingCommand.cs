namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork;

    internal class PingCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("PONG");
        }
    }
}