using BuisnessLayer.Mappers;
using DataLayer;
using DataLayer.DTOs;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.EntityFrameworkCore;

using Ref = DataLayer.Entites.ReferencesEF;

namespace BuisnessLayer.Services
{
    public class ReferencesService : IReferencesRepo
    {
        private readonly ResourcesDbContext _DbContext;
        private Ref _Ref;

        public ReferencesService(ResourcesDbContext DbContext)
        {
            _DbContext = DbContext;
            _Ref = new Ref();
        }

        private bool Duplicate(string name)
        {
            return _DbContext.References
                .AsNoTracking()
                .FirstOrDefault(R => R.ReferenceName == name) != null;
        }

        public bool Delete(int ID)
        {
            try
            {
                Ref Reference = _DbContext.References
                    .FirstOrDefault(R => R.ID == ID)!;
                if (Reference is null)
                    throw new Exception($"References with {ID} ID not found");
                _DbContext.References.Remove(Reference);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<RefDTO> GetAll()
        {
            try
            {
                return RefMapper.GetAllReferences(_DbContext.References
                    .Include(R => R.Subject)
                    .ThenInclude(S => S.Semeseter)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<RefDTO> GetAll(int SubjectId)
        {
            try
            {
                return RefMapper.GetAllReferences(_DbContext.References
                    .Where(R => R.SubjectId == SubjectId)
                    .Include(R => R.Subject)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RefDTO GetById(int Id)
        {
            try
            {
                Ref Refe = _DbContext.References
                    .AsNoTracking()
                    .Include(R => R.Subject)
                    .ThenInclude(R => R.Semeseter)
                    .FirstOrDefault(R => R.ID == Id)!;
                return RefMapper.ToDTO(Refe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Valid(RefDTO Ref)
        {
            if (Ref is null)
                throw new ArgumentNullException("Reference was null");
            if (string.IsNullOrEmpty(Ref.RefName))
                throw new Exception("Name of reference is null");

            if (Duplicate(Ref.RefName))
                throw new DuplicateWaitObjectException($"Reference with {Ref.RefName} already exist");

            return true;
        }

        public bool Save(RefDTO Ref, enMode Mode)
        {
            if (!Valid(Ref))
                return false;

            SubjectEF Subj = _DbContext.Subjects.FirstOrDefault(S => S.ID == Ref.SubId)!;

            if (Subj == null)
                throw new Exception($"Subject with {Ref.SubId} ID not found");

            _DbContext.Entry(Subj).State = EntityState.Unchanged;
            _Ref = RefMapper.ToEF(Ref);
            _DbContext.Entry(_Ref).State = Mode == enMode.AddNew ? EntityState.Added : EntityState.Modified;

            if (Mode == enMode.AddNew)
                _DbContext.References.Add(_Ref);
            else if (Mode == enMode.Update)
                _DbContext.References.Update(_Ref);

            return _DbContext.SaveChanges() > 0;
        }

    }
}
