using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectsem3.Models;
using projectsem3;

namespace projectsem3.Models.Dao
{
    public class StudentDao
    {
        private ManageStudentEntities db = null;
        public StudentDao()
        {
            db = new ManageStudentEntities();
        }

        public void Create()
        {

        }
        public long Insert(TABULAR entity)
        {
            var student = new STUDENT();
            student.StudentID = entity.UniqueID;
            student.FirstName = entity.FirstName;
            student.LastName = entity.LastName;
            student.FatherName = entity.FatherName;
            student.MotherName = entity.MotherName;
            student.Birthday = entity.Birthday;
            student.Gender = entity.Gender;
            student.Email = entity.Email;
            student.ResidentialAddress = entity.ResidentialAddress;
            student.PermanentAddress = entity.PermanentAddress;
            student.Sports = entity.Sports;
            student.AdmissionFor = entity.AdmissionFor;
            student.DepartmentId = entity.DepartmentId;
            student.CourseId = entity.CourseId;
            student.Status = false;
            student.Password = "1".ToMD5();

            db.STUDENTs.Add(student);
            db.SaveChanges();
            return entity.Id;
        }

    }
}