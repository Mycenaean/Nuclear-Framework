using Nuclear.Channels.Auth;
using Nuclear.Channels.Base;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    [Channel]
    [AuthorizeChannel(ChannelAuthenticationSchemes.Token)]

    public class TestChannel : ChannelBase
    {
        [ChannelMethod(HttpMethod = ChannelHttpMethod.GET)]
        public string HelloWorld()
        {
            return "HELLO WORLD FROM CHANNEL METHOD";
        }

        [ChannelMethod(HttpMethod = ChannelHttpMethod.POST)]
        public string PostParams(string name)
        {
            return $"Hello {name} from ChannelMethod";
        }

        [ChannelMethod]
        public TestClass CreateTestClass(string id, string name)
        {
            return new TestClass { Id = id, Name = name };
        }
    }


    public class TestClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
