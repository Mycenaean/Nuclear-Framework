// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Authentication
{
    public class ChannelAuthenticationContext
    {
        public HttpListenerContext Context { get; set; }
        public ChannelAuthenticationSchemes Scheme { get; set; }
        public Func<string, string, bool> BasicAuthenticationDelegate { get; set; }
        public Func<string, bool> TokenAuthenticationDelegate { get; set; }
        public AuthenticationSettings AuthenticationSettings { get; set; }
    }
}

