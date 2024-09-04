using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface ISpecialtiesRepo
    {
        public bool Save(SpecialtiesEF Specialty, enMode Mode);
        public bool Delete(int Id);
        public IEnumerable <SpecialtiesEF> GetAll();
        public IEnumerable<SpecialtiesEF> GetAll(int CollageId);
        public SpecialtiesEF GetById(int Id);
    }
}
