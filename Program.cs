using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using PhotoWebApi.Helpers;

namespace PhotoWebApi
{
    public class Program
    {
        // Environement string const can be taken from Environment.Development, etc.
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .SetupWebHost(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}
