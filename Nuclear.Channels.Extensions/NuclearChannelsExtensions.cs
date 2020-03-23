using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuclear.Channels.Extensions
{
    public static class NuclearChannelsExtensions
    {
        public static IChannelServer _server = ChannelServerBuilder.CreateServer();
        public static Func<string, bool> _tokenValidationDelegate;
        public static Func<string, string, bool> _basicValidationDelegate;
        public static string _baseUrl = "http://localhost:4200";
        public static AppDomain _appDomain;

        public static IApplicationBuilder UseNuclearChannels(this IApplicationBuilder app)
        {
            _server.LoadAssemblies(_appDomain);
            if (_tokenValidationDelegate != null)
                _server.AuthenticationOptions(_tokenValidationDelegate);
            if (_basicValidationDelegate != null)
                _server.AuthenticationOptions(_basicValidationDelegate);

            _server.StartHosting(_baseUrl);
            return app;
        }

        public static IApplicationBuilder UseNuclearChannels(this IApplicationBuilder app, string baseUrl, string[] assemblies = null, Func<string, bool> tokenValidator = null, Func<string, string, bool> basicAuthValidator = null)
        {
            if (!String.IsNullOrEmpty(baseUrl))
                _baseUrl = baseUrl;
            _tokenValidationDelegate = tokenValidator;
            _basicValidationDelegate = basicAuthValidator;
            
            if (assemblies != null)
            {
                return app.UseNuclearChannels(assemblies);
            }
            else
                return app.UseNuclearChannels();

        }

        public static IApplicationBuilder UseNuclearChannels(this IApplicationBuilder app, string baseUrl, AppDomain domain = null, Func<string, bool> tokenValidator = null, Func<string, string, bool> basicAuthValidator = null)
        {
            if (!String.IsNullOrEmpty(baseUrl))
                _baseUrl = baseUrl;
            if (domain != null)
                _appDomain = domain;

            _tokenValidationDelegate = tokenValidator;
            _basicValidationDelegate = basicAuthValidator;

            return app.UseNuclearChannels();

        }

        private static IApplicationBuilder UseNuclearChannels(this IApplicationBuilder app, string[] assemblies)
        {
            for (int i = 0; i < assemblies.Count(); i++)
                AppDomain.CurrentDomain.Load(assemblies[i]);

            return app.UseNuclearChannels();
        }



    }
}
