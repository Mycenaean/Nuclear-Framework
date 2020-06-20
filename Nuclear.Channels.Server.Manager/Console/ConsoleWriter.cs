// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Handlers;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DesktopConsole = System.Console;

namespace Nuclear.Channels.Server.Manager.Console
{
    [Export(typeof(IConsoleWriter), ExportLifetime.Transient)]
    public class ConsoleWriter : IConsoleWriter
    {
        private IChannelMethodHandlerCollection _methodHandlers;
        private const string lineWrapper = "============================================";

        [DebuggerStepThrough]
        public ConsoleWriter()
        {
            _methodHandlers = ServiceLocatorBuilder
                                    .CreateServiceLocator()
                                    .Get<IChannelMethodHandlerCollection>();
        }

        public void InjectServerMessagePrefix()
        {
            DesktopConsole.ForegroundColor = ConsoleColor.Yellow;
            DesktopConsole.Write("server message# ");
            DesktopConsole.ResetColor();
        }

        public void InjectServerPrefix()
        {
            DesktopConsole.ForegroundColor = ConsoleColor.Green;
            DesktopConsole.Write("server# ");
            DesktopConsole.ResetColor();
        }

        public void Write(string line)
        {
            InjectServerMessagePrefix();
            DesktopConsole.WriteLine(line);            
        }

        public void WriteUrls()
        {
            ChannelMethodHandler[] handlers = _methodHandlers.AsArray();
            for (int i = 0; i < handlers.Length; i++)
            {
                InjectServerMessagePrefix();
                DesktopConsole.WriteLine($"{handlers[i]?.HandlerId} {handlers[i]?.Url} {handlers[i]?.State}");
            }
        }

        public void WriteHelp()
        {
            ServerCommandList cmdList = new ServerCommandList();
            FieldInfo[] serverCommands = typeof(ServerCommandList).GetFields();
            for (int i = 0; i < serverCommands.Length; i++)
            {
                InjectServerMessagePrefix();
                DesktopConsole.WriteLine(serverCommands[i]?.GetValue(cmdList)?.ToString());
            }
        }

        public void TextColor(ConsoleColor color)
        {
            DesktopConsole.ForegroundColor = color;
        }
    }
}
