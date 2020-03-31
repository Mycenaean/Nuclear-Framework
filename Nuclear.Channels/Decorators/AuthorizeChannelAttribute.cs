// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using System;

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
        /// Claim to base Authorization on
        /// </summary>
        public string ClaimName { get; set; }

        /// <summary>
        /// Claim value
        /// </summary>
        public string ClaimValue { get; set; }

        /// <summary>
        /// Get the Auth type for the channel
        /// </summary>
        /// <param name="schemes">Specified Auth Schemes</param>
        /// <param name="Claim">Claim used for authorization</param>
        public AuthorizeChannelAttribute(ChannelAuthenticationSchemes schemes, string Claim = null, string claimValue = null)
        {
            Schema = schemes;
            ClaimName = Claim;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Set authorization based on claim
        /// </summary>
        /// <param name="Claim">Claim used for authorization</param>
        public AuthorizeChannelAttribute(string Claim, string claimValue)
        {
            ClaimName = Claim;
            ClaimValue = claimValue;
        }
    }
}
