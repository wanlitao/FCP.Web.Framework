using System;
using System.Web.Http;
using System.Web.Http.Filters;
using FCP.Util;
using System.Linq;
using System.Net.Http.Formatting;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace FCP.Web.Api
{
    /// <summary>
    /// HttpServer配置初始化工厂
    /// </summary>
    public abstract class HttpConfigurationInitializerFactory : IHttpConfigurationInitializerFactory
    {
        private readonly JsonMediaTypeFormatter _jsonMediaTypeFormatter;

        private readonly HttpFilterCollection _filters = new HttpFilterCollection();  //过滤器集合
        private readonly Collection<DelegatingHandler> _messageHandlers = new Collection<DelegatingHandler>();

        public HttpConfigurationInitializerFactory(string dateTimeFormat = null)
        {
            _jsonMediaTypeFormatter = new JsonNetMediaTypeFormatter(dateTimeFormat);
        }

        #region 属性
        protected JsonMediaTypeFormatter jsonMediaTypeFormatter
        {
            get { return _jsonMediaTypeFormatter; }
        }

        protected HttpFilterCollection filters
        {
            get { return _filters; }
        }

        protected Collection<DelegatingHandler> messageHandlers
        {
            get { return _messageHandlers; }
        }
        #endregion

        /// <summary>
        /// 创建初始化Action
        /// </summary>
        /// <param name="oldInitializer">原初始化Action</param>
        /// <returns></returns>
        public Action<HttpConfiguration> createInitializerAction(Action<HttpConfiguration> oldInitializer)
        {
            Action<HttpConfiguration> initializer = config =>
            {
                oldInitializer?.Invoke(config);
                config.Formatters.Remove(config.Formatters.JsonFormatter);
                config.Formatters.Insert(0, jsonMediaTypeFormatter);  //替换默认JsonFormatter
                if (filters.isNotEmpty())
                {
                    config.Filters.AddRange(filters.Select(m => m.Instance));
                }
                if (messageHandlers.isNotEmpty())
                {
                    foreach(var messageHandler in messageHandlers)
                    {
                        config.MessageHandlers.Add(messageHandler);
                    }
                }
                HttpCustomInitializer(config);
            };

            return initializer;
        }

        /// <summary>
        /// Http自定义初始化
        /// </summary>
        /// <param name="config"></param>
        protected abstract void HttpCustomInitializer(HttpConfiguration config);
    }
}
