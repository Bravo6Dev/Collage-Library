using DataLayer.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public enum enMode { AddNew , Update }
    public interface ICollageRepo
    {
        public bool Save(CollagesEF Collage, enMode Mode);
        public bool Delete(int ID);
        public CollagesEF GetById(int ID);
        public CollagesEF GetByName(string Name);
        public IEnumerable<CollagesEF> GetAll();
    }
}
