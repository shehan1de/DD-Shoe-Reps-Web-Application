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
    public class BranchProductController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            
            string loggedInUsername = Session["UserName"] as string;
            IEnumerable<mvcBranchProductModel> branchProductList;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("BranchProduct").Result;
            branchProductList = response.Content.ReadAsAsync<IEnumerable<mvcBranchProductModel>>().Result;

            branchProductList = branchProductList.Where(p => p.BranchName == loggedInUsername);

            return View(branchProductList);
        }

        public ActionResult ViewList()
        {
            IEnumerable<mvcBranchProductModel> BranchProductList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("BranchProduct").Result;
            BranchProductList = response.Content.ReadAsAsync<IEnumerable<mvcBranchProductModel>>().Result;
            return View(BranchProductList);
        }


        public ActionResult AddOrEdit(int id = 0)
        {
            mvcBranchProductModel model;

            if (id == 0)
            {
                string loggedInUsername = Session["UserName"] as string;
                model = new mvcBranchProductModel { BranchName = loggedInUsername };
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("BranchProduct/" + id.ToString()).Result;
                model = response.Content.ReadAsAsync<mvcBranchProductModel>().Result;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcBranchProductModel st, HttpPostedFileBase postedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (postedFile != null && postedFile.ContentLength > 0)
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
                        string loggedInUsername = Session["UserName"] as string;

                        if (string.IsNullOrEmpty(loggedInUsername))
                        {
                            TempData["ErrorMessage"] = "Session UserName is null or empty.";
                            return RedirectToAction("Index");
                        }

                        st.BranchName = loggedInUsername;

                        HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("BranchProduct", st).Result;
                        TempData["SuccessMessage"] = "Product Added Successfully";
                    }
                    else
                    {
                        HttpResponseMessage existingResponse = GlobalVariables.WebApiClient.GetAsync("BranchProduct/" + st.ProductID.ToString()).Result;
                        var existingModel = existingResponse.Content.ReadAsAsync<mvcBranchProductModel>().Result;

                        if (existingModel != null)
                        {
                            st.BranchName = existingModel.BranchName;
                        }

                        HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("BranchProduct/" + st.ProductID, st).Result;
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
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("BranchProduct/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }



        public ActionResult ViewStock()
        {
            IEnumerable<mvcBranchProductModel> sellProductList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("SellProduct").Result;
            sellProductList = response.Content.ReadAsAsync<IEnumerable<mvcBranchProductModel>>().Result;
            return View(sellProductList);
        }

    }


}