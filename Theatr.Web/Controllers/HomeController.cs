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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Theatr.Web.Controllers
{
    public class HomeController : Controller
    {
        IAuthorizationService authorizationService;
        IRegistrationService registrationService;
        IUserService UserService;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public HomeController(IAuthorizationService authorizationService, IRegistrationService registrationService, IUserService userService)
        {
            this.authorizationService = authorizationService;
            this.registrationService = registrationService;
            UserService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }
        #region Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Логін або пароль невірний.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Surname = model.Surname,
                    Role = "user"
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    ViewBag.OperationDetails = operationDetails.Message;
                    return View("SuccessRegister");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
        #endregion
        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "somemail@mail.ru",
                Password = "ad46D_ewr3",
                Name = "Akuma",
                Surname = "Izao",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }

        




    }
}