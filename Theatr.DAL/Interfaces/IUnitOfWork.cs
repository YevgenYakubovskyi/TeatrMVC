using System;
using System.Threading.Tasks;
using Theatr.DAL.Entities;
using Theatr.DAL.Identity;

namespace Theatr.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IAuthorRepository AuthorsCustom { get; }
        IRepository<Performance> Perfomances { get; }
        IGenreRepository GenresCustom { get; }
        IRepository<Ticket> Tickets { get; }
        IRepository<Genre> Genres { get; }
        IRepository<Author> Authors { get; }
        IUserRepository ClientProfiles { get; }
        void Save();
        Task SaveAsync();
    }
}
