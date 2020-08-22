// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Settings
{
    public class UserSettings
    {
        public string Username { get; }
        public string Password { get; }
        public string Role { get; }

        public UserSettings(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

    }
}
