using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entites
{
    public class SubjectEF
    {
        public int ID { get; set; }
        public string SubjectName { get; set; } = null!;
        public int SemesterId { get; set; }
        public SemestersEF Semeseter { get; set; } = null!;
    }
}
