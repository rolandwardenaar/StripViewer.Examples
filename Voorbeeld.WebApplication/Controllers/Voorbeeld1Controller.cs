using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Voorbeeld.WebApplication.Controllers
{
    public class Voorbeeld1Controller : Controller
    {
        private readonly ILogger<Voorbeeld1Controller> _logger;

        public Voorbeeld1Controller(ILogger<Voorbeeld1Controller> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("/api/getstripjson/{id}")]
        public async Task<string> GetStripJson(int id)
        {
            return   await new Api.ApiClient().GetStripJson(id);
        }

        [HttpGet("/api/getarticlejson/{id}")]
        public async Task<string> GetArticleJson(int id)
        {
            return await new Api.ApiClient().GetArticleJson(id);
        }

    }
}