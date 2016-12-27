using FCP.Configuration.Cluster;
using FCP.Web.Cluster;
using System.Web.Http.Dependencies;

namespace FCP.Web.Api.Gateway
{
    public abstract class FCPGatewayHttpConfigurationInitializerFactory : FCPHttpConfigurationInitializerFactory
    {
        public FCPGatewayHttpConfigurationInitializerFactory(IDependencyResolver dependencyResolver, string dateTimeFormat = null)
            : base(dependencyResolver, dateTimeFormat)
        {
            messageHandlers.Add(CreateGatewayMessageHandler(dependencyResolver));
            messageHandlers.Add(new ProxyMessageHandler());
        }

        protected static GatewayMessageHandler CreateGatewayMessageHandler(IDependencyResolver dependencyResolver)
        {
            var loadBalanceClusterProvider = dependencyResolver.GetService<ILoadBalanceClusterProvider>();
            var serviceConfigProvider = dependencyResolver.GetService<IServiceConfigurationProvider>();

            return new FCPGatewayMessageHandler(loadBalanceClusterProvider, serviceConfigProvider);
        }
    }
}
