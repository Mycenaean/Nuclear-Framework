// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Manager.Commands;
using Nuclear.Channels.Server.Manager.Exceptions;
using Nuclear.ExportLocator;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Diagnostics;
using System.Linq;
using DesktopConsole = System.Console;

namespace Nuclear.Channels.Server.Manager.Console
{
    [Export(typeof(IConsoleReader), ExportLifetime.Transient)]
    public class ConsoleReader : IConsoleReader
    {
        private readonly string[] _instructions;

        [DebuggerStepThrough]
        public ConsoleReader()
        {
            _instructions = new string[] { "read", "start", "stop", "restart" };
        }

        public ServerCommandContext Read(string consoleLine)
        {
            var userInput = consoleLine.Split(' ');
            if (userInput.Length > 4)
                throw new InvalidInstructionException("Number of arguments exceeds 3");

            if (!_instructions.Any(x => x.Equals(userInput[0])))
                throw new InvalidInstructionException("Instruction not recognized");

            if (!userInput[1].Equals("channel", StringComparison.OrdinalIgnoreCase) && !userInput[1].Equals("channelMethod", StringComparison.OrdinalIgnoreCase))
                throw new InvalidInstructionTargetException("Target not recognized");

            return userInput[0].Equals("read", StringComparison.OrdinalIgnoreCase)
                ? new ServerCommandContext(ServiceLocatorBuilder.CreateServiceLocator(), userInput[3], userInput[0])
                : new ServerCommandContext(ServiceLocatorBuilder.CreateServiceLocator(), userInput[2], userInput[0]);

        }
    }
}
