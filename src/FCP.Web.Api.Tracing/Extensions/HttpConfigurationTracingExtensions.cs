using System;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace FCP.Web.Api.Tracing
{
    public static class HttpConfigurationTracingExtensions
    {
        public static HttpConfiguration UseNLogTracing(this HttpConfiguration configuration, TraceLevel minimumLevel)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            NLogTraceWriter traceWriter = new NLogTraceWriter() { MinimumLevel = minimumLevel };

            configuration.Services.Replace(typeof(ITraceWriter), traceWriter);

            return configuration;
        }
    }
}
