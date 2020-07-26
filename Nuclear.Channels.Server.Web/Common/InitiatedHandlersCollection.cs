using System.Collections.Generic;
using System.Linq;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;

namespace Nuclear.Channels.Server.Web.Common
{
    [Export((typeof(IInitiatedHandlersCollection)), ExportLifetime.Singleton)]
    public class InitiatedHandlersCollection : IInitiatedHandlersCollection
    {
        private readonly List<HandlerInformation> _handlers;

        public InitiatedHandlersCollection()
        {
            _handlers = new List<HandlerInformation>();
        }

        public void AddHandler(string handlerId, string url, string state)
        {
            _handlers.Add(new HandlerInformation(handlerId,url,state));
        }

        public void UpdateHandlerState(string handlerId, string state)
        {
            var handler = _handlers.FirstOrDefault(x => x.HandlerId == handlerId);
            if (handler != null) handler.State = state;
        }
    }
}