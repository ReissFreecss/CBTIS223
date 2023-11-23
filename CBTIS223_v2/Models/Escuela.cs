using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class Escuela
    {
        public int ID { get; set; }
        public string NombreEscuela { get; set; } = null!;
        public string NombreDirector { get; set; } = null!;
        public string NombreEncargadoEstatal { get; set; } = null!;
        public string NombreDirectorGeneral { get; set; } = null!;
        public string NombreDirectorGeneralProfesiones { get; set; } = null!;

    }
}
