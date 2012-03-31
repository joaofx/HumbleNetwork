namespace HumbleNetwork.Handlers
{
    using System;
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void Execute(IHumbleStream stream, Exception exception)
        {
            var message = exception.GetType().Name + ": " + exception.Message;
            stream.Send(message);
        }
    }
}