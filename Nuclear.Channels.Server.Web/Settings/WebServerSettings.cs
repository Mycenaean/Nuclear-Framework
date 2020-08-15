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
        private List<ChannelSettings> _channelSettings;

        public IEnumerable<UserSettings> Users => _userSettings;
        public IEnumerable<ChannelSettings> Channels => _channelSettings;

        public WebServerSettings()
        {
            _userSettings = new List<UserSettings>();
            _channelSettings = new List<ChannelSettings>();
            InitSettings();
        }

        private void InitSettings()
        {
            var baseDir = SystemSettingsFactory.GetSettings().BaseDirectory;
            var webSettings = XDocument.Load(Path.Combine(baseDir, _settingsFileName));

            var channels = webSettings.Descendants("channels").Elements(_channelsNode).ToList();
            ConfigureChannels(channels);

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

        private void ConfigureChannels(List<XElement> channels)
        {
            channels.ForEach(x =>
            {
                var rolesXml = x.Elements("role").ToList();
                var roles = new List<string>();
                rolesXml.ForEach(roleXml => 
                {
                    var roleString = roleXml.Value;
                    roleString = CapitalizeRoleString(roleString);
                    roles.Add(roleString); 
                });
                var channelSettings = new ChannelSettings(x.Attribute("name").Value, roles.ToArray());
                _channelSettings.Add(channelSettings);
            });
        }

        private string CapitalizeRoleString(string roleString)
        {
            var capitalLetter = roleString.Substring(0, 1);
            capitalLetter = capitalLetter.ToUpper();
            return $"{capitalLetter}{roleString[1..]}";
        }
    }
}
