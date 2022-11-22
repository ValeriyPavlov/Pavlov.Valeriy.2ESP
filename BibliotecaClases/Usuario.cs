using System;
using System.Data.SqlClient;

namespace BibliotecaClases
{
    public abstract class Usuario
    {
        protected string nombreUsuario;
        protected string contraseña;
        protected string nombre;
        protected string apellido;

        public Usuario(string nombreUsuario, string contraseña, string nombre, string apellido)
        {
            this.nombreUsuario = nombreUsuario;
            this.contraseña = contraseña;
            this.nombre = nombre;
            this.apellido = apellido;
        }

        public string NombreUsuario { get { return nombreUsuario; } }
        public string Contraseña { get => contraseña;}
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string NombreCompleto { get { return this.nombre + ' ' + this.apellido; } }
        public virtual string Tipo { get { return ""; } }

        public bool ValidarContraseña(string contraseña)
        {
            bool retorno = false;
            if (this.contraseña == contraseña)
            {
                retorno = true;
            }
            return retorno;
        }

        public static explicit operator Usuario(SqlDataReader v)
        {
            Usuario usuario;
            if (v["rol"].ToString() == "admin")
            {
                usuario = new Admin(v["usuario"].ToString(), v["contraseña"].ToString(), v["nombre"].ToString(), v["apellido"].ToString());
            }
            else
            {
                if (v["rol"].ToString() == "profesor")
                {
                    usuario = new Profesor(v["usuario"].ToString(), v["contraseña"].ToString(), v["nombre"].ToString(), v["apellido"].ToString());
                }
                else
                {
                    usuario = new Alumno(v["usuario"].ToString(), v["contraseña"].ToString(), v["nombre"].ToString(), v["apellido"].ToString());
                }
            }
            return usuario;
        }
    }
}
