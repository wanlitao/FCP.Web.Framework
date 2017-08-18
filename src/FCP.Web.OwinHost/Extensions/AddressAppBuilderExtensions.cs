using FCP.Util;
using Microsoft.Owin.BuilderProperties;
using Owin;
using System;
using System.Collections.Generic;

namespace FCP.Web.OwinHost
{
    public static class AddressAppBuilderExtensions
    {
        public static IEnumerable<Uri> GetAppAddresses(this IAppBuilder app)
        {
            return GetAppAddresses(app, (address) => true);
        }

        public static IEnumerable<Uri> GetAppRpcAddresses(this IAppBuilder app)
        {
            return GetAppAddresses(app, (address) => IsRpcValid(address));
        }

        public static IEnumerable<Uri> GetAppAddresses(this IAppBuilder app, Predicate<Address> addressFilter)
        {
            var appProperties = new AppProperties(app.Properties);           

            foreach (var address in appProperties.Addresses)
            {
                if (addressFilter(address))
                {
                    yield return new UriBuilder(address.Scheme, address.Host,
                        TypeHelper.parseInt(address.Port), address.Path).Uri;
                }                                
            }
        }

        public static bool IsRpcValid(this Address address)
        {
            if (address == null)
                return false;

            var host = address.Host;
            if (host.isNullOrEmpty())
                return false;

            if (StringUtil.compareIgnoreCase(host, "*") || StringUtil.compareIgnoreCase(host, "+"))
                return false;

            if (StringUtil.compareIgnoreCase(host, "localhost") || StringUtil.compareIgnoreCase(host, "127.0.0.1"))
                return false;

            return true;
        }
    }
}
