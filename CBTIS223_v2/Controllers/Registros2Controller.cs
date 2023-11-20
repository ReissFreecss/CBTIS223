using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CBTIS223_v2.Controllers
{
    public class Registros2Controller : Controller
    {
        [BindProperty]
        public EstudiantesPractica modeloEP { get; set; }
        public EstudiantesServicio modeloES { get; set; }
        public PracticasProfesionale modeloP { get; set; }
        public Institucione modeloI { get; set; }

        private IWebHostEnvironment _host;
        private cbtis223Context _context;
        private ILogger<Registros2Controller> _logger;

        public Registros2Controller(ILogger<Registros2Controller> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _host = host;
            _logger = (ILogger<Registros2Controller>?)logger;
            _context = context;
        }
        
        public async Task<IActionResult> obtenerDatos(string NumeroControl)
        {
            List<Institucione> lst = null!;
            using (Models.cbtis223Context db = new Models.cbtis223Context())
            {
                lst = (from d in db.Instituciones
                       where d.TipoInstitucion == "Privada"
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
            cbtis223Context ct = new cbtis223Context();
            IList<EstudiantesServicio> estudiantes = await ct.GetEstudiantes();
            EstudiantesServicio estudiante = estudiantes.FirstOrDefault(x => x.NumeroControl == NumeroControl);
            if (estudiante == null)
            {
                ViewBag.Error = "El alumno no ha realizado el servicio social";
                return View("../Home/Registro2");
            }

            ViewBag.items = items;
            ViewBag.Estudiante = estudiante;
            return View("../Home/Registro2Insercion");

        }

        [HttpPost]
        public async Task<IActionResult> InsertarRegistros(
            [Bind("NumeroControl", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Curp", "Especialidad")] EstudiantesServicio modeloES,
            [Bind("EstudianteNc", "FechaInicioPracticas", "FechaTerminoPracticas", "actividad_practicas", "IdInstiPracticas")] PracticasProfesionale modeloP,
            [Bind("EstudianteNc")] EstudiantesPractica modeloEP)
        {
            
                modeloEP.EstudianteNc = modeloES.NumeroControl;
                modeloP.EstudianteNc = modeloES.NumeroControl;
                _logger.LogInformation(modeloES.NumeroControl);
                _logger.LogInformation(modeloP.EstudianteNc+" "+modeloP.FechaInicioPracticas+" "+modeloP.FechaTerminoPracticas+modeloP.actividad_practicas+" *"+modeloP.IdInstiPracticas);
            try
            {
                _context.Add(modeloP);
                _context.Add(modeloEP);
                await _context.SaveChangesAsync();
                ViewBag.Succesful = "Alumno Registrado con Exito";
                return View("../Home/Registro2");
            }catch(Exception ex)
            {
                ViewBag.Error = "El alumno ya se encuentra registrado";
                return View("../Home/Registro2");
            }
        }
    }
}
