using projectsem3.Models;
using projectsem3.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace projectsem3.Controllers
{
    public class DepartmentController : Controller
    {
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        // GET: Department
        public ActionResult Index(int id = 1)
        {

            List<DEPARTMENT> departmentID = ManageStudent.DEPARTMENTs.Where(v => v.Status == false && v.Id == id).ToList<DEPARTMENT>();
            ViewBag.departmentId = departmentID;
            List<COURSE> courses = ManageStudent.COURSEs.Where(m => m.Status == false && m.DepartmentId == id).ToList<COURSE>();
            ViewBag.courses = courses;
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            return View();
        }

    }
}