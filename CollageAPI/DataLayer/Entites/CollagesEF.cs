using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entites
{
    public class CollagesEF
    {
        [Key]
        public int ID   { get; set; }
        public string CollageName { get; set; } = null!;
    }
}
