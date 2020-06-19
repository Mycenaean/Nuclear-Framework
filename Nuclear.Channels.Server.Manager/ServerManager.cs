using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Console;
using Nuclear.Channels.Server.Manager.IO;
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
        private readonly IPluginService _plugins;

        public ServerManager(IServiceLocator services, IChannelServer server)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _server = server ?? throw new ArgumentNullException(nameof(server));

            _writer = _services.Get<IConsoleWriter>();
            _reader = _services.Get<IConsoleReader>();
            _commandFactory = _services.Get<IServerCommandFactory>();
            _commandResults = _services.Get<ICommandExecutionResults>();
            _plugins = _services.Get<IPluginService>();

            //_plugins.Init();
            Init();
        }

        private void Init()
        {
            //_server.StartHosting(null);
        }

        public void Start()
        {
            _writer.WriteHelp();
            string userInput = DesktopConsole.ReadLine();

            while (!userInput.Equals(ServerCommandList.StopProgram, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (userInput.Equals(ServerCommandList.Help))
                    {
                        _writer.WriteHelp();
                        return;
                    }
                    ServerCommandContext cmdContext = _reader.Read(userInput);
                    IServerCommand command = _commandFactory.GetCommand(cmdContext);
                    command.Execute();

                    CommandId commandId = _commandResults.GetLastCommandId();
                    object commandResult = _commandResults.GetResult(commandId);
                    if (commandResult != null)
                        _writer.Write($"Executed {commandId.Value} with result {commandResult}");
                    else
                        _writer.Write($"Executed {commandId.Value}");
                }
                catch(Exception ex)
                {
                    _writer.Write(ex.Message);
                }
            }
        }
    }

}