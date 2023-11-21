using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CBTIS223_v2.Controllers
{
    public class InstitucionesController : Controller
    {
        [BindProperty]
        public Institucione modeloI { get; set; }

        public IList<Institucione> Institucionesx { get; set; }

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

        public async Task<IActionResult> Editar (int ID)
        {
            cbtis223Context ct = new cbtis223Context();
            IList<Institucione> instituciones = await ct.GetInstituciones();
            Institucione institucion = instituciones.FirstOrDefault(x => x.IdInstitucion == ID);
            if (instituciones == null)
            {
                ViewBag.Error = "Error";
                return View("../Home/Institucion");
            }

            ViewBag.Datos = institucion;
            return View("../Home/EditarInstituciones");
        }



        public async Task<IActionResult> Eliminar(int ID)
        {
            var inst = _context.Instituciones.FirstOrDefault(x => x.IdInstitucion == ID);
            _context.Remove(inst);
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
