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
