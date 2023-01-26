using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Interfaces
{
    public interface IUserRepository
    {
        void Create(ClientProfile item);

        void Delete(string id);


        IEnumerable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> expression);
       

        ClientProfile Get(string id);

        IEnumerable<ClientProfile> GetAll();

        void Update(ClientProfile item);
    }
}