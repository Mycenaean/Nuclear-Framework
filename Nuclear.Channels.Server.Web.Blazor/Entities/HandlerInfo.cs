using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nuclear.Channels.Server.Web.Blazor.Entities
{
    public class HandlerInfo
    {
        public string HandlerId { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
        public bool Protected { get; set; }
        public List<string> History { get; set; }
    }
}
