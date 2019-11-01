using Nuclear.Channels.Base;
using Nuclear.Channels.Decorators;
using Nuclear.Channels.Messaging;

namespace Nuclear.Channels.Test
{
    [Channel]
    public class TestChannel : ChannelBase
    {
        [ChannelMethod(Auth.ChannelAuthenticationSchemes.Token)]
        public string HelloWorld()
        {
            return "Hello World from ChannelMethod";
        }

        [ChannelMethod]
        public string Hello(string name)
        {
            return $"Hello {name} from Hello ChannelMethod";
        }

    }
}
