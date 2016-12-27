using System;
using System.Web.Http.Dependencies;

namespace FCP.Web.Api
{
    public abstract class FCPHttpConfigurationInitializerFactory : HttpConfigurationInitializerFactory
    {
        public FCPHttpConfigurationInitializerFactory(IDependencyResolver dependencyResolver, string dateTimeFormat = null)
            : base(dateTimeFormat)
        {
            filters.Add(new FCPDoResultActionDescriptorFilterAttribute());  //添加 返回结果 自定义HttpResponseMessage Converter
            filters.Add(new FCPApiExceptionFilterAttribute());  //添加异常处理
            filters.Add(CreateApiLogActionFilter(dependencyResolver));  //添加Action日志

            messageHandlers.Add(new FCPCacheRequestContentMessageHandler());  //添加缓存请求内容 MessageHandler
        }

        protected static ApiLogActionFilterAttribute CreateApiLogActionFilter(IDependencyResolver dependencyResolver)
        {
            var apiLogActionFilter = dependencyResolver.GetService<FCPApiLogActionFilterAttribute>();

            if (apiLogActionFilter == null)
                throw new ArgumentException($"not found any type implement {nameof(FCPApiLogActionFilterAttribute)}");

            return apiLogActionFilter;
        }
    }
}
