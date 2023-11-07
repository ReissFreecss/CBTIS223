using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class PracticasProfesionale
    {
        public string EstudianteNc { get; set; } = null!;
        public DateOnly FechaInicioPracticas { get; set; }
        public DateOnly FechaTerminoPracticas { get; set; }
        public int IdInstiPracticas { get; set; }

        public virtual EstudiantesPractica EstudianteNcNavigation { get; set; } = null!;
        public virtual Institucione IdInstiPracticasNavigation { get; set; } = null!;
    }
}
