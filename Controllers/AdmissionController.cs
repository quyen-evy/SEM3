using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

        public ActionResult Ads()
        {
            return Content("fail vcl");
        }

    }
}