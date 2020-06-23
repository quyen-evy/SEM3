using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectsem3.Models;

namespace projectsem3.Models.Dao
{
	public class FacultyDao
	{
        private ManageStudentEntities db = null;
        public FacultyDao()
        {
            db = new ManageStudentEntities();
        }

        public List<FACULTY> ListAll()
        {
            return db.FACULTies.Where(u => u.Status == false).ToList();
        }
    }
}