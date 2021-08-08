using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Models;
using Voorbeeld.WebApplication.Models.Voorbeeld3;

namespace Voorbeeld.WebApplication.Controllers.Api
{
    public class ApiClient
    {
        readonly JsonSerializerOptions _serializeOptions;

        public ApiClient()
        {
            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        HttpClient GetClient()
        {
            // token is beperkt geldig, dit is online te controleren bij: https://jwt.io/
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJEZW1vIiwicm9sZSI6IjBlZmQzYWVmLTdhOTQtNGU2NC04MmU1LTBmMzM2M2U1NGU4ZCIsIm5iZiI6MTYxNjIzMzE5OCwiZXhwIjoxNjQ3NzY5MTk4LCJpYXQiOjE2MTYyMzMxOTgsImlzcyI6Imh0dHBzOi8vZGF0YWJ1aWxkaW5nLmNvbS8iLCJhdWQiOiJodHRwczovL2RhdGFidWlsZGluZy5henVyZXdlYnNpdGVzLm5ldC8ifQ.Ka5wmRFTkR9TeMf643uiEJKCBRRLfAQNFBfOBaTtvyU";
            // het adres van de web api
            string baseurl = "https://databuilding.azurewebsites.net";
            //string baseurl = "https://localhost:44301";// webApi
            HttpClient client = new HttpClient();
            // tussen Bearer en het JWT-token altijd 1 spatie!
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
            client.BaseAddress = new Uri(baseurl);
            return client;
        }

        public async Task<string> GetStripJson(int id)
        {
            var client = GetClient();
            var request = $"/api/getstripbyblockid/{id}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                Debug.WriteLine(result);
                return result;
            }
            return "";
        }

        public async Task<string> GetStripForHotspotImageJson(int id)
        {
            var client = GetClient();
            var request = $"/api/getstripbuttonsbyblockid/{id}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                Debug.WriteLine(result);
                return result;
            }
            return "";
        }

        public async Task<string> GetArticleJson(int id)
        {
            var client = GetClient();
            var request = $"/api/getstriparticles/{id}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                var json = jsonReader.ReadToEnd();
                return json;
            }
            return "";
        }

        public async Task<string> GetArticleJson(int id, int supplierId)
        {
            var client = GetClient();
            var request = $"/api/getstriparticles/{id}/{supplierId}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                var json = jsonReader.ReadToEnd();
                return json;
            }
            return "";
        }
        public async Task<BlockLinkText[]> GetBlockIdsByCarTypeAsync(int carTypeId, int stripGroupId)
        {
            var client = GetClient();
            var request = $"/api/getblocktextbycartype/{carTypeId}/{stripGroupId}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                Debug.WriteLine(result);
                return JsonConvert.DeserializeObject<BlockLinkText[]>(result);
            }
            return null;
        }

        public async Task<IQueryable<IdName>> GetSuppliers()
        {
            var client = GetClient();
            var request = $"/api/getclientsuppliers";
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                var json = jsonReader.ReadToEnd();
                var suppliers = System.Text.Json.JsonSerializer.Deserialize<List<IdName>>(json, _serializeOptions);
                return suppliers.AsQueryable();
            }
            return default;
        }
    }
}
