using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuclear.Channels.Remoting;
using Nuclear.Channels.Server.Web.Blazor.Endpoints;
using Nuclear.Channels.Server.Web.Blazor.Entities;

namespace Nuclear.Channels.Server.Web.Blazor
{
    [ExcludeFromCodeCoverage]
    public static class BlazorChannelServerExtensions
    {
        public static IServiceCollection ConfigureRemoteChannelEndpoints(this IServiceCollection services, IConfiguration configuration)
        {
            RemoteChannelEndpoints remoteEndpoints = new RemoteChannelEndpoints();
            configuration.Bind("RemoteChannelEndpoints", remoteEndpoints);
            services.AddSingleton(remoteEndpoints);

            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, RemoteChannelEndpoints remoteEndpoints)
        {
            services.AddHttpClient("Plugins", http =>
            {
                http.BaseAddress = new Uri(remoteEndpoints.PluginsChannel);
            });

            services.AddHttpClient("Server", http =>
            {
                http.BaseAddress = new Uri(remoteEndpoints.ServerChannel);
            });
            return services;
        }

        public static IServiceCollection ConfigureChannelBasicAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection channelAuth = configuration.GetSection("ChannelAuthUser");
            services.Configure<ChannelAuthUser>(channelAuth);
            return services;
        }

        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddSingleton<IChannelEndpointProvider, ChannelEndpointProvider>();
            services.AddTransient<IChannelRemotingClient, ChannelRemotingClient>();
            return services;
        }

    }
}
