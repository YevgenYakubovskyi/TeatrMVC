using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Interfaces
{
    public interface IClientManager
    {
        void Create(ClientProfile item);
        void Dispose();
        void Delete(int id);
        IEnumerable<ClientProfile> GetAll();
        ClientProfile Get(int id);
        IEnumerable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> expression);
    }
}