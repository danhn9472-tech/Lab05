using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.DAL
{
    public class StudentDAL
    {
        private StudentModel context = new StudentModel();

        public List<Student> GetAll()
        {
            return context.Students.ToList();
        }

        public void Insert(Student s)
        {
            context.Students.Add(s);
            context.SaveChanges();
        }

        public void Update(Student s)
        {
            var old = context.Students.FirstOrDefault(x => x.StudentID == s.StudentID);
            if (old != null)
            {
                old.FullName = s.FullName;
                old.AverageScore = s.AverageScore;
                old.FacultyID = s.FacultyID;
                old.MajorID = s.MajorID;
                old.Avatar = s.Avatar;
                context.SaveChanges();
            }
        }
        public void Delete(string id)
        {
            var s = context.Students.Find(id);
            if (s != null)
            {
                context.Students.Remove(s);
                context.SaveChanges();
            }
        }
    }
}
