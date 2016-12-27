using System;
using System.Web.Http.Dependencies;

namespace FCP.Web.Api
{
    public static class HttpDependencyExtensions
    {
        public static TService GetService<TService>(this IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException(nameof(dependencyResolver));

            using (dependencyResolver.BeginScope())
            {
                return (TService)dependencyResolver.GetService(typeof(TService));
            }
        }
    }
}
