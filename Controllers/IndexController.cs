﻿using projectsem3.Models;
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
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        // GET: Index
        public ActionResult Index(int id = 1)

        {

            List<DEPARTMENT> departmentID = ManageStudent.DEPARTMENTs.Where(v => v.Status == false && v.Id == id).ToList<DEPARTMENT>();
            ViewBag.departmentId = departmentID;
            List<FACULTY> faculty = ManageStudent.FACULTies.Where(v => v.Status == false && v.Id == id).ToList<FACULTY>();
            ViewBag.faculty = faculty;
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(v => v.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            return View();
        }
        
    }
}