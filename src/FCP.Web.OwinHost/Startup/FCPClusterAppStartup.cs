using FCP.Util.Async;
using FCP.Web.Cluster;
using Microsoft.Owin.BuilderProperties;
using Owin;
using System;
using System.Linq;
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

            ConfigurationClusterRegister(app, clusterProvider, properties);
            ConfigurationClusterDeregister(clusterProvider, properties);
        }

        private void ConfigurationClusterRegister(IAppBuilder app, IClusterProvider clusterProvider, AppProperties appProperties)
        {
            var token = appProperties.Get<CancellationToken>(FCPOwinHostConstants.AppProperty_OnStarting);
            var addressUri = app.GetAppRpcAddresses().FirstOrDefault();
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
