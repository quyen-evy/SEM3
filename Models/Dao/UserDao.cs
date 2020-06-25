using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectsem3.Models;

namespace projectsem3.Models.Dao
{
    public class UserDao
    {
        private ManageStudentEntities db = null;
        public UserDao()
        {
            db = new ManageStudentEntities();
        }

        public long Insert(USER entity)
        {
            db.USERs.Add(entity);
            db.SaveChanges();
            return entity.Id;
        }

        //public IEnumerable<COURSE> ListAllPaging(int page, int pageSize)
        //{
        //    return db.COURSEs.OrderByDescending(u => u.Id).ToPagedList(page, pageSize);
        //}

        public USER GetByID(int id)
        {
            return db.USERs.SingleOrDefault(u => u.Id == id);
        }

        public int Login(string email, string passWord)
        {
            var result = db.USERs.SingleOrDefault(u => u.Email == email);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == true)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                        return 1;
                    else return 2;
                }
            }
        }

        public List<USER> ListAll()
        {
            return db.USERs.Where(u => u.Status == false).ToList();
        }
    }
}