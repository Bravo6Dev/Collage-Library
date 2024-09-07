using DataLayer;
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
            return _DbContext.References.FirstOrDefault(R => R.ReferenceName == name) != null;
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

        public IEnumerable<Ref> GetAll()
        {
            try
            {
                return _DbContext.References
                    .Include(R => R.Subject)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Ref> GetAll(int SubjectId)
        {
            try
            {
                return _DbContext.References
                    .Where(R => R.SubjectId == SubjectId)
                    .Include(R => R.Subject)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Ref GetById(int Id)
        {
            try
            {
                return _DbContext.References.FirstOrDefault(R => R.ID == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Valid(Ref Ref)
        {
            if (Ref is null)
                throw new ArgumentNullException("Reference was null");
            if (string.IsNullOrEmpty(Ref.ReferenceName))
                throw new Exception("Name of reference is null");
            SubjectEF Subj = _DbContext.Subjects.FirstOrDefault(S => S.ID == Ref.SubjectId)!;

            if (Subj is null)
                throw new Exception($"Subject with {Ref.SubjectId} ID not found");

            if (Duplicate(Ref.ReferenceName))
                throw new DuplicateWaitObjectException($"Reference with {Ref.ReferenceName} already exist");

            return true;
        }

        public bool Save(Ref Ref, enMode Mode)
        {
            if (Ref is null)
                throw new ArgumentNullException("Reference was null");
            if (string.IsNullOrEmpty(Ref.ReferenceName))
                throw new Exception("Name of reference is null");
            SubjectEF Subj = _DbContext.Subjects.FirstOrDefault(S => S.ID == Ref.SubjectId)!;

            if (Subj is null)
                throw new Exception($"Subject with {Ref.SubjectId} ID not found");

            if (Duplicate(Ref.ReferenceName))
                throw new DuplicateWaitObjectException($"Reference with {Ref.ReferenceName} already exist");

            _DbContext.Entry(Subj).State = EntityState.Unchanged;
            _Ref.ReferenceName = Ref.ReferenceName;
            _Ref.ReferencePath = Ref.ReferencePath;
            _Ref.ImagePath = Ref.ImagePath;
            _Ref.SubjectId = Ref.SubjectId;
            _Ref.Subject = Subj;

            if (Mode == enMode.AddNew)
                _DbContext.References.Add(_Ref);
            else if (Mode == enMode.Update)
                _DbContext.References.Update(_Ref);

            return _DbContext.SaveChanges() > 0;
        }

    }
}
