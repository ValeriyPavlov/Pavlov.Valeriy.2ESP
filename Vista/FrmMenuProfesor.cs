using BibliotecaClases;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class FrmMenuProfesor : Form, IActualizarEstadosCursada
    {
        Usuario usuarioProfesor;
        public Usuario UsuarioProfesor { get => usuarioProfesor; set => usuarioProfesor = value; }

        public FrmMenuProfesor()
        {
            InitializeComponent();
        }

        private void btn_menuProf_salir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FrmMenuProfesor_Load(object sender, EventArgs e)
        {
            cb_menuProfe_materia.DataSource = CursadaDao.TraerCursadas($"SELECT * FROM dbo.cursadas WHERE profesor = '{usuarioProfesor.NombreUsuario}'");
            cb_menuProfe_materia.DisplayMember = "IdCursada";
            cb_seleccionar_nota.SelectedIndex = -1;
            lbl_fechaActual_examen.Text = "";
        }

        private void cb_menuProfe_materia_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_fechaActual_examen.Text = "";
            lbl_fechaActual_examen.Text = ((Cursada)cb_menuProfe_materia.SelectedItem).FechaExamen;
            cb_seleccionar_alumno.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas INNER JOIN usuarios ON estado_cursadas.alumno = usuarios.usuario WHERE idCursada = {((Cursada)cb_menuProfe_materia.SelectedItem).IdCursada} AND nota = 0");
            cb_seleccionar_alumno.DisplayMember = "usuarioAlumno";
        }

        private void btn_crear_examen_Click(object sender, EventArgs e)
        {
            Cursada cursadaAux = (Cursada)cb_menuProfe_materia.SelectedItem;
            CursadaDao.ModificarFechaExamen(cursadaAux, ConstuirFechaExamen());
            cb_menuProfe_materia.DataSource = CursadaDao.TraerCursadas($"SELECT * FROM dbo.cursadas WHERE profesor = '{usuarioProfesor.NombreUsuario}'");
            cb_menuProfe_materia.DisplayMember = "IdCursada";
            lbl_fechaActual_examen.Text = "";
            lbl_fechaActual_examen.Text = ((Cursada)cb_menuProfe_materia.SelectedItem).FechaExamen;
        }

        /// <summary>
        /// Construye un string de fecha de examen.
        /// </summary>
        /// <returns></returns> Retorna el string construido.
        public string ConstuirFechaExamen()
        {
            string retorno = "  ";
            if (txb_dia_examen.Text.Length > 0 && txb_mes_examen.Text.Length > 0)
            {
                if (int.TryParse(txb_dia_examen.Text, out int dia) && int.TryParse(txb_mes_examen.Text, out int mes))
                {
                    if (dia > 0 && dia < 31 && mes > 0 && mes < 13)
                    {
                        StringBuilder sb = new();
                        sb.Append("2022-");
                        sb.Append(mes);
                        sb.Append('-');
                        sb.Append(dia);
                        retorno = sb.ToString();
                    }
                }
            }
            return retorno;
        }

        string? notaStr;
        private void btn_evaluarAlumno_Click(object sender, EventArgs e)
        {
            if (cb_seleccionar_alumno.SelectedIndex != -1 && cb_seleccionar_nota.SelectedIndex != -1)
            {
                if (cb_seleccionar_nota.SelectedItem is not null)
                {
                    notaStr = (cb_seleccionar_nota.SelectedItem).ToString();
                    if (int.TryParse(notaStr, out int nota))
                    {
                        EstadoCursadaDao.ModificarEstadoCursada($"UPDATE estado_cursadas SET nota = {nota} WHERE alumno = '{((EstadoCursada)cb_seleccionar_alumno.SelectedItem).UsuarioAlumno}' AND idCursada = '{((EstadoCursada)cb_seleccionar_alumno.SelectedItem).IdCursada}'");
                        IActualizarEstadosCursada.ActualizarEstadosCursada();
                        MostrarNotaAlumno();
                        lbl_notaActual.Text = notaStr;
                        cb_seleccionar_nota.SelectedIndex = -1;
                    }
                }
            }
        }

        private void cb_seleccionar_alumno_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarNotaAlumno();
        }

        /// <summary>
        /// Muestra la nota actualizada del alumno seleccionado.
        /// </summary>
        private void MostrarNotaAlumno()
        {
            try
            {
                lbl_notaActual.Text = (((EstadoCursada)cb_seleccionar_alumno.SelectedItem).Nota).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
