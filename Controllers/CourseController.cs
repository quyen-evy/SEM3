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
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        // GET: Course
        public ActionResult Index()
        {
            //List<COURSE> courseID = ManageStudent.COURSEs.Where(u => u.Status == false && u.Id == id).ToList<COURSE>();
            //ViewBag.courseId = courseID;
            //List<COURSE> course = ManageStudent.COURSEs.Where(u => u.Status == false ).ToList<COURSE>();
            //TempData["courses"] = course;
            return View();
        }
    }
}