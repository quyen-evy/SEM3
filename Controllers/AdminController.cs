using projectsem3.Models;
using projectsem3.Models.Dao;

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
        // COURSE
        public ActionResult Course()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(u => u.Status == false).ToList<COURSE>();
            return View("Course", course);
        }

        [HttpGet]
        public ActionResult AddCourse()
        {
            SetViewBag();
            return View();
        }
        
        [HttpGet]
        public ActionResult UpdateCourse(int id)
        {
            var course = new CourseDao().ViewDetail(id); 
            SetViewBag();
            return View(course);
        }
       
        [HttpPost]
        public string DeleteCourse(int id)
        {
           
            var course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == id && u.Status == false);
            if(course != null)
            {
                course.Status = true;
                ManageStudent.SaveChanges();
            }
            var courses = Load();
            string json = "{\"\"}";
            return null;
        }
        public List<COURSE> Load()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(u => !u.Status.Value).ToList<COURSE>();
            Session["course"] = course;
            return course;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateCourses(COURSE course, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (SaveImage(postedFile))
                {
                    if (postedFile != null)
                    {
                        course.Images = "images/" + postedFile.FileName;
                    }   
                    var cou = new CourseDao();
                    var result = cou.Update(course);
                    if (result)
                    {
                        return Content("Update Success");
                    }
                    else
                    {
                        return Content("Update course unsuccessful");
                    }
                }
                else
                {
                    return Content("Update course unsuccessful");
                }
            }
            return Content("Update course unsuccessful");
        }

        public ActionResult DeleteCourse(int id)
        {
           
            var course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == id && u.Status == false);
            if(course != null)
            {
                course.Status = true;
                ManageStudent.SaveChanges();
            }
           
            return Content ("Delete successful");
        }
        

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCourses(COURSE course, HttpPostedFileBase PostedFile)
        {
            if(SaveImage(PostedFile))
            {
                course.Images = "images/" + PostedFile.FileName;
                course.Status = false;
                course.CourseName = GetNameById(course.Id);
                ManageStudent.COURSEs.Add(course);
                ManageStudent.SaveChanges();
                return Content ("Add course successful");
            }
            return Content("Add course unsuccessful");
        }
      
        // FACILITY
        public ActionResult UpdateFacilities(int id)
        {
            SetViewBag();
            return View();
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

        public string GetNameById(int id)
        {
            return ManageStudent.COURSEs.SingleOrDefault(u => u.Status == false && u.Id == id).CourseName;
        }

    }
}