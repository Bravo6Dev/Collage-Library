using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class SpecialtiesService : ISpecialtiesRepo
    {
        private readonly ResourcesDbContext _DbContext;
        private SpecialtiesEF _Specialty;

        public SpecialtiesService(ResourcesDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public bool Delete(int Id)
        {
            try
            {
                 _Specialty = _DbContext.Specialties
                    .FirstOrDefault(S => S.ID == Id)!;
                if (_Specialty == null)
                    throw new ArgumentNullException(nameof(_Specialty));
                _DbContext.Specialties.Remove(_Specialty);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SpecialtiesEF> GetAll()
        {
            try
            {
                return _DbContext.Specialties
                    .Include(S => S.Collage)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SpecialtiesEF> GetAll(int CollageId)
        {
            try
            {
                if (_DbContext.Collages.FirstOrDefault(C => C.ID == CollageId) is null)
                    throw new ArgumentNullException("Collage not found");
                return _DbContext.Specialties
                    .Where(S => S.CollageID == CollageId)
                    .Include(S => S.Collage)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SpecialtiesEF GetById(int Id)
        {
            try
            {
                return _DbContext.Specialties
                    .Include(S => S.Collage)
                    .FirstOrDefault(S => S.ID == Id)!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Save(SpecialtiesEF Specialty, enMode Mode)
        {
            if (Specialty is null)
                throw new ArgumentException(nameof(Specialty));
            if (string.IsNullOrEmpty(Specialty.SpecialtyName))
                throw new Exception("Specialty name was null");

            CollagesEF Collage = _DbContext.Collages
                .FirstOrDefault(C => C.ID == Specialty.CollageID)!;
            if (Collage is null)
                throw new Exception($"There is no collage with {Specialty.CollageID} ID");

            _DbContext.Entry(Collage).State = EntityState.Unchanged;
            Specialty.Collage = Collage;

            if (Mode == enMode.AddNew)
                _DbContext.Specialties.Add(Specialty);
            else
                _DbContext.Specialties.Update(Specialty);
            return _DbContext.SaveChanges() > 0;
        }
    }
}
