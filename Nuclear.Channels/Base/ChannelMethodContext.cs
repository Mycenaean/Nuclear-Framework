using Nuclear.Channels.Enums;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using Nuclear.ExportLocator.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Base
{
    [Export(typeof(IChannelMethodContext), ExportLifetime.Scoped)]
    public class ChannelMethodContext : IChannelMethodContext
    {
        public ChannelMethodContext(HttpListenerRequest request, HttpListenerResponse response, ChannelHttpMethod method, List<object> parameters, bool isAuthenticated)
        {
            ChannelMethodRequest = request;
            ChannelMethodResponse = response;
            ChannelHttpMethod = method;            
            IsAuthenticated = isAuthenticated;
            Parameters = parameters;
        }

        public HttpListenerRequest ChannelMethodRequest { get;  }
        public HttpListenerResponse ChannelMethodResponse { get; }
        public ChannelHttpMethod ChannelHttpMethod { get;  }
        public List<object> Parameters { get;  }
        public bool IsAuthenticated { get;  }


    }
}
