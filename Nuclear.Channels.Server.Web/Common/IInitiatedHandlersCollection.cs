using System.Collections.Generic;

namespace Nuclear.Channels.Server.Web.Common
{
    public interface IInitiatedHandlersCollection
    {
        public IReadOnlyCollection<HandlerInformation> Handlers { get; }
        void AddHandler(string handlerId, string url, string state, bool isProtected);
        void UpdateHandlerState(string caller, string handlerId, string state);
    }
}