using DataLayer.DTOs;
using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLayer.Mappers;

namespace BuisnessLayer.Services
{
    public class SubjectsService : ISubjectRepo
    {
        private readonly ResourcesDbContext _DbContext;

        public SubjectsService(ResourcesDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        private bool Duplicate(string name)
        {
            try
            {
                return _DbContext.Subjects
                    .AsNoTracking()
                    .Select(S => S.SubjectName)
                    .FirstOrDefault(S => S == name) != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(int Id)
        {
            try
            {
                SubjectEF Sub = _DbContext.Subjects.FirstOrDefault(S => S.ID == Id)!;
                if (Sub == null)
                    throw new ArgumentNullException("Subject not found");
                _DbContext.Subjects.Remove(Sub);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SubjectDTO GetById(int Id)
        {
            SubjectEF Sub = _DbContext.Subjects
                .AsNoTracking()
                .Include(s => s.Semeseter)
                .FirstOrDefault(s => s.ID == Id)!;
            return SubjectMapper.ToDTO(Sub);
        }

        public IEnumerable<SubjectDTO> GetSubjects()
        {
            try
            {
                return SubjectMapper.GetAllSubjects(
                    _DbContext.Subjects
                    .Include(s => s.Semeseter)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SubjectDTO> GetSubjects(int SemId)
        {
            try
            {
                return SubjectMapper.GetAllSubjects(
                    _DbContext.Subjects
                    .Where(S => S.SemeseterID == SemId)
                    .Include(s => s.Semeseter)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save(SubjectDTO Subject, enMode Mode)
        {
            try
            {
                if (Subject == null)
                    throw new ArgumentNullException("Subject was null");

                if (string.IsNullOrEmpty(Subject.SubjectName))
                    throw new Exception("Name of subject was null");

                SemestersEF Semester = _DbContext.Semesters
                    .FirstOrDefault(s => s.ID == Subject.SemesterId)!;

                if (Semester == null)
                    throw new Exception($"There is no semester with {Subject.SemesterId} ID");

                if (Duplicate(Subject.SubjectName) && Mode == enMode.AddNew)
                    throw new DuplicateWaitObjectException("Name of subject is already exist");

                _DbContext.Entry(Semester).State = EntityState.Unchanged;

                SubjectEF Sub = SubjectMapper.ToEF(Subject);
                Sub.Semeseter = Semester;
                _DbContext.Entry(Sub).State = Mode == enMode.AddNew ? EntityState.Added : EntityState.Modified;

                if (Mode == enMode.AddNew)
                    _DbContext.Subjects.Add(Sub);
                else if (Mode == enMode.Update)
                    _DbContext.Subjects.Update(Sub);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
