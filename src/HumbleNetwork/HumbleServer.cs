namespace HumbleNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using Commands;
    using Handlers;

    public class HumbleServer
    {
        private readonly string delimiter;
        private readonly IDictionary<string, Func<ICommand>> commands = new Dictionary<string, Func<ICommand>>();
        private readonly Framing framing;
        private readonly IList<Session> sessions = new List<Session>();
        private TcpListener listener;
        
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

        public void Stop()
        {
            this.listener.Stop();
            this.CloseAllSessions();
        }

        public void AddCommand(string commandName, Func<ICommand> howToInstanceCommand)
        {
            this.commands.Add(commandName.ToLower(), howToInstanceCommand);
        }

        private void AcceptClients()
        {
            this.listener.BeginAcceptTcpClient(ar =>
            {
                try
                {
                    TcpClient client = this.listener.EndAcceptTcpClient(ar);
                    this.AcceptClients();
                    new Session(this, client, this.framing, this.delimiter, this.sessions).ProcessNextCommand();
                }
                catch (ObjectDisposedException)
                {
                }
            }, null);
        }
        
        private void CloseAllSessions()
        {
            var sessionsCopy = this.sessions.ToArray();

            foreach (var session in sessionsCopy)
            {
                session.Dispose();
            }
        }
    }
}