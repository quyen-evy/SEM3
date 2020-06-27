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

        [HttpGet]
        public ActionResult AddCourse()
        {
            SetCourseViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCourses(COURSE course, HttpPostedFileBase PostedFile)
        {
            if (SaveImage(PostedFile))
            {
                course.Images = "images/" + PostedFile.FileName;
                course.Status = false;
                ManageStudent.COURSEs.Add(course);
                ManageStudent.SaveChanges();
                return Content("Add course successful");
            }
            return Content("Add course unsuccessful");
        }

        [HttpGet]
        public ActionResult UpdateCourse(int id)
        {
            var course = new CourseDao().ViewDetail(id); 
            SetCourseViewBag();
            return View(course);
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

        [HttpPost]
        public bool DeleteCourse(int id)
        {
            var course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == id);
            if(course != null)
            {
                course.Status = !course.Status;
                ManageStudent.SaveChanges();
            }
            return (bool)course.Status;
        }

        public void SetCourseViewBag(long? selectedId = null)
        {
            var dao = new DepartmentDao();
            ViewBag.DepartmentID = new SelectList(dao.ListAll(), "Id", "DepartmentName", selectedId);
            var fac = new FacultyDao();
            ViewBag.FacultyID = new SelectList(fac.ListAll(), "Id", "FirstName", selectedId);
            var cou = new CourseDao();
            ViewBag.ID = new SelectList(cou.ListAll(), "Id", "CourseName", selectedId);
        }


        // ----------=============== END COURSE ===============-----------------------




        // ----------------=============== FACILITY ===============---------------------
        public ActionResult Facility()
        {
            List<FACILITy> facility = ManageStudent.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            return View(facility);
        }

        [HttpGet]
        public ActionResult AddFacility()
        {
            SetFacViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddFacility(FACILITy facility, HttpPostedFileBase PostedFile)
        {
            if (SaveImage(PostedFile))
            {
                facility.Images = "images/" + PostedFile.FileName;
                facility.Status = false;
                ManageStudent.FACILITIES.Add(facility);
                ManageStudent.SaveChanges();
                ViewBag.Status = "Add Facility " +facility.Name +" successful !!!";
            }
            else
            {
                ViewBag.Status = "Add Facility " + facility.Name + " failed !!!";
            }
            List<FACILITy> fac = ManageStudent.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            return View("Facility", fac);
        }

        [HttpGet]
        public ActionResult UpdateFacility(int id=1)
        {
            var facility = ManageStudent.FACILITIES.Find(id);
            SetFacViewBag();
            return View(facility);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateFacility(FACILITy facilities, HttpPostedFileBase postedFile)
        {
            int facilitiesId = facilities.Id;
            FACILITy facility = ManageStudent.FACILITIES.SingleOrDefault(u => u.Id == facilitiesId && u.Status == false);
       
            if(ModelState.IsValid)
            {
                if(SaveImage(postedFile))
                {
                    if (postedFile != null)
                    {
                        facility.Images = "images/" + postedFile.FileName;
                    }
                    facility.Name = facilities.Name;
                    facility.Description = facilities.Description;
                    facility.Time = facilities.Time;
                    facility.writer = facilities.writer;
                    ManageStudent.SaveChanges();
                    ViewBag.Status = "Update "+facilities.Name+" successful !!!";
                }
                else
                {
                    ViewBag.Status = "Update " + facilities.Name + " failed !!!";
                }
            }
            List<FACILITy> fac = ManageStudent.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            return View("Facility", fac);
        }

        [HttpPost]
        public bool DeleteFacility(int id)
        {
            var facility = ManageStudent.FACILITIES.SingleOrDefault(u => u.Id == id);
            if (facility != null)
            {
                facility.Status = !facility.Status;
                ManageStudent.SaveChanges();
            }
            return (bool)facility.Status;
        }

        public void SetFacViewBag(int? selectedId = null)
        {
            var fac = new FacilityDao();
            ViewBag.FacilityId = new SelectList(fac.ListAll(), "Id", "Name", selectedId);
        }
        // ----------------=============== END FACILITY ===============---------------------

        // ----------------=============== OTHERS ===============---------------------


    }
}