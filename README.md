# Nuclear Framework
Nuclear framework provides alternative way to build lightweight API endpoints using custom Attributes. Even tought this framework is intended to be used as a whole , ExportLocator ClassLibrary can be used as a standalone Dependency Injection tool.

## [Nuclear.Channels](https://github.com/Mycenaean/Nuclear-Framework/tree/master/Nuclear.Channels)
 Class Library that contains all the logic neccessary for the initialization of channel endpoints. It depends heavily on Nuclear.Data Class Library but its dependancy can be removed from Nuclear.ExportLocator. To install it with Package Manager:
 ```
 Install-Package Nuclear.Channels -Version 3.1.4
 ```
 To install it with .NET CLI:
 ```
 dotnet add package Nuclear.Channels --version 3.1.4
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

# Authors
 Nikola Milinkovic - *initial work and maintainer* - [Mycenaean](https://github.com/Mycenaean)

# License
 All Class Libraries under this solution are MIT Licensed. Check [License](https://github.com/Mycenaean/Nuclear-Framework/blob/master/LICENSE.txt).

# Contributing
 Everyone is welcome to contribute.
