namespace HumbleServer
{
    using System.Net.Sockets;

    public interface ICommand
    {
        void SetContext(Socket socket);
        void Execute();
    }
}