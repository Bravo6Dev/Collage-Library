using DataLayer.DTOs;
using DataLayer.Entites;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface ISubjectRepo
    {
        public bool Save(SubjectDTO Subject, enMode Mode);

        public IEnumerable<SubjectDTO> GetSubjects();
        public IEnumerable<SubjectDTO> GetSubjects(int SemId);

        public SubjectDTO GetById(int Id);

        public bool Delete(int Id);
    }
}
