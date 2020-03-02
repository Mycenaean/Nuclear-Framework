﻿using Nuclear.Channels.Auth;
using Nuclear.Channels.Base;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    [Channel(Name = "MarkoChannel",Description = "Swagger demo za marka")]
    
    public class TestChannel : ChannelBase
    {
        [ChannelMethod(Description = "Method that will return Hello World string",HttpMethod = ChannelHttpMethod.GET)]
        public string HelloWorld()
        {
            return "HELLO WORLD FROM CHANNEL METHOD";
        }

        [ChannelMethod(HttpMethod = Nuclear.Channels.Enums.ChannelHttpMethod.POST)]
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
