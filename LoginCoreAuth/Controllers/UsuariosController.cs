using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LoginCoreAuth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LoginCoreAuth.Controllers
{
    public class UsuariosController : Controller
    {
        List<Usuario> users = new List<Usuario>();
        public UsuariosController()
        {
            users.Add(new Usuario {id=1, username="admin",password="1234" });
            users.Add(new Usuario {id=2, username = "juan", password = "1234" });
        }

        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/Usuarios")]
        public ActionResult Index()
        {
            //this.Token();
            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/Usuarios/{id}")]
        public ActionResult Details(int id)
        {
            return Ok(users.First(p=>p.id==id));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/Usuarios")]
        [HttpPost]
        public ActionResult CrearUsuario(Usuario user)
        {
            users.Add(user);
            return Ok();
        }




        [HttpPost]
        public IActionResult Token()
        {
            var header = Request.Headers["Authorization"];
            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic".Length).Trim();
                var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                var usernameAndPass = usernameAndPassenc.Split(":");
                if (usernameAndPass[0]=="Admin" && usernameAndPass[1]=="pass")
                {
                    var claimsData = new[] { new Claim(ClaimTypes.Name, usernameAndPass[0]) };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdcasafhgfhfghrtyfvhfghfhfghfghfghfghsasxbvfgbhjkiyursfsvhnjnm"));
                    var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: "mysite.com",
                        audience: "mysite.com",
                        expires: DateTime.Now.AddMinutes(1),
                        claims: claimsData,
                        signingCredentials: signInCred
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(tokenString);
                }
                
            }
            return BadRequest("wrong request");
        }
    }
}