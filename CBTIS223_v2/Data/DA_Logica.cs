using CBTIS223_v2.Models;

namespace CBTIS223_v2.Data
{
    public class DA_Logica
    {
        public List<Models.Usuario> ListaUsuario()
        {
            using (var context = new cbtis223Context())
            {
                return context.Usuario.ToList();
            }
        }

        public Models.Usuario ListaUsuario(string _correo, string _contraseña)
        {
            using (var context = new cbtis223Context())
            {
                return context.Usuario.FirstOrDefault(item => item.Correo == _correo && item.Contraseña == _contraseña);
            }
        }
        public Models.Usuario ValidarUsuario(string _correo, string _contraseña)
        {
            return ListaUsuario().Where(item => item.Correo == _correo && item.Contraseña == _contraseña).FirstOrDefault();
        }

        /*
        public List<Usuario> ListaUsuario()
        {
            return new List<Usuario>
            {
                new Usuario {Id = 1,  Nombre = "El pepe", Correo = "JefeDep@gmail.com",   Contraseña = "123", Rol = "Root" },
                new Usuario {Id = 2,  Nombre = "Maria",   Correo = "JefeSer@gmail.com",   Contraseña = "123", Rol = "JefeServicio"},
                new Usuario {Id = 3,  Nombre = "Mario",   Correo = "JefePra@gmail.com",   Contraseña = "123", Rol = "JefePracticas"},
                new Usuario {Id = 4,  Nombre = "Juan",    Correo = "SecrePra@gmail.com",  Contraseña = "123", Rol = "SecretarioPracticas"},
                new Usuario {Id = 5,  Nombre = "Oscar",   Correo = "SecreSer@gmail.com",  Contraseña = "123", Rol = "SecretarioServicio"}
            };
        }

        //Metodo que valida al usuario
        public Usuario ValidarUsuario(string _correo, string _contraseña)
        {
            return ListaUsuario().Where(item => item.Correo == _correo && item.Contraseña == _contraseña).FirstOrDefault();
        }
        */
    }
}
