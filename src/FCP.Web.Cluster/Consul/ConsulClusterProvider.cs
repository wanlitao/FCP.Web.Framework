using System;
using System.Threading.Tasks;
using Consul.RsetApi.Client;
using FCP.Util;
using System.Linq;
using System.Net;

namespace FCP.Web.Cluster
{
    public class ConsulClusterProvider : IClusterProvider
    {
        private Uri _apiBaseUri;
        private int _checkInterval;

        #region 构造函数
        public ConsulClusterProvider()
            : this(ConsulConstants.DefaultApiBaseUri)
        { }

        public ConsulClusterProvider(Uri apiBaseUri)
            : this(apiBaseUri, ClusterConstants.DefaultCheckInterval)
        { }

        public ConsulClusterProvider(Uri apiBaseUri, int checkInterval)            
        {
            _apiBaseUri = apiBaseUri ?? ConsulConstants.DefaultApiBaseUri;
            _checkInterval = checkInterval < 1 ? ClusterConstants.DefaultCheckInterval : checkInterval;
        }
        #endregion

        /// <summary>
        /// 根据名称获取服务Id
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        protected string getServiceIdByName(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
                return string.Empty;

            return string.Format("{0}_{1}", serviceName, Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// 查询服务实例
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public async Task<ServiceInfo[]> findHealthServicesAsync(string name)
        {
            if (name.isNullOrEmpty())
                return null;

            using (var client = new ConsulRsetApiClient(_apiBaseUri))
            {
                var response = await client.healthServiceAsync(name, string.Empty, true).ConfigureAwait(false);

                if (response.ResponseData.isEmpty())
                    return null;

                return response.ResponseData.Select(m => new ServiceInfo
                    {
                        id = m.Service.ID,
                        name = m.Service.Service,
                        address = m.Service.Address,
                        port = m.Service.Port
                    }).ToArray();
            }
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="address">地址</param>
        /// <param name="statusUrl">状态Url地址</param>
        /// <returns></returns>
        public async Task<string> registerServiceAsync(string name, Uri address, string statusUrl)
        {
            if (name.isNullOrEmpty() || address == null || statusUrl.isNullOrEmpty())
                return string.Empty;

            var registerInfo = new ConsulAgentServiceRegistration
            {
                ID = getServiceIdByName(name),
                Name = name,
                Tags = new[] { name },
                Address = address.Host,
                Port = address.Port,
                Check = new ConsulAgentServiceCheck
                {
                    HTTP = string.Format("{0}{1}", address, statusUrl),
                    Interval = string.Format("{0}s", _checkInterval)
                }
            };

            using (var client = new ConsulRsetApiClient(_apiBaseUri))
            {
                var response = await client.agentServiceRegisterAsync(registerInfo).ConfigureAwait(false);

                return response.StatusCode == HttpStatusCode.OK ? registerInfo.ID : string.Empty;
            }
        }

        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="serviceId">服务Id</param>
        /// <returns></returns>
        public async Task<bool> deregisterServiceAsync(string serviceId)
        {
            if (serviceId.isNullOrEmpty())
                return false;

            using (var client = new ConsulRsetApiClient(_apiBaseUri))
            {
                var response = await client.agentServiceDeregisterAsync(serviceId).ConfigureAwait(false);

                return response.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}