HumbleNetwork
=============

Not stable. Don't use it in production.

[![Build Status](https://travis-ci.org/joaofx/HumbleNetwork.png)](https://travis-ci.org/joaofx/HumbleNetwork)

It's a library to help build network services based in sockets. It offers a client and a server class and a stream helper. See the example:

<pre>
public class Program
{
	static void Main(string[] args)
	{
		var server = new HumbleServer();
		server.AddCommand("echo", () => new EchoCommand());
		server.Start(0);

		var client = new HumbleClient();
		client.Connect("localhost", server.Port);
		client.Send("echo").Send("hello world");

		Console.WriteLine("Client received: " + client.Receive());
		Console.ReadKey();
	}
}

public class EchoCommand : ICommand
{
	public void Execute(IHumbleStream stream)
	{
		stream.Send(stream.Receive());
	}
}
</pre>

Build
----

build quick

build test

build package


TODO
----

* License
* CI generate nuget package and upload it to nuget.org
* Accept send and receive stream
* Timeout when send or receive
* Work with other types of data
* Send and receive file
* Server wait for command using async on streams
* Server works with async on streams
* Stream send command method
* Client close or disconnect method
* Control connected clients
* Cryptography messages
* Compact messages
