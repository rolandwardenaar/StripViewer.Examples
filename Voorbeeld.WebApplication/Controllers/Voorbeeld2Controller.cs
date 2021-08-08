using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Models;

namespace Voorbeeld.WebApplication.Controllers
{
    public class Voorbeeld2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Een of meer strips (JSON-array) via KtypeId en voertuigsoort
        /// </summary>
        /// <param name="carTypeId"></param>
        /// <param name="stripGroupId"></param>
        /// <returns></returns>

        [HttpGet("/api/getblocktextbycartype/{carTypeId}")]
        public async Task<BlockLinkText[]> GetBlockIdsByCarTypeAsync(int carTypeId)
        {
            // CarTypeId is Ktype nummer van het voertuig.
            // stripGroup is productgroep: bv. assen, uitlaten, stuurdelen etc...
            int stripGroupId = 9;
            return await new Api.ApiClient().GetBlockIdsByCarTypeAsync(carTypeId, stripGroupId);
        }
      
    }
    
}