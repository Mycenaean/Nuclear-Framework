// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;

[assembly: InternalsVisibleTo("Nuclear.Channels.UnitTestss")]
namespace Nuclear.Channels.Authentication.Identity
{
    internal static class HttpListenerIdentityMiddleware
    {
        internal static bool IsTokenHeader(HttpListenerRequest request, out string token)
        {
            string tokenHeader = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(tokenHeader))
            {
                token = string.Empty;
                return false;
            }

            string[] tokenBearer = tokenHeader.Split(' ');
            if (tokenBearer[0].Equals("bearer", StringComparison.OrdinalIgnoreCase))
            {
                token = tokenBearer[1];
                return true;
            }
            else
            {
                token = string.Empty;
                return false;
            }
        }

        internal static bool IsBasicHeader(HttpListenerRequest request, out string username, out string password)
        {
            string basicIdentity = request.Headers["Authorization"];
            if (!String.IsNullOrEmpty(basicIdentity))
            {
                string[] AuthenticationHeader = basicIdentity.Split(' ');
                string usernamePasswordDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(AuthenticationHeader[1]));
                string[] usernamePassword = usernamePasswordDecoded.Split(':');
                username = usernamePassword[0];
                password = usernamePassword[1];
                return true;
            }
            username = string.Empty;
            password = string.Empty;
            return false;
        }

        internal static IPrincipal ParseBasicAuthentication(string username, string password)
        {
            HttpListenerBasicIdentity identity = new HttpListenerBasicIdentity(username, password);
            return new GenericPrincipal(identity, Array.Empty<string>());
        }

        internal static IPrincipal ParseTokenAuthentication(string token)
        {
            token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            return new GenericPrincipal(new HttpListenerTokenIdentity(token), Array.Empty<string>());
        }
    }
}
