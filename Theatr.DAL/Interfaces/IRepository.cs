using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Theatr.DAL.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Expression<Func<T, Boolean>> expression);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}