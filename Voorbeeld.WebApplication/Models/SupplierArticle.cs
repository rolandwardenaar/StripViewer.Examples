namespace Voorbeeld.WebApplication.Models
{
    public class SupplierArticle
    {
        public int ArticleId { get; set; }
        public string Placeholder { get; set; }
        public int SupplierArticleId { get; set; }
        public string SupplierArticleName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        public string NrInImage { get; set; }
    }
}