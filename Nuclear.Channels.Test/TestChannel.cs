using Nuclear.Channels;
using Nuclear.Channels.Base.Enums;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Base.Decorators;
using Nuclear.Channels.Messaging;
using Nuclear.ExportLocator.Decorators;
using System;
using Nuclear.Channels.Authentication;
using Nuclear.Channels.Heuristics;

namespace ConsoleApp1
{
    [Channel]
    // [AuthorizeChannel(ChannelAuthenticationSchemes.Token, "Role", "Admin")]
    public class TestChannel : ChannelBase
    {
        [ImportedService]
        public ITestService Service { get; set; }

        [ChannelMethod]
        [EnableCache(20, CacheDurationUnit.Seconds)]
        public string HelloWorld()
        {
            return Service.Write();
        }

        [ChannelMethod]
        [EnableCache(1, CacheDurationUnit.Minutes)]
        public string PostParams(string name)
        {
            return $"Hello {name} from ChannelMethod";
        }

        [ChannelMethod]
        public void WriteStringWithMessageWriter(string message)
        {
            ChannelMessageWriter.Write(new ChannelMessage()
            {
                Success = true,
                Output = message,
                Message = message
            }, Context.Response);
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

            ChannelMessageWriter.Write(msg, Context.Response);
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
