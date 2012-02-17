namespace HumbleServer
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    //// TODO: mudar parametro threadpool.setmin
    public class NetworkServer
    {
        private TcpListener listener;
        private readonly IDictionary<string, Func<ICommand>> commands =
            new Dictionary<String, Func<ICommand>>();

        public NetworkServer()
        {
            this.UnknowCommand = () => new Unknow();
        }

        public Func<ICommand> UnknowCommand
        {
            get;
            set;
        }

        public NetworkServer Start(int port)
        {
            this.listener = new TcpListener(IPAddress.Any, port);
            this.listener.Start();
            this.AcceptClients();
            return this;
        }

        private void AcceptClients()
        {
            this.listener.BeginAcceptTcpClient(ar =>
            {
                Socket client;
                try
                {
                    client = this.listener.EndAcceptSocket(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }

                this.AcceptClients();
                this.WaitForCommand(client);
            }, null);
        }

        public void Stop()
        {
            this.listener.Stop();
        }

        private void WaitForCommand(Socket client)
        {
            var buffer = new byte[4];
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ar =>
            {
                client.EndReceive(ar);
                try
                {
                    var commandName = Encoding.ASCII.GetString(buffer).ToLower();

                    //// TODO: criar abstracao de socket
                    ICommand command = this.GetCommand(commandName);
                    command.SetContext(client);
                    command.Execute();
                }
                catch (Exception exception)
                {
                    var message = exception.GetType().Name + ": " + exception.Message;
                    var data = Encoding.ASCII.GetBytes(message);
                    client.Send(data, 0, data.Length, SocketFlags.None);
                }

                this.WaitForCommand(client);
            }, client);
        }

        private ICommand GetCommand(string commandName)
        {
            if (this.commands.ContainsKey(commandName) == false)
            {
                return this.UnknowCommand();
            }

            return this.commands[commandName.ToLower()]();
        }

        public void AddCommand(string commandName, Func<ICommand> howToInstanceCommand)
        {
            this.commands.Add(commandName.ToLower(), howToInstanceCommand);
        }
    }
}