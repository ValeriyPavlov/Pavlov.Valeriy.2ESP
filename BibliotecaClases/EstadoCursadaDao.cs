using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class EstadoCursadaDao
    {
        private static SqlConnection _sqlConnection;
        private static SqlCommand _sqlCommand;

        static EstadoCursadaDao()
        {
            _sqlConnection = new SqlConnection(@"Server=.;Database=parcial_dos;Trusted_Connection=True;");
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandType = System.Data.CommandType.Text;
        }

        public static List<EstadoCursada> TraerEstadoCursadas(string comando)
        {
            List<EstadoCursada> list = new List<EstadoCursada>();
            try
            {
                _sqlCommand.CommandText = comando;
                _sqlConnection.Open();
                SqlDataReader reader = _sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EstadoCursada estadoCursada = (EstadoCursada)reader;
                    list.Add(estadoCursada);
                }
            }
            catch (Exception)
            {
                throw new Exception("Error de conexión a la base de datos");
            }
            finally
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }

            return list;
        }

        public static int AgregarEstadoCursada(EstadoCursada estadoCursada)
        {
            int filas = 0;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open();
                _sqlCommand.CommandText = "INSERT into estado_cursadas (idCursada, alumno, nota, asistencia, regularidad) VALUES (@idCursada, @alumno, @nota, @asistencia, @regularidad)";
                _sqlCommand.Parameters.AddWithValue("@idCursada", estadoCursada.IdCursada);
                _sqlCommand.Parameters.AddWithValue("@alumno", estadoCursada.UsuarioAlumno);
                _sqlCommand.Parameters.AddWithValue("@nota", estadoCursada.Nota);
                _sqlCommand.Parameters.AddWithValue("@asistencia", estadoCursada.Asistencia);
                _sqlCommand.Parameters.AddWithValue("@regularidad", estadoCursada.Regularidad);
                filas = _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("Error de conexión a la base de datos");
            }
            finally
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
            return filas;
        }

        public static void ModificarRegularidad(EstadoCursada estadoCursada)
        {
            string regularidad = estadoCursada.Regularidad;
            if (estadoCursada.Nota > 5 && estadoCursada.Regularidad == "Regular" && estadoCursada.Asistencia == "Presente")
            {
                regularidad = "Aprobada";
            }
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open();
                _sqlCommand.CommandText = $"UPDATE estado_cursadas SET regularidad = '{regularidad}' WHERE alumno = '{estadoCursada.UsuarioAlumno}' AND idCursada = '{estadoCursada.IdCursada}'";
                _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("Error de conexión a la base de datos");
            }
            finally
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
        }

        public static void CalificarAlumno(EstadoCursada estadoCursada, int nota)
        {
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open();
                _sqlCommand.CommandText = $"UPDATE estado_cursadas SET nota = {nota} WHERE alumno = '{estadoCursada.UsuarioAlumno}' AND idCursada = '{estadoCursada.IdCursada}'";
                _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("Error de conexión a la base de datos");
            }
            finally
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
        }

        public static void ModificarEstadoCursada(string comando)
        {
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open(); 
                _sqlCommand.CommandText = comando;
                _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("Error de conexión a la base de datos");
            }
            finally
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
        }

        public static void DejarLibre(EstadoCursada estadoCursada)
        {
            if (estadoCursada.Regularidad != "Aprobada")
            {
                string regularidad = "Regular";
                if (estadoCursada.Regularidad == "Regular")
                {
                    regularidad = "Libre";
                }
                else
                {
                    if (estadoCursada.Regularidad == "Libre")
                    {
                        regularidad = "Regular";
                    }
                }

                try
                {
                    _sqlCommand.Parameters.Clear();
                    _sqlConnection.Open();
                    _sqlCommand.CommandText = $"UPDATE estado_cursadas SET regularidad = '{regularidad}' WHERE alumno = '{estadoCursada.UsuarioAlumno}' AND idCursada = '{estadoCursada.IdCursada}'";
                    _sqlCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Error de conexión a la base de datos");
                }
                finally
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        _sqlConnection.Close();
                    }
                }
            }
        }
    }
}
