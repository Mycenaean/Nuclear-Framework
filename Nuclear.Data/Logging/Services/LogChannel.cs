using Nuclear.Data.Logging.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Nuclear.Data.Logging.Services
{
    /// <summary>
    /// Logger class
    /// </summary>
    public class LogChannel
    {
        //Base path for .NET Core application since KernelHost is running on core
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

        /// <summary>
        /// Method that writes the log for ChannelInterface
        /// </summary>
        /// <param name="severity">Severity Level</param>
        /// <param name="message">Message to write</param>
        /// <param name="method">Method in which logging occured AUTO-GENERATED</param>
        [DebuggerStepThrough]
        public static void Interface(LogSeverity severity, string message, [CallerMemberName]string method = "")
        {
            string path = $"{(new FileInfo(AppDomain.CurrentDomain.BaseDirectory)).Directory.Parent.Parent.FullName}\\Logs\\{DateString}_ChInterfaceLog.txt";
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

        /// <summary>
        /// Method for writing Log info regarding the benchmarks
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="method">Method in which logging occured AUTO-GENERATED</param>
        [DebuggerStepThrough]
        public static void BenchmarkWrite(string message, [CallerMemberName]string method = "")
        {
            string path = $"{RootPath}\\Logs\\{DateString}_BenchmarkLogChannel.txt";
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(path, append: true))
                    {
                        string Line = $"{DateTime.Now}::::{LogSeverity.Debug.ToString()}::::Method {method}::::{message}";
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
