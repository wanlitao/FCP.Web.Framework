using FCP.Core;
using FCP.Util;
using System;
using System.Web.Http.Controllers;

namespace FCP.Web.Api
{
    public class FCPDoResultApiActionDescriptor : ApiActionDescriptor
    {
        private static readonly Type doResultGenericTypeDefinition = typeof(FCPDoResult<>);

        public FCPDoResultApiActionDescriptor(HttpControllerContext controllerContext, HttpActionDescriptor actionDescriptor)
            : base(controllerContext, actionDescriptor)
        { }

        protected override IActionResultConverter GetCustomActionResultConverter()
        {
            if (ReturnType.Is(doResultGenericTypeDefinition))
            {
                var genericTypeArgument = ReturnType == typeof(FCPDoResult) ? typeof(object)
                    : ReturnType.GetGenericArguments()[0];

                var resultConverterType = typeof(FCPDoResultConverter<>).MakeGenericType(genericTypeArgument);
                return TypeActivator.Create<IActionResultConverter>(resultConverterType).Invoke();
            }

            return null;
        }
    }
}
