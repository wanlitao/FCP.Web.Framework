using FCP.Util;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Engine;
using System;

namespace FCP.Web.OwinHost
{
    public static class FCPWebApp
    {
        #region Helper Functions
        private static StartOptions BuildOptions(string url)
        {
            return new StartOptions(url);
        }
        #endregion

        #region Cluster App Start
        public static IDisposable StartClusterApp<TClusterAppStartup>(string url)
            where TClusterAppStartup : FCPClusterAppStartup
        {
            return Start<TClusterAppStartup>(url);
        }

        public static IDisposable StartClusterApp<TClusterAppStartup>(StartOptions options)
            where TClusterAppStartup : FCPClusterAppStartup
        {
            return Start<TClusterAppStartup>(options);
        }
        #endregion

        public static IDisposable Start<TStartup>(string url)
        {
            if (url.isNullOrEmpty())
                throw new ArgumentNullException(nameof(url));

            return Start<TStartup>(BuildOptions(url));
        }

        public static IDisposable Start<TStartup>(StartOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.AppStartup = typeof(TStartup).AssemblyQualifiedName;

            return Start(options);
        }

        public static IDisposable Start(string url)
        {
            if (url.isNullOrEmpty())
                throw new ArgumentNullException(nameof(url));

            return Start(BuildOptions(url));
        }

        public static IDisposable Start(StartOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            //replace IHostingEngine implementation
            options.Settings[typeof(IHostingEngine).FullName] = typeof(FCPHostingEngine).AssemblyQualifiedName;

            return WebApp.Start(options);
        }
    }
}
