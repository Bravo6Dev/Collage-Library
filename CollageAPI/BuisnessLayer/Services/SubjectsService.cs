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

namespace BuisnessLayer.Services
{
    public class SubjectsService : ISubjectRepo
    {
        private readonly ResourcesDbContext _DbContext;
        private SubjectEF _Subject; 

        public SubjectsService(ResourcesDbContext DbContext)
        {
            _DbContext = DbContext;
            _Subject = new SubjectEF();
        }

        private bool Duplicate(string name)
        {
            try
            {
                return _DbContext.Subjects.FirstOrDefault(S => S.SubjectName == name) != null;
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
                SubjectEF Subject = _DbContext.Subjects.FirstOrDefault(S => S.ID == Id);
                if (Subject is null)
                    throw new ArgumentNullException("Subject not found");
                _DbContext.Subjects.Remove(Subject);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SubjectEF GetById(int Id)
        {
            return _DbContext.Subjects
                .Include(s => s.Semeseter)
                .FirstOrDefault(s => s.ID == Id)!;
        }

        public IEnumerable<SubjectEF> GetSubjects()
        {
            try
            {
                return _DbContext.Subjects
                    .Include(S => S.Semeseter)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<SubjectEF> GetSubjects(int SemId)
        {
            try
            {
                return _DbContext.Subjects
                    .Where(S => S.SemeseterID == SemId)
                    .Include(s => s.Semeseter)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save(SubjectEF Subject, enMode Mode)
        {
            if (Subject is null)
                throw new ArgumentNullException("Subject was null");

            if (string.IsNullOrEmpty(Subject.SubjectName))
                throw new Exception("Name of subject was null");

            SemestersEF Semester = _DbContext.Semesters
                .FirstOrDefault(s => s.ID == Subject.SemeseterID)!;

            if (Semester is null)
                throw new Exception($"There is no semester with {Subject.SemeseterID} ID");

            if (Duplicate(Subject.SubjectName))
                throw new DuplicateWaitObjectException("Name of subject is already exist");

            _DbContext.Entry(Semester).State = EntityState.Unchanged;

            _Subject.SubjectName = Subject.SubjectName;
            _Subject.SemeseterID = Subject.SemeseterID;
            _Subject.Semeseter = Semester;

            if (Mode == enMode.AddNew)
                _DbContext.Subjects.Add(_Subject);
            else if (Mode == enMode.Update)
                _DbContext.Subjects.Update(_Subject);

            return _DbContext.SaveChanges() > 0;
        }
    }
}
