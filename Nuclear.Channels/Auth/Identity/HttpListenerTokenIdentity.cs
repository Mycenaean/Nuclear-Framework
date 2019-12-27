// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Nuclear.Channels.Auth.Identity
{
    /// <summary>
    /// Token Identity provided to the Channel
    /// </summary>
    public class HttpListenerTokenIdentity : GenericIdentity
    {
        public HttpListenerTokenIdentity(string token) :
            base(token, "Bearer")
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}
