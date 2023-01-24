using System.Collections.Generic;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetByNamePerformance(string name);
        Genre GetByName(string name);
    }
}