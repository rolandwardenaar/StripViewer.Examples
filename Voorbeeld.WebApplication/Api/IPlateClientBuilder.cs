using System.Net.Http;

namespace Voorbeeld.WebApplication.Api
{

    public interface IPlateClientBuilder
    {
        HttpClient Build();
    }
}
