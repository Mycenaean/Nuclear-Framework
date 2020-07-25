// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class CommandId
    {
        public string Value { get; }

        public CommandId(Type command, string handlerId, DateTime executedTime)
        {
            Value = $"{command.Name} on {handlerId} at {executedTime}";
        }

    }
}
