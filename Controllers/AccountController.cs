
using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class AccountController : Controller
    {
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        
        // GET: Account
        public ActionResult Login()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();

        }
        public ActionResult SignIn(string email, string password)
        {

            if (ModelState.IsValid)
            {
                string passwordMD5 = password.ToMD5();
                USER user = ManageStudent.USERs.SingleOrDefault(u => u.Email == email && u.Password == passwordMD5 && u.Status == false);
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
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
        private bool SaveImage(HttpPostedFileBase postedFile)
        {
            try
            {
                string path = Server.MapPath("../Content/images/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (postedFile != null)
                {
                    string filename = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(path + filename);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
       
    }
}