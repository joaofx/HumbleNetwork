namespace HumbleNetwork
{
    using System;
    using Streams;

    public interface IExceptionHandler
    {
        void Execute(IHumbleStream stream, Exception exception);
    }
}