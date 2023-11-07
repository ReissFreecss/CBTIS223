using Microsoft.AspNetCore.Mvc;
using CBTIS223_v2.Models;
using CBTIS223_v2.Data;


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CBTIS223_v2.Controllers
{
    public class AccesoController : Controller
    {
        //Busca de acuerdo al model del usuario si hay uno registrado
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Models.Usuario _usuario)
        {
            DA_Logica _da_usuario = new DA_Logica();
            var usuario = _da_usuario.ValidarUsuario(_usuario.Correo, _usuario.Contraseña);

            if (usuario != null)
            {
                var claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", usuario.Correo)
                };

                string rol = usuario.Rol;
                claims.Add(new Claim(ClaimTypes.Role, rol));
                

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Documentos", "Home");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}
