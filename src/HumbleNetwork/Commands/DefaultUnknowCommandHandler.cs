namespace HumbleNetwork.Commands
{
    using HumbleNetwork.Streams;

    internal class DefaultUnknowCommandHandler : IUnknowCommandHandler
    {
        public void Execute(IHumbleStream stream)
        {
            stream.Send("UNKN");
        }
    }
}