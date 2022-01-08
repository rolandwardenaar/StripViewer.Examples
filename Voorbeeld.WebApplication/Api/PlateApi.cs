using Newtonsoft.Json.Linq;
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
                    Model = new Model { Name = "Plate service needs a Token to function!!"},
                    Type = new Type { Id = 18586 }
                };
            }
            var command = $"/dev/lookup/plate?category=1&plate={plate}&country=nl";
            var result = await _plateClient.GetAsync(command);

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();

                var root = JObject.Parse(json);
                return root["data"].ToObject<CarViewModel>();
            }
            return null;
        }

    }
}
