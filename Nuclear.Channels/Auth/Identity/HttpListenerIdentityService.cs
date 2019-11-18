using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTests")]
namespace Nuclear.Channels.Auth.Identity
{
    /// <summary>
    /// Service for HttpListenerRequest identity authentication and authorization
    /// </summary>
    internal class HttpListenerIdentityService
    {
        /// <summary>
        /// Constructor taking delegates that will be used to authenticate and authorize user
        /// </summary>
        /// <param name="BasicAuthenticationMethod">Delegate for basic authentication</param>
        /// <param name="TokenAuthenticationMethod">Delegate for token authentication</param>
        public HttpListenerIdentityService(Func<string, string, bool> BasicAuthenticationMethod, Func<string, bool> TokenAuthenticationMethod)
        {
            this.BasicAuthenticationMethod = BasicAuthenticationMethod;
            this.TokenAuthenticationMethod = TokenAuthenticationMethod;
        }

        internal Func<string, string, bool> BasicAuthenticationMethod { get; set; }
        internal Func<string, bool> TokenAuthenticationMethod { get; set; }

        /// <summary>
        /// Function that will do authentication and authorization
        /// </summary>
        /// <param name="context">Current HttpListenerContext </param>
        /// <param name="Schemes">AuthenticationSchemes</param>
        /// <exception cref="ChannelCredentialsException"></exception>
        /// <returns>True if user is authenticated and authorized , False if not</returns>
        public bool AuthenticatedAndAuthorized(HttpListenerContext context,ChannelAuthenticationSchemes Schemes)
        {
            if(Schemes == ChannelAuthenticationSchemes.Token)
            {
                string token = string.Empty;
                bool isToken = HttpListenerIdentityMiddleware.IsTokenHeader(context.Request, out token);
                if (isToken)
                {
                    IPrincipal tokenIdentity = HttpListenerIdentityMiddleware.ParseTokenAuthentication(token);
                    return AuthenticateRequest(tokenIdentity.Identity, Schemes);
                }
                else
                    throw new ChannelCredentialsException("Malformed or missing header for the token authentication");
            }
            else
            {
                return AuthenticateRequest(context.User.Identity, Schemes);
            }            
        }

        internal bool AuthenticateRequest(IIdentity identity, ChannelAuthenticationSchemes AuthSchema)
        {
            if (identity == null)
            {
                throw new ChannelCredentialsException("Credentials are not provided");
            }
            else
            {
                if (AuthSchema == ChannelAuthenticationSchemes.Basic)
                {
                    KeyValuePair<string, string> userCredentials = GetCredentialsForBasicAuthentication(identity);
                    return BasicAuthenticationMethod.Invoke(userCredentials.Key, userCredentials.Value);
                }
                else
                {
                    string token = GetCredentialsForTokenAuthentication(identity);
                    return TokenAuthenticationMethod.Invoke(token);
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
                throw new ChannelCredentialsException("Authentication required is Basic ");
        }

        internal string GetCredentialsForTokenAuthentication(IIdentity identity)
        {
            if (identity is HttpListenerTokenIdentity)
            {
                HttpListenerTokenIdentity token = identity as HttpListenerTokenIdentity;
                return token.Token;
            }
            else
                throw new ChannelCredentialsException("Authentication required is Token");
        }
    }
}
