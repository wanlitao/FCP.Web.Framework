using System.Collections.Generic;

namespace FCP.Web.OwinHost
{
    internal static class DictionaryExtensions
    {
        internal static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            object obj;
            if (!dictionary.TryGetValue(key, out obj))
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}
