using System;
using System.Threading.Tasks;

namespace FCP.Web.Cluster
{
    /// <summary>
    /// 集群提供程序 接口
    /// </summary>
    public interface IClusterProvider
    {
        /// <summary>
        /// 查询健康的服务实例
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        Task<ServiceInfo[]> findHealthServicesAsync(string name);

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="address">地址</param>
        /// <param name="statusUrl">状态Url地址</param>
        /// <returns>服务Id</returns>
        Task<string> registerServiceAsync(string name, Uri address, string statusUrl);

        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="serviceId">服务Id</param>
        /// <returns></returns>
        Task<bool> deregisterServiceAsync(string serviceId);
    }
}