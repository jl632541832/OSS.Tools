using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace OSS.Tools.Config
{
    public static class ConfigHelper
    {
        private static IConfiguration config;

        public static IConfiguration Configuration
        {
            get
            {
                if (config != null) return config;

                var basePat = GetBasePath();
                config = new ConfigurationBuilder()
                    .SetBasePath(basePat)
                    .Add(new JsonConfigurationSource
                    {
                        Path = "appsettings.json",
                        ReloadOnChange = true
                    }).Build();
                return config;
            }
            set => config = value;
        }

        private static string GetBasePath()
        {
            var basePat  = Directory.GetCurrentDirectory();
            var sepChar  = Path.DirectorySeparatorChar;
            var binIndex = basePat.IndexOf(string.Concat(sepChar, "bin", sepChar), StringComparison.OrdinalIgnoreCase);
            if (binIndex > 0)
            {
                basePat = basePat.Substring(0, binIndex);
            }

            return basePat;
        }

        /// <summary>
        ///     Gets a configuration sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</returns>
        /// <remarks>
        ///     This method will never return <c>null</c>. If no matching sub-section is found with the specified key,
        ///     an empty <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" /> will be returned.
        /// </remarks>
        public static IConfigurationSection GetSection(string key)
        {
            return Configuration?.GetSection(key);
        }


        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }

        /// <summary>
        ///     Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public static IEnumerable<IConfigurationSection> GetChildren()
        {
            return Configuration?.GetChildren();
        }
    }
}
