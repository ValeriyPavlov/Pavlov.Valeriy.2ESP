using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class MateriaDao
    {
        private static SqlConnection _sqlConnection;
        private static SqlCommand _sqlCommand;

        static MateriaDao()
        {
            _sqlConnection = new SqlConnection(@"Server=.;Database=parcial_dos;Trusted_Connection=True;");
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandType = System.Data.CommandType.Text;
        }

        public static List<Materia> TraerMaterias()
        {
            List<Materia> list = new List<Materia>();
            try
            {
                _sqlCommand.CommandText = "SELECT * FROM dbo.materias";
                _sqlConnection.Open();
                SqlDataReader reader = _sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Materia materia = (Materia)reader;
                    list.Add(materia);
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
    }
}
