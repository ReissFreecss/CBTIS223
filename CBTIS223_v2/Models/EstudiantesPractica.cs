using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class EstudiantesPractica
    {
        public string EstudianteNc { get; set; } = null!;

        public virtual EstudiantesServicio EstudianteNcNavigation { get; set; } = null!;
        public virtual PracticasProfesionale? PracticasProfesionale { get; set; }
    }
}
