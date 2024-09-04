using DataLayer.Entites;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface ISubjectsRepo
    {
        public bool Save(SubjectEF Subject, enMode Mode);
        public IEnumerable<SubjectEF> GetSubjects();
        public IEnumerable<SubjectEF> GetSubjects(int SemId);
        public SubjectEF GetById(int Id);
        public bool Delete(int Id);
    }
}
