# Nuclear Channels

Channels Library is part of the Nuclear Framework set of .NET Standard class libraries used to build lightweight API Endpoints using HttpListener class. 

# Installation

To install it with Package Manager
 ```
 Install-Package Nuclear.Channels -Version 3.1.0
 ```
 To install it with .NET CLI
 ```
 dotnet add package Nuclear.Channels --version 3.1.0
 ```

# How to use

Decorate class in which you have methods you want to expose as endpoints with ChannelAttribute and methods you want to expose as API endpoints with ChannelMethodAttribute. Channels Library is using ExportLocator for dependency injection. Every service from this Library can be injected from IServiceLocator.

## ChannelAttribute

``` c#
[Channel]
public class TestChannel
{
	...
}
```
This code will make base route for all ChannelMethods in ~/channels/TestChannel/. In case you want to change this route you can provide Name property of the ChannelAttribute.
```c#
[Channel(Name = "CustomChannel")]
public class TestChannel
{
	...
}
```
Route for this will be ~/channels/CustomChannel/ instead of ~/channels/TestChannel/.
Other property of the ChannelAttribute is Description property which when provided will generate Description for the builtin documentation tool.
```c#
[Channel(Description = "Test Channel Description")]
```
If you want to make global authorization for targeted channel you can provide AuthorizeChannelAttribute beneath ChannelAttribute.

```c#
[Channel]
[AuthorizeChannel(Schema = ChannelAuthenticationSchemes.Basic)]
public class TestChannel
{
	...
}
```

If you want to enable sessions and get cookies use EnableSessions property and set it to true

```c#
[Channel(EnableSessions = true)]
public class TestChannel
{
	...
}
```


## ChannelMethodAttribute

Now that you exposed class as a Channel you need to provide endpoints for your methods.

```c#
[ChannelMethod]
public string HelloWorld()
{
	return "Hello World from ChannelMethod";
}
```
This method will be exposed as an API Endpoint on ~/channels/{Name of your channel}/HelloWorld/.

### HttpMethod GET
	
If you dont specify HttpMethod property of the ChannelMethod , it will accept both GET and POST requests.
```c#
[ChannelMethod(HttpMethod = ChannelHttpMethod.GET)]
public string HelloWorld()
{
	return "Hello World from ChannelMethod";
}
```
If you want to provide parameters with GET as usual you need to provide them in QueryString
```c#
[ChannelMethod(HttpMethod = ChannelHttpMethod.GET)]
public string HelloWorld(string name)
{
	return $"Hello World {name} from ChannelMethod";
}
```
~/channels/{Name of you channel}/HelloWorld/?name=Nikola. Important thing is that the parameter in the QueryString must match parameter in the ChannelMethod. Its case insensitive

### HttpMethod POST

```c#
[ChannelMethod(HttpMethod = ChannelHttpMethod.POST)]
public string HelloWorld(string name)
{
	return $"Hello World {name} from ChannelMethod";
}
```
For this route will be the same ~/channels/{Name of you channel}/HelloWorld/ input body in xml format would look like this.

```xml
<channels>
	<name>nikola</name>
</channels>
```
For complex entities is the same. For JSON input body is like for every other API.
```c#
[ChannelMethod(HttpMethod = ChannelHttpMethod.POST)]
public SomeEntity EntityMethod(SomeEntity entity)
{
	return entity;
}
 ```

 ```xml
<channels>
	<SomeEntity>
		<Id>1</Id>
		<Name>Your Name</Name>
		...
	</SomeEntity>
</channels>
```
Other properties are Description and Schema. Both are the same as for ChannelAttribute , Description if provided will autogenerate description for builtin tool for documentation. ChannelAuthenticationSchemes will enforce desired authentication.

```c#
[ChannelMethod(HttpMethod = ChannelHttpMethod.POST,Schema = ChannelAuthenticationSchemes.Basic,Description = "EntityMethod Description")]
public SomeEntity EntityMethod(SomeEntity entity)
{
	return entity;
}
```

## Caching response

You can cache ChannelMethod response with EnableCacheAttribute. But note that EnableCacheAttribute can not be implemented on top of method that returns void. Parameteres are Duration and Duration Unit.

```c#
	[ChannelMethod]
        [EnableCache(20, CacheDurationUnit.Seconds)]
        public string HelloWorld()
        {
            return "Hello World";
        }
```

In case you are worried about the state of memory, you can setup background cache cleaner with extension method ConfigureCacheCleaner , located in Nuclear.Channels.Heuristics.CacheCleaner namespace, which will clear expired cached responses in time interval provided.

```c#
	IChannelServer server = ChannelServerBuilder.CreateServer();
	...
	server.ConfigureCacheCleaner(TimeSpan.FromSeconds(30)); // put your time interval here
```

## ChannelBase class

If you want to have access to all global services , request context and messaging service your channel class can inherit from ChannelBase class

```c#
public abstract class ChannelBase
{
    IServiceLocator Services { get; }
    IChannelMethodContext Context { get; }
    IChannelMessageOutputWriter ChannelMessageWriter { get; }
    void RedirectToUrl(string url, bool isHttps = true);
}
```

## Dependency Injection into Channels

If you have services you want to inject into channels you can do it with ImportedServiceAttribute.

```c#
[Channel]
public class TestChannel
{
    [ImportedService]
    public ISomeService1 _someService1 { get; set; }
    
    [ImportedService]
    public ISomeService2 _someService2 { get; set; }
    
    ...
```
Note that Service must be public and must be Property

### IChannelMessageOutputWriter service

You can use IChannelMessageOutputWriter service to return response from your method

```c#
[ChannelMethod]
public void HelloWorld(string name)
{
    ChannelMessage msg = new ChannelMessage()
    {
        Message = string.Empty,
        Output = $"Hello World {name} from ChannelMethod",
        Success = true
    };

    ChannelMessageWriter.Write(msg, Context.Response);
}
```



## Full Example
```c#
[Channel]
public class TestChannel
{
    [ChannelMethod]
    public string HelloWorld()
    {
        return "Hello World from ChannelMethod";
    }

    [ChannelMethod(ChannelHttpMethod.GET)]
    public string Hello(string name)
    {
        return $"Hello {name} from Hello ChannelMethod";
    }

	[ChannelMethod(HttpMethod = ChannelHttpMethod.POST)]
	public SomeEntity EntityMethod(SomeEntity entity)
	{
		return entity;
	}
}
```
## Response Object
Response of the ChannelMethods is always in the form of IChannelMessage containing properties , Success , Message and Output. Methods return object is always in the Output field.

```c#
public interface IChannelMessage
{
     bool Success { get; set; }
     object Output { get; set; }
     string Message { get; set; }
}
```

Example response in browser:
```json
{
  "Success": true,
  "Output": "Hello World from ChannelMethod",
  "Message": null
}
```

# Documentation tool

Documentation for channels is autogenerated by IChannelDocumentationService.
```c#
IServiceLocator Services = ServiceLocator.CreateServiceLocator();
IChannelDocumentationService _documentationService = Services.Get<IChannelDocumentationService>().GetDocumentation(AppDomain.CurrentDomain);
```

# Initializing Channels

You can test and initialize Channels in both Console Apps and Web Apps. To initialize it internaly as a Windows Service you can create Console app.
```c#
    class Program
    {
        static void Main(string[] args)
        {
            IChannelServer server = ChannelServerBuilder.CreateServer();
            server.LoadAssemblies(AppDomain.CurrentDomain, null);
            server.StartHosting(null);

            Console.ReadLine();
        }
    }
```
IChannelServer service will start the hosting of the channels. In Web apps just apply same 3 lines of code in Program.cs.

# Authors
 Nikola Milinkovic - *initial work and maintainer* - [Mycenaean](https://github.com/Mycenaean)

# License
 Nuclear.Channels are MIT Licensed. Check [License](https://github.com/Mycenaean/Nuclear-Framework/blob/master/LICENSE.txt).

