using System;
using System.Web.Http;

namespace FCP.Web.Api
{
    /// <summary>
    /// HttpServer配置初始化工厂 接口
    /// </summary>
    public interface IHttpConfigurationInitializerFactory
    {
        /// <summary>
        /// 创建初始化Action
        /// </summary>
        /// <param name="oldInitializer">原初始化Action</param>
        /// <returns></returns>
        Action<HttpConfiguration> createInitializerAction(Action<HttpConfiguration> oldInitializer);
    }
}
