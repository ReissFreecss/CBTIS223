﻿using System;
using System.Collections.Generic;

namespace CBTIS223_v2.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string token_recovery { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
