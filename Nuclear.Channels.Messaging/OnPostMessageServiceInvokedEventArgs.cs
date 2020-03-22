// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;

namespace Nuclear.Channels.Messaging
{
    public class OnPostMessageServiceInvokedEventArgs : EventArgs
    {
        public object ChannelReturnValue { get; set; }
    }
}
