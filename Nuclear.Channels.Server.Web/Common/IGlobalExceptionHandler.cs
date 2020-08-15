using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuclear.Channels.Server.Web.Common
{
    public interface IGlobalExceptionHandler
    {
        public List<ExceptionInformation> Exceptions { get; }
        void AddExceptionInformation(Exception exception, string invokationMethod);
        void RemoveExceptionInformation(string invokationMethod);

    }
}
