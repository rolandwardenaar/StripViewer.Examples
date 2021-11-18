using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        readonly IConfiguration _configuration;

        public ApiClient(IConfiguration configuration)
        {
            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _configuration = configuration;
        }

        HttpClient GetClient()
        {
            // token is beperkt geldig, dit is online te controleren bij: https://jwt.io/
            var jwtToken = _configuration["Databuilding:Token"];

            // het adres van de web api
            string baseurl = _configuration["Databuilding:BaseUrlApi"];

            var client = new HttpClient();

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
                return reader.ReadToEnd();
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
                return reader.ReadToEnd();
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
                return jsonReader.ReadToEnd();
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
                return jsonReader.ReadToEnd();
            }
            return "";
        }
        public async Task<BlockLinkText[]> GetBlockTextsByCarTypeAsync(int carTypeId, int stripGroupId)
        {
            var client = GetClient();
            var request = $"/api/getblocktextbycartype/{carTypeId}/{stripGroupId}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BlockLinkText[]>(result);
            }
            return null;
        }

        public async Task<BlockLink[]> GetBlockLinkByCarTypeAsync(int carTypeId, int stripGroupId)
        {
            var client = GetClient();
            var request = $"/api/getblocklinkbycartype/{carTypeId}/{stripGroupId}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BlockLink[]>(result);
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

        public async Task<IQueryable<IdName>> GetStripGroups()
        {
            var client = GetClient();
            var request = $"/api/getclientstripgroups";
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                var json = jsonReader.ReadToEnd();
                var stripgroups = System.Text.Json.JsonSerializer.Deserialize<List<IdName>>(json, _serializeOptions);
                return stripgroups.AsQueryable();
            }
            return default;
        }
    }
}
