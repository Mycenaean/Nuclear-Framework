using Nuclear.Channels.Auth;
using Nuclear.Channels.Base;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using Nuclear.Channels.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    [Channel(EnableSessions = true)]
    [AuthorizeChannel(ChannelAuthenticationSchemes.Token)]
    public class TestChannel : ChannelBase
    {
        [ChannelMethod]
        public string HelloWorld()
        {
            return "HELLO WORLD FROM CHANNEL METHOD";
        }

        [ChannelMethod(HttpMethod = ChannelHttpMethod.POST)]
        public string PostParams(string name)
        {
            return $"Hello {name} from ChannelMethod";
        }

        [ChannelMethod(ChannelHttpMethod.POST)]
        public void CreateTestClass(string id, string name)
        {
            ChannelMessage msg = new ChannelMessage()
            {
                Message = string.Empty,
                Output = new TestClass() { Id = id, Name = name },
                Success = true
            };

            ChannelMessageWriter.Write(msg, Context.ChannelMethodResponse);
        }
    }


    public class TestClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
