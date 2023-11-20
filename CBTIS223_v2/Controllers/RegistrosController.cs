using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        [HttpPost]
        public async Task<IActionResult> InsertarRegistros(
            [Bind("NumeroControl", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Curp", "Especialidad","Ciclo")] EstudiantesServicio modeloE,
            [Bind("EstudianteNc", "FechaInicioServicio", "FechaTerminoServicio", "actividad_servicio","IdInstiServicio")] ServicioSocial modeloS)
        {
            modeloS.EstudianteNc = modeloE.NumeroControl;
            _logger.LogInformation(modeloE.NumeroControl+" "+modeloE.Nombre+" "+modeloE.ApellidoPaterno+" *"+modeloE.ApellidoMaterno);
            _logger.LogInformation(modeloS.EstudianteNc+" "+modeloS.FechaInicioServicio+" "+modeloS.FechaTerminoServicio+modeloS.actividad_servicio+" *"+modeloS.IdInstiServicio);
            try
            {
                _context.Add(modeloE);
                _context.Add(modeloS);
                await _context.SaveChangesAsync();
                ViewBag.Succesful = "Alumno registrado con exito";
                return View("../Home/Registro");
            }catch (Exception ex)
            {
                ViewBag.Error="El alumno ya se encuentra registrado";
                return View("../Home/Registro");
            }
        }

        }
    }
