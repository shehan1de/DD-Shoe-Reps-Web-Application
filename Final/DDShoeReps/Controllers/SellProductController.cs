using DDShoeReps.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DDShoeReps.Controllers
{
    [AllowAnonymous]
    public class SellProductController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            
            string loggedInUsername = Session["UserName"] as string;
            IEnumerable<mvcSellProductModel> sellProductList;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("SellProduct").Result;
            sellProductList = response.Content.ReadAsAsync<IEnumerable<mvcSellProductModel>>().Result;

            sellProductList = sellProductList.Where(p => p.SellerName == loggedInUsername);

            return View(sellProductList);
        }

        public ActionResult ViewList()
        {
            IEnumerable<mvcSellProductModel> sellProductList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("SellProduct").Result;
            sellProductList = response.Content.ReadAsAsync<IEnumerable<mvcSellProductModel>>().Result;
            return View(sellProductList);
        }


        public ActionResult AddOrEdit(int id = 0)
        {
            mvcSellProductModel model;

            if (id == 0)
            {
               
                string loggedInUsername = Session["UserName"] as string;
                model = new mvcSellProductModel { SellerName = loggedInUsername };
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("SellProduct/" + id.ToString()).Result;
                model = response.Content.ReadAsAsync<mvcSellProductModel>().Result;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcSellProductModel st, HttpPostedFileBase postedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(postedFile.FileName);
                        string fileName = "SellProductIMG-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + extension;
                        string savePath = Server.MapPath("~/image/");
                        string filePath = Path.Combine(savePath, fileName);

                        postedFile.SaveAs(filePath);
                        st.ProductImage = fileName;
                    }

                    if (st.ProductID == 0)
                    {
                        string loggedInUsername = Session["UserName"] as string;

                        if (string.IsNullOrEmpty(loggedInUsername))
                        {
                            TempData["ErrorMessage"] = "Session UserName is null or empty.";
                            return RedirectToAction("Index");
                        }

                        st.SellerName = loggedInUsername;

                        HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("SellProduct", st).Result;
                        TempData["SuccessMessage"] = "Product Added Successfully";
                    }
                    else
                    {
                        HttpResponseMessage existingResponse = GlobalVariables.WebApiClient.GetAsync("SellProduct/" + st.ProductID.ToString()).Result;
                        var existingModel = existingResponse.Content.ReadAsAsync<mvcSellProductModel>().Result;

                        if (existingModel != null)
                        {
                            st.SellerName = existingModel.SellerName;
                        }

                        HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("SellProduct/" + st.ProductID, st).Result;
                        TempData["SuccessMessage"] = "Product Updated Successfully";
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    var errorMessage = string.Join("\n", errorMessages);

                    TempData["ErrorMessage"] = errorMessage;
                    return View(st);
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while processing the request: {ex.Message}";
                return View(st);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while processing the request: {ex.Message}";
                return View(st);
            }
        }



        public ActionResult ViewStock()
        {
            IEnumerable<mvcSellProductModel> sellProductList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("SellProduct").Result;
            sellProductList = response.Content.ReadAsAsync<IEnumerable<mvcSellProductModel>>().Result;
            return View(sellProductList);
        }

    }
}