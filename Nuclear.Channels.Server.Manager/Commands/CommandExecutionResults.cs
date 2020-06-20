// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuclear.Channels.Server.Manager.Commands
{
    [Export(typeof(ICommandExecutionResults), ExportLifetime.Singleton)]
    public class CommandExecutionResults : ICommandExecutionResults
    {
        private Dictionary<CommandId, object> _commandResults;

        public CommandExecutionResults()
        {
            _commandResults = new Dictionary<CommandId, object>();
        }

        public void AddResult(CommandId cmdId, object result)
        {
            _commandResults.Add(cmdId, result);
        }

        public object GetResult(CommandId cmdId)
        {
            return _commandResults.FirstOrDefault(x => x.Key.Equals(cmdId)).Value;
        }

        public CommandId GetLastCommandId()
        {
            return _commandResults.Keys.Last();
        }
    }
}
