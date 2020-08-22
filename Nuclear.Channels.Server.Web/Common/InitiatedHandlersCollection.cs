// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace Nuclear.Channels.Server.Web.Common
{
    [Export((typeof(IInitiatedHandlersCollection)), ExportLifetime.Singleton)]
    public class InitiatedHandlersCollection : IInitiatedHandlersCollection
    {
        private readonly List<HandlerInformation> _handlers;
        public IReadOnlyCollection<HandlerInformation> Handlers => _handlers;

        public InitiatedHandlersCollection()
        {
            _handlers = new List<HandlerInformation>();
        }

        public void AddHandler(string handlerId, string url, string state, bool isProtected)
        {
            _handlers.Add(new HandlerInformation(handlerId, url, state, isProtected));
        }

        public void UpdateHandlerState(string caller, string handlerId, string state)
        {
            var handler = _handlers.FirstOrDefault(x => x.HandlerId == handlerId);
            if (handler != null)
            {
                var historyInfo = $"Executed {caller} , changed state to {state}";
                handler.State = state;
                handler.History.Add(historyInfo);
            }
        }
    }
}