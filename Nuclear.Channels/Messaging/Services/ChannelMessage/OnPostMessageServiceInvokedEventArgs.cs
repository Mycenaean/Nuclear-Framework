using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Messaging.Services.ChannelMessage
{
    public class OnPostMessageServiceInvokedEventArgs : EventArgs
    {
        public object ChannelReturnValue { get; set; }
    }
}
