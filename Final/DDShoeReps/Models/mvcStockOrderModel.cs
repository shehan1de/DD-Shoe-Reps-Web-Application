using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDShoeReps.Models
{
    public class mvcStockOrderModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string OrderAddress { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalOrderValue { get; set; }


        public virtual ICollection<mvcOrderItemModel> OrderItems { get; set; }
    }


}