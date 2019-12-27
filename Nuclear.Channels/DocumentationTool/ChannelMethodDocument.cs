// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Auth;
using Nuclear.Channels.Enums;
using System;
using System.Net;

namespace Nuclear.Channels.DocumentationTool
{
    /// <summary>
    /// Object containing all information about ChannelMethod
    /// </summary>
    public class ChannelMethodDocument
    {
        public string URL { get; set; }
        public string Description { get; set; }
        public string[] InputParameters { get; set; }
        public Type[] InputParameterTypes { get; set; }
        public ChannelHttpMethod HttpMethod { get; set; }
        public string ReturnTypeName { get; set; }
        public Type ReturnType { get; set; }
        public ChannelAuthenticationSchemes AuthSchema { get; set; }

    }
}
