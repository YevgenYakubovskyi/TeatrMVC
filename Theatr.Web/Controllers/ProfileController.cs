using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Theatr.BLL.DTO;
using Theatr.BLL.Interfaces;
using Theatr.Web.Models;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Theatr.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        IAuthorizationService authorizationService;
        IManagePerfomanceService managePerfomanceService;
        IUserService userService;
        public ProfileController(IAuthorizationService authorizationService, IManagePerfomanceService managePerfomanceService, IUserService userService)
        {
            this.authorizationService = authorizationService;
            this.managePerfomanceService = managePerfomanceService;
            this.userService = userService;
        }

        public ProfileController()
        {
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = authorizationService.FindUserById(userId);
            var model = new IndexViewModel
            {
                Name = user.Name,
                Email = user.Email, 
                Surname = user.Surname,
            };
            return View("Index", model);
        }

        public ActionResult TicketList()
        {
            var userId = User.Identity.GetUserId();

            var ptu = new PTUViewModel();
            try
            {
                var cTickets = authorizationService.GetTicketsByUserId(userId);

                ptu.Tickets = cTickets.Select(a => new TicketViewModel
                {
                    Id = a.Id,
                    Price = a.Price,
                    TicketCategory = a.TicketCategory,
                    TicketState = a.TicketState,
                    Performance = new PerformanceViewModel { Name = managePerfomanceService.FindPerfomance(a.PerfomanceId).Name }
                });
                ViewBag.HaveTicket = "Yes";

                return View(ptu);
            }
            catch
            {
                ViewBag.HaveTicket = "No";
                return View(ptu);
            }
        }

        public ActionResult PerformanceList()
        {
            var configuration = configPerformance();
            var ptu = new PTUViewModel();
            ptu.Performance = configuration.Map<
                IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                (managePerfomanceService.GetAll());
            return View(ptu);
        }

        [HttpPost]
        public ActionResult SearchPerformanceByName(string namePerf)
        {
            var configuration = configPerformance();

            var ptu = new PTUViewModel();;
            ptu.Performance = configuration.Map<
                IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                (managePerfomanceService.FindByName(namePerf));
            return View(ptu);
        }
        [HttpPost]
        public ActionResult SearchPerformanceByAuthor(string nameAuthor)
        {
            var ptu = new PTUViewModel();
            try
            {
                ViewBag.Error = " ";
                var configuration = configPerformance();

                ptu.Performance = configuration.Map<
                    IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                    (managePerfomanceService.FindByAuthorName(nameAuthor));

                return View(ptu);
            }
            catch
            {
                ViewBag.Error = "Вистав з заданим іменем автора не знайдено. Відображені всі вистави";
                var configuration = configPerformance();
                ptu.Performance = configuration.Map<
                    IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                    (managePerfomanceService.GetAll());
                return View(ptu);

            }
            
        }
        [HttpPost]
        public ActionResult SearchPerformanceByDate(DateTime dateTimeStart, DateTime dateTimeEnd)
        {

            var ptu = new PTUViewModel();
            var configuration = configPerformance();

            ptu.Performance = configuration.Map<
                IEnumerable<PerformanceDTO>, IEnumerable<PerformanceViewModel>>
                (managePerfomanceService.FindByTime(dateTimeStart, dateTimeEnd));
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
                var userId = User.Identity.GetUserId();
                var ptu = new PTUSoloViewModel();

                var configuration = configPerformance();

                ptu.Performance = configuration.Map<
                    PerformanceDTO, PerformanceViewModel>
                    (managePerfomanceService.FindPerfomance(ticket.PerfomanceId));
                var configuration1 = configUser();
                ptu.User = configuration1.Map<UserDTO, UserViewModel>(authorizationService.FindUserById(userId));
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
                    return Content("Будь ласка, коректно дані введіть");
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

                var userId = User.Identity.GetUserId();
                var ptu = new PTUSoloViewModel();
                var configuration = configPerformance();

                ptu.Performance = configuration.Map<
                    PerformanceDTO, PerformanceViewModel>
                    (managePerfomanceService.FindPerfomance(ticket.PerfomanceId));
                var configuration1 = configUser();
                ptu.User = configuration1.Map<UserDTO, UserViewModel>(authorizationService.FindUserById(userId));
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

                var userId = User.Identity.GetUserId();
                var performanceDTO = managePerfomanceService.FindPerfomance(ticket.PerfomanceId);

                var tickets = performanceDTO.Tickets.ToList();
                foreach (var ticket1 in tickets.Where(a => a.TicketCategory.Equals(ticket.TicketCategory)))
                {
                    if (ticket1.TicketState == "Can be sold")
                    {
                        if(ticket.TicketState == "Buy") { 
                            ticket1.TicketState = "Sold";
                            ticket1.UserId = userId;
                            managePerfomanceService.SellTicket(ticket1, userId);
                            return Content("Ви придбали квиток");
                        }
                        else
                        {
                            ticket1.TicketState = "Brone";
                            ticket1.UserId = userId;
                            managePerfomanceService.SellTicket(ticket1, userId);
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
        
        public ActionResult AddPerformance()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPerformancePost(DateTime dateTime, string name, string genres, string authors)
        {
            try
            {
                managePerfomanceService.AddPerformance(name, dateTime, authors, genres);
                return RedirectToAction("PerformanceList");
            }
            catch 
            { 
                return Content("Сталась помилка під час добавлення вистави. Введіть дані корректно");
            }
        }
        [HttpPost]
        public ActionResult DeletePerformance(int idPerformance)
        {
            managePerfomanceService.DeletePerformance(idPerformance);
            return RedirectToAction("PerformanceList");
        }

    }
}