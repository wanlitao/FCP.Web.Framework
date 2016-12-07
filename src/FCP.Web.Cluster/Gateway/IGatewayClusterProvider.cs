using System.Threading.Tasks;

namespace FCP.Web.Cluster
{
    public interface IGatewayClusterProvider : IClusterProvider
    {
        /// <summary>
        /// 查询健康的服务实例
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        Task<ServiceInfo> findHealthServiceAsync(string name);
    }
}
