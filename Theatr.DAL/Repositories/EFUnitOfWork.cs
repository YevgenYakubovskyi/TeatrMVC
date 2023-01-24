using System;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;
using Theatr.DAL.Entities;

namespace Theatr.DAL.Repositories
{
    public class EFUnitOfWork<TKey> : IUnitOfWork<TKey>
        where TKey : IComparable
    {
        private readonly DataContext db;
        private GeneralRerository<User, TKey> userRepository;
        private GeneralRerository<Ticket, TKey> ticketRepository;
        private GeneralRerository<Performance, TKey> perfomanceRepository;
        private GeneralRerository<Author, TKey> authorRepository;
        private GeneralRerository<Genre, TKey> genreRepository;
        private AuthorRepository authorCustomRepository;
        private GenreRepository genreCustomRepository;

        public EFUnitOfWork()
        {
            db = new DataContext();
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
        public IRepository<Performance, TKey> Perfomances
        {
            get
            {
                if (perfomanceRepository == null)
                    perfomanceRepository = new GeneralRerository<Performance, TKey>(db);
                return perfomanceRepository;
            }
        }
        public IRepository<Genre, TKey> Genres
        {
            get
            {
                if (genreRepository == null)
                    genreRepository = new GeneralRerository<Genre, TKey>(db);
                return genreRepository;
            }
        }
        public IRepository<Author, TKey> Authors
        {
            get
            {
                if (authorRepository == null)
                    authorRepository = new GeneralRerository<Author, TKey>(db);
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

        public IRepository<Ticket, TKey> Tickets
        {
            get
            {
                if (ticketRepository == null)
                    ticketRepository = new GeneralRerository<Ticket, TKey>(db);
                return ticketRepository;
            }
        }
        public IRepository<User, TKey> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new GeneralRerository<User, TKey>(db);
                return userRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }

    }
}