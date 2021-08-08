using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

using Voorbeeld.WebApplication.Models.Voorbeeld3;

namespace Voorbeeld.WebApplication.Controllers
{
    public class Voorbeeld3Controller : Controller
    {
        readonly JsonSerializerOptions _serializeOptions;

        public Voorbeeld3Controller()
        {
            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> Index(int stripid = 17, string action = "")
        {
            if (action == "Previous")
            {
                stripid--;
            }
            if (action == "Next")
            {
                stripid++;
            }

            var vm = new SelectStripAndArticles();
            if (stripid <= 0) return View(vm);

            var supplierId = await GetFirstSupplierIdAsync();
            var json = await new Api.ApiClient().GetArticleJson(stripid, supplierId);

            vm.StripId = stripid;
            vm.SupplierArticles = JsonSerializer.Deserialize<List<SupplierArticle>>(json, _serializeOptions);

            return View(vm);
        }

        public async Task<int> GetFirstSupplierIdAsync()
        {
            return (await new Api.ApiClient().GetSuppliers()).FirstOrDefault()?.Id ?? 0;
        }

    }
}
