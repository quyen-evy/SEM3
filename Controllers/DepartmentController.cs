using projectsem3.Models;
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
        private Manage_StudentEntities _manageEn = new Manage_StudentEntities();
        // GET: Department
        public ActionResult Index(int id)
        {

            List<COURSE> courses = _manageEn.COURSEs.Where(u => u.Status == false&&u.DepartmentId==id).ToList<COURSE>();
            List<COURSE> course = _manageEn.COURSEs.Where(v => v.Status == false).ToList<COURSE>();
            TempData["course"] = course;
            List<DEPARTMENT> departmentID = _manageEn.DEPARTMENTs.Where(v => v.Status == false && v.Id == id).ToList<DEPARTMENT>();
            ViewBag.departmentId = departmentID;
            List<DEPARTMENT> department = _manageEn.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = _manageEn.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            return View(courses);
        }
      
    }
}