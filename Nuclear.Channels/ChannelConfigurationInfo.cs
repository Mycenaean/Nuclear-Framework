// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base.Contracts;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Decorators;

namespace Nuclear.Channels
{
    internal class ChannelConfigurationInfo
    {
        public ChannelAttribute ChannelAttribute { get; set; }
        public ChannelMethodAttribute MethodAttribute { get; set; }
        public AuthorizeChannelAttribute AuthorizeAttribute { get; set; }
        public ChannelAuthenticationSchemes AuthScheme { get; set; }
        public ChannelHttpMethod HttpMethod { get; set; }
        public ChannelEndpoint Endpoint { get; set; }
        public string MethodUrl { get; set; }
        public bool AuthenticationRequired { get; set; }
    }
}
