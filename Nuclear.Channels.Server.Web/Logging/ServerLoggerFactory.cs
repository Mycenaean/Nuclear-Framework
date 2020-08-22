// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.System;
using System.IO;

namespace Nuclear.Channels.Server.Web.Logging
{
    public static class ServerLoggerFactory
    {
        private static string
        _rootPath = SystemSettingsFactory.GetSettings().BaseDirectory;
        private static DirectoryInfo logsDirectory = new DirectoryInfo(Path.Combine(_rootPath, "ServerLogs"));

        public static void Init()
        {
            if (!Directory.Exists(logsDirectory.FullName))
                logsDirectory.Create();
        }

        public static IChannelLogger CreateLogger(string channel)
        {
            return new ServerLogger(channel, _rootPath);
        }
    }
}
