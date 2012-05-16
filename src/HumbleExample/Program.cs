namespace HumbleExample
{
    using System;
    using HumbleNetwork;

    public class Program
    {
        public static void Main(string[] args)
        {
            //// create the server, configure echo command and start listening
            var server = new HumbleServer();
            server.AddCommand("echo", () => new EchoCommand());
            server.Start(0);

            //// create the client, connect to the server, send the command and the parameters
            var client = new HumbleClient();
            client.Connect("localhost", server.Port);
            client.Send("echo").Send("hello world");

            Console.WriteLine("Client received: " + client.Receive());
            Console.ReadKey();
        }
    }
}
