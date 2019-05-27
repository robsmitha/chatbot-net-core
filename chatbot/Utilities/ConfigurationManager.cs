using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace chatbot.Utilities
{
    static class ConfigurationManager
    {
        public static string ConfigurationsKey = "Configurations";
        public static string StringsKey = "Strings";
        public static IConfiguration AppSetting { get; }
        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

        }
        public static string GetConfiguration(string key)
        {
            return !string.IsNullOrWhiteSpace(AppSetting[$"{ConfigurationsKey}:{key}"])
                    ? AppSetting[$"{ConfigurationsKey}:{key}"]
                    : string.Empty;
        }
        public static string GetString(string key)
        {
            return !string.IsNullOrWhiteSpace(AppSetting[$"{StringsKey}:{key}"])
                    ? AppSetting[$"{StringsKey}:{key}"]
                    : string.Empty;
        }
    }
}
