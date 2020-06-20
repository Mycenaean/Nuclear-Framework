// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Handlers;
using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Console;
using Nuclear.ExportLocator.Services;
using System.Collections.Generic;
using System.Threading;

namespace Nuclear.Channels.Server.Manager.CoreCommands
{
    public class ServerThreadCommand : IServerCommand
    {
        private readonly IServiceLocator _services;
        private readonly IConsoleWriter _writer;
        private readonly IServerCommand _initServerCommand;
        private readonly IChannelMethodHandlerCollection _methodHandlers;
        private List<ChannelMethodHandler> _methodHandlersList;

        public ServerThreadCommand(IServiceLocator services, IConsoleWriter writer, IServerCommand initServerCommand)
        {
            _services = services;
            _writer = writer;
            _initServerCommand = initServerCommand;

            _methodHandlers = _services.Get<IChannelMethodHandlerCollection>();
        }

        public void Execute()
        {
            _writer.Write("Nuclear.Channels.Server version 1.0");
            _writer.Write("Starting the server....");
            _initServerCommand.Execute();
            _writer.Write("Starting the Server Manager....");
            //Wait for handlers to be initialized since IChannelServer will start 
            //a new Task for every ChannelMethod
            for (int i = 0; i < 10; i++)
            {
                _methodHandlersList = _methodHandlers.AsList();
                int startingState = 0;
                foreach (ChannelMethodHandler handler in _methodHandlersList)
                {
                    if (handler.State == EntityState.Starting)
                        startingState++;
                    else if(handler.State != EntityState.Running )
                        _writer.Write($"{handler.HandlerId} {handler.Url} {handler.State}");
                }

                //Give time for IChannelServer to start all ChannelMethod tasks
                Thread.Sleep(1000);

                if (i > 2 && startingState == 0)
                {
                    foreach (ChannelMethodHandler handler in _methodHandlersList)
                    {
                        _writer.Write($"{handler.HandlerId} {handler.Url} {handler.State}");
                    }
                    _writer.Write("Server initialized...");
                    return;
                }
            }
        }
    }
}
