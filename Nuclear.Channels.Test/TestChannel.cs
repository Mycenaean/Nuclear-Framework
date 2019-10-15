using Nuclear.Channels.Base;
using Nuclear.Channels.Decorators;

namespace Nuclear.Channels.Test
{
    [Channel]
    public class TestChannel : ChannelBase
    {
        [ChannelMethod(Schema = Auth.ChannelAuthenticationSchemes.Token)]
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
