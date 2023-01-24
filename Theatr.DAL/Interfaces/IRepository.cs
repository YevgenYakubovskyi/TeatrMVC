using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Theatr.DAL.Interfaces
{
    public interface IRepository<T, TKey>
        where T : class
        where TKey : IComparable
    {
        IEnumerable<T> GetAll();
        T Get(TKey id);
        IEnumerable<T> Find(Expression<Func<T, Boolean>> expression);
        void Create(T item);
        void Update(T item);
        void Delete(TKey id);
    }
}