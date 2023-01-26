using System;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;
using Theatr.DAL.Entities;
using Theatr.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace Theatr.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private readonly DataContext db;
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        private UserRerository userRepository;
        private GeneralRerository<Ticket> ticketRepository;
        private GeneralRerository<Performance> perfomanceRepository;
        private GeneralRerository<Author> authorRepository;
        private GeneralRerository<Genre> genreRepository;
        private AuthorRepository authorCustomRepository;
        private GenreRepository genreCustomRepository;

        public IdentityUnitOfWork()
        {
            db = new DataContext();
            userManager = new ApplicationUserManager(new UserStore<User>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }
        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }
        public IClientManager ClientManager
        {
            get { return clientManager; }
        }
        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }
        public IAuthorRepository AuthorsCustom
        {
            get
            {
                if (authorCustomRepository == null)
                    authorCustomRepository = new AuthorRepository(db);
                return authorCustomRepository;
            }
        }
        public IRepository<Performance> Perfomances
        {
            get
            {
                if (perfomanceRepository == null)
                    perfomanceRepository = new GeneralRerository<Performance>(db);
                return perfomanceRepository;
            }
        }
        public IRepository<Genre> Genres
        {
            get
            {
                if (genreRepository == null)
                    genreRepository = new GeneralRerository<Genre>(db);
                return genreRepository;
            }
        }
        public IRepository<Author> Authors
        {
            get
            {
                if (authorRepository == null)
                    authorRepository = new GeneralRerository<Author>(db);
                return authorRepository;
            }
        }

        public IGenreRepository GenresCustom
        {
            get
            {
                if (genreCustomRepository == null)
                    genreCustomRepository = new GenreRepository(db);
                return genreCustomRepository;
            }
        }

        public IRepository<Ticket> Tickets
        {
            get
            {
                if (ticketRepository == null)
                    ticketRepository = new GeneralRerository<Ticket>(db);
                return ticketRepository;
            }
        }
        public IUserRepository ClientProfiles
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRerository(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

    }
}