namespace HumbleServer
{
    using System.Net.Sockets;
    using Streams;

    /// <summary>
    /// TODO: inverter client, stream
    /// </summary>
    public interface ICommand
    {
        void Execute(TcpClient client, IHumbleStream stream);
    }
}