using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NLog.Config;

namespace NLog.Extensions.Logging
{
    /// <summary>
    /// Helpers for Core
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Enable NLog as logging provider in ASP.NET Core.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddNLog(this ILoggerFactory factory)
        {
            return AddNLog(factory, null);
        }

        /// <summary>
        /// Enable NLog as logging provider in ASP.NET Core.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="options">NLog options</param>
        /// <returns></returns>
        public static ILoggerFactory AddNLog(this ILoggerFactory factory, NLogProviderOptions options)
        {
            //ignore this
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));

            try
            {
                //try the Filter ext
                var filterAssembly = Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Filter"));
                LogManager.AddHiddenAssembly(filterAssembly);
            }
            catch (Exception ex)
            {
                //ignore
            }

            using (var provider = new NLogLoggerProvider(options))
            {
                factory.AddProvider(provider);
            }
            return factory;
        }

        /// <summary>
        /// Apply NLog configuration from XML config.
        /// </summary>
        /// <param name="fileName">absolute path  NLog configuration file.</param>
        private static void ConfigureNLog(string fileName)
        {
            LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);
        }
    }
}
