using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectsem3.Models;

namespace projectsem3.Models.Dao
{
    public class CourseDao
    {
        private ManageStudentEntities db = null;
        public CourseDao()
        {
            db = new ManageStudentEntities();
        }

        public List<COURSE> ListAll()
        {
            return db.COURSEs.Where(u => u.Status == false).ToList();
        }

    }
}