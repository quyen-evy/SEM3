using projectsem3.Models;
using projectsem3.Models.Dao;
using projectsem3.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admission()
        {
            List<TABULAR> tabular = ManageStudent.TABULARs.Where(u => u.Status == false).ToList<TABULAR>();

            return View(tabular);
        }
        public ActionResult UpdateFacilities(int id)
        {
            SetViewBag();
            //var get = GetNameById(id);
            //return ViewBag.Fac = new SelectList(get, "Id", "Name", id);
            return View();
        }
        //public IEnumerable<FACILITy> GetNameById(int id)
        //{
        //    List<FacModel> fac = new List<FacModel>();
        //    foreach (var facs in ManageStudent.FACILITIES.Where(u => u.Status == false).ToList())
        //    {
        //        FacModel fm = new FacModel(facs.Id);
        //        fac.Add(fm);
        //    }                
        //    return null;
        //}

            public ActionResult UpdateCourse()
        {
            SetViewBag();
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
                USER user = ManageStudent.USERs.SingleOrDefault(u => u.Email == email && u.Password == passwordMD5 && u.Status == false);
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
            int facilitiesId = facilities.Id;
            FACILITy facility = ManageStudent.FACILITIES.SingleOrDefault(u => u.Id == facilitiesId && u.Status == false);
       
            if(ModelState.IsValid)
            {
                if(SaveImage(postedFile))
                {
                    facility.Images = postedFile != null ? postedFile.FileName : facilities.Images;
                    facility.Description = facilities.Description;
                    facility.Time = facilities.Time;
                    facility.writer = facilities.writer;
                    ManageStudent.SaveChanges();
                    ViewBag.Status = "Update successful";
                }
                else
                {
                    ViewBag.Status = "Update unsuccessful";
                }
            }
            return View("Facilities", facility);
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new DepartmentDao();
            ViewBag.DepartmentID = new SelectList(dao.ListAll(), "Id", "DepartmentName", selectedId);
            var fac = new FacultyDao();
            ViewBag.FacultyID = new SelectList(fac.ListAll(), "Id", "FirstName", selectedId);
            var cou = new CourseDao();
            ViewBag.ID = new SelectList(cou.ListAll(), "Id", "CourseName", selectedId);
        }
    }
}