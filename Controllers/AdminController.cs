using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admission()
        {
            List<TABULAR> tabular = ManageEntities.TABULARs.Where(u => u.Status == false).ToList<TABULAR>();

            return View(tabular);
        }
        public ActionResult Update()
        {
            return View();
        }
        public ActionResult Login()
        {
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
                    return RedirectToAction("Admission", "Admin");

                }
            }
            ViewBag.SignInErrorMessage = "The email or the password that you've entered is incorrect";
            return View("Index", "Error");
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
        private ActionResult UpdateFacilities(FACILITy facilities, HttpPostedFileBase postedFile)
        {
            FACILITy facility = ManageEntities.FACILITIES.SingleOrDefault(u => u.Status == false);
            if(ModelState.IsValid)
            {
                if(SaveImage(postedFile))
                {
                    facility.Images = postedFile != null ? postedFile.FileName : facilities.Images;
                    facility.Description = facilities.Description;
                    facility.Time = facilities.Time;
                    facility.writer = facilities.writer;
                    ManageEntities.SaveChanges();
                    ViewBag.Status = "Update successful";
                }
                else
                {
                    ViewBag.Status = "Update unsuccessful";
                }
                return View("Update", facility);
            }
            return null;
        }

    }
}