using Nuclear.Channels.Enums;
using System;
using System.Net;

namespace Nuclear.Channels.Decorators
{
    /// <summary>
    /// Attribute that will targeted method initialize as an Http Endpoint
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ChannelMethodAttribute : Attribute
    {
        /// <summary>
        /// Auth Schema
        /// </summary>
        public AuthenticationSchemes Schema { get; set; }

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
            Schema = AuthenticationSchemes.Anonymous;
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
        public ChannelMethodAttribute(ChannelHttpMethod HttpMethod, string Description = null)
        {
            this.HttpMethod = HttpMethod;
            Schema = AuthenticationSchemes.Anonymous;
        }

        /// <summary>
        /// Auth Type
        /// </summary>
        /// <param name="Schemes">Specified Auth Type for ChannelMethod</param>        
        public ChannelMethodAttribute(AuthenticationSchemes Schemes, string Description = null)
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
        public ChannelMethodAttribute(AuthenticationSchemes Schemes, ChannelHttpMethod HttpMethod, string Description = null)
        {
            Schema = Schemes;
            this.HttpMethod = HttpMethod;
            this.Description = Description;
        }
    }
}
