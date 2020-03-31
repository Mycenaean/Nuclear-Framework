// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication.Decorators;
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
                object[] args = new object[] { username, password };

                Type trueDelegate = basicDelegate.GetType();
                object delegateResult = trueDelegate.InvokeMember(basicDelegate.Name, BindingFlags.InvokeMethod, null, _context.AuthenticationSettings, args, null);

                delegateResponse = ResolveReturnType(basicDelegate.Name, delegateResult);
            }
            else
            {
                IPrincipal principal = GetTokenPrincipal(out string token);
                if (principal == null)
                    throw new ChannelCredentialsException("Missing or malformed token authentication header");

                PropertyInfo tokenDelegate = GetNotNullDelegate();
                object[] args = new object[] { token };
                Type trueDelegate = tokenDelegate.GetType();
                object delegateResult = trueDelegate.InvokeMember(tokenDelegate.Name, BindingFlags.InvokeMethod, null, _context.AuthenticationSettings, args, null);
                delegateResponse = ResolveReturnType(tokenDelegate.Name, delegateResult);
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

        private object ResolveReturnType(string name , object result)
        {
            if (name.Contains("principal", StringComparison.OrdinalIgnoreCase))
                return (ClaimsPrincipal)result;
            else if (name.Contains("claim", StringComparison.OrdinalIgnoreCase))
                return (Claim[])result;
            else
                return (bool)result;
        }

        


    }
}
