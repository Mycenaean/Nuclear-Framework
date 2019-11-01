﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace Nuclear.Channels.Auth.Identity
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

        internal static IPrincipal ParseTokenAuthentication(string token)
        {
            token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            return new GenericPrincipal(new HttpListenerTokenIdentity(token), Array.Empty<string>());
        }
    }
}
