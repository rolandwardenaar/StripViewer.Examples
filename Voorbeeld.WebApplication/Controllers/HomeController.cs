using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Api;
using Voorbeeld.WebApplication.Models;

namespace Voorbeeld.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        readonly StripViewModel _stripViewModel;
        readonly StripApi _stripApi;
        readonly PlateApi _plateApi;

        public HomeController(IStripClientBuilder stripClientBuilder, IPlateClientBuilder plateClientBuilder)
        {
            _stripApi = new StripApi(stripClientBuilder);
            _plateApi = new PlateApi(plateClientBuilder);

            _stripViewModel = new StripViewModel();
        }

        public IActionResult Index()
        {
            _stripViewModel.Car.Plate = "23nkz7";
            return View(_stripViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetCar(StripViewModel vm)
        {
            var result = await _plateApi?.Plate(vm.Car.Plate);

            var stripGroups = (await _stripApi.GetStripGroups())
                                              .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                                              .ToList();
            vm.StripGroupList = stripGroups;
            vm.Car = result;

            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetStripMenu(StripViewModel vm)
        {
            var stripGroups = (await _stripApi.GetStripGroups()).ToList();
            vm.StripGroupList = stripGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            var result = await _stripApi.GetBlockTextsByCarTypeAsync(vm.Car.Type.Id, vm.StripGroupId);

            if (result?.Length > 0)
            {
                // show menu options : 
                vm.BlockLinkTexts = result;
            }
            else
            {
                // only one linked blockid, so show strip without menu :

                var r = await _stripApi.GetBlockLinkByCarTypeAsync(vm.Car.Type.Id, vm.StripGroupId);
                if (r != null && r.Length > 0)
                {
                    vm.StripId = r.FirstOrDefault().BlockId;
                    vm.SupplierArticles = await _stripApi.GetSupplierArticles(vm.StripId);

                    if (stripGroups.FirstOrDefault(x => x.Id == vm.StripGroupId).Name == "Exhaust")
                    {
                        vm.Height = 400; // exhaust strops are smaller than other systems
                    }
                    else
                    {
                        vm.Height = 800;
                    }
                }

            }

            return View("Index", vm);
        }


        [HttpPost]
        public async Task<ActionResult> GetStrip(StripViewModel vm)
        {
            var stripGroups = (await _stripApi.GetStripGroups()).ToList();
            vm.StripGroupList = stripGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            vm.SupplierArticles = await _stripApi.GetSupplierArticles(vm.StripId);

            if (stripGroups?.FirstOrDefault(x => x.Id == vm.StripGroupId).Name == "Exhaust")
            {
                vm.Height = 400;
            }
            else
            {
                vm.Height = 800;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// This call is expected and used by strip-viewer
        /// It is used for calls with only the blockid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/Home/api/getstripjson/{id}")]
        public async Task<string> GetStripJson(int id)
        {
            return await _stripApi.GetStripJson(id);
        }

        /// <summary>
        /// This call is expected and used by strip-viewer
        /// It is used for calls with the blockid and the platenumber
        /// </summary>
        /// <param name="id"></param>
        /// <param name="platenumber"></param>
        /// <returns></returns>
        [HttpGet("/Home/api/getstripjson/{id}/{platenumber}")]
        public async Task<string> GetStripJson(int id, string platenumber)
        {
            return await _stripApi.GetStripJsonWithPlateNumber(id, platenumber);
        }

    }
}