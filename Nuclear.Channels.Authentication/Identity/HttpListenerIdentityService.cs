// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Authentication.Identity
{
    /// <summary>
    /// Service for HttpListenerRequest identity Authenticationentication and Authenticationorization
    /// </summary>
    public class HttpListenerIdentityService
    {
        /// <summary>
        /// Constructor taking delegates that will be used to Authenticationenticate and Authenticationorize user
        /// </summary>
        /// <param name="BasicAuthenticationenticationMethod">Delegate for basic Authenticationentication</param>
        /// <param name="TokenAuthenticationenticationMethod">Delegate for token Authenticationentication</param>
        public HttpListenerIdentityService(Func<string, string, bool> BasicAuthenticationenticationMethod, Func<string, bool> TokenAuthenticationenticationMethod)
        {
            this.BasicAuthenticationenticationMethod = BasicAuthenticationenticationMethod;
            this.TokenAuthenticationenticationMethod = TokenAuthenticationenticationMethod;
        }

        internal Func<string, string, bool> BasicAuthenticationenticationMethod { get; set; }
        internal Func<string, bool> TokenAuthenticationenticationMethod { get; set; }

        /// <summary>
        /// Function that will do Authenticationentication and Authenticationorization
        /// </summary>
        /// <param name="context">Current HttpListenerContext </param>
        /// <param name="Schemes">AuthenticationenticationSchemes</param>
        /// <exception cref="ChannelCredentialsException"></exception>
        /// <returns>True if user is Authenticationenticated and Authenticationorized , False if not</returns>
        public bool AuthenticatedAndAuthorized(HttpListenerContext context, ChannelAuthenticationSchemes Schemes)
        {
            if (Schemes == ChannelAuthenticationSchemes.Token)
            {
                string token = string.Empty;
                bool isToken = HttpListenerIdentityMiddleware.IsTokenHeader(context.Request, out token);
                if (isToken)
                {
                    IPrincipal tokenIdentity = HttpListenerIdentityMiddleware.ParseTokenAuthenticationentication(token);
                    return AuthenticateRequest(tokenIdentity.Identity, Schemes);
                }
                else
                    throw new ChannelCredentialsException("Malformed or missing header for the token Authenticationentication");
            }
            else
            {
                string username = string.Empty;
                string password = string.Empty;
                bool isBasic = HttpListenerIdentityMiddleware.IsBasicHeader(context.Request, out username, out password);

                if (isBasic)
                {
                    IPrincipal basicIdentity = HttpListenerIdentityMiddleware.ParseBasicAuthenticationentication(username, password);
                    return AuthenticateRequest(basicIdentity.Identity, ChannelAuthenticationSchemes.Basic);
                }
                throw new ChannelCredentialsException("Malformed or missing header for the basic Authenticationentication");
            }
        }

        internal bool AuthenticateRequest(IIdentity identity, ChannelAuthenticationSchemes AuthenticationSchema)
        {
            if (identity == null)
            {
                throw new ChannelCredentialsException("Credentials are not provided");
            }
            else
            {
                if (AuthenticationSchema == ChannelAuthenticationSchemes.Basic)
                {
                    KeyValuePair<string, string> userCredentials = GetCredentialsForBasicAuthentication(identity);
                    return BasicAuthenticationenticationMethod.Invoke(userCredentials.Key, userCredentials.Value);
                }
                else
                {
                    string token = GetCredentialsForTokenAuthentication(identity);
                    return TokenAuthenticationenticationMethod.Invoke(token);
                }
            }
        }

        internal KeyValuePair<string, string> GetCredentialsForBasicAuthentication(IIdentity identity)
        {
            if (identity is HttpListenerBasicIdentity)
            {
                HttpListenerBasicIdentity basicIdentity = identity as HttpListenerBasicIdentity;
                return new KeyValuePair<string, string>(basicIdentity.Name, basicIdentity.Password);
            }
            else
                throw new ChannelCredentialsException("Authenticationentication required is Basic ");
        }

        internal string GetCredentialsForTokenAuthentication(IIdentity identity)
        {
            if (identity is HttpListenerTokenIdentity)
            {
                HttpListenerTokenIdentity token = identity as HttpListenerTokenIdentity;
                return token.Token;
            }
            else
                throw new ChannelCredentialsException("Authenticationentication required is Token");
        }
    }
}
