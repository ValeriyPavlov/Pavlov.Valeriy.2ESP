using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaClases
{
    public class UsuarioDao
    {
        private static SqlConnection _sqlConnection;
        private static SqlCommand _sqlCommand;

        static UsuarioDao()
        {
            _sqlConnection = new SqlConnection(@"Server=.;Database=parcial_dos;Trusted_Connection=True;");
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandType = System.Data.CommandType.Text;
        }

        public static List<Usuario> TraerUsuarios(string comando)
        {
            List<Usuario> list = new List<Usuario>();
            try
            {
                _sqlCommand.CommandText = comando;
                _sqlConnection.Open();
                SqlDataReader reader = _sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Usuario usuario = (Usuario)reader;
                    list.Add(usuario);
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

        public static int AgregarUsuario(Usuario user)
        {
            int filas = 0;
            string rol = "";
            if (user.Tipo == "Alumno")
            {
                rol = "alumno";
            }
            else
            {
                if (user.Tipo == "Profesor")
                {
                    rol = "profesor";
                }
                else
                {
                    rol = "admin";
                }
            }

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlConnection.Open();
                _sqlCommand.CommandText = "INSERT into usuarios (usuario, contraseña, nombre, apellido, rol) VALUES (@usuario, @contraseña, @nombre, @apellido, @rol)";
                _sqlCommand.Parameters.AddWithValue("@usuario", user.NombreUsuario);
                _sqlCommand.Parameters.AddWithValue("@contraseña", user.Contraseña);
                _sqlCommand.Parameters.AddWithValue("@nombre", user.Nombre);
                _sqlCommand.Parameters.AddWithValue("@apellido", user.Apellido);
                _sqlCommand.Parameters.AddWithValue("@rol", rol);
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
    }
}
