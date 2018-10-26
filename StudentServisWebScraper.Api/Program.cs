using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.Globalization;
using NLog.Web;
using NLog;

namespace StudentServisWebScraper.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo hrCulture = new CultureInfo("hr-HR");
            Thread.CurrentThread.CurrentCulture = hrCulture;
            Thread.CurrentThread.CurrentUICulture = hrCulture;
            CultureInfo.DefaultThreadCurrentCulture = hrCulture;
            CultureInfo.DefaultThreadCurrentUICulture = hrCulture;

            Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                BuildWebHost(args).Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseNLog()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();
    }
}