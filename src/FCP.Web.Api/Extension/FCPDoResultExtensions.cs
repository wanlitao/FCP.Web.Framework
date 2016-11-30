using FCP.Core;
using FCP.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.ModelBinding;

namespace FCP.Web.Api
{
    internal static class FCPDoResultExtensions
    {
        private static readonly IDictionary<string, HttpStatusCode> _doResultTypeMapHttpStatusCode;

        static FCPDoResultExtensions()
        {
            _doResultTypeMapHttpStatusCode = new Dictionary<string, HttpStatusCode>();
            _doResultTypeMapHttpStatusCode.Add(FCPDoResultType.success.ToString(), HttpStatusCode.OK);
            _doResultTypeMapHttpStatusCode.Add(FCPDoResultType.fail.ToString(), HttpStatusCode.InternalServerError);
            _doResultTypeMapHttpStatusCode.Add(FCPDoResultType.validFail.ToString(), HttpStatusCode.BadRequest);
            _doResultTypeMapHttpStatusCode.Add(FCPDoResultType.notFound.ToString(), HttpStatusCode.NotFound);
            _doResultTypeMapHttpStatusCode.Add(FCPDoResultType.unauthorized.ToString(), HttpStatusCode.Unauthorized);
        }

        internal static HttpStatusCode GetHttpStatusCode<T>(this FCPDoResult<T> doResult)
        {
            if (doResult == null)
                throw new ArgumentNullException(nameof(doResult));

            return _doResultTypeMapHttpStatusCode[doResult.type];
        }

        internal static ModelStateDictionary GetValidFailStateDictionary<T>(this FCPDoResult<T> doResult)
        {
            if (doResult == null)
                throw new ArgumentNullException(nameof(doResult));

            if (!doResult.isValidFail)
                throw new ArgumentException("doResult type must be valid fail");

            var modelStateDict = new ModelStateDictionary();
            if (doResult.validFailResults.isNotEmpty())
            {
                foreach (var validFailResult in doResult.validFailResults)
                {
                    modelStateDict.AddModelError(validFailResult.Key, validFailResult.Value);
                }
            }

            return modelStateDict;
        }
    }
}
