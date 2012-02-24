namespace HumbleServer
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using Commands;
    using Streams;

    public class NetworkServer
    {
        private TcpListener listener;
        private readonly IDictionary<string, Func<ICommand>> commands = new Dictionary<String, Func<ICommand>>();

        public NetworkServer()
        {
            this.UnknowCommand = () => new UnknowCommand();
        }

        public Func<ICommand> UnknowCommand
        {
            get;
            set;
        }

        public int Port
        {
            get { return ((IPEndPoint)this.listener.LocalEndpoint).Port; }
        }

        public NetworkServer Start(int port)
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
                return this.UnknowCommand();
            }

            return this.commands[commandName.ToLower()]();
        }

        private void AcceptClients()
        {
            this.listener.BeginAcceptTcpClient(ar =>
            {
                TcpClient client;
                try
                {
                    client = this.listener.EndAcceptTcpClient(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }

                this.AcceptClients();
                this.WaitForCommand(client);
            }, null);
        }

        private void WaitForCommand(TcpClient client)
        {
            //// TODO: async
            IHumbleStream stream = new FixedLengthStream(client.GetStream());

            try
            {
                var commandName = stream.Receive().ToLower();

                //// TODO: criar abstracao de socket
                ICommand command = this.GetCommand(commandName);
                command.SetContext(client, stream);
                command.Execute();
            }
            catch (Exception exception)
            {
                var message = exception.GetType().Name + ": " + exception.Message;
                stream.Send(message);
            }

            this.WaitForCommand(client);
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