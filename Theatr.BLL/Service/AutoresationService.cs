using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatr.BLL.DTO;
using Theatr.BLL.Infrastructure;
using Theatr.BLL.Interfaces;
using Theatr.DAL.Entities;
using Theatr.DAL.EF;
using Theatr.DAL.Interfaces;
using Theatr.DAL.Repositories;
using System.Web.Helpers;
using System.Web.Hosting;

namespace Theatr.BLL.Service
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly EFUnitOfWork<int> UnitOfWork;

        public AuthorizationService(EFUnitOfWork<int> UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }
        public UserDTO FindUser(string email)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>()
                    .IncludeAllDerived();
                cfg.CreateMap<Ticket, TicketDTO>();
            }).CreateMapper();
            var usersDTO = configuration.Map<IEnumerable<User>, IEnumerable<UserDTO>>(UnitOfWork.Users.Find(ex => ex.Email.Equals(email))).ToList();
            if (usersDTO.Count == 0)
                throw new ValidationException("There is no such User", "");
            UserDTO userDTO = usersDTO[0];
            return userDTO;
        }


        public UserDTO Login(string email, string password)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>()
                    .IncludeAllDerived();
                cfg.CreateMap<Ticket, TicketDTO>();
            }).CreateMapper();
            var userasDTO = configuration.Map<IEnumerable<User>, IEnumerable<UserDTO>>(UnitOfWork.Users.Find(ex => ex.Email.Equals(email))).ToList();

            if (userasDTO.Count == 0)
                throw new ValidationException("There is no such User", "");

            UserDTO userDTO = userasDTO[0];

            if (userDTO.Password.Equals(password))
            {
                return userDTO;
            }
            else
            {
                throw new ValidationException("Password is wrong", "");
            }
        }


        public IEnumerable<TicketDTO> GetTicketsByUserId(int userId)
        {
            var ticketsDal = UnitOfWork.Tickets.Find(ex => ex.UserId.Equals(userId));

            if (ticketsDal.Count() == 0) 
            {
                throw new ValidationException("No Tickets","");
            }
            var ticketsDTO = ticketsDal.Select(a => new TicketDTO
            {
                Id = a.Id,
                UserId = a.UserId,
                Price = a.Price,
                PerfomanceId = a.PerfomanceId,
                TicketCategory = a.TicketCategory,
                TicketState = a.TicketState,
                Perfomance = FindPerfomance(a.PerfomanceId),
                
            }).ToList();
            return ticketsDTO;
        }
        public PerformanceDTO FindPerfomance(int id)
        {
            if (id < 0)
                throw new ValidationException("Id cannot be less than zero", "");
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<Performance, PerformanceDTO>()
                    .IncludeAllDerived();
                cfg.CreateMap<Ticket, TicketDTO>();
                cfg.CreateMap<Genre, GenreDTO>();
                cfg.CreateMap<Author, AuthorDTO>();
            }).CreateMapper();
            var performance = configuration.Map<Performance, PerformanceDTO>(UnitOfWork.Perfomances.Get(id));

            if (performance == null)
                throw new ValidationException("Perfomance was not found", "");

            return performance;
        }

        public UserDTO FindUserById(int id)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>()
                    .IncludeAllDerived();
                cfg.CreateMap<Ticket, TicketDTO>();
            }).CreateMapper();
            var usersDTO = configuration.Map<IEnumerable<User>, IEnumerable<UserDTO>>(UnitOfWork.Users.Find(ex => ex.Id.Equals(id))).ToList();

           
            if (usersDTO.Count == 0)
                throw new ValidationException("There is no such User", "");
            UserDTO userDTO = usersDTO[0];
            return userDTO;
        }
    }
}
