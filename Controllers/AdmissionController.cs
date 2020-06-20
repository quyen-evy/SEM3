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
            return View();
        }

        public ActionResult Ads()
        {
            return Content("fail vcl");
        }

    }
}