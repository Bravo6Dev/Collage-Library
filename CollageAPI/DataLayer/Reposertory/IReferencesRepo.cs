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
        public bool Save(ReferencesEF Ref, enMode Mode);

        public IEnumerable<ReferencesEF> GetAll();
        public IEnumerable<ReferencesEF> GetAll(int SubjectId);

        public ReferencesEF GetById(int Id);

        public bool Delete(int ID);

        public bool Valid(ReferencesEF Ref);
    }
}
