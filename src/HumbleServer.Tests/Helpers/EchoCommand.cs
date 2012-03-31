namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    public class EchoCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            var message = stream.Receive();
            stream.Send(message);
        }
    }
}