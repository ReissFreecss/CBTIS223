using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace CBTIS223_v2.Models
{
    public partial class EstudiantesServicio
    {
        [Required(ErrorMessage = "El número de control es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de control debe tener exactamente 8 dígitos.")]
        public string NumeroControl { get; set; } = null!;

        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = " ";
        public string Curp { get; set; } = null!;
        public string Especialidad { get; set; } = null!;
        public string Ciclo { get; set; } = null!;

        public virtual EstudiantesPractica? EstudiantesPractica { get; set; }
        public virtual ServicioSocial? ServicioSocial { get; set; }
    }
}
