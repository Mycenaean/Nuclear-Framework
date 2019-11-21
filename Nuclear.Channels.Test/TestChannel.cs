using Nuclear.Channels.Auth;
using Nuclear.Channels.Base;
using Nuclear.Channels.Decorators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    [Channel(EnableSessions = true)]
    [AuthorizeChannel(ChannelAuthenticationSchemes.Basic)]
    public class TestChannel : ChannelBase
    {
        [ChannelMethod]
        public string HelloWorld()
        {
            return "HELLO WORLD FROM CHANNEL METHOD";
        }

        [ChannelMethod]
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
