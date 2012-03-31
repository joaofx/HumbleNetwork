namespace HumbleNetwork.Tests.Helpers
{
    using HumbleNetwork.Streams;

    internal class CustomUnknowCommandHandler : IUnknowCommandHandler
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("CustomUnknowCommandHandler");
        }
    }
}