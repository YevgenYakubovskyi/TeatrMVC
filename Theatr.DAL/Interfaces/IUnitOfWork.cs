using System;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Interfaces
{
    public interface IUnitOfWork<TKey> where TKey : IComparable
    {
        IAuthorRepository AuthorsCustom { get; }
        IRepository<Performance, TKey> Perfomances { get; }
        IGenreRepository GenresCustom { get; }
        IRepository<Ticket, TKey> Tickets { get; }
        IRepository<Genre, TKey> Genres { get; }
        IRepository<Author, TKey> Authors { get; }
        IRepository<User, TKey> Users { get; }
        void Save();
    }
}
