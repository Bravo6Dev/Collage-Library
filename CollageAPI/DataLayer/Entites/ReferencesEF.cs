using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entites
{
    public class ReferencesEF
    {
        public int ID { get; set; }
        public string ReferenceName { get; set; } = null!;
        public string ReferencePath { get; set; } = null!;
        public string? ImagePath { get; set; } = null!;
        public int SubjectId { get; set; }
        public SubjectEF Subject { get; set; } = null!;
    }
}
