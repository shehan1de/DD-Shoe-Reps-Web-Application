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
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            IEnumerable<mvcStockModel> stockList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Stock").Result;
            stockList = response.Content.ReadAsAsync<IEnumerable<mvcStockModel>>().Result;
            return View(stockList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new mvcStockModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Stock/" + id).Result;
                return View(response.Content.ReadAsAsync<mvcStockModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcStockModel st, HttpPostedFileBase postedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (postedFile != null)
                    {
                        string extension = Path.GetExtension(postedFile.FileName);
                        string fileName = "IMG-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + extension;
                        string savePath = Server.MapPath("~/image/");
                        string filePath = Path.Combine(savePath, fileName);

                        postedFile.SaveAs(filePath);
                        st.ProductImage = fileName; 
                    }

                    if (st.ProductID == 0)
                    {
                        HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Stock", st).Result;
                        response.EnsureSuccessStatusCode();
                        TempData["SuccessMessage"] = "Product Added Successfully";
                    }
                    else
                    {
                        HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Stock/" + st.ProductID, st).Result;
                        response.EnsureSuccessStatusCode();
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


        public ActionResult Delete(int id)
        {
            try
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Stock/" + id).Result;
                response.EnsureSuccessStatusCode();
                TempData["SuccessMessage"] = "Product Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while processing the request: {ex.Message}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while processing the request.";
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult ViewStock()
        {
            IEnumerable<mvcStockModel> stockList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Stock").Result;
            stockList = response.Content.ReadAsAsync<IEnumerable<mvcStockModel>>().Result;
            return View(stockList);
        }
    }
}
