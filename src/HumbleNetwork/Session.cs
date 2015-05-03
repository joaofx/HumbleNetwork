namespace HumbleNetwork
{
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Session : IDisposable
    {
        private readonly HumbleServer _server;
        private readonly TcpClient _client;
        private readonly Sessions _sessions;
        private readonly IHumbleStream _stream;

        public Session(Sessions sessions, HumbleServer server, TcpClient client, Framing framing, string delimiter)
        {
            _stream = MessageFraming.Create(framing, client, delimiter);
            _server = server;
            _client = client;
            _sessions = sessions;
        }

        public void ProcessNextCommand()
        {
            var taskExecute = Task<string>.Factory.StartNew(
                () => ExecuteHandlingExceptions(() => _stream.Receive(4)));

            var commandProcessed = taskExecute.ContinueWith(antecedent =>
            {
                if (antecedent.Status == TaskStatus.Faulted)
                {
                    return false;
                }

                return ExecuteHandlingExceptions(() =>
                {
                    var commandName = antecedent.Result.ToLower();

                    if (string.IsNullOrEmpty(commandName))
                    {
                        return false;
                    }

                    var command = _server.GetCommand(commandName);
                    ExecuteCommand(command);

                    return true;
                });
            });

            ContinueProcessingNextCommand(commandProcessed);
        }

        public void Dispose()
        {
            _sessions.Disposed(this);
            _stream.Close();
            _client.Close();
        }

        private void ContinueProcessingNextCommand(Task<bool> commandProcessed)
        {
            commandProcessed.ContinueWith(antecedent =>
            {
                if (antecedent.Result)
                {
                    ProcessNextCommand();
                }
            });
        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute(_stream);
            }
            catch (Exception exception)
            {
                _server.ExceptionHandler().Execute(_stream, exception);
            }
        }

        private T ExecuteHandlingExceptions<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                Dispose();
                return default(T);
            }
        }
    }
}