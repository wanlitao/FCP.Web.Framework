namespace FCP.Web.Api
{
    public abstract class FCPHttpConfigurationInitializerFactory : HttpConfigurationInitializerFactory
    {
        public FCPHttpConfigurationInitializerFactory(string dateTimeFormat = null)
            : base(dateTimeFormat)
        {
            filters.Add(new FCPDoResultActionDescriptorFilterAttribute());  //添加 返回结果 自定义HttpResponseMessage Converter
            filters.Add(new FCPApiExceptionFilterAttribute());  //添加异常处理            

            messageHandlers.Add(new FCPCacheRequestContentMessageHandler());  //添加缓存请求内容 MessageHandler
        }
    }
}
