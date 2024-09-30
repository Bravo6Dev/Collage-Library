using DataLayer.DTOs;
using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Mappers
{
    public static class SemesterMapper
    {
        public static SemesterDTO ToDTO(SemestersEF Semester)
        {
            if (Semester == null)
                throw new ArgumentNullException("Semester object was null");
            return new SemesterDTO()
            {
                Id = Semester.ID,
                SemNumber = Semester.NumOfSem,
                SpecId = Semester.SpeicaltyID,
                SpecName = Semester.Speicalty.SpecialtyName
            };
        }

        public static SemestersEF ToEF(SemesterDTO Semester)
        {
            if (Semester == null)
                throw new ArgumentNullException("Semester object was null");
            return new SemestersEF()
            {
                NumOfSem = Semester.SemNumber,
                SpeicaltyID = Semester.SpecId
            };
        }

        public static IEnumerable<SemesterDTO> GetAllSemesters(IEnumerable<SemestersEF> SemestersList)
        {
            foreach (SemestersEF item in SemestersList)
                yield return new SemesterDTO()
                {
                    Id = item.ID,
                    SemNumber = item.NumOfSem,
                    SpecId = item.SpeicaltyID,
                    SpecName = item.Speicalty.SpecialtyName
                };
        }
    }
}
