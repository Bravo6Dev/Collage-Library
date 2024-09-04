using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entites
{
    public class SemestersEF
    {
        [Key]
        public int ID { get; set; }
        public Int16 NumOfSem { get; set; }
        [ForeignKey("SpeicaltyID")]
        public int SpeicaltyID { get; set; }
        public SpecialtiesEF Speicalty { get; set; } = null!;
    }
}
