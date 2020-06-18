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
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        // GET: Course
        public ActionResult Index()
        {
            List<COURSE> course = ManageEntities.COURSEs.Where(u => u.Status == false).ToList<COURSE>();
            return View(course);
        }
    }
}