using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class Materia
    {
        private string nombreMateria;
        private string materiaCorrelativa;
        private int cuatrimestre;

        public Materia(string nombreMateria, string materiaCorrelativa, int cuatrimestre)
        {
            this.nombreMateria = nombreMateria;
            this.materiaCorrelativa = materiaCorrelativa;
            this.cuatrimestre = cuatrimestre;
        }

        public string NombreMateria { get => nombreMateria; }
        public string MateriaCorrelativa { get => materiaCorrelativa; }
        public int Cuatrimestre { get => cuatrimestre; }

        public static explicit operator Materia(SqlDataReader v)
        {
            Materia materia = new Materia(v["nombre_materia"].ToString(), v["correlativa"].ToString(), Convert.ToInt32(v["cuatrimestre"]));
            return materia;
        }
    }
}
