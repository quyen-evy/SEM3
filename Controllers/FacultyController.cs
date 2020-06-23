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
        private Manage_StudentEntities _manageEn = new Manage_StudentEntities();
        public ActionResult Index()
        {
            List<FACILITy> facilities = _manageEn.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = _manageEn.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
    }
}