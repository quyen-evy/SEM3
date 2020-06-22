using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsem3.Controllers
{
    public class FacilitiesController : Controller
    {
        private ManageStudentEntities ManageStudent = new ManageStudentEntities();

        // GET: Facilities
        public ActionResult Index(int id = 1)
        {
            List<FACILITy> facilitiesId = ManageStudent.FACILITIES.Where(u => u.Status == false && u.Id == id).ToList<FACILITy>();
            ViewBag.facilitiesId = facilitiesId;
            List<FACILITy> facilities = ManageStudent.FACILITIES.Where(u => u.Status == false).ToList<FACILITy>();
            TempData["facilities"] = facilities;
            List<DEPARTMENT> department = ManageStudent.DEPARTMENTs.Where(u => u.Status == false).ToList<DEPARTMENT>();
            TempData["department"] = department;
            List<FEEDBACK> feedback = ManageStudent.FEEDBACKs.Where(u => u.Status == false && u.FacilitiesId == id).ToList<FEEDBACK>();
            TempData["feedback"] = feedback;
            List<COURSE> course = ManageStudent.COURSEs.Where(m => m.Status == false).ToList<COURSE>();
            TempData["courses"] = course;

            return View(facilitiesId);
        }
        public ActionResult AddFB(FEEDBACK feedback, string description, int id)
        {
            if (ModelState.IsValid)
            {
                feedback.Description = description;
                feedback.Status = false;
                feedback.StudentId = (Session["student"] as STUDENT).Id;
                feedback.Time = DateTime.Now;
                feedback.FacilitiesId = id;

                ManageStudent.FEEDBACKs.Add(feedback);
                ManageStudent.SaveChanges();
                return Content("Thanks your comment");
            }
            return Content("Please check comment");
        }
    }
}