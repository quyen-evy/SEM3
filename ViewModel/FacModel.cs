using projectsem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectsem3.ViewModel
{
    public class FacModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FacModel ( int id )
        {
            int FacID = id;
            if( FacID > 0)
            {
                using (ManageStudentEntities db = new ManageStudentEntities())
                {
                    Name = db.FACILITIES.SingleOrDefault(u => u.Id == id && u.Status == false).Name;
                }
            }

        }

    }
}