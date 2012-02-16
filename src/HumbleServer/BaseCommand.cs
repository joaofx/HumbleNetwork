namespace HumbleServer
{
    using System.Net.Sockets;

    public abstract class BaseCommand : ICommand
    {
        protected Socket socket;

        public abstract void Execute();

        public void SetContext(Socket socket)
        {
            this.socket = socket;
        }
    }
}