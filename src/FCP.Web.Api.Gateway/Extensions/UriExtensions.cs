using System;
using System.Linq;
using FCP.Util;

namespace FCP.Web.Api.Gateway
{
    public static class UriExtensions
    {
        public static string GetSegment(this Uri uri, int index)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "index must not be minus");

            var segments = uri.Segments;
            if (segments.isEmpty() || index > segments.Length - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            return segments.Skip(index).Take(1).FirstOrDefault();
        }

        public static string GetSegmentNoSlash(this Uri uri, int index)
        {
            var targetSegment = uri.GetSegment(index);

            if (targetSegment.isNullOrEmpty())
                return targetSegment;

            return targetSegment.Substring(0, targetSegment.Length - 1);
        }
    }
}
