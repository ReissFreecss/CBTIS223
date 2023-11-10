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
        private ILogger<RegistrosController> _logger;

        public RegistrosController(ILogger<RegistrosController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _host = host;
            _logger = (ILogger<RegistrosController>?)logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Registro(EstudiantesServicio modeloE)
        {
            return View();
        }
        // OLIIIIIIIIIIaaaaa
        //Metodo que guarda cambios en BD
        [HttpPost]
        public async Task<IActionResult> InsertarRegistros(
            [Bind("NumeroControl", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Curp", "Especialidad")] EstudiantesServicio modeloE,
            [Bind("EstudianteNc", "FechaInicioServicio", "FechaTerminoServicio", "IdInstiServicio")] ServicioSocial modeloS)
        {
            modeloS.EstudianteNc = modeloE.NumeroControl;
            _logger.LogInformation(modeloE.NumeroControl+" "+modeloE.Nombre+" "+modeloE.ApellidoPaterno+" *"+modeloE.ApellidoMaterno);
            _logger.LogInformation(modeloS.EstudianteNc+" "+modeloS.FechaInicioServicio+" "+modeloS.FechaTerminoServicio+" *"+modeloS.IdInstiServicio);
            _context.Add(modeloE);
            _context.Add(modeloS);
            await _context.SaveChangesAsync();
            return View("../Home/Registro");
        }
    }
}
