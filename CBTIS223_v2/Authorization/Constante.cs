namespace GestorAlumnos_V1.Authorizaton
{
    public class Constante
    {
        //Jefe de Oficina de Practicas 
        public static readonly string CreateAlumnoJP = "CreateAlumno";
        public static readonly string ReadAlumnoJP = "ReadAlumno";
        public static readonly string UpdateAlumnoJP = "UpdateAlumno";
        public static readonly string DeleteAlumnoJP = "DeleteAlumno";

        //Secretario de Oficina de Practicas
        public static readonly string CreateAlumnoSP = "CreateAlumno";
        public static readonly string ReadAlumnoSP = "ReadAlumno";

        //Jefe de Oficina Servicio Social
        public static readonly string CreateAlumnoJS = "CreateAlumno";
        public static readonly string ReadAlumnoJS = "ReadAlumno";
        public static readonly string UpdateAlumnoJS = "UpdateAlumno";
        public static readonly string DeleteAlumnoJS = "DeleteAlumno";

        //Secretario de Oficina de Servicio Social
        public static readonly string CreateAlumnoSS = "CreateAlumno";
        public static readonly string ReadAlumnoSS = "ReadAlumno";

        //Root
        public static readonly string CreateCarreraRoot = "CreateCarrera";
        public static readonly string DeleteCarreraRoot = "CreateServicio";
        public static readonly string CreateAlumnoRoot = "CreateAlumno";
        public static readonly string ReadAlumnoRoot = "ReadAlumno";
        public static readonly string UpdateAlumnoRoot = "UpdateAlumno";
        public static readonly string DeleteAlumnoRoot = "DeleteAlumno";
        public static readonly string ApproveOperationRoot = "AprobarOperacion";
        public static readonly string RejectOperationRoot = "RechazarOperacion";


        //Roles
        public static readonly string RootRole = "Root";
        public static readonly string jefeVinc = "JefePracticas";
        public static readonly string jefeEsc = "JefeServicio";
        public static readonly string ofServ = "SecretarioServicio";
        public static readonly string ofPrac = "SecretarioPracticas";
    }
}
