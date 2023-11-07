using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class Institucione
    {
        public Institucione()
        {
            PracticasProfesionales = new HashSet<PracticasProfesionale>();
            ServicioSocials = new HashSet<ServicioSocial>();
        }

        public int IdInstitucion { get; set; }
        public string Institucion { get; set; } = null!;
        public string TipoInstitucion { get; set; } = null!;
        public string Supervisor { get; set; } = null!;
        public string UbicacionInstitucion { get; set; } = null!;

        public virtual ICollection<PracticasProfesionale> PracticasProfesionales { get; set; }
        public virtual ICollection<ServicioSocial> ServicioSocials { get; set; }
    }
}
