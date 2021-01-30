using Foodzfame2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Foodzfame2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View(new LoginUser());
        }

        [HttpPost]
        public IActionResult Login(LoginUser login)
        {
            if(login.UserName=="foodzfame" && login.Password== "p$wYgs}5n?xG[29a")
            {
                var userClaims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, login.UserName),
                new Claim(ClaimTypes.Email, "info@foodzfame.com"),
                 };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                HttpContext.SignInAsync(userPrincipal);
            }
            else
            {
                return Unauthorized();
            }
            return RedirectToAction("Index","Admin");
        }
    }
}
