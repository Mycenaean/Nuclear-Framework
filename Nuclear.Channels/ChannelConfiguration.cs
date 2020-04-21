using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Decorators;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace Nuclear.Channels
{
    [Export(typeof(IChannelConfiguration), ExportLifetime.Transient)]
    internal class ChannelConfiguration : IChannelConfiguration
    {
        public ChannelConfigurationInfo Configure(HttpListener httpChannel, Type channel, MethodInfo method, string baseURL)
        {
            ChannelEndpoint endpoint = new ChannelEndpoint();
            ChannelAttribute channelAttr = channel.GetCustomAttribute<ChannelAttribute>();
            if (!String.IsNullOrEmpty(channelAttr.Name))
                endpoint.URL = "/channels/" + channelAttr.Name + "/" + method.Name + "/";
            else
                endpoint.URL = "/channels/" + channel.Name + "/" + method.Name + "/";

            endpoint.Name = channel.Name + "." + method.Name;

            ChannelMethodAttribute channelMethod = method.GetCustomAttribute<ChannelMethodAttribute>();
            AuthorizeChannelAttribute authAttr = channel.GetCustomAttribute<AuthorizeChannelAttribute>();
            ChannelAuthenticationSchemes channelSchema = channelMethod.Schema;
            ChannelHttpMethod httpMethod = channelMethod.HttpMethod;

            string methodURL = string.Empty;
            if (baseURL == null)
                methodURL = "http://localhost:4200" + endpoint.URL;
            else
                methodURL = baseURL + endpoint.URL;

            bool httpAuthRequired = false;

            //Add prefixes for hosting
            httpChannel.Prefixes.Add(methodURL);
            if (authAttr != null)
            {
                if (authAttr.Schema != ChannelAuthenticationSchemes.Anonymous)
                {
                    httpAuthRequired = true;
                }
            }
            //ChannelMethod can override ChannelAttribute Authentication Schemes
            if (channelSchema != ChannelAuthenticationSchemes.Anonymous)
            {
                httpAuthRequired = true;
            }
            else
            {
                if (authAttr != null)
                    channelSchema = authAttr.Schema;
            }

            return new ChannelConfigurationInfo
            {
                ChannelAttribute = channelAttr,
                MethodAttribute = channelMethod,
                MethodUrl = methodURL,
                HttpMethod = httpMethod,
                AuthScheme = channelSchema,
                AuthorizeAttribute = authAttr,
                AuthenticationRequired = httpAuthRequired,
                Endpoint = endpoint
            };
        }
    }
}
