using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Theatr.BLL.DTO;
using Theatr.BLL.Infrastructure;
using Theatr.BLL.Interfaces;
using Theatr.DAL.Entities;
using Theatr.DAL.Repositories;

namespace Theatr.BLL.Service
{
    public class ManagePerfomanceService : IManagePerfomanceService
    {
        private readonly EFUnitOfWork<int> UnitOfWork;

        public ManagePerfomanceService(EFUnitOfWork<int> UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }
        public IMapper configPerformance()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<Performance, PerformanceDTO>()
                    .IncludeAllDerived();
                cfg.CreateMap<Ticket, TicketDTO>();
                cfg.CreateMap<Author, AuthorDTO>();
                cfg.CreateMap<Genre, GenreDTO>();
            }).CreateMapper();
        }
        public IEnumerable<PerformanceDTO> GetAll()
        {
            var configuration = configPerformance();
            var performancesDTO = configuration.Map<IEnumerable<Performance>, IEnumerable<PerformanceDTO>>(UnitOfWork.Perfomances.GetAll());

            return performancesDTO;
        }

        public PerformanceDTO FindPerfomance(int id)
        {
            if (id < 0)
                throw new ValidationException("Id cannot be less than zero", "");
            var configuration = configPerformance();
            var performance = configuration.Map<Performance, PerformanceDTO>(UnitOfWork.Perfomances.Get(id));
            return performance;
        }
        public void SellTicket(TicketDTO ticketDTO, UserDTO userDTO)
        {
            var ticketDal = UnitOfWork.Tickets.Get(ticketDTO.Id);
            var userDal = UnitOfWork.Users.Get(userDTO.Id);
            ticketDal.TicketState = ticketDTO.TicketState;
            ticketDal.UserId = ticketDTO.UserId;
            userDal.Tickets.Add(ticketDal);
            UnitOfWork.Tickets.Update(ticketDal);
            UnitOfWork.Users.Update(userDal);
            UnitOfWork.Save();
        }
        public void SellBronedTicket(TicketDTO ticketDTO)
        {
            var ticketDal = UnitOfWork.Tickets.Get(ticketDTO.Id);
            ticketDal.TicketState = "Sold";
            UnitOfWork.Tickets.Update(ticketDal);
            UnitOfWork.Save();
        }

        public IEnumerable<GenreDTO> GetAllGenres()
        {
            var genresDal = UnitOfWork.Genres.GetAll();
            return genresDal.Select(a => new GenreDTO
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        public IEnumerable<PerformanceDTO> FindByGenreName(string genreName)
        {
            var genre = UnitOfWork.GenresCustom.GetByName(genreName);
            if (genre == null) throw new ValidationException("No such genre name", " ");
            return MapPerformance(genre.Perfomances);
        }
        public IEnumerable<PerformanceDTO> FindByName(string name)
        {
            var performances = UnitOfWork.Perfomances.Find(a => a.Name.Contains(name));
            return MapPerformance(performances);
        }
        public IEnumerable<PerformanceDTO> FindByAuthorName(string authorName)
        {
            var author = UnitOfWork.AuthorsCustom.GetByName(authorName);
            if (author == null) throw new ValidationException("No such author name", " ");
            return MapPerformance(author.Perfomances);
        }

        public IEnumerable<PerformanceDTO> FindByTime(DateTime start, DateTime end)
        {
            var performances = UnitOfWork.Perfomances.Find(a => a.Date > start && a.Date < end);
            return MapPerformance(performances);
        }

        public IEnumerable<GenreDTO> GetGenresByPerformanceId(int id)
        {
            var performance = UnitOfWork.Perfomances.Get(id);
            if (performance == null) throw new ValidationException("No performance with such id", " ");
            return performance.Genres.Select(a => new GenreDTO()
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
        }
        public IEnumerable<TicketDTO> GetTicketsByPerformanceId(int id)
        {
            var performance = UnitOfWork.Perfomances.Get(id);
            if (performance == null) throw new ValidationException("No performance with such id", " ");
            return performance.Tickets.Select(a => new TicketDTO()
            {
                Id = a.Id,
                Price = a.Price,
                TicketCategory = a.TicketCategory,
                TicketState = a.TicketState
            }).ToList();
        }
        public IEnumerable<AuthorDTO> GetAuthorsByPerformanceId(int id)
        {
            var performance = UnitOfWork.Perfomances.Get(id);
            if (performance == null) throw new ValidationException("No performance with such id", " ");
            return performance.Authors.Select(a => new AuthorDTO()
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
        }
        private IEnumerable<PerformanceDTO> MapPerformance(IEnumerable<Performance> performancesDal)
        {
            try
            {
                return performancesDal.Select(a => new PerformanceDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Date = a.Date,
                    Authors = GetAuthorsByPerformanceId(a.Id),
                    Genres = GetGenresByPerformanceId(a.Id),
                    Tickets = GetTicketsByPerformanceId(a.Id),
                });
            }
            catch
            {
                throw new ValidationException("List of performance clear with such parametr", " ");
            }
        }

        public IEnumerable<GenreDTO> GetGenreOfPerformance(string performanceName)
        {
            var genres = UnitOfWork.GenresCustom.GetByNamePerformance(performanceName);
            return genres.Select(a => new GenreDTO
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        public IEnumerable<AuthorDTO> GetAllAuthors()
        {
            var authors = UnitOfWork.Authors.GetAll();
            return authors.Select(author => new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name
            });
        }

        public void AddPerformance(string name, DateTime dateTime, string aut, string genres)
        {
            if (name.Equals(""))
                throw new ValidationException("You didn`t enter performance name", "");
            if (aut.Equals(""))
                throw new ValidationException("You didn`t enter authors", "");
            string[] authors = aut.Split(',');
            if (genres.Equals(""))
                throw new ValidationException("You didn`t enter genres", "");
            string[] genres1 = genres.Split(',');
            List<Performance> allPerformance = UnitOfWork.Perfomances.GetAll().ToList();
            foreach (Performance perf in allPerformance)
            {
                if (perf.Name.Equals(name))
                {
                    throw new ValidationException("We have such performance", "");
                }
            }

            Performance perfo = new Performance
            {
                Name = name,
                Date = dateTime,
                Authors = new List<Author>(),
                Genres = new List<Genre>()
            };
            List<string> givenAuthorsList = authors.ToList();
            List<Author> allAuthors = UnitOfWork.Authors.GetAll().ToList();
            foreach (string autr in givenAuthorsList.ToArray())
            {
                foreach (Author autr1 in allAuthors)
                {
                    if (autr.Equals(autr1.Name))
                    {
                        perfo.Authors.Add(autr1);
                        givenAuthorsList.Remove(autr);
                    }
                }
            }
            foreach (string auth in givenAuthorsList)
            {
                perfo.Authors.Add(new Author
                {
                    Name = auth,
                });
            }
            List<string> givenGenres = genres1.ToList();
            List<Genre> allGenres = UnitOfWork.Genres.GetAll().ToList();
            foreach (string genre in givenGenres.ToArray())
            {
                foreach (Genre genr in allGenres)
                {
                    if (genre.Equals(genr.Name))
                    {
                        perfo.Genres.Add(genr);
                        givenGenres.Remove(genre);
                    }
                }
            }
            foreach (string gen in givenGenres)
            {
                perfo.Genres.Add(new Genre
                {
                    Name = gen,
                });
            }
            UnitOfWork.Perfomances.Create(perfo);
            UnitOfWork.Save();
        }

        public void AddTicket(string ticCat, float ticPrice, int idPerformance)
        {
            if (ticCat.Equals(""))
                throw new ValidationException("You didn`t enter ticket category", "");
            if (ticPrice.Equals(""))
                throw new ValidationException("You didn`t enter price", "");
            Ticket ticke = new Ticket
            {
                PerfomanceId = idPerformance,
                Price = ticPrice,
                TicketCategory = ticCat,
                TicketState = "Can be sold",
                UserId = 1,
                Perfomance = UnitOfWork.Perfomances.Get(idPerformance)
            };
            UnitOfWork.Tickets.Create(ticke);
            UnitOfWork.Save();
        }

        public void DeletePerformance(int id)
        {
            UnitOfWork.Perfomances.Delete(id);
            UnitOfWork.Save();
        }

        public TicketDTO GetTicketById(int Id)
        {
            if (Id < 0)
                throw new ValidationException("Id cannot be less than zero", "");
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<Ticket, TicketDTO>()
                    .IncludeAllDerived();
                cfg.CreateMap<Performance, PerformanceDTO>();
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper(); ;
            var ticketDTO = configuration.Map<IEnumerable<Ticket>, IEnumerable<TicketDTO>>(UnitOfWork.Tickets.Find(ex => ex.Id.Equals(Id))).FirstOrDefault();

            return ticketDTO;
        }
    }
}
