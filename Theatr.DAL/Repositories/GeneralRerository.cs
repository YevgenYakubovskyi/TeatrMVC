using System;
using System.Collections.Generic;
using System.Linq;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections;

namespace Theatr.DAL.Repositories
{
    class GeneralRerository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IComparable
    {
        private readonly DataContext db;
        private readonly DbSet<TEntity> entities;

        public GeneralRerository(DataContext context)
        {
            db = context;
            entities = db.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            entities.Add(item);
        }

        public void Delete(TKey id)
        {
            TEntity item = entities.Find(id);
            if (item != null)
            {
                entities.Remove(item);
            }
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return entities.Where(expression).ToList();
        }

        public TEntity Get(TKey id)
        {
            return entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return entities;
        }

        public void Update(TEntity item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
