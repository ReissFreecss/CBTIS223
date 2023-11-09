using Microsoft.AspNetCore.Mvc;
using CBTIS223_v2.Models;
using CBTIS223_v2.Data;


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;

namespace CBTIS223_v2.Controllers
{
    public class AccesoController : Controller
    {
        //Url general del dominio
        string urlDomain = "https://localhost:7202/";
        //Busca de acuerdo al model del usuario si hay uno registrado
        public IActionResult Login()
        {
            return View();
        }


        //Metodo para comenzar la recuperacion de contraseña enviando el modelo de usuario
        [HttpGet]
        public ActionResult StartRecovery()
        {
            Models.Usuario model = new Models.Usuario();
            return View(model);
        }

        //Metodo ya con el token de recuperacion obtenido
        [HttpPost]
        public ActionResult StartRecovery(Models.Usuario model)
        {
            try
            {

                //Hasheo del token
                string token = getSha256(Guid.NewGuid().ToString());
                //
                using (Models.cbtis223Context db = new Models.cbtis223Context())
                {

                    var oUser = db.Usuario.Where(d => d.Correo == model.Correo).FirstOrDefault();
                    if (oUser != null)
                    {
                        //Insertamos el token recovery en la Bd
                        oUser.token_recovery = token;
                        db.Entry(oUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();

                        //Enviar Email
                        SendEmail(oUser.Correo, token);

                    }
                }


                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Recovery(string token)
        {
            Models.RecoveryPasswordViewModel model = new Models.RecoveryPasswordViewModel();
            model.token = token;
            using (Models.cbtis223Context db = new Models.cbtis223Context())
            {
                if (model.token == null || model.token.Trim().Equals(""))
                {
                    return View("Login");
                }
                var oUser = db.Usuario.Where(d => d.token_recovery == model.token).FirstOrDefault();
                if (oUser == null)
                {
                    ViewBag.Error = "Tu token ha expirado";
                    return View("Login");
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Recovery(Models.RecoveryPasswordViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                using (Models.cbtis223Context db = new Models.cbtis223Context())
                {
                    var oUser = db.Usuario.Where(d => d.token_recovery == model.token).FirstOrDefault();

                    if (oUser != null)
                    {
                        oUser.Contraseña = model.password;
                        oUser.token_recovery = "";
                        db.Entry(oUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            ViewBag.Message = "Contraseña modificada con exito";
            return View("Login");
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

        #region HELPERS

        //Encriptamiento del token
        private string getSha256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        //Metodo para enviar correos de soporte para el reestablecimiento de la contraseña
        private void SendEmail(string EmailDestino, string token)
        {
            string EmailOrigen = "soportecbtisgaleana223@gmail.com";
            string contra = "bnoh agdb gdlj wsfo";
            string url = urlDomain + "Acceso/Recovery/?token=" + token;
            MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de Contraseña", "<p>Este es un correo para recuperar tu contraseña" +
                "</p><br>" + "<a href='" + url + "'>Clic aquí para recuperar</a>");

            oMailMessage.IsBodyHtml = true;
            SmtpClient osmtpClient = new SmtpClient("smtp.gmail.com");
            osmtpClient.EnableSsl = true;
            osmtpClient.UseDefaultCredentials = false;
            osmtpClient.Port = 587;
            osmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, contra);

            osmtpClient.Send(oMailMessage);

            #endregion

        }
    }
}
