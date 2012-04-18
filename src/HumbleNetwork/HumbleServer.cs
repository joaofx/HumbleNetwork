namespace HumbleNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using Commands;
    using Handlers;

    public class HumbleServer
    {
        private readonly string delimiter;
        private TcpListener listener;
        private readonly IDictionary<string, Func<ICommand>> commands = new Dictionary<String, Func<ICommand>>();
        private readonly Framing framing;

        public HumbleServer(Framing framing = Framing.LengthPrefixed, string delimiter = MessageFraming.DefaultDelimiter)
        {
            this.delimiter = delimiter;
            this.UnknowCommandHandler = () => new DefaultUnknowCommandHandler();
            this.ExceptionHandler = () => new DefaultExceptionHandler();
            this.framing = framing;
        }

        public Func<IUnknowCommandHandler> UnknowCommandHandler
        {
            get;
            set;
        }

        public int Port
        {
            get { return ((IPEndPoint)this.listener.LocalEndpoint).Port; }
        }

        public Func<IExceptionHandler> ExceptionHandler
        {
            get;
            set;
        }

        public HumbleServer Start(int port)
        {
            this.listener = new TcpListener(IPAddress.Any, port);
            this.listener.Start();
            this.AcceptClients();
            return this;
        }

        public ICommand GetCommand(string commandName)
        {
            if (this.commands.ContainsKey(commandName.ToLower()) == false)
            {
                return this.UnknowCommandHandler();
            }

            return this.commands[commandName.ToLower()]();
        }

        private void AcceptClients()
        {
            this.listener.BeginAcceptTcpClient(ar =>
            {
                try
                {
                    TcpClient client = this.listener.EndAcceptTcpClient(ar);
                    this.AcceptClients();
                    new Session(this, client, this.framing, this.delimiter).ProcessCommand();
                }
                catch (ObjectDisposedException)
                {
                }
            }, null);
        }

        public void Stop()
        {
            this.listener.Stop();
        }

        public void AddCommand(string commandName, Func<ICommand> howToInstanceCommand)
        {
            this.commands.Add(commandName.ToLower(), howToInstanceCommand);
        }
    }
}