namespace HumbleExample
{
    using HumbleNetwork;

    public class EchoCommand : ICommand
    {
        public void Execute(IHumbleStream stream)
        {
            //// command echo send what was received
            stream.Send(stream.Receive());
        }
    }
}