using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

using Voorbeeld.WebApplication.Models.Voorbeeld3;

namespace Voorbeeld.WebApplication.Controllers
{
    public class Voorbeeld3Controller : Controller
    {
        private readonly IConfiguration _configuration;
        readonly JsonSerializerOptions _serializeOptions;

        public Voorbeeld3Controller(IConfiguration configuration)
        {
            _configuration = configuration;

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

            var supplierId = await GetSupplierIdAsync("BPW");
            var json = await new Api.ApiClient(_configuration).GetArticleJson(stripid, supplierId);

            vm.StripId = stripid;
            vm.SupplierArticles = JsonSerializer.Deserialize<List<SupplierArticle>>(json, _serializeOptions);

            return View(vm);
        }

        public async Task<int> GetSupplierIdAsync(string name)
        {
            return (await new Api.ApiClient(_configuration)
                                 .GetSuppliers())
                                 .Where( x => x.Name == name)
                                 .FirstOrDefault()?.Id ?? 0;
        }

    }
}
