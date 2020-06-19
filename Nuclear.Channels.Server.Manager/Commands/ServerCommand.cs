using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Manager.Console;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Services;
using System;
using System.Drawing;
using System.Windows.Input;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public abstract class ServerCommand
    {
        private IServiceLocator _services;
        protected ICommandExecutionResults _results;
        protected IConsoleWriter _writer;
        protected ChannelMethodHandler _methodHandler;
        protected ChannelHandler _channelHandler;
        protected bool _isMethod;
        protected object _result;

        public ConsoleColor Color { get; private set; }

        protected ServerCommand(ChannelHandler channelHandler, ConsoleColor color)
        {
            _channelHandler = channelHandler;
            Init(color, false);
        }

        protected ServerCommand(ChannelMethodHandler methodHandler, ConsoleColor color)
        {
            _methodHandler = methodHandler;
            Init(color, true);
        }

        private void Init(ConsoleColor color, bool isMethod)
        {
            _services = ServiceLocatorBuilder.CreateServiceLocator();
            Color = color;
            _isMethod = isMethod;
            _results = _services.Get<ICommandExecutionResults>();
            _writer = _services.Get<IConsoleWriter>();
            _writer.TextColor(color);
        }

        public void Execute()
        {
            if (_isMethod)
                ExecuteOnMethod();
            else
                ExecuteOnChannel();

            AddResult();
        }

        private void AddResult()
        {
            string handlerId = _isMethod ? _methodHandler.HandlerId : _channelHandler.HandlerId;
            _results.AddResult(new CommandId(this.GetType(), handlerId, DateTime.Now), _result);
        }

        protected abstract void ExecuteOnMethod();
        protected abstract void ExecuteOnChannel();

    }

}
