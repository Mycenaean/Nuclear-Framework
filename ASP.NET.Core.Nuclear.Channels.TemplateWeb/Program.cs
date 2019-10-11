using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nuclear.Channels;
using Nuclear.Channels.Base;

namespace ASP.NET.Core.Nuclear.Channels.TemplateWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IChannelHost host = ChannelHost.GetHost;
            host.LoadAssemblies(AppDomain.CurrentDomain, null);
            host.StartHosting(null);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
