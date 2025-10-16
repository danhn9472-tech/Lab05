using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BLL
{
    public class MajorService
    {
        public List<Major> GetAllByFaculty(int FacultyID)
        {
            using (var context = new StudentModel())
            {
                return context.Majors.Where(p => p.FacultyID == FacultyID).ToList();
            }
        }
    }
}
