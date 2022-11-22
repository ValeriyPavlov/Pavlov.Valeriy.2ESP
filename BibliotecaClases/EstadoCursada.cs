using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class EstadoCursada
    {
        private int idCursada;
        private string usuarioAlumno;
        private int nota;
        private string asistencia;
        private string regularidad;

        public EstadoCursada(int idEstadoCursada, string usuarioAlumno, int nota, string asistencia, string regularidad) : this(idEstadoCursada, usuarioAlumno, asistencia, regularidad)
        {
            this.nota = nota;
        }

        public EstadoCursada(int idEstadoCursada, string usuarioAlumno, string asistencia, string regularidad)
        {
            this.idCursada = idEstadoCursada;
            this.usuarioAlumno = usuarioAlumno;
            this.asistencia = asistencia;
            this.regularidad = regularidad;
        }

        public int IdCursada { get => idCursada; set => idCursada = value; }
        public string UsuarioAlumno { get => usuarioAlumno; set => usuarioAlumno = value; }
        public int Nota { get => nota; set => nota = value; }
        public string Asistencia { get => asistencia; set => asistencia = value; }
        public string Regularidad { get => regularidad; set => regularidad = value; }

        public static explicit operator EstadoCursada(SqlDataReader v)
        {
            EstadoCursada estadoCursada = new EstadoCursada(Convert.ToInt32(v["idCursada"]), v["alumno"].ToString(), Convert.ToInt32(v["nota"]), v["asistencia"].ToString(), v["regularidad"].ToString());
            return estadoCursada;
        }
    }
}
