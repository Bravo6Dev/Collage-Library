using DataLayer;
using DataLayer.Entites;
using DataLayer.Reposertory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    public class CollageServices : ICollageRepo
    {
        public CollagesEF Collage { get; set; }
        public ResourcesDbContext _DbContext { get; set; }

        public CollageServices(ResourcesDbContext Context)
        {
            _DbContext = Context;
            Collage = new CollagesEF();
        }

        public bool Save(CollagesEF CollageArg, enMode Mode)
        {
            try
            {
                if (CollageArg is null)
                    throw new ArgumentNullException(nameof(CollageArg));
                if (string.IsNullOrEmpty(CollageArg.CollageName))
                    throw new Exception("Name was empty");
                Collage.CollageName = CollageArg.CollageName;

                if (Mode == enMode.AddNew)
                    _DbContext.Collages.Add(Collage);
                else
                    _DbContext.Collages.Update(Collage);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(int ID)
        {
            try
            {
                Collage = _DbContext.Collages.FirstOrDefault(C => C.ID == ID)!;
                if (Collage is null)
                    throw new ArgumentNullException($"There is no collage with {ID} ID");
                _DbContext.Collages.Remove(Collage);
                return _DbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CollagesEF GetById(int ID)
        {
            try
            {
                return _DbContext.Collages.FirstOrDefault(C => C.ID == ID)!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CollagesEF> GetAll()
        {
            try
            {
                return _DbContext.Collages.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CollagesEF GetByName(string Name)
        {
            try
            {
                return _DbContext.Collages.FirstOrDefault(C => C.CollageName == Name)!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
