using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class SemesterDTO
    {
        public int Id { get; set; }
        public short SemNumber { get; set; }
        public int SpecId { get; set; }
        public string SpecName { get; set; } = null!;
    }
}
