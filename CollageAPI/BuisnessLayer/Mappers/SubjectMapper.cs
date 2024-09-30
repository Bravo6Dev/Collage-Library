using DataLayer.DTOs;
using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Mappers
{
    public static class SubjectMapper
    {
        public static SubjectDTO ToDTO(SubjectEF Subject)
        {
            if (Subject == null) throw new ArgumentNullException("Subject was null");

            return new SubjectDTO()
            {
                Id = Subject.ID,
                SemesterId = Subject.SemeseterID,
                SemesterNumber = Subject.Semeseter.NumOfSem,
                SubjectName = Subject.SubjectName,
            };
        }

        public static SubjectEF ToEF(SubjectDTO Subject)
        {
            return new SubjectEF
            {
                ID = Subject.Id,
                SubjectName = Subject.SubjectName,
                SemeseterID = Subject.SemesterId,
            };
        }

        public static IEnumerable<SubjectDTO> GetAllSubjects(IEnumerable<SubjectEF> SubjectList)
        {
            foreach (SubjectEF Subject in SubjectList)
                yield return new SubjectDTO()
                {
                    Id = Subject.ID,
                    SemesterId = Subject.SemeseterID,
                    SemesterNumber = Subject.Semeseter.NumOfSem,
                    SubjectName = Subject.SubjectName,
                };
        }
    }
}
