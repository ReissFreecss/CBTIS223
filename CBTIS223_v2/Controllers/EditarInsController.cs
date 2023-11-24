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
        public async Task<IActionResult> Editar(int ID)
        {
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
        public async Task<IActionResult> EditarInstitucion(int IdInstitucion)
        {
            //Recibe los datos del submit y los cambia valor por valor en el modelo de la escuela. 
            //Al final guarda los cambios y regresa a la vista de escuela principal
            try
            {
                cbtis223Context ct = new cbtis223Context();
                var institucion = await _context.Instituciones.FindAsync(IdInstitucion);
                institucion.Institucion = modeloI.Institucion;
                institucion.TipoInstitucion = modeloI.TipoInstitucion;
                institucion.Supervisor = modeloI.Supervisor;
                institucion.Calle = modeloI.Calle;
                institucion.NoCalle = modeloI.NoCalle;
                institucion.CodigoPostal = modeloI.CodigoPostal;
                institucion.Colonia = modeloI.Colonia;
                institucion.Municipio = modeloI.Municipio;
                institucion.Estado= modeloI.Estado;


                await _context.SaveChangesAsync();
                ViewBag.Succesful = "Datos modificados con exito";
                return View("../Home/Institucion");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se pudo realizar la actualización";
                return View("../Home/Institucion");
            }
        }
    }
}
