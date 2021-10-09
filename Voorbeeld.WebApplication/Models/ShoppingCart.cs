using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
