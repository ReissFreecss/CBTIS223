using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CBTIS223_v2.Controllers
{
    public class EditarInsController : Controller
    {
        [BindProperty]
        public Institucione modeloI { get; set; }

        private IWebHostEnvironment _host;
        private cbtis223Context _context;
        private ILogger<InstitucionesController> _logger;
        public EditarInsController(ILogger<InstitucionesController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _host = host;
            _logger = (ILogger<InstitucionesController>?)logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int ID) { 
            cbtis223Context ct = new cbtis223Context();
            modeloI = await ct.Instituciones.FindAsync(ID);
            if (modeloI == null)
            {
                return NotFound();
            }
            _logger.LogInformation(ID.ToString());
            _logger.LogInformation(modeloI.IdInstitucion.ToString());
            _logger.LogInformation(modeloI.Institucion);
            ViewData["modelo"] = modeloI;
            return View(modeloI);
        }

        [HttpPost]
        public async Task<IActionResult> EditarInstitucion(
            [Bind("IdInstitucion", "Institucion", "TipoInstitucion", "Supervisor", "UbicacionInstitucion")] Institucione modelI)
        {
            _logger.LogInformation(modeloI.IdInstitucion+" "+modeloI.Institucion+" "+modeloI.TipoInstitucion+" "+modeloI.Supervisor+" *"+modeloI.UbicacionInstitucion);
            _context.Instituciones.Update(modeloI);
            await _context.SaveChangesAsync();
            return View("../Home/Institucion");
        }
    }
}
