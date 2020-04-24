// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Authentication;
using Nuclear.Channels.Base.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace Nuclear.Channels.Decorators
{
    /// <summary>
    /// Attribute that will targeted method initialize as an Http Endpoint
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ChannelMethodAttribute : Attribute
    {
        /// <summary>
        /// Auth Schema
        /// </summary>
        public ChannelAuthenticationSchemes Schema { get; set; }

        /// <summary>
        /// Http Method to be used
        /// </summary>
        public ChannelHttpMethod HttpMethod { get; set; }

        /// <summary>
        /// Description of the method
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// No Auth CTOR
        /// </summary>

        public ChannelMethodAttribute()
        {
            Schema = ChannelAuthenticationSchemes.Anonymous;
            HttpMethod = ChannelHttpMethod.Unknown;
        }

        public ChannelMethodAttribute(string Description)
        {
            this.Description = Description;
        }

        /// <summary>
        /// Http Method to be used
        /// </summary>
        /// <param name="HttpMethod">Http Method</param>
        /// <param name="Description">Description to be used in documentation tool</param>        
        public ChannelMethodAttribute(ChannelHttpMethod HttpMethod, string Description = null)
        {
            this.HttpMethod = HttpMethod;
            Schema = ChannelAuthenticationSchemes.Anonymous;
        }

        /// <summary>
        /// Auth Type
        /// </summary>
        /// <param name="Schemes">Specified Auth Type for ChannelMethod</param>
        /// <param name="Description">Description to be used in documentation tool</param>        
        public ChannelMethodAttribute(ChannelAuthenticationSchemes Schemes, string Description = null)
        {
            Schema = Schemes;
            HttpMethod = ChannelHttpMethod.Unknown;
            this.Description = Description;
        }

        /// <summary>
        /// Method to be used and Auth Type
        /// </summary>
        /// <param name="Schemes">Specified Auth Type for ChannelMethod</param>
        /// <param name="HttpMethod">Http Method</param>
        /// <param name="Description">Description to be used in documentation tool</param>        
        public ChannelMethodAttribute(ChannelAuthenticationSchemes Schemes, ChannelHttpMethod HttpMethod, string Description = null)
        {
            Schema = Schemes;
            this.HttpMethod = HttpMethod;
            this.Description = Description;
        }
    }

    internal class ChannelMethodReflector
    {
        public static MethodInfo[] GetChannelMethods(Type channel)
        {
            return channel.GetMethods().Where(x => x.GetCustomAttribute<ChannelMethodAttribute>() != null).ToArray();
        }
    }
}
