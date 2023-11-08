using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CBTIS223_v2.Controllers
{
    public class RegistrosController : Controller
    {
        [BindProperty]
        public EstudiantesServicio modeloE { get; set; }
        public ServicioSocial modeloS { get; set; }
        public Institucione modeloI { get; set; }

        private IWebHostEnvironment _host;
        private cbtis223Context _context;

        public RegistrosController(ILogger<HomeController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _host = host;
            _context = context;
        }
        [HttpGet]
        public IActionResult Registro(EstudiantesServicio modeloE)
        {
            return View();
        }
        // OLIIIIIIIIII
        //Metodo que guarda cambios en BD
        [HttpPost]
        public async Task<IActionResult> InsertarRegistros(
            [Bind("NumeroControl", "Nombre", "AP", "AM", "CURP", "Especialidad")] EstudiantesServicio modeloE,
            [Bind("NumeroControl", "FechaInicio", "FechaFinal", "AM")] ServicioSocial modeloS)
        {
            _context.Add(modeloE);
            await _context.SaveChangesAsync();
            _context.Add(modeloS);
            await _context.SaveChangesAsync();
            return RedirectToAction("Registro");
        }
    }
}
