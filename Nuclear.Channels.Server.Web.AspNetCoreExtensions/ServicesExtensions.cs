// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuclear.Channels.Remoting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Server.Web.AspNetCoreExtensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection ConfigureRemoteChannelEndpoints(this IServiceCollection services)
        {
            return services.AddSingleton<RemoteChannelEndpoints>();
        }

        public static IServiceCollection AddChannelRemotingClient(this IServiceCollection services)
        {
            return services.AddTransient<IChannelRemotingClient, ChannelRemotingClient>();
        }

        public static IServiceCollection AddChannelEndpointProvider(this IServiceCollection services)
        {
           return services.AddTransient<IChannelRemotingClient, ChannelRemotingClient>();
        }
    }
}
