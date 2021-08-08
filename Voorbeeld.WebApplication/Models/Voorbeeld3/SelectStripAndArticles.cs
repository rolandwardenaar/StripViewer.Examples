using System.Collections.Generic;

namespace Voorbeeld.WebApplication.Models.Voorbeeld3
{
    public class SelectStripAndArticles
    {
        public int StripId { get; set; }
        public List<SupplierArticle> SupplierArticles { get; set; } = new List<SupplierArticle>();

    }
}
