using System.Collections.Generic;

namespace Voorbeeld.WebApplication.Models
{
    public class ShoppingCart
    {
        public List<string> Articles { get; set; }

        public ShoppingCart()
        {
            Articles = new List<string>();
        }
    }
}
