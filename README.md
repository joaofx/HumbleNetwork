HumbleNetwork
=============

[![Build status](https://ci.appveyor.com/api/projects/status/whppw7n2ejs667ku?svg=true)](https://ci.appveyor.com/project/joaofx/humblenetwork)
<a href="https://www.nuget.org/packages/HumbleNetwork/"><img src="http://img.shields.io/nuget/v/HumbleNetwork.svg?style=flat-square" alt="NuGet version" height="18"></a> 
<a href="https://www.nuget.org/packages/HumbleNetwork/"><img src="http://img.shields.io/nuget/dt/HumbleNetwork.svg?style=flat-square" alt="NuGet version" height="18"></a>
[![NuGet Status](http://nugetstatus.com/HumbleNetwork.png)](http://nugetstatus.com/packages/HumbleNetwork)

HumbleNetwork is a library to help build simple network services based in sockets. It offers a client and a server class, a stream helper and a request/response command handler protocol. 

### Why use this?

I worked on a project that had many VB6 clients talking by socket with some VB6 servers. We started migrating the VB6 servers to .NET and developed HumbleNetwork to make socket programming easy on the server. In the end we were using HumbleNetwork even in .NET clients

### Example

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

```cmd
build quick
```

```cmd
build test
```

```cmd
build package -D:version=YYYY.MM.DD.BUILD
```

.nupkg is saved in build\package
