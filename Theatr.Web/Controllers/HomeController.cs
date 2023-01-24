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

namespace Theatr.Web.Controllers
{
    public class HomeController : Controller
    {
        IAuthorizationService authorizationService;
        IRegistrationService registrationService;
        public HomeController(IAuthorizationService authorizationService, IRegistrationService registrationService)
        {
            this.authorizationService = authorizationService;
            this.registrationService = registrationService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterToProfile(UserViewModel user)
        {
            try {
                var configuration = new MapperConfiguration(cfg => {
                    cfg.CreateMap<UserViewModel, UserDTO>();
                }).CreateMapper();
                var userDTO = configuration.Map<UserViewModel, UserDTO>(user);
                registrationService.Register(userDTO);
                ViewBag.Message = "Аккаунт успішно зареєстровано";
                return View("Result");
            }
            catch{
                ViewBag.Message = "Сталась помилка при спробі реєстрації";
                return View("Result");
            }
        }

        [HttpPost]
        public ActionResult LoginToProfile(UserViewModel user)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => {
                    cfg.CreateMap<UserDTO, UserViewModel>()
                        .IncludeAllDerived();
                    cfg.CreateMap<TicketDTO, TicketViewModel>();
                }).CreateMapper(); ;
                var userC = mapper.Map<UserDTO, UserViewModel>(authorizationService.Login(user.Email, user.Password));
                return RedirectToAction("Index", "Profile", userC);
            }
            catch
            {

                ViewBag.Message = "Сталась помилка при спробі увійти";
                return View("Result");
            }
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}