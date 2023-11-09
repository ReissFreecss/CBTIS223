using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using QuestPDF.Fluent;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace CBTIS223_v2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _host;
        private cbtis223Context _context;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _logger = logger;
            _host = host;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Root, jefeVinc, jefeEsc, ofServ, ofPrac, jefeDep")]
        public IActionResult Documentos()
        {
            return View();
        }

        [Authorize(Roles = "Root, jefeVinc, jefeEsc, ofServ, ofPrac, jefeDep")]
        public IActionResult Estadisticas()
        {
            return View();
        }

        [Authorize(Roles = "Root, jefeEsc, jefeDep, ofServ")]
        public IActionResult Registro()
        {
            return View();
        }

        [Authorize(Roles = "Root, jefeVinc, jefeDep, ofPrac")]
        public IActionResult Registro2()
        {
            return View();
        }

        [Authorize(Roles = "Root")]
        public IActionResult Institucion()
        {
            return View();
        }

        [Authorize(Roles = "Root")]
        public IActionResult Escuela()
        {
            return View();
        }

        [Authorize(Roles = "Root")]
        public IActionResult Bitacora()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*
            string carrera = "abc";
            List<Escuela> escuelas = _context.Escuelas.ToList();

            foreach (var escuela in escuelas)
            {
                carrera = escuela.NombreEscuela;
            }
         */
        
    }
}