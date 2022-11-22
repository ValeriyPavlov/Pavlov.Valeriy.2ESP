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
using BibliotecaClases;

namespace Vista
{
    public partial class FrmAltaMaterias : Form, IValidador 
    {
        public FrmAltaMaterias()
        {
            InitializeComponent();
            cb_altaMateria_profesor.DataSource = UsuarioDao.TraerUsuarios("SELECT * FROM dbo.usuarios WHERE rol = 'profesor'");
            cb_altaMateria_profesor.DisplayMember = "NombreUsuario";
            cb_altaMaterias_materia.DataSource = MateriaDao.TraerMaterias();
            cb_altaMaterias_materia.DisplayMember = "NombreMateria";
        }

        private void btn_materias_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_materias_aceptar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                CursadaDao.AgregarCursada(ConstruirCursada());
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Construye una materia con los datos proporcionados por el usuario.
        /// </summary>
        private Cursada ConstruirCursada()
        {
            Cursada cursada = new Cursada(0, ((Materia)(cb_altaMaterias_materia.SelectedItem)).NombreMateria, ((Usuario)(cb_altaMateria_profesor.SelectedItem)).NombreUsuario, "1900-01-01");
            return cursada;
        }

        /// <summary>
        /// Validacion de datos de los campos del formulario.
        /// </summary>
        /// <returns></returns> false si estan incompletos | true si estan completos.
        public bool ValidarDatos()
        {
            bool retorno = true;
            if (cb_altaMaterias_materia.SelectedIndex == -1 || cb_altaMateria_profesor.SelectedIndex == -1)
            {
                MessageBox.Show("Debe completar todos los campos!");
                retorno = false;
            }
            return retorno;
        }

        private void FrmAltaMaterias_Load(object sender, EventArgs e)
        {
            
        }
    }
}
