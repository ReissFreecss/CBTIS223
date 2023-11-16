using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class PracticasProfesionale
    {
        public string EstudianteNc { get; set; } = null!;
        public DateTime FechaInicioPracticas { get; set; }
        public DateTime FechaTerminoPracticas { get; set; }
        public string actividad_practicas { get; set; }
        public int IdInstiPracticas { get; set; }

        public virtual EstudiantesPractica EstudianteNcNavigation { get; set; } = null!;
        public virtual Institucione IdInstiPracticasNavigation { get; set; } = null!;
    }
}
