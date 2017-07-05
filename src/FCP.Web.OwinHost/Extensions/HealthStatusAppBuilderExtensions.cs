using FCP.Util;
using Owin;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FCP.Web.OwinHost
{
    public static class HealthStatusAppBuilderExtensions
    {
        public static IAppBuilder UseHealthStatus(this IAppBuilder app, string statusRoute)
        {
            if (statusRoute.isNullOrEmpty())
                throw new ArgumentNullException(nameof(statusRoute));

            var mapRoutePath = "/" + statusRoute.TrimStart('/');

            app.Map(mapRoutePath,
                appBuilder => appBuilder.Run(ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                    return Task.FromResult(0);
                })
            );

            return app;
        }
    }
}
