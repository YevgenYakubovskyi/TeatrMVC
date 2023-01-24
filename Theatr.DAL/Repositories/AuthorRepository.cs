using System;
using System.Collections.Generic;
using System.Linq;
using Theatr.DAL.Entities;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;

namespace Theatr.DAL.Repositories
{
    class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext db;

        public AuthorRepository(DataContext context)
        {
            this.db = context;
        }
        public Author GetByName(string name)
        {
            return db.Authors.Where(c => c.Name.Equals(name)).FirstOrDefault();
        }
    }
}
