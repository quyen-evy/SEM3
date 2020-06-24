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
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();

        // ----------========== LOGIN && REGISTER ==========----------
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
        // ----------========== END LOGIN && REGISTER ==========----------


        // ----------========== ADMISSION ==========----------
        public ActionResult Admission()
        {
            List<TABULAR> tabular = ManageStudent.TABULARs.Where(u => u.Status == false).ToList<TABULAR>();

            return View(tabular);
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
        // ----------========== END ADMISSION ==========----------

        // ----------========== COURSE ==========----------
        public ActionResult Course()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(u => u.Status == false).ToList<COURSE>();
            return View("Course", course);
        }
        public ActionResult Addcourse()
        {
            return View();
        }
        
        public ActionResult UpdateCourse(int id = 1)
        {
            COURSE course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == id && !u.Status.Value);
            SetCourseViewBag();
            return View(course);
        }
        private ActionResult UpdateCourses(COURSE course, HttpPostedFileBase postedFile)
        {
            int courseId = (Session["Course"] as COURSE).Id;
            if (ModelState.IsValid == false)
            {
                if (SaveImage(postedFile))
                {
                    var courses = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == courseId && u.Status == false);
                    courses.Images = postedFile != null ? postedFile.FileName : course.Images;
                    courses.Time = course.Time;
                    courses.FacultyId = course.FacultyId;
                    courses.DepartmentId = course.DepartmentId;
                    courses.Seat = course.Seat;
                    courses.Description = course.Description;
                    courses.Time = course.Time;

                    ManageStudent.SaveChanges();
                    ViewBag.Status = "Update successful";
                }
                else
                {
                    ViewBag.Status = "Update unsuccessful";
                }
            }
            return View("UpdateCourse", course);
        }
        public void SetCourseViewBag(long? selectedId = null)
        {
            var dao = new DepartmentDao();
            ViewBag.DepartmentID = new SelectList(dao.ListAll(), "Id", "DepartmentName");
            var fac = new FacultyDao();
            ViewBag.FacultyID = new SelectList(fac.ListAll(), "Id", "FirstName");
            var cou = new CourseDao();
            ViewBag.ID = new SelectList(cou.ListAll(), "Id", "CourseName", selectedId);
        }
        // ----------========== END COURSE ==========----------

        // ----------========== FACILITY ==========----------
        public ActionResult UpdateFacilities(int id=1)
        {
            SetCourseViewBag();
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
        // ----------========== END FACILITY ==========----------

        // ----------========== FACULTY ==========----------
    }
}