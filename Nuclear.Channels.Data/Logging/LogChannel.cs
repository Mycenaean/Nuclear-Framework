// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Nuclear.Channels.Data.Logging
{
    /// <summary>
    /// Logger class
    /// </summary>
    public class LogChannel
    {
        //Base path for .NET Core application 
        private static string RootPath = (new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.Parent.FullName;

        //Undependant on DateTime formatter on choosen PC
        private static string DateString = DateTime.Now.ToString("yyyy.MM.dd");



        /// <summary>
        /// Method that writes the log
        /// </summary>
        /// <param name="severity">Severity Level</param>
        /// <param name="message">Message to write</param>
        /// <param name="method">Method in which logging occured AUTO-GENERATED</param>
        [DebuggerStepThrough]
        [SuppressMessage("Style", "IDE0059", Justification = "Not needed")]
        [SuppressMessage("Variable.Not.Used", "CS0168", Justification = "Not needed")]
        public static void Write(LogSeverity severity, string message, [CallerMemberName]string method = "")
        {
            string path = $"{RootPath}\\Logs\\{DateString}_LogChannel.txt";

            for (int i = 0; i < 1; i++)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(path, append: true))
                    {
                        string Line = $"{DateTime.Now}::::{severity.ToString()}::::Method {method}::::{message}";
                        writer.WriteLine(Line);
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                }
            }
        }

    }
}
