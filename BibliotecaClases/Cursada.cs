using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class Cursada
    {
        private int idCursada;
        private string materia;
        private string profesor;
        private string fechaExamen;

        public Cursada(int idCursada, string materia, string profesor, string fechaExamen)
        {
            this.idCursada = idCursada;
            this.materia = materia;
            this.profesor = profesor;
            this.fechaExamen = fechaExamen;
        }

        public int IdCursada { get => idCursada; set => idCursada = value; }
        public string Materia { get => materia; set => materia = value; }
        public string Profesor { get => profesor; set => profesor = value; }
        public string FechaExamen { get => fechaExamen; set => fechaExamen = value; }

        public static explicit operator Cursada(SqlDataReader v)
        {
            Cursada cursada = new Cursada(Convert.ToInt32(v["id"]), v["materia"].ToString(), v["profesor"].ToString(), v["fechaExamen"].ToString());
            return cursada;
        }
    }
}
