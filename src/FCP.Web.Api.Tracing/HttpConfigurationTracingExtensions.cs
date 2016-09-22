using System;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace FCP.Web.Api.Tracing
{
    public static class HttpConfigurationTracingExtensions
    {
        public static NLogTraceWriter EnableNLogTracing(this HttpConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            NLogTraceWriter traceWriter = new NLogTraceWriter() { MinimumLevel = TraceLevel.Info };

            configuration.Services.Replace(typeof(ITraceWriter), traceWriter);

            return traceWriter;
        }
    }
}
