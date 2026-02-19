using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TaskManagerDesktop
{
    public static class AppConfiguration
    {
        private static IConfiguration _configuration;

        static AppConfiguration()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            var connectionString = _configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{name}' not found in appsettings.json");
            }

            return connectionString;
        }

        public static string GetSetting(string key)
        {
            return _configuration[key];
        }
    }
}