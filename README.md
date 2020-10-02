# Nuclear Framework
Nuclear framework provides alternative way to build lightweight API endpoints using custom Attributes. Even tought this framework is intended to be used as a whole , ExportLocator ClassLibrary can be used as a standalone Service registration tool.

# NOTE

Nuclear.Channels has been moved to a monolithic solution on [Nuclear.Channels.Monolithic](https://github.com/Mycenaean/Nuclear-Channels-Monolithic) . Dlls which are merged into a monolithic solutions are

### Nuclear.Channels
### Nuclear.Channels.Authentication
### Nuclear.Channels.Base
### Nuclear.Channels.Data
### Nuclear.Channels.Generators
### Nuclear.Channels.Heuristics
### Nuclear.Channels.InvokerServices
### Nuclear.Channels.Messaging
### Nuclear.ExportLocator

## [Nuclear.Channels](https://github.com/Mycenaean/Nuclear-Framework/tree/master/Nuclear.Channels)
 Class Library that contains all the logic neccessary for the initialization of channel endpoints. To install it with Package Manager:
 ```
 Install-Package Nuclear.Channels -Version 3.1.5
 ```
 To install it with .NET CLI:
 ```
 dotnet add package Nuclear.Channels --version 3.1.5
 ```
  
## [Nuclear.ExportLocator](https://github.com/Mycenaean/Nuclear-Framework/tree/master/Nuclear.ExportLocator)
 Class Library that`s been used as an Dependency Injection tool. To install it with Package Manager
 ```
 Install-Package Nuclear.ExportLocator -Version 3.0.1
 ```
 To install it with .NET CLI
 ```
 dotnet add package Nuclear.ExportLocator --version 3.0.1
 ```


## [Nuclear.Channels.Remoting](https://github.com/Mycenaean/Nuclear-Framework/tree/master/Nuclear.Channels.Remoting)
 Class Library for easy calls to the Channels.
 ### Currently work in progress
 
## [Nuclear.Channels.Server.Manager](https://github.com/Mycenaean/Nuclear-Framework/tree/master/Nuclear.Channels.Server.Manager)
 Class Library containing all neccessary logic for CLI management of the Channels

# Authors
 Nikola Milinkovic - *initial work and maintainer* - [Mycenaean](https://github.com/Mycenaean)

# License
 All Class Libraries under this solution are MIT Licensed. Check [License](https://github.com/Mycenaean/Nuclear-Framework/blob/master/LICENSE.txt).

# Contributing
 Everyone is welcome to contribute.
