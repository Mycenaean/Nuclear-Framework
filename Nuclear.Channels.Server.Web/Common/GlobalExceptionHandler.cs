// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Nuclear.Channels.Server.Web.Common
{
    [Export(typeof(IGlobalExceptionHandler), ExportLifetime.Singleton)]
    public class GlobalExceptionHandler : IGlobalExceptionHandler
    {
        public List<ExceptionInformation> Exceptions { get; private set; }

        public GlobalExceptionHandler()
        {
            Exceptions = new List<ExceptionInformation>();
        }

        public void AddExceptionInformation(Exception exception, string invokationMethod)
        {
            var exceptionInfo = new ExceptionInformation(exception.Message, invokationMethod);
            Exceptions.Add(exceptionInfo);
        }

        public void RemoveExceptionInformation(string invokationMethod)
        {
            Exceptions.Remove(Exceptions.FirstOrDefault(x => x.InvokationMethod == invokationMethod));
        }
    }
}
