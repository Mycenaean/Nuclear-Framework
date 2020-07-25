// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Manager
{
    public class ServerCommandList 
    {        
        public static string StartChannel = "start channel [channelId]";
        public static string RestartChannel = "restart channel [channelId]";
        public static string StopChannel = "stop channel [channelId]";
        public static string StartChannelMethod = "start channelMethod [channelMethodId]";
        public static string RestartChannelMethod = "restart channelMethod [channelMethodId]";
        public static string StopChannelMethod = "stop channelMethod [channelMethodId]";
        public static string ReadChannelState = "read channel state [channelId]";
        public static string ReadChannelMethodState = "read channelMethod state [channelMethodId]";
        public static string StopProgram = "channel server stop";
        public static string StartProgram = "channel server start";
        public static string ShutDown = "channel server shutdown";
        public static string Help = "channel server help";

    }
}
