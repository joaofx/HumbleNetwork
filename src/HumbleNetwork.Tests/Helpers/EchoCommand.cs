namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    internal class EchoCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            var message = stream.Receive();
            stream.Send(message);
        }
    }
}