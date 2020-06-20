using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectsem3.Models;

namespace projectsem3.Controllers
{
    public class ContactController : Controller
    {
        private ManageStudentEntities ManageEntities = new ManageStudentEntities();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
    }
}