using System;
using System.Net.Http;

namespace Voorbeeld.WebApplication.Api
{
    public class PlateClientBuilder : IPlateClientBuilder
    {
        private readonly IHttpClientFactory _clientFactory;

        public PlateClientBuilder(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public HttpClient Build()
        {
            var baseUrl = "https://2zykqjkvnj.execute-api.eu-central-1.amazonaws.com";//
            // get a token at yarodataservice.com to be able to use plate-service !!!
            var appKey = "";
            if (!string.IsNullOrEmpty(appKey))
            {
                var client = _clientFactory.CreateClient();

                client.DefaultRequestHeaders.Add("Authorization", appKey);
                client.DefaultRequestHeaders.Add("X-API-Version", "1.0.0");
                client.BaseAddress = new Uri(baseUrl);
                return client;
            }
            return null;
        }
    }
}
