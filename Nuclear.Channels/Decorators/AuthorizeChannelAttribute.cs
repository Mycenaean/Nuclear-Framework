// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Auth;
using System;
using System.Net;

namespace Nuclear.Channels.Decorators
{
    /// <summary>
    /// Attribute that will require Authorization for specified Channel
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AuthorizeChannelAttribute : Attribute
    {
        /// <summary>
        /// Auth Schema
        /// </summary>
        public ChannelAuthenticationSchemes Schema { get; set; }

        /// <summary>
        /// Get the Auth type for the channel
        /// </summary>
        /// <param name="schemes">Specified Auth Schemes</param>
        public AuthorizeChannelAttribute(ChannelAuthenticationSchemes schemes)
        {
            Schema = schemes;
        }
    }
}
