namespace HumbleNetwork.Tests.Helpers
{
    using System;
    using HumbleNetwork;
    using HumbleNetwork.Streams;

    internal class CustomExceptionHandler : IExceptionHandler
    {
        public void Execute(IHumbleStream stream, Exception exception)
        {
            stream.Send("CustomExceptionHandler: " + exception.GetType().Name);
        }
    }
}