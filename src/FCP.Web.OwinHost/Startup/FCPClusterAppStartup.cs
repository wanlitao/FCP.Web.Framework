using FCP.Util;
using FCP.Util.Async;
using FCP.Web.Cluster;
using Microsoft.Owin.BuilderProperties;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FCP.Web.OwinHost
{
    public abstract class FCPClusterAppStartup
    {
        #region Properties
        protected abstract string AppCode { get; }

        protected abstract string StatusUrl { get; }
        #endregion

        public void Configuration(IAppBuilder app)
        {
            ConfigurationApp(app);

            ConfigurationCluster(app);
        }

        protected abstract void ConfigurationApp(IAppBuilder app);

        #region Cluster Configuration

        #region App Rpc Address
        private static Uri GetAppRpcAddressUri(AppProperties appProperties)
        {
            var addressList = appProperties.Addresses.List;
            foreach (IDictionary<string, object> current in addressList)
            {
                var host = current.Get<string>("host");
                if (CheckRpcValidHost(host))
                {
                    var scheme = current.Get<string>("scheme");
                    var port = TypeHelper.parseInt(current.Get<string>("port"));

                    return new UriBuilder(scheme, host, port).Uri;
                }
            }

            return null;
        }

        private static bool CheckRpcValidHost(string host)
        {
            if (host.isNullOrEmpty())
                return false;

            if (StringUtil.compareIgnoreCase(host, "*") || StringUtil.compareIgnoreCase(host, "+"))
                return false;

            if (StringUtil.compareIgnoreCase(host, "localhost") || StringUtil.compareIgnoreCase(host, "127.0.0.1"))
                return false;

            return true;
        }
        #endregion

        #region Cluster Provider
        private static IClusterProvider GetClusterProvider(AppProperties appProperties)
        {
            return appProperties.Get<IClusterProvider>(FCPOwinHostConstants.AppProperty_ClusterProvider);
        }
        #endregion

        private void ConfigurationCluster(IAppBuilder app)
        {
            var properties = new AppProperties(app.Properties);

            var clusterProvider = GetClusterProvider(properties);
            if (clusterProvider == null)
                throw new ArgumentException("not found cluster provider instance");

            ConfigurationClusterRegister(clusterProvider, properties);
            ConfigurationClusterDeregister(clusterProvider, properties);
        }

        private void ConfigurationClusterRegister(IClusterProvider clusterProvider, AppProperties appProperties)
        {
            var token = appProperties.Get<CancellationToken>(FCPOwinHostConstants.AppProperty_OnStarting);
            var addressUri = GetAppRpcAddressUri(appProperties);
            token.Register(() =>
            {
                var clusterAppId = AsyncFuncHelper.RunSync(() => clusterProvider.registerServiceAsync(AppCode, addressUri, StatusUrl));
                appProperties.Set(FCPOwinHostConstants.AppProperty_ClusterAppId, clusterAppId);
            });
        }

        private static void ConfigurationClusterDeregister(IClusterProvider clusterProvider, AppProperties appProperties)
        {
            var token = appProperties.OnAppDisposing;
            token.Register(() =>
            {
                var clusterAppId = appProperties.Get<string>(FCPOwinHostConstants.AppProperty_ClusterAppId);
                AsyncFuncHelper.RunSync(() => clusterProvider.deregisterServiceAsync(clusterAppId));
            });
        }
        #endregion
    }
}
