namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork.Streams;

    public class CustomUnknowCommandHandler : IUnknowCommandHandler
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("CustomUnknowCommandHandler");
        }
    }
}