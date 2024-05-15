using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DDShoeReps.Models
{
    public class mvcSellProductModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Display(Name = "Product Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a Number with up to two decimal places")]
        public double? ProductPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Quantity must be a Number")]
        public int? AvailableQuantity { get; set; }

        public string ProductImage { get; set; }

        public string SellerName { get; set; }

    }
}