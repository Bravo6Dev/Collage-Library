using DataLayer.DTOs;
using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface ISemesterRepo
    {
        public bool Save(SemesterDTO Semester, enMode Mode);
        public bool Delete(int ID);

        public IEnumerable<SemesterDTO> GetAll();
        public IEnumerable<SemesterDTO> GetAll(int SpecialtyId);

        public SemesterDTO GetById(int ID);
    }
}
