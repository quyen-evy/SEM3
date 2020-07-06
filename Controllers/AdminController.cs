using projectsem3;
using projectsem3.Models;
using projectsem3.Models.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        // ----------========== ADMISSION ==========----------
        public ActionResult Admission()
        {
            return View("Admission", GetAdmission());
        }

        public ActionResult StudentApply(int id)
        {
            var student = ManageStudent.TABULARs.Find(id);
            if (student != null)
            {
                var stu = new StudentDao();
                stu.Insert(student);
                student.Status = true;
                ManageStudent.SaveChanges();
                Mail(student);
            }
            return View("Admission", GetAdmission());
        }

        public ActionResult StudentDeny(int id)
        {
            var student = ManageStudent.TABULARs.Find(id);

            if (student != null)
            {
                student.Status = null;
                ManageStudent.SaveChanges();
                DenyMail(student);
            }
            return View("Admission", GetAdmission());
        }

        public List<TABULAR> GetAdmission()
        {
            List<TABULAR> tab = ManageStudent.TABULARs.Where(u => u.Status == false).ToList<TABULAR>();
            return tab;
        }
        // ----------========== END ADMISSION ==========----------

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

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult SignUp(USER user, HttpPostedFileBase postedFile)
        {

            if (SaveImage(postedFile))
            {
                user.Avatar = "../Content/images/" + postedFile.FileName;
                user.Password = user.Password.ToMD5();
                user.Status = false;
                ManageStudent.USERs.Add(user);
                ManageStudent.SaveChanges();
                return Content("Thêm khách hàng thành công!");
            }


            return Content("Thêm thất bại");
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
        // ----------========== END LOGIN && REGISTER ==========----------

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
            var courses = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == course.Id && u.Status == false);
            if(courses != null)
            {
                courses.Images = "images/" + PostedFile.FileName;
                courses.Status = false;
                ManageStudent.COURSEs.Add(courses);
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



        // ----------------=============== FACULTY ===============---------------------
        public ActionResult Faculty()
        {
            List<FACULTY> faculty = ManageStudent.FACULTies.Where(u => u.Status == false).ToList<FACULTY>();
            return View(faculty);
        }
        [HttpGet]
        public ActionResult UpdateFaculty(int id = 1)
        {
            var faculty = ManageStudent.FACULTies.Find(id);
            SetCourseViewBag();
            return View(faculty);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateFaculty(FACULTY faculties, HttpPostedFileBase postedFile)
        {
            int facultiesId = faculties.Id;
            FACULTY faculty = ManageStudent.FACULTies.SingleOrDefault(u => u.Id == facultiesId && u.Status == false);

            if (ModelState.IsValid)
            {
                if (SaveImage(postedFile))
                {
                    if (postedFile != null)
                    {
                        faculty.Avatar = "images/" + postedFile.FileName;
                    }
                    faculty.FirstName = faculties.FirstName;
                    faculty.LastName = faculties.LastName;
                    faculty.Profiles = faculties.Profiles;
                    faculty.Birthday = faculties.Birthday;
                    faculty.Address = faculties.Address;
                    faculty.Gender = faculties.Gender;
                    ManageStudent.SaveChanges();
                    ViewBag.Status = "Update " + faculties.LastName + " successful !!!";
                }
                else
                {
                    ViewBag.Status = "Update " + faculties.LastName + " failed !!!";
                }
            }
            List<FACULTY> fac = ManageStudent.FACULTies.Where(u => u.Status == false).ToList<FACULTY>();
            return View("Faculty", fac);
        }
        [HttpGet]
        public ActionResult AddFaculty()
        {
            SetCourseViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddFaculty(FACULTY faculty, HttpPostedFileBase PostedFile)
        {
            if (SaveImage(PostedFile))
            {
                faculty.Avatar = "images/" + PostedFile.FileName;
                faculty.Status = false;
                ManageStudent.FACULTies.Add(faculty);
                ManageStudent.SaveChanges();
                ViewBag.Status = "Add Faculty " + faculty.LastName + " successful !!!";
            }
            else
            {
                ViewBag.Status = "Add Faculty " + faculty.LastName + " failed !!!";
            }
            List<FACULTY> fac = ManageStudent.FACULTies.Where(u => u.Status == false).ToList<FACULTY>();
            return View("Faculty", fac);
        }
        [HttpPost]
        public bool DeleteFaculty(int id)
        {
            var faculty = ManageStudent.FACULTies.SingleOrDefault(u => u.Id == id);
            if (faculty != null)
            {
                faculty.Status = !faculty.Status;
                ManageStudent.SaveChanges();
            }
            return (bool)faculty.Status;
        }
        // ----------------=============== END FACULTY ===============---------------------

        // ----------------=============== DEPARTMENT ===============---------------------
        public ActionResult Department()
        {
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            return View(department);
        }
        [HttpGet]
        public ActionResult AddDepartment()
        {
            SetCourseViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDepartment(DEPARTMENT department, HttpPostedFileBase PostedFile)
        {
            if (SaveImage(PostedFile))
            {
                department.Images = "images/" + PostedFile.FileName;
                department.Status = false;
                ManageStudent.DEPARTMENTs.Add(department);
                ManageStudent.SaveChanges();
                ViewBag.Status = "Add Department " + department.DepartmentName + " successful !!!";
            }
            else
            {
                ViewBag.Status = "Add Facility " + department.DepartmentName + " failed !!!";
            }
            List<DEPARTMENT> dep = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            return View("Department", dep);
        }

        [HttpPost]
        public bool DeleteDepartment(int id)
        {
            var department = ManageStudent.DEPARTMENTs.SingleOrDefault(u => u.Id == id);
            if (department != null)
            {
                department.Status = !department.Status;
                ManageStudent.SaveChanges();
            }
            return (bool)department.Status;
        }
        [HttpGet]
        public ActionResult UpdateDepartment(int id = 1)
        {
            var dep = ManageStudent.DEPARTMENTs.Find(id);
            SetCourseViewBag();
            return View(dep);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateDepartment(DEPARTMENT departments, HttpPostedFileBase postedFile)
        {
            int departmentId = departments.Id;
            DEPARTMENT department = ManageStudent.DEPARTMENTs.SingleOrDefault(u => u.Id == departmentId && u.Status == false);

            if (ModelState.IsValid)
            {
                if (SaveImage(postedFile))
                {
                    if (postedFile != null)
                    {
                        department.Images = "images/" + postedFile.FileName;
                    }
                    department.DepartmentName = departments.DepartmentName;
                    department.Description = departments.Description;
                    
                    department.Seat = departments.Seat;
                    ManageStudent.SaveChanges();
                    ViewBag.Status = "Update " + departments.DepartmentName + " successful !!!";
                }
                else
                {
                    ViewBag.Status = "Update " + departments.DepartmentName + " failed !!!";
                }
            }
            List<DEPARTMENT> dep = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            return View("Department", dep);
        }
        // ----------------=============== END DEPARTMENT ===============---------------------

        // ----------------=============== SEND MAIL ===============---------------------
        public void Mail(TABULAR student)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/SubmitMail.html"));

            content = content.Replace("{{Name}}", "ITM College");
            content = content.Replace("{{StudentName}}", student.FirstName);
            content = content.Replace("{{StudentID}}", student.UniqueID);
            content = content.Replace("{{EmailSender}}", ConfigurationManager.AppSettings["FromEmailAddress"].ToString());
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(student.Email, "Congratulation mail on ITM College Admission", content);
            new MailHelper().SendMail(toEmail, "Congratulation mail on ITM College Admission", content);
        }

        public void DenyMail(TABULAR student)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/SubmitMail.html"));

            content = content.Replace("{{Name}}", "ITM College");
            content = content.Replace("{{StudentName}}", student.FirstName);
            content = content.Replace("{{StudentID}}", student.UniqueID);
            content = content.Replace("{{EmailSender}}", ConfigurationManager.AppSettings["FromEmailAddress"].ToString());
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(student.Email, "Admission Information from ITM College", content);
            new MailHelper().SendMail(toEmail, "Admission Information from ITM College", content);
        }
        // ----------------=============== SEND MAIL END ===============---------------------
    }
}