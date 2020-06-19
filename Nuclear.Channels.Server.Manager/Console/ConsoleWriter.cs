using Nuclear.Channels.Handlers;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using DesktopConsole = System.Console;

namespace Nuclear.Channels.Server.Manager.Console
{
    [Export(typeof(IConsoleWriter), ExportLifetime.Transient)]
    public class ConsoleWriter : IConsoleWriter
    {
        private IChannelMethodHandlerCollection _methodHandlers;
        private const string lineWrapper = "============================================";

        public ConsoleWriter()
        {
            _methodHandlers = ServiceLocatorBuilder
                                    .CreateServiceLocator()
                                    .Get<IChannelMethodHandlerCollection>();
        }

        public void Write(string line)
        {
            DesktopConsole.WriteLine(lineWrapper);
            DesktopConsole.WriteLine();
            DesktopConsole.WriteLine(line);
        }

        public void WriteUrls()
        {
            ChannelMethodHandler[] handlers = _methodHandlers.AsArray();
            for (int i = 0; i < handlers.Length; i++)
            {
                DesktopConsole.WriteLine($"{handlers[i]?.HandlerId} {handlers[i]?.Url} {handlers[i]?.State}");
            }
        }

        public void WriteHelp()
        {
            ServerCommandList cmdList = new ServerCommandList();
            FieldInfo[] serverCommands = typeof(ServerCommandList).GetFields();
            for (int i = 0; i < serverCommands.Length; i++)
            {
                DesktopConsole.WriteLine(serverCommands[i]?.GetValue(cmdList)?.ToString());
            }
        }

        public void TextColor(ConsoleColor color)
        {
            DesktopConsole.ForegroundColor = color;
        }
    }
}
