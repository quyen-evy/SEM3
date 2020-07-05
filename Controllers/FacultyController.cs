using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace projectsem3.Controllers
{
    public class FacultyController : Controller
    {
        // GET: Faculty
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();
        public ActionResult Index(int? page)
        {

            
            List<COURSE> course = ManageStudent.COURSEs.Where(v => v.Status == false).ToList<COURSE>();
            TempData["courses"] = course;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;


            if (page == null) page = 1;
            int pageSize = 2;
            int pageNumber = page ?? 1;

            List<FACULTY> products = ManageStudent.FACULTies.Where(u => u.Status == false ).ToList<FACULTY>();
            return View(products.ToPagedList(pageNumber, pageSize));
        }
    }
}