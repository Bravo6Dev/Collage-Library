using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entites;

namespace DataLayer
{
    public class ResourcesDbContext : DbContext
    {
        public DbSet<CollagesEF> Collages { get; set; }
        public DbSet<SpecialtiesEF> Specialties { get; set; }

        public ResourcesDbContext(DbContextOptions<ResourcesDbContext> options)
            : base(options)
        {
        }
    }
}
