using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CBTIS223_v2.Models
{
    public partial class ServicioSocial
    {
        public string EstudianteNc { get; set; } = null!;

        public DateTime FechaInicioServicio { get; set; }
        public DateTime FechaTerminoServicio { get; set; }
        public int IdInstiServicio { get; set; }
        public string? actividad_servicio { get; set; }

        public virtual EstudiantesServicio EstudianteNcNavigation { get; set; } = null!;
        public virtual Institucione IdInstiServicioNavigation { get; set; } = null!;
    }
}
