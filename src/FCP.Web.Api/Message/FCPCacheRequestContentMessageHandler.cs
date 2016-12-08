using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FCP.Web.Api
{
    public class FCPCacheRequestContentMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await CacheRequestContentToPropertiesAsync(request).ConfigureAwait(false);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task CacheRequestContentToPropertiesAsync(HttpRequestMessage request)
        {
            if (!request.Properties.ContainsKey(FCPApiConstants.requestContentPropertyKey)) //避免重复赋值
            {
                var requestContent = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

                request.Properties[FCPApiConstants.requestContentPropertyKey] = requestContent ?? string.Empty;
            }
        }
    }
}
