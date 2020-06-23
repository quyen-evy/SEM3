using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectsem3.Models;
namespace projectsem3.Controllers
{
    public class IndexController : Controller
    {
        private Manage_StudentEntities _manageEn = new Manage_StudentEntities();
        // GET: Index
        public ActionResult Index()
        
        {
            List<COURSE> course= _manageEn.COURSEs.Where(v => v.Status == false).ToList<COURSE>();
            TempData["course"] = course;

            List<FACILITy> facilities= _manageEn.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = _manageEn.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
    }
}