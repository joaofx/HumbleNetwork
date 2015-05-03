namespace HumbleNetwork.Tests.Helpers
{
    internal class CustomUnknowCommandHandler : IUnknowCommandHandler
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("CustomUnknowCommandHandler");
        }
    }
}