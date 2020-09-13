// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Data.Logging;
using Nuclear.Channels.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;

namespace Nuclear.Channels
{
    internal class ChannelAuthenticationInspector
    {
        private readonly Func<string, string, bool> _basicAuthenticationMethod;
        private readonly Func<string, bool> _tokenAuthenticationMethod;
        private readonly AuthenticationSettings _settings;
        private readonly IChannelAuthenticationService _authenticationService;
        private readonly IChannelMessageService _msgService;
        private readonly ISessionService _session;

        public ChannelAuthenticationInspector(IChannelAuthenticationService authService,
            IChannelMessageService msgService,
            AuthenticationSettings settings,
            ISessionService session,
            Func<string, string, bool> basicAuthMethod,
            Func<string, bool> tokenAuthMethod)
        {
            _authenticationService = authService;
            _msgService = msgService;
            _session = session;
            _settings = settings;
            _basicAuthenticationMethod = basicAuthMethod;
            _tokenAuthenticationMethod = tokenAuthMethod;
        }

        internal bool ValidSession(HttpListenerRequest request)
        {
            Cookie sessionCookie = request.Cookies["channelAuthCookie"];
            if (sessionCookie == null)
                return false;
            bool validSessionKey = _session.Exists(sessionCookie);
            if (validSessionKey && !sessionCookie.Expired)
                return true;
            else if (validSessionKey && sessionCookie.Expired)
            {
                _session.Remove(sessionCookie);
                return false;
            }
            else
                return false;

        }

        internal bool AuthenticationFailedIfRequired(HttpListenerContext context, HttpListenerRequest request, HttpListenerResponse response, ChannelConfigurationInfo channelConfig, out bool authenticated)
        {
            bool failed = false;
            bool validCookie = false;
            authenticated = false;
            bool authorized = false;
            if (channelConfig.ChannelAttribute.EnableSessions)
                validCookie = ValidSession(request);
            if (channelConfig.AuthenticationRequired && !validCookie)
            {
                try
                {
                    ChannelAuthenticationContext authContext = new ChannelAuthenticationContext
                    {
                        Context = context,
                        Scheme = channelConfig.AuthScheme,
                        BasicAuthenticationDelegate = _basicAuthenticationMethod,
                        TokenAuthenticationDelegate = _tokenAuthenticationMethod,
                        AuthenticationSettings = _settings

                    };

                    KeyValuePair<bool, object> authenticationResult = _authenticationService.CheckAuthenticationAndGetResponseObject(authContext);
                    if (authenticationResult.Key == true)
                        authenticated = true;
                    else
                    {
                        _msgService.FailedAuthenticationResponse(channelConfig.AuthScheme, response);
                        return true;                        
                    }
                    LogChannel.Write(LogSeverity.Info, "User Authenticated");
                    string claimName = channelConfig.AuthorizeAttribute.ClaimName;
                    string claimValue = channelConfig.AuthorizeAttribute.ClaimValue;
                    if (!String.IsNullOrEmpty(claimName) && !String.IsNullOrEmpty(claimValue))
                    {
                        if (authenticationResult.Value.GetType() == typeof(ClaimsPrincipal))
                            authorized = _authenticationService.Authorized(claimName, claimValue, (ClaimsPrincipal)authenticationResult.Value);
                        else
                            authorized = _authenticationService.Authorized(claimName, claimValue, (Claim[])authenticationResult.Value);

                        if (!authorized)
                        {
                            _msgService.FailedAuthorizationResponse(response);
                            LogChannel.Write(LogSeverity.Error, "Failed authorization");
                            failed = true;
                        }
                        else
                            LogChannel.Write(LogSeverity.Info, "User Authorized");
                    }
                }
                catch (Exception ex)
                {
                    using (StreamWriter writer = new StreamWriter(response.OutputStream))
                    {
                        _msgService.ExceptionHandler(writer, ex, response);
                        LogChannel.Write(LogSeverity.Error, "Authentication Failed");
                        failed = true;
                    }
                }
                if (!authenticated)
                    failed = true;
                else
                {
                    if (channelConfig.ChannelAttribute.EnableSessions)
                    {
                        string sessionKey = Guid.NewGuid().ToString();
                        Cookie sessionCookie = new Cookie()
                        {
                            Expires = DateTime.Now.AddMinutes(30),
                            Name = "channelAuthCookie",
                            Secure = true,
                            Value = sessionKey
                        };
                        response.SetCookie(sessionCookie);
                        _session.Add(sessionCookie);
                    }
                }
            }

            return failed;
        }
    }
}
