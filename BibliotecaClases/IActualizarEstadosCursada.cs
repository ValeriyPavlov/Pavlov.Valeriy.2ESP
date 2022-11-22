using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public interface IActualizarEstadosCursada
    {
        /// <summary>
        /// Actualiza los estados de la cursada de todos los alumnos aprobados.
        /// </summary>
        public static void ActualizarEstadosCursada()
        {
            foreach (EstadoCursada estadoCursada in EstadoCursadaDao.TraerEstadoCursadas("SELECT * FROM estado_cursadas"))
            {
                EstadoCursadaDao.ModificarRegularidad(estadoCursada);
            }
        }
    }
}
