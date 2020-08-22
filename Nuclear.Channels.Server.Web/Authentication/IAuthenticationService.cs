// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Security.Claims;

namespace Nuclear.Channels.Server.Web.Authentication
{
    public interface IAuthenticationService
    {
        Claim[] AuthenticateUser(string username, string password);
    }

}
