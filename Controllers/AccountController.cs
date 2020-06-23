using DemoMVC;
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

        private Manage_StudentEntities _manageEn = new Manage_StudentEntities();
        // GET: Account
        public ActionResult Login()
        {
            
            List<FACILITy> facilities = _manageEn.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = _manageEn.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
        public ActionResult Register()
        {
            List<FACILITy> facilities = _manageEn.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = _manageEn.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
        public ActionResult SignIn(string email, string password)
        {
            if (ModelState.IsValid)
            {
                string passMD5 = password.ToMD5();
                STUDENT student = _manageEn.STUDENTs.SingleOrDefault(u => u.Email == email && u.Password == passMD5 && !u.Status.Value);
                if (student != null)
                {
                    Session["student"] = student;
                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    return Content("Login false");
                }
            }
            return View("login");
        }
    }
}