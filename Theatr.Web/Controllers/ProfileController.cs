using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Theatr.BLL.DTO;
using Theatr.BLL.Interfaces;
using Theatr.BLL.Service;
using Theatr.BLL.Infrastructure;
using Theatr.Web.Models;
using AutoMapper;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Configuration;

namespace Theatr.Web.Controllers
{
    public class ProfileController : Controller
    {
        IAuthorizationService authorizationService;
        IManagePerfomanceService managePerfomanceService;
        public ProfileController(IAuthorizationService authorizationService, IManagePerfomanceService managePerfomanceService)
        {
            this.authorizationService = authorizationService;
            this.managePerfomanceService = managePerfomanceService;
        }
        public ActionResult Index(UserViewModel user)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDTO, UserViewModel>()
                    .IncludeAllDerived();
                cfg.CreateMap<TicketDTO, TicketViewModel>();
            }).CreateMapper();
            var userC = configuration.Map<UserDTO, UserViewModel>(authorizationService.FindUserById(user.Id));

            ViewBag.Message = "Вітаю, " + userC.Name + " " + userC.Surname;
            return View(userC);
        }
        public ActionResult PerformanceList(TicketViewModel tickets)
        {
            var configuration = configPerformance();
            var ptu = new PTUViewModel();
            ptu.Performance = configuration.Map<
                IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                (managePerfomanceService.GetAll().Where(a => a.Id != 3));
            ptu.User = new UserViewModel { Id= tickets.UserId };
            return View(ptu);
        }

        [HttpPost]
        public ActionResult SearchPerformanceByName(string namePerf, int idUser)
        {
            var configuration = configPerformance();

            var ptu = new PTUViewModel();
            ptu.User = new UserViewModel { Id = idUser };
            ptu.Performance = configuration.Map<
                IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                (managePerfomanceService.FindByName(namePerf).Where(a => a.Id != 3));
            return View(ptu);
        }
        [HttpPost]
        public ActionResult SearchPerformanceByAuthor(string nameAuthor, int idUser)
        {
            var ptu = new PTUViewModel();
            ptu.User = new UserViewModel { Id = idUser };
            try
            {
                ViewBag.Error = " ";
                var configuration = configPerformance();

                ptu.Performance = configuration.Map<
                    IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                    (managePerfomanceService.FindByAuthorName(nameAuthor).Where(a => a.Id != 3));

                return View(ptu);
            }
            catch
            {
                ViewBag.Error = "Вистав з заданим іменем автора не знайдено. Відображені всі вистави";
                var configuration = configPerformance();
                ptu.Performance = configuration.Map<
                    IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                    (managePerfomanceService.GetAll().Where(a => a.Id != 3));
                return View(ptu);

            }
            
        }
        [HttpPost]
        public ActionResult SearchPerformanceByDate(DateTime dateTimeStart, DateTime dateTimeEnd, int idUser)
        {

            var ptu = new PTUViewModel();
            ptu.User = new UserViewModel { Id = idUser };
            var configuration = configPerformance();

            ptu.Performance = configuration.Map<
                IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                (managePerfomanceService.FindByTime(dateTimeStart, dateTimeEnd).Where(a => a.Id != 3));
            return View(ptu);
        }
        public IMapper configPerformance()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<PerformanceDTO, PerformanceViewModel>()
                    .IncludeAllDerived();
                cfg.CreateMap<TicketDTO, TicketViewModel>();
                cfg.CreateMap<AuthorDTO, AuthorViewModel>();
                cfg.CreateMap<GenreDTO, GenreViewModel>();
            }).CreateMapper();
        }
        public IMapper configUser()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDTO, UserViewModel>()
                    .IncludeAllDerived();
                cfg.CreateMap<TicketDTO, TicketViewModel>();
            }).CreateMapper();
        }
        public ActionResult TicketList(UserViewModel user)
        {
            var ptu = new PTUViewModel();
            try
            {
                ptu.User = new UserViewModel { Id = user.Id };
                var cTickets = authorizationService.GetTicketsByUserId(user.Id).Where(a=>a.PerfomanceId!=3);

                    ptu.Tickets = cTickets.Select(a => new TicketViewModel
                    {
                        Id = a.Id,
                        PerfomanceId = a.PerfomanceId,
                        Price = a.Price,
                        TicketCategory = a.TicketCategory,
                        TicketState = a.TicketState,
                        UserId = a.UserId,
                        Performance = new PerformanceViewModel { Name = managePerfomanceService.FindPerfomance(a.PerfomanceId).Name }
                    });

                return View(ptu);
            }
            catch
            {
                ptu.User = new UserViewModel { Id = user.Id };
                var cTickets = authorizationService.GetTicketsByUserId(7);
                ptu.Tickets = cTickets.Select(a => new TicketViewModel
                {
                    Id = a.Id,
                    PerfomanceId = a.PerfomanceId,
                    Price = a.Price,
                    UserId = user.Id,
                });
                return View(ptu);
            }
        }

        [HttpPost]
        public ActionResult SellBronedTicket(TicketViewModel ticket)
        {
            try
            {
                var getTicket = managePerfomanceService.GetTicketById(ticket.Id);
                if (getTicket.TicketState != "Brone")
                {
                    return Content("Цей квиток не заброньовано! Він уже куплений!");
                }
                managePerfomanceService.SellBronedTicket(getTicket);
                var get1Ticket = managePerfomanceService.GetTicketById(ticket.Id);
                return Content("Ви придбали заброньований квиток");
            }
            catch
            {
                return Content("Сталась помилка при спробі купити заброньований квиток");
            }
        }
        [HttpPost]
        public ActionResult AddTicket(TicketViewModel ticket)
        {
            try
            {
                var ptu = new PTUSoloViewModel();

                var configuration = configPerformance();

                ptu.Performance = configuration.Map<
                    PerformanceDTO, PerformanceViewModel>
                    (managePerfomanceService.FindPerfomance(ticket.PerfomanceId));
                var configuration1 = configUser();
                ptu.User = configuration1.Map<UserDTO, UserViewModel>(authorizationService.FindUserById(ticket.UserId));
                return View(ptu);
            }
            catch
            {
                return Content("Сталась помилка при спробі добавити квиток");
            }
        }

        [HttpPost]
        public ActionResult AddTicketPost(TicketViewModel ticket, int Count)
        {
            try
            {
                if ((ticket.TicketCategory != "VIP") && (ticket.TicketCategory != "Standart"))
                {
                    return Content("Читати не вмієте? Коректно дані введіть");
                }
                for (int i = 0; i < Count; i++)
                {
                    managePerfomanceService.AddTicket(ticket.TicketCategory, ticket.Price, ticket.PerfomanceId);
                }
                return Content("Ви добавили квиток");
            }
            catch
            {
                return Content("Сталась помилка при спробі добавити квиток");
            }
        }

        [HttpPost]
        public ActionResult BuyTicket(TicketViewModel ticket)
        {
            try
            {
                var ptu = new PTUSoloViewModel();
                var configuration = configPerformance();

                ptu.Performance = configuration.Map<
                    PerformanceDTO, PerformanceViewModel>
                    (managePerfomanceService.FindPerfomance(ticket.PerfomanceId));
                var configuration1 = configUser();
                ptu.User = configuration1.Map<UserDTO, UserViewModel>(authorizationService.FindUserById(ticket.UserId));
                
                return View(ptu);
            }
            catch
            {
                return Content("Сталась помилка при спробі купити квиток");
            }
        }


        [HttpPost]
        public ActionResult ConfirmBuyTicket(TicketViewModel ticket)
        {
            var ptu = new PTUSoloViewModel();
            try
            {
                if (((ticket.TicketCategory != "VIP") && (ticket.TicketCategory != "Standart"))
                    || ((ticket.TicketState != "Buy") && (ticket.TicketState != "Brone")))
                {
                    return Content("Читати не вмієте? Коректно дані введіть");
                }
                var t = managePerfomanceService.FindPerfomance(ticket.PerfomanceId).Tickets.ToList();
                foreach (var t1 in t)
                {
                    if ((t1.TicketCategory == ticket.TicketCategory) && (t1.TicketState == "Can be sold"))
                    {
                        ticket.Price = t1.Price;
                        break;
                    }
                }
                var conf = configPerformance();
                ptu.Performance = conf.Map<PerformanceDTO, PerformanceViewModel>
                (managePerfomanceService.FindPerfomance(ticket.PerfomanceId));

                var conf2 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<UserDTO, UserViewModel>()
                        .IncludeAllDerived();
                    cfg.CreateMap<TicketDTO, TicketViewModel>();
                }).CreateMapper();
                ptu.User = conf2.Map<UserDTO, UserViewModel>
                (authorizationService.FindUserById(ticket.UserId));
                ptu.Ticket = ticket;
                return View(ptu);
            }
            catch
            {
                return Content("Сталась помилка при спробі купити квиток");
            }
        }

        [HttpPost]
        public ActionResult BuyTicketPost(TicketViewModel ticket)
        {
            try
            {
                var performanceDTO = managePerfomanceService.FindPerfomance(ticket.PerfomanceId);
                var cUser = authorizationService.FindUserById(ticket.UserId);

                var tickets = performanceDTO.Tickets.ToList();
                foreach (var ticket1 in tickets.Where(a => a.TicketCategory.Equals(ticket.TicketCategory)))
                {
                    if (ticket1.TicketState == "Can be sold")
                    {
                        if(ticket.TicketState == "Buy") { 
                            ticket1.TicketState = "Sold";
                            ticket1.UserId = cUser.Id;
                            managePerfomanceService.SellTicket(ticket1, cUser);
                            return Content("Ви придбали квиток");
                        }
                        else
                        {
                            ticket1.TicketState = "Brone";
                            ticket1.UserId = cUser.Id;
                            managePerfomanceService.SellTicket(ticket1, cUser);
                            return Content("Ви придбали квиток");
                        }
                    }
                }
                return Content("Квитка з заданими параметрами на жаль немає");
            }
            catch
            {
                return Content("Сталась помилка при спробі купити квиток");
            }
        }
        [HttpPost]
        public ActionResult AddPerformance(UserViewModel user)
        {
            var configuration1 = configUser();
            var aUser = configuration1.Map<UserDTO, UserViewModel>(authorizationService.FindUserById(user.Id));
            var ticket = new TicketViewModel { UserId = aUser.Id};
            return View(ticket);
        }
        [HttpPost]
        public ActionResult AddPerformancePost(DateTime dateTime, string name, string genres, string authors)
        {
            try
            {
                managePerfomanceService.AddPerformance(name, dateTime, authors, genres);
                return Content("Ви добавили виставу");
            }
            catch 
            { 
                return Content("Сталась помилка під час добавлення вистави. Введіть дані корректно");
            }
        }
        [HttpPost]
        public ActionResult DeletePerformance(int idPerformance, int idUser)
        {
            managePerfomanceService.DeletePerformance(idPerformance);
            var ticket = new TicketViewModel { PerfomanceId = idPerformance, UserId = idUser };

            return RedirectToAction("PerformanceList", ticket);
        }

    }
}