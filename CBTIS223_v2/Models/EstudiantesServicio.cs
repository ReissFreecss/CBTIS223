using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class EstudiantesServicio
    {
        public string NumeroControl { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string Curp { get; set; } = null!;
        public string Especialidad { get; set; } = null!;

        public virtual EstudiantesPractica? EstudiantesPractica { get; set; }
        public virtual ServicioSocial? ServicioSocial { get; set; }
    }
}
