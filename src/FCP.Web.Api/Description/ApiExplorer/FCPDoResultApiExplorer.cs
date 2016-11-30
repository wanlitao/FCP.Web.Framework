using FCP.Core;
using FCP.Util;
using System;
using System.Web.Http;

namespace FCP.Web.Api
{
    public class FCPDoResultApiExplorer : FCPApiExplorer
    {
        private static readonly Type doResultGenericTypeDefinition = typeof(FCPDoResult<>);

        public FCPDoResultApiExplorer(HttpConfiguration configuration)
            : base(configuration)
        { }

        protected override Type GetActualResponseType(Type declaredResponseType)
        {
            if (declaredResponseType.Is(doResultGenericTypeDefinition))
            {
                return declaredResponseType == typeof(FCPDoResult) ? typeof(object)
                    : declaredResponseType.GetGenericArguments()[0];
            }

            return null;
        }
    }
}
