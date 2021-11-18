using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Voorbeeld.WebApplication.Models;

namespace Voorbeeld.WebApplication.Controllers
{
    public class Voorbeeld1Controller : Controller
    {
        private readonly ILogger<Voorbeeld1Controller> _logger;
        private readonly IConfiguration _configuration;

        public static ShoppingCart ShoppingCart { get; set; }

        public Voorbeeld1Controller(ILogger<Voorbeeld1Controller> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            if (ShoppingCart == null) ShoppingCart = new ShoppingCart();
        }

        public IActionResult Index()
        {
            return View(ShoppingCart);
        }

        public IActionResult ClearCart()
        {
            ShoppingCart.Articles.Clear();
            return View("Index", ShoppingCart);
        }
        public List<string> AddArticle(string name)
        {
            ShoppingCart.Articles.Add(name);

            foreach (var item in ShoppingCart.Articles)
            {
                Debug.WriteLine(item);
            }

            return ShoppingCart.Articles;
        }

        [HttpGet("/api/getstripjson/{id}")]
        public async Task<string> GetStripJson(int id)
        {
            return await new Api.ApiClient(_configuration).GetStripJson(id);
        }

        [HttpGet("/api/getarticlejson/{id}")]
        public async Task<string> GetArticleJson(int id)
        {
            return await new Api.ApiClient(_configuration).GetArticleJson(id);
        }

    }
}