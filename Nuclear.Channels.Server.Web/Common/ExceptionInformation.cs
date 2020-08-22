// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

namespace Nuclear.Channels.Server.Web.Common
{
    public class ExceptionInformation
    {
        public string Message { get; }
        public string InvokationMethod { get; }

        public ExceptionInformation(string message,string invokationMethod)
        {
            Message = message;
            InvokationMethod = invokationMethod;
        }

    }
}
