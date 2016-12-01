using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FCP.Web.Api
{
    public abstract class ApiActionDescriptor : HttpActionDescriptor
    {
        private readonly HttpActionDescriptor _innerDescriptor;
        private IActionResultConverter _resultConverter;

        public ApiActionDescriptor(HttpControllerContext controllerContext, HttpActionDescriptor actionDescriptor)
            : base(controllerContext.ControllerDescriptor)
        {
            if (actionDescriptor == null)
                throw new ArgumentNullException(nameof(actionDescriptor));

            _innerDescriptor = actionDescriptor;
        }

        public override ConcurrentDictionary<object, object> Properties
        {
            get
            {
                return _innerDescriptor.Properties;
            }
        }

        public override HttpActionBinding ActionBinding
        {
            get
            {
                return _innerDescriptor.ActionBinding;
            }
            set
            {
                _innerDescriptor.ActionBinding = value;
            }
        }

        public override Collection<HttpMethod> SupportedHttpMethods
        {
            get
            {
                return _innerDescriptor.SupportedHttpMethods;
            }
        }

        public override string ActionName
        {
            get
            {
                return _innerDescriptor.ActionName;
            }
        }

        public override IActionResultConverter ResultConverter
        {
            get
            {
                if (_resultConverter == null)
                {
                    _resultConverter = GetCustomActionResultConverter() ?? base.ResultConverter;
                }

                return _resultConverter;
            }
        }

        /// <summary>
        /// Custom Action Result Converter
        /// </summary>
        /// <returns></returns>
        protected abstract IActionResultConverter GetCustomActionResultConverter();

        public override Type ReturnType
        {
            get
            {
                return _innerDescriptor.ReturnType;
            }
        }

        public override Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments, CancellationToken cancellationToken)
        {
            return _innerDescriptor.ExecuteAsync(controllerContext, arguments, cancellationToken);
        }

        public override Collection<T> GetCustomAttributes<T>()
        {
            return _innerDescriptor.GetCustomAttributes<T>();
        }

        public override Collection<T> GetCustomAttributes<T>(bool inherit)
        {
            return _innerDescriptor.GetCustomAttributes<T>(inherit);
        }

        public override Collection<IFilter> GetFilters()
        {
            return _innerDescriptor.GetFilters();
        }

        public override Collection<FilterInfo> GetFilterPipeline()
        {
            return _innerDescriptor.GetFilterPipeline();
        }

        public override Collection<HttpParameterDescriptor> GetParameters()
        {
            return _innerDescriptor.GetParameters();
        }
    }
}
