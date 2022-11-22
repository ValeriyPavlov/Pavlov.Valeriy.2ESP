using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class CursadaDao
    {
        private static SqlConnection _sqlConnection;
        private static SqlCommand _sqlCommand;

        static CursadaDao()
        {
            _sqlConnection = new SqlConnection(@"Server=.;Database=parcial_dos;Trusted_Connection=True;");
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandType = System.Data.CommandType.Text;
        }

        public static List<Cursada> TraerCursadas(string comando)
        {
            List<Cursada> list = new List<Cursada>();
            try
            {
                _sqlCommand.CommandText = comando;
                _sqlConnection.Open();
                SqlDataReader reader = _sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Cursada cursada = (Cursada)reader;
                    list.Add(cursada);
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

        public static int AgregarCursada(Cursada cursada)
        {
            int filas = 0;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open();
                _sqlCommand.CommandText = "INSERT into cursadas (materia, profesor, fechaExamen) VALUES (@materia, @profesor, @fechaExamen)";
                _sqlCommand.Parameters.AddWithValue("@materia", cursada.Materia);
                _sqlCommand.Parameters.AddWithValue("@profesor", cursada.Profesor);
                _sqlCommand.Parameters.AddWithValue("@fechaExamen", cursada.FechaExamen);
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

        public static void ModificarFechaExamen(Cursada cursada, string nuevaFecha)
        {
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open();
                _sqlCommand.CommandText = $"UPDATE cursadas SET fechaExamen = '{nuevaFecha}' WHERE id = '{cursada.IdCursada}'";
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
