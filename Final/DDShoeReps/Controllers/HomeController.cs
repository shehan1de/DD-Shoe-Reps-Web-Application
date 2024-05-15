using DDShoeReps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;



namespace DDShoeReps.Controllers
{
    
    public class HomeController : Controller
    {

        DDShoeRepsEntities1 db = new DDShoeRepsEntities1();

        public ActionResult IndexList()
        {
            return View(db.Employees.ToList());
        }



        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Employee emp)
        {
            var user = db.Employees.SingleOrDefault(m => m.UserEmail == emp.UserEmail && m.UserPassword == emp.UserPassword);

            if (user != null)
            {
                Session["UserID"] = user.UserID.ToString();
                Session["UserName"] = user.UserName.ToString();
                Session["UserEmail"] = user.UserEmail.ToString();

                if (user.UserType == "Admin")

                {
                    string userName = Session["UserName"].ToString();
                    TempData["SuccessMessage"] = $"Administrative Login successful. Welcome, {userName}!";

                    return RedirectToAction("AdminDashboard", "Home");
                }
                else if (user.UserType == "Branch")
                {
                    string userName = Session["UserName"].ToString();
                    TempData["SuccessMessage"] = $"Branch Login successful. Welcome, {userName}!";
                    return RedirectToAction("BranchDashboard", "Home");
                }
                else if (user.UserType == "Partner")
                {
                    string userName = Session["UserName"].ToString();
                    TempData["SuccessMessage"] = $"Partner Login successful. Welcome, {userName}!";
                    return RedirectToAction("PartnerDashboard", "Home");
                }
                else if (user.UserType == "Customer")
                {
                    string userName = Session["UserName"].ToString();
                    TempData["SuccessMessage"] = $"Customer Login successful. Welcome, {userName}! ";
                    return RedirectToAction("CustomerDashboard", "Home");
                }
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }


        public ActionResult Logout()
        {

            Session.Clear();
            TempData["SuccessMessage"] = "Logout Successfully";
            return RedirectToAction("Login");
        }




        public ActionResult AdminDashboard()
            {
                Session["LastVisitedDashboard"] = "AdminDashboard";
                return View();
            }

            public ActionResult BranchDashboard()
            {
                Session["LastVisitedDashboard"] = "BranchDashboard";
                return View();
            }

            public ActionResult PartnerDashboard()
            {
                Session["LastVisitedDashboard"] = "PartnerDashboard";
                return View();
            }

        public ActionResult CustomerDashboard()
        {
            Session["LastVisitedDashboard"] = "CustomerDashboard";
            return View();
        }

        public ActionResult DefaultDashboard()
            {
                Session["LastVisitedDashboard"] = "DefaultDashboard";
                return View();
            }
        


      










        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
      
    }
}

    
