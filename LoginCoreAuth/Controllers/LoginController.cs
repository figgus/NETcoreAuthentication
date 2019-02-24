using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginCoreAuth.Controllers
{
    public class LoginController : Controller
    {
        [Route("login")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LogIn(string username, string password)
        {
            if (username=="admin" && password=="1234")
            {
                this.Logear();
                return Json("true");
            }
            else
            {
                return Json("false");
            }
            
        }


        private async void Logear()
        {
            const string Issuer = "https://contoso.com";
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "joaquin", ClaimValueTypes.String, Issuer));
            var userIdentity = new ClaimsIdentity("UserIdentity");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
               userPrincipal,
               new AuthenticationProperties
               {
                   ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                   IsPersistent = false,
                   AllowRefresh = false
               });
        }
    }
}