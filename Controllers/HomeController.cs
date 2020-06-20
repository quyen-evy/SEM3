﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectsem3.Models;

namespace projectsem3.Controllers
{
    public class HomeController : Controller
    {
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        public ActionResult Index()
        {
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(y => y.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}