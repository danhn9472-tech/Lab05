using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BLL
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            using (var context = new StudentModel())
            {
                return context.Faculties.ToList();
            }
        }
    }
}
