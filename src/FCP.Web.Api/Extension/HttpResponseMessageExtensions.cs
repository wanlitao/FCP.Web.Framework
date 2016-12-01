using System.Net.Http;

namespace FCP.Web.Api
{
    public static class HttpResponseMessageExtensions
    {
        internal static void EnsureResponseHasRequest(this HttpResponseMessage response, HttpRequestMessage request)
        {
            if (response != null && response.RequestMessage == null)
            {
                response.RequestMessage = request;
            }
        }
    }
}
