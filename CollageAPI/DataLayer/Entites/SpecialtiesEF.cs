using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entites
{
    public class SpecialtiesEF
    {
        [Key]
        public int ID { get; set; }
        public string SpecialtyName { get; set; } = null!;
        [ForeignKey("CollageID")]
        public int CollageID { get; set; }
        public CollagesEF Collage { get; set; } = null!;
    }
}
