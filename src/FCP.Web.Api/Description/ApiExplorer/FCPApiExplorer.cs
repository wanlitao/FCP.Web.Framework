using System;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Web.Http.Description;

namespace FCP.Web.Api
{
    /// <summary>
    /// overwrite ApiExplorer，Set Actual Response Type
    /// </summary>
    public abstract class FCPApiExplorer : ApiExplorer, IApiExplorer
    {
        private Lazy<Collection<ApiDescription>> _apiDescriptions;

        public FCPApiExplorer(HttpConfiguration configuration)
            : base(configuration)
        {
            _apiDescriptions = new Lazy<Collection<ApiDescription>>(InitializeApiDescriptions);
        }

        Collection<ApiDescription> IApiExplorer.ApiDescriptions
        {
            get
            {
                return _apiDescriptions.Value;
            }
        }

        private Collection<ApiDescription> InitializeApiDescriptions()
        {
            Collection<ApiDescription> apiDescriptions = base.ApiDescriptions;

            foreach (var apiDescription in apiDescriptions)
            {
                CheckSetActualResponseType(apiDescription.ResponseDescription);
            }

            return apiDescriptions;
        }

        private void CheckSetActualResponseType(ResponseDescription responseDescription)
        {
            if (responseDescription == null)
                return;
            
            var actualResponseType = GetActualResponseType(responseDescription.DeclaredType);

            responseDescription.ResponseType = actualResponseType ?? responseDescription.ResponseType;
        }

        protected abstract Type GetActualResponseType(Type declaredResponseType);
    }
}
