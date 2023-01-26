using System;
using System.Collections.Generic;
using System.Linq;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Repositories
{
    class UserRerository : IUserRepository
    {
        private readonly DataContext db;

        public UserRerository(DataContext context)
        {
            this.db = context;
        }

        public void Create(ClientProfile item)
        {
            db.ClientProfiles.Add(item);
        }

        public void Delete(string id)
        {
            ClientProfile item = db.ClientProfiles.Find(id);
            if (item != null)
            {
                db.ClientProfiles.Remove(item);
            }
        }

        public IEnumerable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> expression)
        {
            return db.ClientProfiles.Where(expression).ToList();
        }

        public ClientProfile Get(string id)
        {
            return db.ClientProfiles.Find(id);
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return db.ClientProfiles;
        }

        public void Update(ClientProfile item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
