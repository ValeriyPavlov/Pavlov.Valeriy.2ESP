using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaClases;
using System.Text.Json;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.IO;

namespace Vista
{
    public partial class FrmMenuAdmin : Form
    {
        FrmAltaUsuarios formAltaUsuarios;
        FrmAltaMaterias formAltaMaterias;

        public FrmMenuAdmin()
        {
            InitializeComponent();
        }

        public FrmAltaUsuarios FormAltaUsuarios { get => formAltaUsuarios; set => formAltaUsuarios = value; }

        private void btn_alta_usuarios_Click(object sender, EventArgs e)
        {
            formAltaUsuarios = new FrmAltaUsuarios();

            if (formAltaUsuarios.ShowDialog() == DialogResult.OK)
            {
                dgv_admin_usuarios.DataSource = null;
                dgv_admin_usuarios.DataSource = UsuarioDao.TraerUsuarios("SELECT * FROM dbo.usuarios");
                dgv_admin_usuarios.Columns["Nombre"].Visible = false;
                dgv_admin_usuarios.Columns["Apellido"].Visible = false;
            }
        }

        private void btn_menuAdmin_salir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FrmMenuAdmin_Load(object sender, EventArgs e)
        {
            dgv_admin_usuarios.DataSource = UsuarioDao.TraerUsuarios("SELECT * FROM dbo.usuarios");
            dgv_admin_usuarios.Columns["Nombre"].Visible = false;
            dgv_admin_usuarios.Columns["Apellido"].Visible = false;
            dgv_admin_materias.DataSource = CursadaDao.TraerCursadas("SELECT * FROM dbo.cursadas");
            cb_menuAdmin_materias.DataSource = CursadaDao.TraerCursadas("SELECT * FROM dbo.cursadas");
            cb_menuAdmin_materias.DisplayMember = "IdCursada";
            dgv_estadoMateriasAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas("SELECT * FROM dbo.estado_cursadas");
            cb_menuAdmin_seleccionAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM dbo.estado_cursadas INNER JOIN dbo.usuarios ON dbo.estado_cursadas.alumno = dbo.usuarios.usuario WHERE idCursada = {((Cursada)cb_menuAdmin_materias.SelectedItem).IdCursada}");
            cb_menuAdmin_seleccionAlumno.DisplayMember = "UsuarioAlumno";
        }

        private void btn_crear_materias_Click(object sender, EventArgs e)
        {
            formAltaMaterias = new FrmAltaMaterias();
            if (formAltaMaterias.ShowDialog() == DialogResult.OK)
            {
                dgv_admin_materias.DataSource = null;
                dgv_admin_materias.DataSource = CursadaDao.TraerCursadas("SELECT * FROM dbo.cursadas");
                cb_menuAdmin_materias.DataSource = CursadaDao.TraerCursadas("SELECT * FROM dbo.cursadas");
                cb_menuAdmin_materias.DisplayMember = "IdCursada";
            }
        }

        private void btn_menuAdmin_estadoAlumno_Click(object sender, EventArgs e)
        {
            if (cb_menuAdmin_seleccionAlumno.SelectedIndex == -1 || cb_menuAdmin_materias.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un alumno!");
            }
            else
            {
                EstadoCursadaDao.DejarLibre((EstadoCursada)cb_menuAdmin_seleccionAlumno.SelectedItem);
                dgv_estadoMateriasAlumno.DataSource = null;
                dgv_estadoMateriasAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas("SELECT * FROM dbo.estado_cursadas");
                cb_menuAdmin_seleccionAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM dbo.estado_cursadas INNER JOIN dbo.usuarios ON dbo.estado_cursadas.alumno = dbo.usuarios.usuario WHERE idCursada = {((Cursada)cb_menuAdmin_materias.SelectedItem).IdCursada}");
                cb_menuAdmin_seleccionAlumno.DisplayMember = "UsuarioAlumno";
                cb_menuAdmin_seleccionAlumno.SelectedIndex = -1;
            }
        }

        private void cb_menuAdmin_seleccionAlumno_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_estadoMateriasAlumno.DataSource = null;
            dgv_estadoMateriasAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas("SELECT * FROM dbo.estado_cursadas");
        }

        private void cb_menuAdmin_materias_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_estadoMateriasAlumno.DataSource = null;
            dgv_estadoMateriasAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas("SELECT * FROM dbo.estado_cursadas");
            cb_menuAdmin_seleccionAlumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM dbo.estado_cursadas INNER JOIN dbo.usuarios ON dbo.estado_cursadas.alumno = dbo.usuarios.usuario WHERE idCursada = {((Cursada)cb_menuAdmin_materias.SelectedItem).IdCursada}");
            cb_menuAdmin_seleccionAlumno.DisplayMember = "UsuarioAlumno";
        }

        private void btn_exportarAlumnos_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine("IdCursada,UsuarioAlumno,Nota,Asistencia,Regularidad");
            List<EstadoCursada> lista = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE idCursada = {((Cursada)cb_menuAdmin_materias.SelectedItem).IdCursada}");
            foreach (EstadoCursada estado in lista)
            {
                sb.AppendLine(SerializarJson(estado));
                sb2.AppendLine(SerializacionCSV(estado));
            }
            if (rbtn_json.Checked == true)
            {
                File.WriteAllText("alumnos.json", sb.ToString()); //Pavlov.Valeriy.2ESP\Vista\bin\Debug\net5.0-windows\alumnos.json
                MessageBox.Show("Archivo JSON guardado con exito!");
            }
            else
            {
                if (rbtn_csv.Checked == true)
                {
                    File.WriteAllText("alumnos.csv", sb2.ToString()); //Pavlov.Valeriy.2ESP\Vista\bin\Debug\net5.0-windows\alumnos.csv
                    MessageBox.Show("Archivo CSV guardado con exito!");
                }
                else
                {
                    MessageBox.Show("Seleccione la opcion de guardado.");
                }
            }
        }

        /// <summary>
        /// Serializacion de datos de la cursada en formato JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public string SerializarJson<T>(T objeto) where T : EstadoCursada // Restriccion Generics
        {
            string jsonString = JsonSerializer.Serialize(objeto);
            return jsonString;
        }

        /// <summary>
        /// Serializacion de datos de la cursada en formato CSV.
        /// </summary>
        /// <param name="estadoCursada"></param>
        /// <returns></returns>
        public string SerializacionCSV(EstadoCursada estadoCursada)
        {
            string unEstado = estadoCursada.IdCursada.ToString() + ',' + estadoCursada.UsuarioAlumno + ',' + estadoCursada.Nota.ToString() + ',' + estadoCursada.Asistencia + ',' + estadoCursada.Regularidad;
            return unEstado;
        }
    }
}
