using DDShoeReps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DDShoeReps.Controllers
{
    public class SellCartController : Controller
    {
        private readonly CartItem db = new CartItem();
        private IEnumerable<mvcSellProductModel> mvcSellProductModels;
        // GET: SellCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int ProductID)
        {
            if (Session["cart"] == null)
            {
                List<CartItem> cart = new List<CartItem>();
                CartItem cartItem = new CartItem();

                mvcSellProductModel product = GetProductDetails(ProductID);
                cartItem.SellProduct = product;
                cartItem.CartQuantity = 1;

                cart.Add(cartItem);
                Session["cart"] = cart;
            }
            else
            {
                List<CartItem> cart = (List<CartItem>)Session["cart"];
                int index = IsInCart(ProductID);

                if (index != -1)
                {
                    cart[index].CartQuantity++;
                }
                else
                {
                    CartItem cartItem = new CartItem();

                    mvcSellProductModel product = GetProductDetails(ProductID);
                    cartItem.SellProduct = product;
                    cartItem.CartQuantity = 1;

                    cart.Add(cartItem);
                }

                Session["cart"] = cart;
            }

            return RedirectToAction("Index");
        }
        public int IsInCart(int ProductID)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].SellProduct.ProductID == ProductID)
                {
                    return i;
                }
            }

            return -1;
        }
        public ActionResult RemoveFromCart(int ProductID)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            int index = IsInCart(ProductID);

            if (index != -1)
            {
                if (cart[index].CartQuantity > 1)
                {
                    cart[index].CartQuantity--;
                }
                else
                {
                    cart.RemoveAt(index);
                }
            }

            Session["cart"] = cart;

            return RedirectToAction("Index");
        }
        private mvcSellProductModel GetProductDetails(int ProductID)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("SellProduct/" + ProductID.ToString()).Result;
            return response.Content.ReadAsAsync<mvcSellProductModel>().Result;
        }
        [HttpPost]
        public ActionResult PlaceOrder(string address, string paymentMethod)
        {
            TempData["SuccessMessage"] = "Order placed successfully.";

            return RedirectToAction("ViewList", "SellProduct");
        }
    }
}