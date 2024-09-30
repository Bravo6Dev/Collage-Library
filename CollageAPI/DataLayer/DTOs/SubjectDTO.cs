using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = null!;
        public int SemesterId { get; set; }
        public int SemesterNumber { get; set; }
    }
}
