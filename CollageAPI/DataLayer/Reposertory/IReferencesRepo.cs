using DataLayer.DTOs;
using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface IReferencesRepo
    {
        public bool Save(RefDTO Ref, enMode Mode);

        public IEnumerable<RefDTO> GetAll();
        public IEnumerable<RefDTO> GetAll(int SubjectId);

        public RefDTO GetById(int Id);

        public bool Delete(int ID);

        public bool Valid(RefDTO Ref);
    }
}
