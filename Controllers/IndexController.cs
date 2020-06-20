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
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        // GET: Index
        public ActionResult Index()

        {


            List<FACILITy> facilities = ManageEntities.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageEntities.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
        
    }
}