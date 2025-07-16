using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Services;

namespace Voorbeeld.WebApplication.Api
{

    public class StripClientBuilder : IStripClientBuilder
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public StripClientBuilder(IHttpClientFactory clientFactory, IConfiguration configuration, ITokenService tokenService)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public HttpClient Build()
        {
            return BuildAsync().GetAwaiter().GetResult();
        }

        public async Task<HttpClient> BuildAsync()
        {
            string baseUrl = _configuration["Yaro:BaseUrlApi"];
            var jwtToken = await _tokenService.GetValidTokenAsync();

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
            client.BaseAddress = new Uri(baseUrl);
            return client;
        }
     
    }
}
