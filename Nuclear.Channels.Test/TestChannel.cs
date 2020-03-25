using Nuclear.Channels;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Generators;
using Nuclear.Channels.Messaging;
using Nuclear.ExportLocator.Decorators;
using System;

namespace ConsoleApp1
{
    [Channel]
    public class TestChannel : ChannelBase
    {
        [ImportedService]
        public ITestService Service { get; set; }

        [ChannelMethod]
        public string HelloWorld()
        {
            return Service.Write();
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

        [ChannelMethod]
        public void RedirectionTest(string url)
        {
            RedirectToUrl(url);
        }
    }

    public interface ITestService { string Write(); }
    
    [Export(typeof(ITestService))]
    public class TestService : ITestService
    {
        public string Write()
        {
            return "HELLO WORLD FROM CHANNEL METHOD";
        }
    }
    public class TestClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
