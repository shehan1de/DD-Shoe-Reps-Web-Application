using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDShoeReps.Models
{
    public class CartItem
    {
        public mvcSellProductModel SellProduct { get; set; }

        public int CartQuantity { get; set; }
    }
}