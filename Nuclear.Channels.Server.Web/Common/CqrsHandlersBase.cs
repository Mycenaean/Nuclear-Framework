using Nuclear.Channels.Handlers;
using Nuclear.ExportLocator;
using System.Collections.Generic;

namespace Nuclear.Channels.Server.Web.Common
{
    public abstract class CqrsHandlersBase
    {
        public IInitiatedHandlersCollection HandlerInfoCollection;
        public IReadOnlyCollection<HandlerInformation> HandlerInfos;
        public List<ChannelMethodHandler> RawHandlers;

        public CqrsHandlersBase()
        {
            var services = ServiceLocatorBuilder.CreateServiceLocator();
            RawHandlers = services.Get<IChannelMethodHandlerCollection>().AsList();
            HandlerInfoCollection = services.Get<IInitiatedHandlersCollection>();
            HandlerInfos = HandlerInfoCollection.Handlers;
        }
    }
}
