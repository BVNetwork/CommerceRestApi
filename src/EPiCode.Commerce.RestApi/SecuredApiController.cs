using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.UI;

namespace EPiCode.Commerce.RestService
{
    public class SecuredApiController : ApiController
    {
        private const string ApiKeyName = "apikey";
        private const string ApiKeySettingsName = "EPiCode.Commerce.Rest.ApiKey";
        private string _apiKey = null;

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            // Allows us to run scripts without exposing the api key
            if(User.IsInRole("CmsAdmins") == false)
            {
                if (ValidateApiKey(controllerContext.Request) == false)
                {
                    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    TaskCompletionSource<HttpResponseMessage> tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(resp);
                    return tsc.Task;
                }
                
            }
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }

        protected virtual bool ValidateApiKey(HttpRequestMessage request)
        {
            string apiKey = null;
            if (request.Headers.Contains(ApiKeyName))
            {
                // Validate api key
                apiKey = request.Headers.GetValues(ApiKeyName).FirstOrDefault();
            }

            if (string.Compare(apiKey, GetApiKey(), StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return false;
            }
            return true;
        }

        protected string GetApiKey()
        {
            if(string.IsNullOrEmpty(_apiKey))
            {
                _apiKey = ConfigurationManager.AppSettings[ApiKeySettingsName];
            }
            return _apiKey;
        }

        protected HttpResponseMessage CreateResponseMessage(string message)
        {
            return CreateResponseMessage(HttpStatusCode.OK, message);
        }

        protected HttpResponseMessage CreateResponseMessage(HttpStatusCode status, string message)
        {
            HttpResponseMessage response = Request.CreateResponse<string>(status, "");
            response.Content = new StringContent(message);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }


    }
}
