// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.Channels.Server.Web.System;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Nuclear.Channels.Server.Web.Settings
{
    [Export(typeof(IWebServerSettings), ExportLifetime.Singleton)]
    public class WebServerSettings : IWebServerSettings
    {
        private readonly string _settingsFileName = "webserver.xml";
        private readonly string _channelsNode = "plugin";
        private List<UserSettings> _userSettings;

        public IEnumerable<UserSettings> Users => _userSettings;

        public WebServerSettings()
        {
            _userSettings = new List<UserSettings>();
            InitSettings();
        }

        private void InitSettings()
        {
            var baseDir = SystemSettingsFactory.GetSettings().BaseDirectory;
            var webSettings = XDocument.Load(Path.Combine(baseDir, _settingsFileName));

            var users = webSettings.Descendants("authentication").Descendants("users").Elements("user");
            ConfigureUsers(users);
        }

        private void ConfigureUsers(IEnumerable<XElement> users)
        {
            foreach(var user in users)
            {
                var name = user.Attribute("username").Value;
                var password = user.Attribute("password").Value;
                var role = CapitalizeRoleString(user.Attribute("role").Value);
                var userSettings = new UserSettings(name, password, role);
                _userSettings.Add(userSettings);
            }
        }

        private string CapitalizeRoleString(string roleString)
        {
            var capitalLetter = roleString.Substring(0, 1);
            capitalLetter = capitalLetter.ToUpper();
            return $"{capitalLetter}{roleString[1..]}";
        }
    }
}
