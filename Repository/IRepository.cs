using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(object id);
        TEntity Insert(TEntity entity);
        bool Update(TEntity entity ,object id);
        void Delete(TEntity entity);
        void Save();
    }
}
