using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class Frm_Login : Form
    {

        FrmMenuAdmin menuPrincipalAdmin = new FrmMenuAdmin();
        FrmMenuAlumno menuPrincipalAlumno = new FrmMenuAlumno();
        FrmMenuProfesor menuPrincipalProfesor = new FrmMenuProfesor();

        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btn_login_aceptar_Click(object sender, EventArgs e)
        {
            Usuario usuario = ValidarLogin(this.txb_usuario.Text, this.txb_contraseña.Text);
            if (usuario != null)
            {
                if (usuario.Tipo == "Administrador")
                {
                    menuPrincipalAdmin.ShowDialog();
                }
                else if (usuario.Tipo == "Alumno")
                {
                    menuPrincipalAlumno.UsuarioAlumno = usuario;
                    menuPrincipalAlumno.ShowDialog();
                }
                else if (usuario.Tipo == "Profesor")
                {
                    menuPrincipalProfesor.UsuarioProfesor = usuario;
                    menuPrincipalProfesor.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Usuario/Contraseña Incorrecta");
            }
        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {

        }

        private void btn_login_salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_login_admin_Click(object sender, EventArgs e)
        {
            txb_usuario.Text = "admin";
            txb_contraseña.Text = "admin";
        }

        private void btn_login_alumno_Click(object sender, EventArgs e)
        {
            txb_usuario.Text = "alumno";
            txb_contraseña.Text = "alumno";
        }

        private void btn_login_profesor_Click(object sender, EventArgs e)
        {
            txb_usuario.Text = "profesor";
            txb_contraseña.Text = "profesor";
        }

        /// <summary>
        /// Valida que el usuario exista en la base de datos y si es asi, que la contraseña coincida con la guardad.
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>
        public static Usuario ValidarLogin(string nombreUsuario, string contraseña)
        {
            Usuario? user = null;
            List<Usuario> usuarios = UsuarioDao.TraerUsuarios("SELECT * FROM dbo.usuarios");
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.NombreUsuario == nombreUsuario)
                {
                    if (ValidarContraseña(usuario, contraseña))
                    {
                        user = usuario;
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Valida que las contraseñas coincidan.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>
        private static bool ValidarContraseña(Usuario usuario, string contraseña)
        {
            bool retorno = false;
            if (usuario.ValidarContraseña(contraseña))
            {
                retorno = true;
            }
            return retorno;
        }
    }
}
