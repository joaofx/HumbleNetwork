namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork;

    internal class EchoCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            var message = stream.Receive();
            stream.Send(message);
        }
    }
}