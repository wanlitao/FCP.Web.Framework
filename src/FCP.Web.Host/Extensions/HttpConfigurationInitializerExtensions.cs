using FCP.Web.Api;
using System;
using System.Web.Http;

namespace FCP.Web.Host
{
    public static class HttpConfigurationInitializerExtensions
    {
        public static HttpConfiguration UseInitializer(this HttpConfiguration configuration,
            IHttpConfigurationInitializerFactory httpConfigInitializerFactory)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (httpConfigInitializerFactory == null)
                throw new ArgumentNullException(nameof(httpConfigInitializerFactory));           

            configuration.Initializer = httpConfigInitializerFactory.createInitializerAction(configuration.Initializer);

            return configuration;
        }
    }
}
