using DDShoeReps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace DDShoeReps.Controllers
{
    public class CartController : Controller
    {
        private readonly Item db = new Item();
        private IEnumerable<mvcStockModel> mvcStockModels;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int ProductID)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                Item item = new Item();

                mvcStockModel product = GetProductDetails(ProductID);
                item.mvcStockModel = product;
                item.Quantity = 1;

                cart.Add(item);
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = IsInCart(ProductID);

                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    Item item = new Item();

                    mvcStockModel product = GetProductDetails(ProductID);
                    item.mvcStockModel = product;
                    item.Quantity = 1;

                    cart.Add(item);
                }

                Session["cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        public int IsInCart(int ProductID)
        {
            List<Item> cart = (List<Item>)Session["cart"];

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].mvcStockModel.ProductID == ProductID)
                {
                    return i;
                }
            }

            return -1;
        }

        public ActionResult RemoveFromCart(int ProductID)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = IsInCart(ProductID);

            if (index != -1)
            {
                if (cart[index].Quantity > 1)
                {
                    cart[index].Quantity--;
                }
                else
                {
                    cart.RemoveAt(index);
                }
            }

            Session["cart"] = cart;

            return RedirectToAction("Index");
        }

        private mvcStockModel GetProductDetails(int ProductID)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Stock/" + ProductID.ToString()).Result;
            return response.Content.ReadAsAsync<mvcStockModel>().Result;
        }



        private void UpdateStockQuantities(List<mvcOrderItemModel> orderItems)
        {
            foreach (var item in orderItems)
            {

                mvcStockModel stockItem = GetStockItemById(item.ProductID);

                if (stockItem != null)
                {
                    stockItem.Quantity -= item.Quantity;

                    HttpResponseMessage stockUpdateResponse = GlobalVariables.WebApiClient.PutAsJsonAsync($"Stock/{stockItem.ProductID}", stockItem).Result;
                    stockUpdateResponse.EnsureSuccessStatusCode();
                }
            }
        }

        private mvcStockModel GetStockItemById(int productId)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync($"Stock/{productId}").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<mvcStockModel>().Result;
            }

            return null;
        }


        public ActionResult PlaceOrder(string address, string paymentMethod)
        {

            if (Session["cart"] != null && Session["UserId"] != null && Session["UserName"] != null)
            {
                if (int.TryParse(Session["UserId"].ToString(), out int userId))
                {
                    string userName = Session["UserName"].ToString();

                    List<Item> cart = (List<Item>)Session["cart"];

                    double totalOrderValue = (double)cart.Sum(item => item.mvcStockModel.ProductPrice * item.Quantity);

                    mvcStockOrderModel order = new mvcStockOrderModel
                    {
                        UserID = userId,
                        UserName = userName,
                        OrderAddress = address,
                        PaymentMethod = paymentMethod,
                        TotalOrderValue = totalOrderValue,
                        OrderItems = cart.Select(item => new mvcOrderItemModel
                        {
                            ProductID = item.mvcStockModel.ProductID,
                            ProductName = item.mvcStockModel.ProductName,
                            Quantity = item.Quantity,
                            OrderValue = (double)(item.mvcStockModel.ProductPrice * item.Quantity)
                        }).ToList()
                    };


                    HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("StockOrder", order).Result;

                    UpdateStockQuantities((List<mvcOrderItemModel>)order.OrderItems);


                    TempData["SuccessMessage"] = "Order Placed Successfully";

                    Session["cart"] = null;



                    return RedirectToAction("ViewStock", "Stock");

                }
            }

            return RedirectToAction("Index");
        }




    }
}







    



