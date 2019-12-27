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
            //----------------------------------------------------------------------------------------
            //Hosting configuration goes in here
            //If you have your channels in different DLL add that DLL in an array
            //string[] channelAssemblies = new string[] { "Your DLL " };
            //host.LoadAssemblies(AppDomain.CurrentDomain, channelAssemblies);
            //If you dont specify your base address channels will always be located
            //on http://localhost:4200/channels/ ....
            //To change base address add your base address in host.StartHosting("your base address");
            //----------------------------------------------------------------------------------------

            AuthMethods methods = new AuthMethods();
            IChannelServer host = ChannelServerBuilder.CreateServer();
            host.LoadAssemblies(AppDomain.CurrentDomain, null);
            host.AuthenticationOptions(methods.AuthenticateBasic);
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

    public class AuthMethods
    {
        public bool AuthenticateBasic(string username, string password)
        {
            return true;
        }

        public bool AuthenticateToken(string token)
        {
            return true;
        }
    }
}
