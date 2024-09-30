using BuisnessLayer.Mappers;
using DataLayer;
using DataLayer.DTOs;
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

        public SemestersService(ResourcesDbContext DbContext)
        {
            _DbContext = DbContext;
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

        public IEnumerable<SemesterDTO> GetAll()
        {
            try
            {
                return SemesterMapper.GetAllSemesters(_DbContext.Semesters
                    .Include(s => s.Speicalty)
                    .ThenInclude(S => S.Collage)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SemesterDTO> GetAll(int SpecialtyId)
        {
            try
            {
                if (_DbContext.Specialties.FirstOrDefault(s => s.ID == SpecialtyId) is null)
                    throw new Exception("Specialty not found");
                return SemesterMapper.GetAllSemesters(_DbContext.Semesters
                    .Where(s => s.SpeicaltyID == SpecialtyId)
                    .Include(s => s.Speicalty)
                    .ThenInclude(S => S.Collage)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SemesterDTO GetById(int ID)
        {
            try
            {
                SemestersEF Sem = _DbContext.Semesters
                    .AsNoTracking()
                    .Include(s => s.Speicalty)
                    .FirstOrDefault(s => s.ID == ID)!;
                if (Sem == null)
                    throw new KeyNotFoundException($"Semester with {ID} ID not found");

                return SemesterMapper.ToDTO(Sem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save(SemesterDTO Semester, enMode Mode)
        {
            try
            {
                if (Semester == null)
                    throw new ArgumentNullException("Semester was null");

                if (Semester.SemNumber < 1)
                    throw new Exception("Number of sem is not valid");

                SpecialtiesEF Specialty = _DbContext.Specialties
                    .FirstOrDefault(s => s.ID == Semester.SpecId)!;

                if (Specialty == null)
                    throw new NullReferenceException($"There is no semester with {Semester.SpecId} ID");

                _DbContext.Entry(Specialty).State = EntityState.Unchanged;

                SemestersEF Sem = SemesterMapper.ToEF(Semester);
                Sem.Speicalty = Specialty;

                if (Mode == enMode.AddNew)
                    _DbContext.Semesters.Add(Sem);
                else if (Mode == enMode.Update)
                    _DbContext.Semesters.Update(Sem);

                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
