using Lab05.DAL;
using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BLL
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            StudentModel context = new StudentModel();
            return context.Students.ToList();
        }

        public List<Student> GetAllHasNoMajor()
        {
            StudentModel context = new StudentModel();
            return context.Students.Where(p => p.MajorID == null).ToList();
        }

        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            StudentModel context = new StudentModel();
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();
        }

        public Student FindById(string studentId)
        {
            StudentModel context = new StudentModel();
            return context.Students.FirstOrDefault(p => p.StudentID == studentId);
        }

        public void InsertUpdate(Student s)
        {
            StudentModel context = new StudentModel();
            context.Students.AddOrUpdate(s);
            context.SaveChanges();

        }
        public void AddStudent(Student s)
        {
            // Có thể thêm kiểm tra hợp lệ ở đây
            StudentDAL studentDAL = new StudentDAL();
            studentDAL.Insert(s);
        }

        public void UpdateStudent(Student s)
        {
            StudentDAL studentDAL = new StudentDAL();
            studentDAL.Update(s);
        }

        public void DeleteStudent(string id)
        {
            StudentDAL studentDAL = new StudentDAL();
            studentDAL.Delete(id);
        }
    }
}