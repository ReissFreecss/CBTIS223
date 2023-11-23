using CBTIS223_v2.Models;
using GestorAlumnos_V1.Authorizaton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CBTIS223_v2.Controllers
{
    public class EscuelasController : Controller
    {
        [BindProperty]
        public Escuela modeloE { get; set; }

        private IWebHostEnvironment _host;
        private cbtis223Context _context;
        //private ILogger<EscuelasController> _logger;
        public EscuelasController(ILogger<InstitucionesController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _host = host;
            //_logger = (ILogger<EscuelasController>?)logger;
            _context = context;
        }

        //Despues de darle al boton editar en la vista Escuela, recibes el ID y creas el modelo con los valores cuando empaten 
        //el valor del ID en la lista con todos los valores posibles de escuela. Y se dirige a la vista para recibir los nuevos datos
        [Authorize(Roles ="Root")]
        public async Task<IActionResult> Editar(int ID)
        {

            cbtis223Context ct = new cbtis223Context();
            IList<Escuela> escuelas = await ct.GetEscuelas();
            Escuela escuela = escuelas.FirstOrDefault(x => x.ID == ID);
            if (escuelas == null)
            {
                ViewBag.Error = "Error";
                return View("../Home/Escuela");
            }

            ViewBag.Datos = escuela;
            return View("../Home/EditarEscuela");
        }

        [HttpPost]
        public async Task<IActionResult> Insertar(int ID)
        {
            //Recibe los datos del submit y los cambia valor por valor en el modelo de la escuela. 
            //Al final guarda los cambios y regresa a la vista de escuela principal
            try
            {
                cbtis223Context ct = new cbtis223Context();
                var escuela = await _context.Escuelas.FindAsync(ID);
                escuela.NombreEscuela = modeloE.NombreEscuela;
                escuela.NombreDirector = modeloE.NombreDirector;
                escuela.NombreEncargadoEstatal = modeloE.NombreEncargadoEstatal;
                escuela.NombreDirectorGeneral = modeloE.NombreDirectorGeneral;
                escuela.NombreDirectorGeneralProfesiones = modeloE.NombreDirectorGeneralProfesiones;
                await _context.SaveChangesAsync();
                ViewBag.Succesful = "Datos modificados con exito";
                return View("../Home/Escuela");
            }catch(Exception ex)
            {
                ViewBag.Error = "No se pudo realizar la actualización";
                return View("../Home/Escuela");
            }
        }

    }
}
