using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace EPiCode.Commerce.RestApi.Client
{
    public class SystemClient
    {
        private readonly EndpointConfiguration _endpointConfiguration;

        public SystemClient(EndpointConfiguration endpointConfiguration)
        {
            _endpointConfiguration = endpointConfiguration;
        }

        public object ClearCatalogAndModels()
        {
            RestEndpoint<string> systemEndpoint = new RestEndpoint<string>(_endpointConfiguration.EndpointApiUri + "/" + _endpointConfiguration.Controller, "ClearCatalogAndModels");
            systemEndpoint.ApiKey = _endpointConfiguration.ApiKey;
            object result = (JObject)systemEndpoint.Get<object>();

            return result;
            
        }

        public string ClearCache()
        {
            RestEndpoint<string> systemEndpoint = new RestEndpoint<string>(_endpointConfiguration.EndpointApiUri + "/" + _endpointConfiguration.Controller, "ClearCache");
            systemEndpoint.ApiKey = _endpointConfiguration.ApiKey;
            string result = systemEndpoint.Get<string>();
            return result;
        }
    }
}
