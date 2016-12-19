using System;
using System.Net.Http;
using System.Threading.Tasks;
using FCP.Web.Cluster;
using FCP.Configuration.Cluster;
using System.Linq;
using FCP.Util;

namespace FCP.Web.Api.Gateway
{
    public class FCPGatewayMessageHandler : GatewayMessageHandler
    {
        private readonly ILoadBalanceClusterProvider _loadBalanceClusterProvider;
        private readonly IServiceConfigurationProvider _serviceConfigurationProvider;

        public FCPGatewayMessageHandler(ILoadBalanceClusterProvider loadBalanceClusterProvider,
            IServiceConfigurationProvider serviceConfigProvider)
        {
            if (loadBalanceClusterProvider == null)
                throw new ArgumentNullException(nameof(loadBalanceClusterProvider));

            if (serviceConfigProvider == null)
                throw new ArgumentNullException(nameof(serviceConfigProvider));

            _loadBalanceClusterProvider = loadBalanceClusterProvider;
            _serviceConfigurationProvider = serviceConfigProvider;
        }

        #region Properties
        protected ILoadBalanceClusterProvider LoadBalanceClusterProvider { get { return _loadBalanceClusterProvider; } }

        protected IServiceConfigurationProvider ServiceConfigurationProvider { get { return _serviceConfigurationProvider; } }
        #endregion

        /// <summary>
        /// 获取请求子系统编码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task<string> getRequestSubSystemCodeAsync(HttpRequestMessage request)
        {
            var requestPathFirstSegment = request.RequestUri.GetSegmentNoSlash(1);

            if (requestPathFirstSegment.isNullOrEmpty())
                return string.Empty;

            var subSystemCodes = await ServiceConfigurationProvider.GetServiceCodesAsync().ConfigureAwait(false);
            if (subSystemCodes.isEmpty())
                return string.Empty;

            return subSystemCodes.FirstOrDefault(m => StringUtil.compareIgnoreCase(m, requestPathFirstSegment));            
        }

        /// <summary>
        /// 生成子系统请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="subSystemCode">子系统编码</param>
        /// <returns></returns>
        protected override async Task<HttpRequestMessage> getSubSystemRequestAsync(HttpRequestMessage request, string subSystemCode)
        {
            if (subSystemCode.isNullOrEmpty())
                return request;

            var subServiceInfo = await LoadBalanceClusterProvider.findHealthServiceAsync(subSystemCode).ConfigureAwait(false);
            if (subServiceInfo == null)
                return null;

            var newRequestPath = request.RequestUri.AbsolutePath.Substring(subSystemCode.Length + 1); //去除子系统编码对应的Path分隔段
            var subSystemRequestUrlBuilder = new UriBuilder(request.RequestUri.Scheme, subServiceInfo.address, subServiceInfo.port,
                newRequestPath, request.RequestUri.Query);

            request.RequestUri = subSystemRequestUrlBuilder.Uri;

            //添加传递 子系统服务实例的Id和Code
            addSubSystemRequestServiceInfoHeader(request, subServiceInfo);

            return request;
        }

        /// <summary>
        /// 添加子系统请求头部 服务信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="serviceInfo">服务信息</param>
        protected void addSubSystemRequestServiceInfoHeader(HttpRequestMessage request, ServiceInfo serviceInfo)
        {
            if (request == null || request.Headers == null || serviceInfo == null)
                return;

            if (!serviceInfo.id.isNullOrEmpty())
            {
                request.Headers.AddOrAppend(FCPApiConstants.httpForwardServiceIdHeaderName, serviceInfo.id);
            }

            if (!serviceInfo.name.isNullOrEmpty())
            {
                request.Headers.AddOrAppend(FCPApiConstants.httpForwardServiceCodeHeaderName, serviceInfo.name);
            }
        }
    }
}
