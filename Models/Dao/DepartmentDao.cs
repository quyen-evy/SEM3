using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectsem3.Models;

namespace projectsem3.Models.Dao
{
    public class DepartmentDao
    {
        private ManageStudentEntities db = null;
        public DepartmentDao()
        {
            db = new ManageStudentEntities();
        }

        public List<DEPARTMENT> ListAll()
        {
            return db.DEPARTMENTs.Where(u => u.Status == false).ToList();
        }
    }
}