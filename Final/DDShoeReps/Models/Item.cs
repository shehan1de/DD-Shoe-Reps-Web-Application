using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDShoeReps.Models
{
    public class Item
    {
        public mvcStockModel mvcStockModel { get; set; }
        
        public int Quantity { get; set; }
        public int OrderID { get; internal set; }
    }
}