using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LaundryTimeWebAppWithIdentity.Repositorys
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity);
        public void Remove(TEntity entity);
        public TEntity Get(int id);
        public IEnumerable<TEntity> GetAll();
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        public TEntity SingleOrDefault(Expression<Func<TEntity,bool>> predicate);

    }
}
