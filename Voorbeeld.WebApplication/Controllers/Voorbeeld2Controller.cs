using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Models;
using Voorbeeld.WebApplication.Models.Voorbeeld3;

namespace Voorbeeld.WebApplication.Controllers
{
    public class Voorbeeld2ViewModel
    {
        public int SelectedStripGroupId;
        public List<SelectListItem> StripGroupList { get; set; }
    }
    public class Voorbeeld2Controller : Controller
    {
        private readonly IConfiguration _configuration;
        readonly JsonSerializerOptions _serializeOptions;

        public Voorbeeld2Controller(IConfiguration configuration)
        {
            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var list = (await new Api.ApiClient(_configuration).GetStripGroups()).ToList();
            var vm = new Voorbeeld2ViewModel
            {
                SelectedStripGroupId = list.FirstOrDefault()?.Id ?? 0,
                StripGroupList = list.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList()
            };

            return View(vm);
        }

        /// <summary>
        /// Een of meer strips (JSON-array) via KtypeId en voertuigsoort
        /// </summary>
        /// <param name="carTypeId"></param>
        /// <param name="stripGroupId"></param>
        /// <returns></returns>

        [HttpGet("/api/getblocktextbycartype/{carTypeId}/{stripGroupId}")]
        public async Task<BlockLinkText[]> GetBlockIdsByCarTypeAsync(int carTypeId, int stripGroupId)
        {
            // CarTypeId is Ktype nummer van het voertuig.
            // stripGroup is productgroep: bv. assen, uitlaten, stuurdelen etc...

            var result = await new Api.ApiClient(_configuration).GetBlockTextsByCarTypeAsync(carTypeId, stripGroupId);
            return result;
        }


        /// <summary>
        /// Een of meer strips (JSON-array) via KtypeId en voertuigsoort
        /// </summary>
        /// <param name="carTypeId"></param>
        /// <param name="stripGroupId"></param>
        /// <returns></returns>

        [HttpGet("/api/getblocklinkbycartype/{carTypeId}/{stripGroupId}")]
        public async Task<BlockLink[]> GetBlockLinkByCarTypeAsync(int carTypeId, int stripGroupId)
        {
            // CarTypeId is Ktype nummer van het voertuig.
            // stripGroup is productgroep: bv. assen, uitlaten, stuurdelen etc...

            return await new Api.ApiClient(_configuration).GetBlockLinkByCarTypeAsync(carTypeId, stripGroupId);
        }
    }

}