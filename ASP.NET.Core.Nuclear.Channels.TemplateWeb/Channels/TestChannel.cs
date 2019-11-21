using Nuclear.Channels.Auth;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET.Core.Nuclear.Channels.TemplateWeb.Channels
{
    [Channel(Description = "Put your description here" , EnableSessions = true)]
    public class TestChannel
    {
        [ChannelMethod(Description = "Method that returns string",Schema = ChannelAuthenticationSchemes.Basic)]
        public string HelloWorld()
        {
            return "Hello World from ChannelMethod!!!";
        }

        [ChannelMethod(HttpMethod = ChannelHttpMethod.POST, Description = "Method that takes one parameter and returns it with hello world string")]
        public string Hello(string name)
        {
            return $"Hello {name} from Hello ChannelMethod";
        }
    }
}
