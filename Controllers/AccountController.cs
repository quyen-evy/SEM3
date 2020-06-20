
using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class AccountController : Controller
    {
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        
        // GET: Account
        public ActionResult Login()
        {
            List<DEPARTMENT> department = ManageEntities.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageEntities.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            
            return View();

        }
        public ActionResult SignIn(string email, string password)
        {

            if (ModelState.IsValid)
            {
                string passwordMD5 = password.ToMD5();
                USER user = ManageEntities.USERs.SingleOrDefault(u => u.Email == email && u.Password == passwordMD5 && u.Status == false);
                if (user != null)
                {
                    Session["user"] = user;
                    if (user.Role == 4)
                    {
                        return Content("User Admin");
                        //return RedirectToAction("Index", "Index");
                    }
                    else
                    {
                        return Content("test");
                        //return RedirectToAction("Admission", "Admin");
                    }
                }
            }
            ViewBag.SignInErrorMessage = "The email or the password that you've entered is incorrect";
            // return Content("Test");
            return RedirectToAction("Index","Error");
        }
        public ActionResult Register()
        {
            return View();
        }

    }
}