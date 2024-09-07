using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    public class SemestersService : ISemesterRepo
    {
        private readonly ResourcesDbContext _DbContext;
        private SemestersEF _Semester;

        public SemestersService(ResourcesDbContext DbContext)
        {
            _DbContext = DbContext;
            _Semester = new SemestersEF();
        }

        public bool Delete(int ID)
        {
            try
            {
                SemestersEF Semester = _DbContext.Semesters
                    .FirstOrDefault(s => s.ID == ID)!;
                if (Semester is null)
                    throw new Exception("Semester doesn't found");
                _DbContext.Semesters.Remove(Semester);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SemestersEF> GetAll()
        {
            try
            {
                return _DbContext.Semesters
                    .Include(s => s.Speicalty)
                    .ThenInclude(S => S.Collage)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SemestersEF> GetAll(int SpecialtyId)
        {
            try
            {
                if (_DbContext.Specialties.FirstOrDefault(s => s.ID == SpecialtyId) is null)
                    throw new Exception("Specialty not found");
                return _DbContext.Semesters
                    .Where(s => s.SpeicaltyID == SpecialtyId)
                    .Include(s => s.Speicalty)
                    .ThenInclude(S => S.Collage)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SemestersEF GetById(int ID)
        {
            try
            {
                return _DbContext.Semesters
                    .Include(s => s.Speicalty)
                    .ThenInclude(S => S.Collage)
                    .FirstOrDefault(s => s.ID == ID)!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save(SemestersEF Semester, enMode Mode)
        {
            try
            {
                if (Semester is null)
                    throw new ArgumentNullException(nameof(Semester));

                if (Semester.NumOfSem < 1)
                    throw new Exception("Number of sem is not valid");

                SpecialtiesEF Specialty = _DbContext.Specialties
                    .FirstOrDefault(s => s.ID == Semester.SpeicaltyID)!;

                if (Specialty is null)
                    throw new NullReferenceException("Specialty not found");
                _DbContext.Entry(Specialty).State = EntityState.Unchanged;

                _Semester.NumOfSem = Semester.NumOfSem;
                _Semester.SpeicaltyID = Semester.SpeicaltyID;
                _Semester.Speicalty = Specialty;

                if (Mode == enMode.AddNew)
                    _DbContext.Semesters.Add(Semester);
                else if (Mode == enMode.Update)
                    _DbContext.Semesters.Update(Semester);

                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
