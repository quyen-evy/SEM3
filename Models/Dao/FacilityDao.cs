using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectsem3.Models;

namespace projectsem3.Models.Dao
{
    public class FacilityDao
    {
        private ManageStudentEntities db = null;
        public FacilityDao()
        {
            db = new ManageStudentEntities();
        }

        public List<FACILITy> ListAll()
        {
            return db.FACILITIES.Where(u => u.Status == false).ToList();
        }
    }
}