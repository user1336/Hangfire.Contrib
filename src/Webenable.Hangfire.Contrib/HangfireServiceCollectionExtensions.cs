﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Webenable.Hangfire.Contrib.Internal;

namespace Webenable.Hangfire.Contrib
{
    /// <summary>
    /// Extensions for Hangfire contrib.
    /// </summary>
    public static class HangfireServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Hangfire contrib extensions.
        /// </summary>
        public static IServiceCollection AddHangfireContrib(this IServiceCollection services) => AddHangfireContrib(services, configAction: null);

        /// <summary>
        /// Adds Hangfire contrib extensions and configures Hangfire with the specified <see cref="IGlobalConfiguration"/> action.
        /// </summary>
        public static IServiceCollection AddHangfireContrib(this IServiceCollection services, Action<IGlobalConfiguration> configAction)
        {
            services.AddHangfire(c =>
            {
                c.UseConsole();
                configAction?.Invoke(c);
            });

            services.Configure<HangfireContribOptions>(o =>
            {
                o.EnableServer = true;
                o.Dasbhoard.Enabled = true;
                o.ScanningAssemblies = new[] { Assembly.GetEntryAssembly() };
            });

            services.AddSingleton<ILoggerProvider, HangfireLoggerProvider>();
            services.AddSingleton<IStartupFilter, HangfireContribStartupFilter>();

            return services;
        }
    }
}
