using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface ISemestersRepo
    {
        public bool Save(SemestersEF Semester, enMode Mode);
        public bool Delete(int ID);

        public IEnumerable<SemestersEF> GetAll();
        public IEnumerable<SemestersEF> GetAll(int SpecialtyId);

        public SemestersEF GetById(int ID);
    }
}
