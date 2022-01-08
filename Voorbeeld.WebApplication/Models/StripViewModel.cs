using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Voorbeeld.WebApplication.Models
{
    public class StripViewModel
    {
        public CarViewModel Car { get; set; }
        //
        public int StripGroupId { get; set; }
        public List<SelectListItem> StripGroupList { get; set; }

        public BlockLinkText[] BlockLinkTexts { get; set; }

        public List<SupplierArticle> SupplierArticles { get; set; }

        public List<SupplierArticle> ShoppingCart { get; set; }

        //
        public int StripId { get; set; }
        public int Height { get; set; } = 800;

        public StripViewModel()
        {
            Car = new CarViewModel();
            SupplierArticles = new List<SupplierArticle>();
            ShoppingCart = new List<SupplierArticle>();
        }

    }
}