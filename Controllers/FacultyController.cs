using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class FacultyController : Controller
    {
        // GET: Faculty
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        public ActionResult Index()
        {
            List<FACILITy> facilities = ManageEntities.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageEntities.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
    }
}