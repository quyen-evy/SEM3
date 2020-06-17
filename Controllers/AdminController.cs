using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admission()
        {
            return View();
        }
        public ActionResult Information(int id)
        {
            STUDENT stu = ManageEntities.STUDENTs.SingleOrDefault(u => u.Id == id && u.Status == false);
            TempData["student"] = stu;
            return View();
        }
    }
}