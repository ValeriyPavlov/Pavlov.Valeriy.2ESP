using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class Alumno : Usuario
    {
        public Alumno(string nombreUsuario, string contraseña, string nombre, string apellido) : base(nombreUsuario, contraseña, nombre, apellido)
        {

        }

        public override string Tipo { get { return "Alumno"; } }
    }
}
