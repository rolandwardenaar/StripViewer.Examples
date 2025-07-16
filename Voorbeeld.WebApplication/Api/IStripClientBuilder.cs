using System.Net.Http;
using System.Threading.Tasks;

namespace Voorbeeld.WebApplication.Api
{
    public interface IStripClientBuilder
    {
        HttpClient Build();
        Task<HttpClient> BuildAsync();
    }
}
