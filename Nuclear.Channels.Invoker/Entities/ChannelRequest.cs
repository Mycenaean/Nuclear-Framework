using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclear.Channels.Invoker.Entities
{
    /// <summary>
    /// Neccessary fields for Channel request
    /// </summary>
    /// 
    public class ChannelRequest
    {
        /// <summary>
        /// Targeted Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// HttpMethod
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// HttpRequest Content-Type 
        /// </summary>
        public string ContentType {get; set;}

        /// <summary>
        /// Input body
        /// </summary>
        public string InputBody { get; set; }

        /// <summary>
        /// Authentication Type , example : Basic
        /// </summary>
        public string AuthType { get; set; }

        /// <summary>
        /// Username for authentication
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password for authentication
        /// </summary>
        public string Password { get; set; }

    }
}
