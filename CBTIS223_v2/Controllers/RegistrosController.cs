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
        [HttpGet]
        public IActionResult Registro(EstudiantesServicio modeloE)
        {
            List<Institucione> lst = null!;
            using (Models.cbtis223Context db = new Models.cbtis223Context())
            {
                lst = (from d in db.Instituciones
                       where d.TipoInstitucion == "Publica"
                       select new Institucione
                       {
                           IdInstitucion = d.IdInstitucion,
                           Institucion = d.Institucion,
                           TipoInstitucion = d.TipoInstitucion,
                           Supervisor = d.Supervisor

                       }).ToList();
            }
            List<SelectListItem> items = lst.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Institucion,
                    Value = d.IdInstitucion.ToString(),
                    Selected = true
                };
            });
           

            ViewBag.items = items;
            return View("../Home/Registro");
        }

        [HttpPost]
        public async Task<IActionResult> InsertarRegistros(
            [Bind("NumeroControl", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Curp", "Especialidad")] EstudiantesServicio modeloE,
            [Bind("EstudianteNc", "FechaInicioServicio", "FechaTerminoServicio", "IdInstiServicio","actividad_servicio")] ServicioSocial modeloS)
        {
            modeloS.EstudianteNc = modeloE.NumeroControl;
            _logger.LogInformation(modeloE.NumeroControl+" "+modeloE.Nombre+" "+modeloE.ApellidoPaterno+" *"+modeloE.ApellidoMaterno);
            _logger.LogInformation(modeloS.EstudianteNc+" "+modeloS.FechaInicioServicio+" "+modeloS.FechaTerminoServicio+" *"+modeloS.IdInstiServicio);
            _context.Add(modeloE);
            _context.Add(modeloS);
            await _context.SaveChangesAsync();
            return View("../Home/Documentos");
        }

        }
    }
