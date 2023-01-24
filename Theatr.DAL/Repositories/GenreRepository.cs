using System.Collections.Generic;
using System.Linq;
using Theatr.DAL.Entities;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;

namespace Theatr.DAL.Repositories
{
    class GenreRepository : IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context)
        {
            _context = context;
        }

        public Genre GetByName(string name)
        {
            return _context.Genres.Where(c => c.Name.Equals(name)).FirstOrDefault();
        }

        public IEnumerable<Genre> GetByNamePerformance(string name)
        {
            var a = _context.Performances.Where(c => c.Name.Equals(name)).First();
            return a.Genres;
        }
    }
}
