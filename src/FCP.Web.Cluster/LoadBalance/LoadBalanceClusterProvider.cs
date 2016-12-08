using FCP.Routing;
using FCP.Util;
using System;
using System.Threading.Tasks;

namespace FCP.Web.Cluster
{
    public class LoadBalanceClusterProvider : ILoadBalanceClusterProvider
    {
        private readonly IClusterProvider _innerClusterProvider;
        private readonly IRouting<ServiceInfo> _routing;

        public LoadBalanceClusterProvider(IClusterProvider clusterProvider, IRouting<ServiceInfo> routing)
        {
            if (clusterProvider == null)
                throw new ArgumentNullException(nameof(clusterProvider));

            if (routing == null)
                throw new ArgumentNullException(nameof(routing));

            _innerClusterProvider = clusterProvider;
            _routing = routing;
        }

        public async Task<ServiceInfo> findHealthServiceAsync(string name)
        {
            var healthServices = await findHealthServicesAsync(name).ConfigureAwait(false);
            if (healthServices.isEmpty())
                return null;

            return _routing.select(null, healthServices);
        }

        public Task<ServiceInfo[]> findHealthServicesAsync(string name)
        {
            return _innerClusterProvider.findHealthServicesAsync(name);
        }

        public Task<string> registerServiceAsync(string name, Uri address, string statusUrl)
        {
            return _innerClusterProvider.registerServiceAsync(name, address, statusUrl);
        }

        public Task<bool> deregisterServiceAsync(string serviceId)
        {
            return _innerClusterProvider.deregisterServiceAsync(serviceId);
        }
    }
}
