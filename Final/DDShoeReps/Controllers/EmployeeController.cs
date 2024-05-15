using DDShoeReps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace DDShoeReps.Controllers
{
    [AllowAnonymous]
    public class EmployeeController : Controller
    {


        // GET: Stock
        public ActionResult Index()
        {
            IEnumerable<mvcEmployeeModel> empList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Employee").Result;
            empList = response.Content.ReadAsAsync<IEnumerable<mvcEmployeeModel>>().Result;
            return View(empList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new mvcEmployeeModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Employee/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<mvcEmployeeModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcEmployeeModel empList)
        {
            if (ModelState.IsValid)
            {
                if (empList.UserID == 0)
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Employee", empList).Result;
                    TempData["SuccessMessage"] = "User Added Successfully";
                }
                else
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Employee/" + empList.UserID, empList).Result;
                    TempData["SuccessMessage"] = "User Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            return View(empList);
        }



        public ActionResult Delete(int id)
        {
            try
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Employee/" + id).Result;
                response.EnsureSuccessStatusCode();
                TempData["SuccessMessage"] = "User Deleted Successfully";
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

        public ActionResult SignUp(int id = 0)
        {
            if (id == 0)
                return View(new mvcEmployeeModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Employee/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<mvcEmployeeModel>().Result);
            }
        }
        [HttpPost]
        public ActionResult SignUp(mvcEmployeeModel empList)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Employee", empList).Result;
                TempData["SuccessMessage"] = "Customer Registration successful. Login now!";

            }
            return RedirectToAction("Login", "Home");
        }

        
        //public ActionResult Login()
        //{
        //    return View();
        //}



        //[HttpPost]
        //public ActionResult Login(mvcEmployeeModel emp)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Employee", new { UserEmail = emp.UserEmail, UserPassword = emp.UserPassword }).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var user = response.Content.ReadAsAsync<mvcEmployeeModel>().Result;

        //            if (user != null)
        //            {
        //                Session["UserID"] = user.UserID.ToString();
        //                Session["UserName"] = user.UserName.ToString();
        //                Session["UserEmail"] = user.UserEmail.ToString();

        //                if (user.UserType == "Admin")

        //                {
        //                    string userName = Session["UserName"].ToString();
        //                    TempData["SuccessMessage"] = $"Administrative Login successful. Welcome, {userName}!";

        //                    return RedirectToAction("AdminDashboard", "Home");
        //                }
        //                else if (user.UserType == "Branch")
        //                {
        //                    string userName = Session["UserName"].ToString();
        //                    TempData["SuccessMessage"] = $"Branch Login successful. Welcome, {userName}!";
        //                    return RedirectToAction("BranchDashboard", "Home");
        //                }
        //                else if (user.UserType == "Partner")
        //                {
        //                    string userName = Session["UserName"].ToString();
        //                    TempData["SuccessMessage"] = $"Partner Login successful. Welcome, {userName}!";
        //                    return RedirectToAction("PartnerDashboard", "Home");
        //                }
        //                else if (user.UserType == "Customer")
        //                {
        //                    string userName = Session["UserName"].ToString();
        //                    TempData["SuccessMessage"] = $"Customer Login successful. Welcome, {userName}! ";
        //                    return RedirectToAction("CustomerDashboard", "Home");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage = "Invalid email or password. Please try again.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
        //    }

        //    return View();
        //}




       

        }
    }
 
