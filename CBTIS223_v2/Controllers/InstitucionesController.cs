using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CBTIS223_v2.Controllers
{
    public class InstitucionesController : Controller
    {
        [BindProperty]
        public Institucione modeloI { get; set; }

        private IWebHostEnvironment _host;
        private cbtis223Context _context;
        private ILogger<InstitucionesController> _logger;
        public InstitucionesController(ILogger<InstitucionesController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _host = host;
            _logger = (ILogger<InstitucionesController>?)logger;
            _context = context;
        }

        public IActionResult Lista()
        {
            var Lista = _context.Instituciones.ToList();
            return View(Lista);
        }

        public IActionResult Editar(int IdInstitucion)
        {
            var ID = _context.Instituciones.FirstOrDefault(x => x.IdInstitucion == IdInstitucion);
            return View(ID);
        }

        public async Task<IActionResult> Eliminar(int IdInstitucion)
        {
            var ID = _context.Instituciones.FirstOrDefault(x => x.IdInstitucion == IdInstitucion);
            _context.Remove(modeloI);
            await _context.SaveChangesAsync();
            return View("../Home/Institucion");
        }

        [HttpPost]
        public async Task<IActionResult> InsertarInstitucion(
            [Bind("IdInstitucion", "Institucion", "TipoInstitucion", "Supervisor", "UbicacionInstitucion")] Institucione modelI)
        {
            _logger.LogInformation(modeloI.IdInstitucion+" "+modeloI.Institucion+" "+modeloI.TipoInstitucion+" "+modeloI.Supervisor+" *"+modeloI.UbicacionInstitucion);
            _context.Add(modeloI);
            await _context.SaveChangesAsync();
            return View("../Home/Institucion");
        }
    }
}
