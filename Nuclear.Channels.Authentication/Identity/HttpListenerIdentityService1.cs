// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Nuclear.Channels.Authentication.Identity
{
    public partial class HttpListenerIdentityService
    {
        private ChannelAuthenticationContext _context;

        public HttpListenerIdentityService(ChannelAuthenticationContext context)
        {
            _context = context;
        }

        public bool Authenticated(out object delegateResponse)
        {
            AuthenticationSettings settings = _context.AuthenticationSettings;
            ChannelAuthenticationSchemes scheme = settings.Schema;
            bool authenticated = false;
            delegateResponse = null;

            if (scheme == ChannelAuthenticationSchemes.Basic)
            {
                IPrincipal principal = GetBasicPrincipal(out string username, out string password);
                if (principal == null)
                    throw new ChannelCredentialsException("Missing or malformed basic authentication header");

                PropertyInfo basicDelegate = GetNotNullDelegate();
                delegateResponse = ResolveBasicDelegate(basicDelegate, username, password, out authenticated);
            }
            else
            {
                IPrincipal principal = GetTokenPrincipal(out string token);
                if (principal == null)
                    throw new ChannelCredentialsException("Missing or malformed token authentication header");

                PropertyInfo tokenDelegate = GetNotNullDelegate();
                delegateResponse = ResolveTokenDelegate(tokenDelegate, token, out authenticated);
            }

            return authenticated;
        }

        public IPrincipal GetBasicPrincipal(out string username, out string password)
        {
            bool isBasic = HttpListenerIdentityMiddleware.IsBasicHeader(_context.Context.Request, out username, out password);
            if (isBasic)
                return HttpListenerIdentityMiddleware.ParseBasicAuthentication(username, password);
            else
                return null;
        }

        public IPrincipal GetTokenPrincipal(out string token)
        {
            bool isToken = HttpListenerIdentityMiddleware.IsTokenHeader(_context.Context.Request, out token);
            if (isToken)
                return HttpListenerIdentityMiddleware.ParseTokenAuthentication(token);
            else
                return null;
        }

        private PropertyInfo GetNotNullDelegate()
        {
            string schema = _context.AuthenticationSettings.Schema.ToString();
            PropertyInfo[] properties = typeof(AuthenticationSettings).GetProperties();
            AuthenticationSettings setts = _context.AuthenticationSettings;
            PropertyInfo[] authDelegates = null;

            if (schema.Equals("basic", StringComparison.OrdinalIgnoreCase))
                authDelegates = properties.Where(x => x.Name.Contains("basic", StringComparison.OrdinalIgnoreCase)).ToArray();
            else
                authDelegates = properties.Where(x => x.Name.Contains("token", StringComparison.OrdinalIgnoreCase)).ToArray();

            PropertyInfo dlgt = null;

            foreach (PropertyInfo @delegate in authDelegates)
            {
                if (@delegate.GetValue(setts) != null)
                {
                    dlgt = @delegate;
                    break;
                }
            }

            return dlgt;
        }


        private object ResolveTokenDelegate(PropertyInfo tokenDelegate, string token, out bool authenticated)
        {
            string name = tokenDelegate.Name;
            MethodInfo delegateMethod = tokenDelegate.GetMethod;
            if (name.Contains("principal", StringComparison.OrdinalIgnoreCase))
            {
                Func<string, ClaimsPrincipal> delegateValue = (Func<string, ClaimsPrincipal>)tokenDelegate.GetValue(_context.AuthenticationSettings);
                ClaimsPrincipal principal = delegateValue.Invoke(token);
                if (principal == null)
                    authenticated = false;
                else
                    authenticated = true;
                return principal;
            }
            else if (name.Contains("claim", StringComparison.OrdinalIgnoreCase))
            {
                Func<string, Claim[]> delegateValue = (Func<string, Claim[]>)tokenDelegate.GetValue(_context.AuthenticationSettings);
                Claim[] claims = delegateValue.Invoke(token);
                if (claims == null)
                    authenticated = false;
                else
                    authenticated = true;

                return claims;
            }
            else
            {
                Func<string, bool> delegateValue = (Func<string, bool>)tokenDelegate.GetValue(_context.AuthenticationSettings);
                authenticated = delegateValue.Invoke(token);
                return authenticated;
            }
        }

        private object ResolveBasicDelegate(PropertyInfo basicDelegate, string username, string password, out bool authenticated)
        {
            string name = basicDelegate.Name;
            MethodInfo delegateMethod = basicDelegate.GetMethod;
            if (name.Contains("principal", StringComparison.OrdinalIgnoreCase))
            {
                Func<string, string, ClaimsPrincipal> delegateValue = (Func<string, string, ClaimsPrincipal>)basicDelegate.GetValue(_context.AuthenticationSettings);
                ClaimsPrincipal principal = delegateValue.Invoke(username, password);
                if (principal == null)
                    authenticated = false;
                else
                    authenticated = true;
                return principal;
            }
            else if (name.Contains("claim", StringComparison.OrdinalIgnoreCase))
            {
                Func<string, string, Claim[]> delegateValue = (Func<string, string, Claim[]>)basicDelegate.GetValue(_context.AuthenticationSettings);
                Claim[] claims = delegateValue.Invoke(username, password);
                if (claims == null)
                    authenticated = false;
                else
                    authenticated = true;

                return claims;
            }
            else
            {
                Func<string, string, bool> delegateValue = (Func<string, string, bool>)basicDelegate.GetValue(_context.AuthenticationSettings);
                authenticated = delegateValue.Invoke(username, password);
                return authenticated;
            }
        }


    }
}
