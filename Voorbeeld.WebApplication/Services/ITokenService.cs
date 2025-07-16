using System.Threading.Tasks;

namespace Voorbeeld.WebApplication.Services
{
    public interface ITokenService
    {
        Task<string> GetValidTokenAsync();
        Task<bool> IsTokenValidAsync(string token);
        Task<string> RefreshTokenAsync();
    }
}
