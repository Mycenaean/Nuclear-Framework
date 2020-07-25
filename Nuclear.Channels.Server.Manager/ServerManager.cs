// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Console;
using Nuclear.Channels.Server.Manager.CoreCommands;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Threading.Tasks;
using DesktopConsole = System.Console;

namespace Nuclear.Channels.Server.Manager
{
    [Export(typeof(IChannelServerManager), ExportLifetime.Singleton)]
    internal class ServerManager : IChannelServerManager
    {
        private readonly IServiceLocator _services;
        private readonly IConsoleWriter _writer;
        private readonly IConsoleReader _reader;
        private readonly IChannelServer _server;
        private readonly IServerCommandFactory _commandFactory;
        private readonly ICommandExecutionResults _commandResults;

        public ServerManager(IServiceLocator services, IChannelServer server)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _server = server ?? throw new ArgumentNullException(nameof(server));

            _writer = _services.Get<IConsoleWriter>();
            _reader = _services.Get<IConsoleReader>();
            _commandFactory = _services.Get<IServerCommandFactory>();
            _commandResults = _services.Get<ICommandExecutionResults>();

            InitCommands();
        }

        private void InitCommands()
        {
            var initServer = new CoreCommand("InitServer");
            initServer.AddService(_server);            

            var initServerCommand = _commandFactory.GetCoreCommand(initServer);

            var serverThreading = new CoreCommand("ServerThread");
            
            // ORDER OF THE ADDED SERVICES MATTERS!
            serverThreading.AddService(initServerCommand);
            serverThreading.AddService(_services);
            serverThreading.AddService(_writer);

            var serverThreadCommand = _commandFactory.GetCoreCommand(serverThreading);
            serverThreadCommand.Execute();
        }

        public void Start()
        {
            _writer.Write("Listening for commands");
            _writer.Write($"To see list of commands type : {ServerCommandList.Help}");

            _writer.InjectServerPrefix();
            var userInput = DesktopConsole.ReadLine();

            ContinueWhenStopped:
            
            while (userInput != null && !userInput.Equals(ServerCommandList.StopProgram, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (userInput.Equals(ServerCommandList.Help))
                    {
                        _writer.WriteHelp();
                    }
                    else
                    {
                        var cmdContext = _reader.Read(userInput);
                        var command = _commandFactory.GetCommand(cmdContext);
                        command.Execute();

                        var commandId = _commandResults.GetLastCommandId();
                        var commandResult = _commandResults.GetResult(commandId);
                        _writer.Write(commandResult != null
                            ? $"{commandResult.ToString()}"
                            : $"Executed {commandId.Value}");
                    }
                }
                catch (Exception ex)
                {
                    _writer.Write(ex.Message);
                }

                _writer.InjectServerPrefix();
                userInput = DesktopConsole.ReadLine();
            }

            if (userInput != null && userInput.Equals(ServerCommandList.ShutDown, StringComparison.OrdinalIgnoreCase))
            {
                DesktopConsole.WriteLine("Application shutting down...");
                return;
            };
            
            _writer.InjectServerPrefix();
            DesktopConsole.WriteLine("Server is stopped...");
            while(!String.IsNullOrEmpty(userInput = DesktopConsole.ReadLine()))
                if(userInput.Equals(ServerCommandList.StartProgram,StringComparison.OrdinalIgnoreCase))
                    goto ContinueWhenStopped;
        }

    }

}