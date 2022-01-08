using System.Net.Http;

namespace Voorbeeld.WebApplication.Api
{
    public interface IStripClientBuilder
    {
        HttpClient Build();
    }
}
