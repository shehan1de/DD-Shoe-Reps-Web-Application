using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDShoeReps.Models
{
    public class mvcOrderItemModel
    {
        
            public int OrderItemID { get; set; }
            public Nullable<int> OrderID { get; set; }
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public double OrderValue { get; set; }

            
        [JsonIgnore] 
        public virtual mvcStockOrderModel StockOrder { get; set; }



    }
}