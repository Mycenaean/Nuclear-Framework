// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nuclear.Channels
{
    [Export(typeof(ISessionService), ExportLifetime.Singleton)]
    internal class SessionService : ISessionService
    {
        public static List<Cookie> _cookies;

        public SessionService()
        {
            _cookies = new List<Cookie>();
        }

        public void Add(Cookie cookie)
        {
            _cookies.Add(cookie);
        }

        public bool Exists(Cookie cookie)
        {
            return _cookies.Any(x => x == cookie);
        }

        public void Remove(Cookie cookie)
        {
            _cookies.Remove(cookie);
        }
    }

}
