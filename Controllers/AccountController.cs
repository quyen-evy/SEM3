
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
            SetTempData();
            return View();

        }
        public ActionResult SignIn(string studentID, string password)
        {

            if (ModelState.IsValid)
            {
                string passwordMD5 = password.ToMD5();
                STUDENT student = ManageStudent.STUDENTs.SingleOrDefault(u => u.StudentID == studentID && u.Password == passwordMD5 && u.Status == false);
                if (student != null)
                {
                    if(student.CourseId != null)
                    {
                        Session["student"] = student;
                        return RedirectToAction("Index", "Index");
                    }
                    else
                    {
                        Session["student"] = student;
                        return RedirectToAction("Select", "Admission");
                    }
                }
            }
            ViewBag.Status = "The email or the password that you've entered is incorrect";
            SetTempData();
            return View("Login");
        }
        public ActionResult Register()
        {
            if (Session["student"] != null)
            {
                int Id = (Session["student"] as STUDENT).Id;
                STUDENT stu = ManageStudent.STUDENTs.SingleOrDefault(u => u.Id == Id && u.Status == false);
                SetTempData();
                return View(stu);
            }
            SetTempData();
            return View();
        }

        public ActionResult Profile()
        {
            if (Session["student"] == null)
            {
                return RedirectToAction("Login");
            }
            SetTempData();
            int studentId = (Session["student"] as STUDENT).Id;
            STUDENT student = ManageStudent.STUDENTs.SingleOrDefault(u => u.Id == studentId && u.Status == false);
            return View(student);
        }
        public ActionResult SignUp(STUDENT student, string ConfirmPassword)
        {
            STUDENT stu = ManageStudent.STUDENTs.SingleOrDefault(u => u.StudentID == student.StudentID && u.Status == false);
            if (stu.Email != student.Email)
            {
                ViewBag.Status = "Email is invalid !";
                SetTempData();
                return View("Register");
            }
            if (student.Password != ConfirmPassword)
            {
                ViewBag.Status = "Password Confirmation Failed !";
                SetTempData();
                return View("Register");
            }
            stu.Password = student.Password.ToMD5();
            ManageStudent.SaveChanges();
            Session["student"] = stu;
            
            if (stu.CourseId != null)
            {
                return RedirectToAction("Index", "Index");
            }
            return RedirectToAction("Select", "Admission");
        }

        public ActionResult Logout()
        {
            Session["student"] = null;

            return RedirectToAction("Index","Index");
        }

        public ActionResult Update(STUDENT student, HttpPostedFileBase postedFile)
        {
            int Id = (Session["student"] as STUDENT).Id;
            STUDENT stu = ManageStudent.STUDENTs.SingleOrDefault(u => u.Id == Id && u.Status == false);
            if (stu!=null)
            {
                if (SaveImage(postedFile))
                {
                    stu.Avatar = "images/" + postedFile.FileName;
                }
                stu.FirstName = student.FirstName;
                stu.LastName = student.LastName;
                stu.Birthday = student.Birthday;
                stu.Gender = student.Gender;
                stu.FatherName = student.FatherName;
                stu.MotherName = student.MotherName;
                stu.ResidentialAddress = student.ResidentialAddress;
                stu.PermanentAddress = student.PermanentAddress;
                stu.AdmissionFor = student.AdmissionFor;
                stu.Sports = student.Sports;
                ManageStudent.SaveChanges();
                Session["student"] = stu;
                ViewBag.Status = "Update Successful";
                SetTempData();
                return View("Profile",stu);
            }
            ViewBag.Status = "Update failed";
            SetTempData();
            return View("Profile",stu);


        }

        private bool SaveImage(HttpPostedFileBase postedFile)
        {
            try
            {
                string path = Server.MapPath("../Content/");
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

        public void SetTempData()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
        }
       
    }
}