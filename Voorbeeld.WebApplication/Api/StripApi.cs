using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Models;

namespace Voorbeeld.WebApplication.Api
{

    public class StripApi
    {
        readonly JsonSerializerOptions _serializeOptions;
        readonly HttpClient _stripClient;

        public StripApi(IStripClientBuilder stripClientBuilder)
        {
            _stripClient = stripClientBuilder.Build();

            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<string> GetStripJson(int id)
        {
            var request = $"/api/getstripbyblockid/{id}";
            var response = await _stripClient.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
            return "";
        }

        public async Task<string> GetStripJsonWithPlateNumber(int id, string platenumber)
        {
            var request = $"/api/getstripbyblockid/{id}/{platenumber}";
            var response = await _stripClient.GetAsync(request);

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
            var request = $"/api/getstripbuttonsbyblockid/{id}";
            var response = await _stripClient.GetAsync(request);

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
            var request = $"/api/getstriparticles/{id}";
            var response = await _stripClient.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                return jsonReader.ReadToEnd();
            }
            return "";
        }

        public async Task<List<SupplierArticle>> GetSupplierArticles(int id)
        {
            var json = await GetArticleJson(id);
            var result = JsonSerializer.Deserialize<List<SupplierArticle>>(json, _serializeOptions);
            return result;
        }

        public async Task<string> GetArticleJson(int id, int supplierId)
        {

            var request = $"/api/getstriparticles/{id}/{supplierId}";
            var response = await _stripClient.GetAsync(request);

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
            var request = $"/api/getblocktextbycartype/{carTypeId}/{stripGroupId}";
            var response = await _stripClient.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                return JsonSerializer.Deserialize<BlockLinkText[]>(result, _serializeOptions);
            }
            return null;
        }

        public async Task<BlockLink[]> GetBlockLinkByCarTypeAsync(int carTypeId, int stripGroupId)
        {
            var request = $"/api/getblocklinkbycartype/{carTypeId}/{stripGroupId}";
            var response = await _stripClient.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);
                var result = reader.ReadToEnd();
                return JsonSerializer.Deserialize<BlockLink[]>(result, _serializeOptions);
            }
            return null;
        }

        public async Task<IQueryable<IdName>> GetSuppliers()
        {
            var request = $"/api/getclientsuppliers";
            var response = await _stripClient.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                var json = jsonReader.ReadToEnd();
                var suppliers = JsonSerializer.Deserialize<List<IdName>>(json, _serializeOptions);
                return suppliers.AsQueryable();
            }
            return default;
        }

        public async Task<IQueryable<IdName>> GetStripGroups()
        {
            var request = $"/api/getclientstripgroups";
            var response = await _stripClient.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var jsonReader = new StreamReader(stream);
                var json = jsonReader.ReadToEnd();
                var stripgroups = JsonSerializer.Deserialize<List<IdName>>(json, _serializeOptions);
                return stripgroups.AsQueryable();
            }
            return default;
        }

        public async Task<string> PostFeedback(Feedback feedback)
        {
            var command = "/api/feedback";

            string json = JsonSerializer.Serialize(feedback);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _stripClient.PostAsync(command, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "error";
        }
    }
}
