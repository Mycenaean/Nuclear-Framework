// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Manager.Commands
{
    public interface ICommandExecutionResults
    {
        void AddResult(CommandId cmdId, object result);
        object GetResult(CommandId cmdId);
        CommandId GetLastCommandId();
    }
}
