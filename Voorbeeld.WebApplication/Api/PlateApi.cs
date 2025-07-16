using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Models;

namespace Voorbeeld.WebApplication.Api
{
    public class PlateApi
    {
        readonly HttpClient _plateClient;

        public PlateApi(IPlateClientBuilder plateClientBuilder)
        {
            _plateClient = plateClientBuilder.Build();
        }

        public async Task<CarViewModel> Plate(string plate)
        {
            if (_plateClient == null)
            {
                return new CarViewModel
                {
                    Plate = plate,
                    Brand = new Brand { Name = "Dummy Car" },
                    Model = new Model { Name = "The Plate Service needs a Token to function, a fixed CarTypeId=18586 will be used!!" },
                    Type = new Type { Id = 18586 }
                };
            }
            var command = $"/dev/lookup/plate?category=1&plate={plate}&country=nl";
            var result = await _plateClient.GetAsync(command);

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();

                var root = JsonDocument.Parse(json);
                return JsonSerializer.Deserialize<CarViewModel>(root.RootElement.GetProperty("data").GetRawText());
            }
            return null;
        }

    }
}
