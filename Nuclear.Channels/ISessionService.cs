// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System.Net;

namespace Nuclear.Channels
{
    internal interface ISessionService
    {
        void Add(Cookie cookie);
        void Remove(Cookie cookie);
        bool Exists(Cookie cookie);
    }
}
