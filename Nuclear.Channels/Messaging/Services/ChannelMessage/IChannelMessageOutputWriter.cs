using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Messaging.Services.ChannelMessage
{
    public interface IChannelMessageOutputWriter
    {
        /// <summary>
        /// Invoking Message service that will write output right away and jump over the chain of execution
        /// </summary>
        /// <param name="message">Your custom ChannelMessage</param>
        /// <param name="response">HttpListenerResponse which you can get from Context (also located in ChannelBase like IChannelMessageOutputWriter).</param>
        void Write(IChannelMessage message, HttpListenerResponse response);

        /// <summary>
        /// EventHandler responsible for jumping the chain of execution. Try not to use it since its meant only for internal purposes.
        /// </summary>
        event EventHandler OnPostMessageServiceInvoked;
    }
}
