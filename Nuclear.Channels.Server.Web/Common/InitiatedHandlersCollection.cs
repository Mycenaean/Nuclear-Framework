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

        public void AddHandler(string handlerId, string url, string state)
        {
            _handlers.Add(new HandlerInformation(handlerId, url, state));
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