using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class CourseController : Controller
    {
        private Manage_StudentEntities _manageEn = new Manage_StudentEntities();
        // GET: Course
        public ActionResult Index(int id)
        {
           List<COURSE> courses = _manageEn.COURSEs.Where(v => v.Status == false&&v.Id==id).ToList<COURSE>();
            List<COURSE> course = _manageEn.COURSEs.Where(v => v.Status == false).ToList<COURSE>();
            TempData["course"] = course;

            List<FACILITy> facilities = _manageEn.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = _manageEn.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View(courses);
        }
    }
}