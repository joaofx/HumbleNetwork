HumbleNetwork
=============

[![Build status](https://ci.appveyor.com/api/projects/status/whppw7n2ejs667ku?svg=true)](https://ci.appveyor.com/project/joaofx/humblenetwork)

HumbleNetwork is a library to help build simple network services based in sockets. It offers a client and a server class and a stream helper. See the example:

``` csharp
public class Program
{
	static void Main(string[] args)
	{
		// create the server
		var server = new HumbleServer();
		
		// set a command to handle echo message
		server.AddCommand("echo", () => new EchoCommand());
		
		// start server at any port
		server.Start(0);

		// create the client
		var client = new HumbleClient();
		
		// connect to the server
		client.Connect("localhost", server.Port);
		
		// send a echo message and then hello world
		client.Send("echo").Send("hello world");

		// receive hello world
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
```

More Examples
-------------

Take a look on Tests files for more examples.


Install
-------

```
PM> Install-Package HumbleNetwork
```

Build
----

build quick

build test

build package