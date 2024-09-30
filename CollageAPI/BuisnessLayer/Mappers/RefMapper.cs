using DataLayer.DTOs;
using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Mappers
{
    public static class RefMapper
    {
        static public RefDTO ToDTO(ReferencesEF Ref)
        {
            if (Ref == null) 
                    throw new ArgumentNullException("Ref subject was null");

            return new RefDTO()
            {
                ID = Ref.ID,
                RefName = Ref.ReferenceName,
                RefPath = Ref.ReferencePath,
                ImagePath = Ref.ImagePath,
                SubjectName = Ref.Subject.SubjectName,
                NumSem = Ref.Subject.Semeseter.NumOfSem,
                SubId = Ref.SubjectId
            };
        }

        static public ReferencesEF ToEF(RefDTO Ref)
        {
            if (Ref == null)
                throw new ArgumentNullException("Ref object was null");
            return new ReferencesEF()
            {
                ID = Ref.ID,
                ImagePath = Ref.ImagePath,
                ReferenceName = Ref.RefName,
                ReferencePath = Ref.RefPath,
                SubjectId = Ref.SubId
            };
        }

        static public IEnumerable<RefDTO> GetAllReferences(IEnumerable<ReferencesEF> Refs) 
        {
            foreach (ReferencesEF Ref in Refs)
            {
                yield return new RefDTO()
                {
                    ID = Ref.ID, 
                    ImagePath = Ref.ImagePath,
                    NumSem = Ref.Subject.Semeseter.NumOfSem,
                    RefName = Ref.ReferenceName,
                    RefPath = Ref.ReferencePath,
                    SubjectName = Ref.Subject.SubjectName,
                    SubId = Ref.SubjectId
                };
            }
        }

    }
}
