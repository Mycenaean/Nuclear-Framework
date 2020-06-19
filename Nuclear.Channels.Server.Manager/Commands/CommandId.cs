using System;

namespace Nuclear.Channels.Server.Manager.Commands
{
    public class CommandId
    {
        public string Value { get; }

        public CommandId(Type command, string handlerId, DateTime executedTime)
        {
            Value = $"{command.GetType()}_{handlerId}_{executedTime}";
        }

    }
}
