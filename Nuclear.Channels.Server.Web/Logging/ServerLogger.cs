// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuclear.Channels.Server.Web.Logging
{
    public class ServerLogger : IChannelLogger
    {
        private string _channel;
        private string _path;
        private JsonSerializerSettings _jsonSerializerSettings;

        public ServerLogger(string channel, string path)
        {
            _channel = channel;
            _path = path;
            InitJsonSettings();
        }

        private void InitJsonSettings()
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        public void LogInfo(string message, [CallerMemberName] string method = null)
        {
            Log(message, "Info", method);
        }

        public void LogDebug(string message, [CallerMemberName] string method = null)
        {
            Log(message, "Debug", method);
        }

        public void LogError(string message, [CallerMemberName] string method = null)
        {
            Log(message, "Error", method);
        }

        public void LogInfo(object response, [CallerMemberName] string method = null)
        {
            var message = JsonConvert.SerializeObject(response, Formatting.Indented, _jsonSerializerSettings);
            Log(message, "Info", method);
        }
        public void LogDebug(object response, [CallerMemberName] string method = null)
        {
            var message = JsonConvert.SerializeObject(response, Formatting.Indented, _jsonSerializerSettings);
            Log(message, "Debug", method);
        }
        public void LogError(object response, [CallerMemberName] string method = null)
        {
            var message = JsonConvert.SerializeObject(response, Formatting.Indented, _jsonSerializerSettings);
            Log(message, "Error", method);
        }

        private void Log(string message, string logLevel, string method)
        {
            string path = $"{_path}\\ServerLogs\\{DateTime.Now:yyyy.MM.dd}_ServerLogs.txt";

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(path, append: true))
                    {
                        string Line = $"{DateTime.Now}::::{logLevel}::::{_channel}::::Method {method}::::{message}";
                        writer.WriteLine(Line);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
    }
}
