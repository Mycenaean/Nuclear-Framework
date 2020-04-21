using Microsoft.AspNetCore.Builder;
using Nuclear.Channels.Authentication.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nuclear.Channels.WebTemplate
{
    public static class ChannelsConfig
    {
        public static IApplicationBuilder UseChannels(this IApplicationBuilder app)
        {
            IChannelServer server = ChannelServerBuilder.CreateServer();

            //If you have channels or services in another project add them into your current AppDomain
            //Same goes for services if you use IServiceLocator from Nuclear.ExportLocator package
            server.LoadAssemblies(AppDomain.CurrentDomain);

            //Add authentication if you have for basic or for token authentication
            //server.AddTokenAuthentication( // your delegate goes here );
            //server.AddBasicAuthentication( // your delegate goes here );

            //or setup server.AuthenticationOption

            //Change the route for your web url
            server.StartHosting("https://localhost:44386");

            return app;
        }
    }
}
