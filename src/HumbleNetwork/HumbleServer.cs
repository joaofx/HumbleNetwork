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
        private readonly string _delimiter;
        private readonly IDictionary<string, Func<ICommand>> _commands = new Dictionary<string, Func<ICommand>>();
        private readonly Framing _framing;
        private readonly Sessions _sessions = new Sessions();
        private TcpListener _listener;
        
        public HumbleServer(Framing framing = Framing.LengthPrefixed, string delimiter = MessageFraming.DefaultDelimiter)
        {
            _delimiter = delimiter;
            UnknowCommandHandler = () => new DefaultUnknowCommandHandler();
            ExceptionHandler = () => new DefaultExceptionHandler();
            _framing = framing;
        }

        public Func<IUnknowCommandHandler> UnknowCommandHandler
        {
            get;
            set;
        }

        public int Port
        {
            get { return ((IPEndPoint)_listener.LocalEndpoint).Port; }
        }

        public Func<IExceptionHandler> ExceptionHandler
        {
            get;
            set;
        }

        public HumbleServer Start(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _sessions.DisposeAllSessions();
            AcceptClients();
            return this;
        }

        public ICommand GetCommand(string commandName)
        {
            if (_commands.ContainsKey(commandName.ToLower()) == false)
            {
                return UnknowCommandHandler();
            }

            return _commands[commandName.ToLower()]();
        }

        public void Stop()
        {
            _listener.Stop();
            _sessions.DisposeAllSessions();
        }

        public void AddCommand(string commandName, Func<ICommand> howToInstanceCommand)
        {
            _commands.Add(commandName.ToLower(), howToInstanceCommand);
        }

        private void AcceptClients()
        {
            _listener.BeginAcceptTcpClient(ar =>
            {
                try
                {
                    TcpClient client = _listener.EndAcceptTcpClient(ar);
                    AcceptClients();

                    _sessions
                        .NewSession(this, client, _framing, _delimiter)
                        .ProcessNextCommand();
                }
                catch (ObjectDisposedException)
                {
                }
            }, null);
        }
    }
}