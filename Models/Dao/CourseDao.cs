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

        public long Insert(COURSE entity)
        {
            db.COURSEs.Add(entity);
            db.SaveChanges();
            return entity.Id;
        }

        public bool Update (COURSE entity)
        {
            try
            {
                var course = db.COURSEs.Find(entity.Id);
                course.CourseName = entity.CourseName;
                course.DepartmentId = entity.DepartmentId;
                course.FacultyId = entity.FacultyId;
                course.Description = entity.Description;
                course.Images = entity.Images;
                course.Time = entity.Time;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //logging
                return false;
            }
        }

        public bool ChangeStatus(int id)
        {
            var course = db.COURSEs.Find(id);
            course.Status = !course.Status;

            return (bool)!course.Status;
        }

        //public IEnumerable<COURSE> ListAllPaging(int page, int pageSize)
        //{
        //    return db.COURSEs.OrderByDescending(u => u.Id).ToPagedList(page, pageSize);
        //}

        public COURSE ViewDetail(int id)
        {
            return db.COURSEs.Find(id);
        }

        public COURSE GetByID(int id)
        {
            return db.COURSEs.SingleOrDefault(u => u.Id == id);
        }
        public string GetNameById(int id)
        {
            return db.COURSEs.SingleOrDefault(u => u.Status == false && u.Id == id).CourseName;
        }

        public List<COURSE> ListAll()
        {
            return db.COURSEs.Where(u => u.Status == false).ToList();
        }

    }
}