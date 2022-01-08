using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace Voorbeeld.WebApplication.Api
{

    public class StripClientBuilder : IStripClientBuilder
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public StripClientBuilder(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public HttpClient Build()
        {
            var jwtToken = _configuration["Databuilding:Token"];            
            string baseUrl = _configuration["Databuilding:BaseUrlApi"];

            CheckToken(jwtToken);

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

            client.BaseAddress = new Uri(baseUrl);
            return client;
        }

        private static void CheckToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            if (jsonToken is not JwtSecurityToken jwtToken || jwtToken.ValidTo < DateTime.Now)
            {
                throw new Exception("Stripviewer token is expired. Get a new token from Yarodataservice.com");
            }
            else
            {
                Debug.WriteLine($"Token for Stripviewer is valid till: {jwtToken.ValidTo:F}");
            }

        }
    }
}
