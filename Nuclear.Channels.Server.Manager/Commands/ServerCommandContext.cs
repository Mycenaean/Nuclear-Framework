using Nuclear.ExportLocator.Services;
using System;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class ServerCommandContext
    {
        private IServiceLocator _services;
        private IServerCommandInterpreter _commandInterpreter;
        
        private string _handlerId;
        private string _command;

        public ServerCommandTarget CommandTarget { get; private set; }
        public ServerCommandType CommandType { get; private set; }
        public string HandlerId { get; private set; }
        

        public ServerCommandContext(IServiceLocator services, string handlerId, string command)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _handlerId = handlerId ?? throw new ArgumentNullException(nameof(services));
            _command = command ?? throw new ArgumentNullException(nameof(command));

            _commandInterpreter = _services.Get<IServerCommandInterpreter>();
            InitProperties();
        }

        private void InitProperties()
        {
            CommandTarget = _commandInterpreter.InterpretTarget(_handlerId);
            CommandType = _commandInterpreter.InterpretType(_command);
            HandlerId = _handlerId;
        }
    }
}
