using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class RefDTO
    {
        public int ID { get; set; }
        public string RefName { get; set; } = null!;
        public string RefPath { get; set; } = null!;
        public string? ImagePath { get; set; } = null!;
        public int SubId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int NumSem { get; set; }
    }
}
