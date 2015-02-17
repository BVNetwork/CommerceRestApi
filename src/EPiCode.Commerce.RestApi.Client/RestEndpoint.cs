using System;
using System.Net.Http;
using System.Net.Http.Headers;


namespace EPiCode.Commerce.RestApi.Client
{

    public class RestEndpoint<T>
    {
        private const int DEFAULT_TIMEOUT_SECONDS = 30;
        private readonly string _endpointAddress;
        private readonly string _action = null;
        private int _timeout = DEFAULT_TIMEOUT_SECONDS;

        public RestEndpoint(string endpointAddress, string action)
        {
            _action = action;
            _endpointAddress = ValidateEndpointAddress(endpointAddress);
        }

        public string Action
        {
            get { return _action; }
        }

        /// <summary>
        /// An API key to send with the request
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Timeout in seconds. Defaults to 30 if not set.
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        private string ValidateEndpointAddress(string endpointAddress)
        {
            if (string.IsNullOrEmpty(endpointAddress))
                throw new ArgumentException(
                    "Missing endpointAddress.");

            if (endpointAddress.EndsWith("/") == false)
                return endpointAddress + "/";
            return endpointAddress;
        }

        public string GetUrl()
        {
            return GetUrl(_action);
        }

        public string GetUrl(string action)
        {
            string endpointAddress = _endpointAddress;

            if (string.IsNullOrEmpty(action) == false)
            {
                endpointAddress = endpointAddress + action;
            }

            return endpointAddress;
        }


        public TResult Get<TResult>()
        {
            Uri uri = new Uri(GetUrl());
            var client = CreateHttpClient(uri);

            HttpResponseMessage response = client.GetAsync(uri.PathAndQuery).Result;

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                return response.Content.ReadAsAsync<TResult>().Result;
            }
            else
            {
                string errorMsg = string.Format("Method failed: {0} ({1})", (int)response.StatusCode,
                    response.ReasonPhrase);
                throw new HttpRequestException(errorMsg);
            }
        }

        public TResult Post<TResult>(T message)
        {
            Uri uri = new Uri(GetUrl());
            var client = CreateHttpClient(uri);

            HttpResponseMessage response = client.PostAsJsonAsync<T>(uri.PathAndQuery, message).Result;

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                return response.Content.ReadAsAsync<TResult>().Result;
            }
            else
            {
                string errorMsg = string.Format("Method failed: {0} ({1})", (int) response.StatusCode,
                    response.ReasonPhrase);
                throw new HttpRequestException(errorMsg);
            }
        }

        protected HttpClient CreateHttpClient(Uri uri)
        {
            HttpClient client = new HttpClient();
            string baseUrl = uri.Scheme + "://" + uri.Authority;

            client.BaseAddress = new Uri(baseUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (string.IsNullOrEmpty(ApiKey) == false)
            {
                client.DefaultRequestHeaders.Add("apikey", ApiKey);
            }

            client.Timeout = new TimeSpan(0, 0, Timeout);
            return client;
        }
    }
}