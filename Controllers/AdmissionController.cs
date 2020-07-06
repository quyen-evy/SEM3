using projectsem3.Models;
using projectsem3.Models.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace projectsem3.Controllers
{
    public class AdmissionController : Controller
    {
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        // GET: Admission
        public ActionResult Index()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Select()
        {
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            SetStudentViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Select(STUDENT tabular)
        {
            if (ModelState.IsValid)
            {
                COURSE course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == tabular.CourseId && u.Status == false);
                tabular.DepartmentId = course.DepartmentId;
                var session = (Session["student"] as STUDENT).Id;
                STUDENT student = ManageStudent.STUDENTs.Find(session);
                student.CourseId = tabular.CourseId;
                student.DepartmentId = tabular.DepartmentId;
                ManageStudent.SaveChanges();
            }
                return RedirectToAction("Index");
        }

        public ActionResult Check(string SearchString)
        {
            if(!string.IsNullOrEmpty(SearchString))
            {
                TABULAR tab = ManageStudent.TABULARs.FirstOrDefault(u => u.UniqueID == SearchString);
                if (tab == null)
                {
                    ViewBag.Status = "The UniqueID " + SearchString + " you entered is not exist !!! Please Try again";
                }
                else
                {
                    if (tab.Status == false)
                    {
                        ViewBag.Status = "The Student " + tab.FirstName + " is waiting for submission. Please Wait and follow your email !";
                    }
                    else if (tab.Status == true)
                    {
                        ViewBag.Status = "The Student " + tab.FirstName + " admission is successful. Congratulation !";
                    }
                    else
                    {
                        ViewBag.Status = "The Student " + tab.FirstName + " has failed admission !";
                    }
                }
            }

            ViewBag.SearchString = SearchString;
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            return View("Check");
        }

        public ActionResult Submit(TABULAR tabular)
        {
            tabular.Status = false;
            ManageStudent.TABULARs.Add(tabular);
            ManageStudent.SaveChanges();
            ViewBag.Status = "Register for " + tabular.FirstName + " successful !!!";
            Mail(tabular);

            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            SetViewBag();
            return View("Index", tabular);
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new DepartmentDao();
            ViewBag.DepartmentID = new SelectList(dao.ListAll(), "Id", "DepartmentName", selectedId);
        }

        public void SetStudentViewBag(int? selectedId = null)
        {
            var dao = new CourseDao();
            ViewBag.CourseID = new SelectList(dao.ListAll(), "Id", "CourseName", selectedId);
        }

        public string GetCourseById(int id)
        {
            COURSE course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == id && u.Status == false);
            return course.CourseName;
        }

        public int GetDepartmentByCourse(int id)
        {
            COURSE course = ManageStudent.COURSEs.SingleOrDefault(u => u.Id == id && u.Status == false);
            return course.DepartmentId ?? 0;
        }

        public void Mail(TABULAR student)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/AdmissionEmail.html"));

            content = content.Replace("{{Name}}", "ITM College");
            content = content.Replace("{{StudentName}}", student.FirstName);
            content = content.Replace("{{StudentID}}", student.UniqueID);
            content = content.Replace("{{EmailSender}}", ConfigurationManager.AppSettings["FromEmailAddress"].ToString());
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(student.Email, "ITM College thank you for you Admission", content);
            new MailHelper().SendMail(toEmail, "ITM College thank you for you Admission", content);
        }
    }
}