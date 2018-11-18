using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Configuration;
using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PhotoWebApi.Helpers
{
    public static class WebHostBuilderExtensions
    {
        private const String WebHostConfigSection = "webHost";

        private static IConfigurationRoot BuildCommonConfiguration(String envName)
        {
            if (String.IsNullOrWhiteSpace(envName))
            {
                throw new ArgumentNullException(nameof(envName), "Argument cannot be null or empty");
            }

            if (!envName.Equals(EnvironmentName.Development) &&
                !envName.Equals(EnvironmentName.Production) &&
                !envName.Equals(EnvironmentName.Staging))
            {
                throw new ArgumentOutOfRangeException(nameof(envName), envName, "Unknown environment name");
            }

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Read 'webHost' section from 'appsettings.[environment.].json' file
        /// </summary>
        /// <param name="config">ConfigurationRoot to read</param>
        /// <returns>ConfigurationRoot containing web host key-valued configuration</returns>
        private static IConfigurationRoot BuildWebHostConfiguration(this IConfigurationRoot config)
        {
            if (!config.GetSection(WebHostConfigSection).Exists())
            {
                throw new ConfigurationErrorsException($"Can't read '{WebHostConfigSection}' configuration section");
            }

            var webApiConfigParams = config.GetSection(WebHostConfigSection).Get<Dictionary<String, String>>();
            if (!webApiConfigParams.Any())
            {
                throw new ConfigurationErrorsException($"'{WebHostConfigSection}' configuration section is empty");
            }

            return new ConfigurationBuilder()
                .AddInMemoryCollection(webApiConfigParams)
                .Build();
        }

        public static IWebHostBuilder SetupWebHost(this IWebHostBuilder hostBuilder, String envName)
        {
            var commonConfig = BuildCommonConfiguration(envName);

            return hostBuilder
                .UseConfiguration(commonConfig)
                .UseConfiguration(commonConfig
                    .BuildWebHostConfiguration());
        }
    }
}
