using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthCore.Core;
using AuthCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthCore.Controllers
{
    public class AccountController : Controller
    {
        DB db;
        public AccountController(DB _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(string login = "guest", string returnUrl = "/home")
        {
            var user = db.Users.SingleOrDefault(_ => _.Email == login);
            if (user != null)
                return Redirect("/Account/login"); // )
            db.Users.Add(new User()
            {
                Email = login
            });
            db.SaveChanges();

            await Authenticate(login);
            return Redirect(returnUrl);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string login = "guest", string returnUrl = "/home")
        {
            var user = db.Users.SingleOrDefault(_ => _.Email == login);

            if(user!=null)
                await Authenticate(user?.Email);
            return Redirect(returnUrl);
        }

        //[ValidateAntiForgeryToken]
        [HttpGet]
        public IActionResult Logout()//string login = "guest", string returnUrl = "home")
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,"myRole")
            };
            ///
            await HttpContext.SignInAsync(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims,
                        "ApplicationCookie"
                        //ClaimsIdentity.DefaultNameClaimType,
                        //ClaimsIdentity.DefaultRoleClaimType
                )));
        }
        }
    }