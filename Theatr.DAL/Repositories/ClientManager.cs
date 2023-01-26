using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Theatr.DAL.EF;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Interfaces
{
    public class ClientManager : IClientManager
    {
        public DataContext Database { get; set; }
        public ClientManager(DataContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }
        public void Delete(int id)
        {
            ClientProfile item = Database.ClientProfiles.Find(id);
            if (item != null)
            {
                Database.ClientProfiles.Remove(item);
            }
        }

        public IEnumerable<ClientProfile> Find(Expression<Func<ClientProfile, bool>> expression)
        {
            return Database.ClientProfiles.Where(expression).ToList();
        }

        public ClientProfile Get(int id)
        {
            return Database.ClientProfiles.Find(id);
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return Database.ClientProfiles;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}