using BibliotecaClases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Vista
{
    public partial class FrmMenuAlumno : Form, IActualizarEstadosCursada
    {
        Usuario usuarioAlumno;

        public Usuario UsuarioAlumno { get => usuarioAlumno; set => usuarioAlumno = value; }

        public FrmMenuAlumno()
        {
            InitializeComponent();
        }

        private void btn_menuAlumno_salir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FrmMenuAlumno_Load(object sender, EventArgs e)
        {
            dgv_menuAlumno_materias_disponibles.DataSource = CursadaDao.TraerCursadas("SELECT * FROM cursadas");
            dgv_menuAlumno_inscripcion.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND regularidad = 'Regular'");
            dgv_materiasAprobadas.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND regularidad = 'Aprobada' AND asistencia = 'Presente'");
            cb_menuAlumno_asistencia.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND asistencia = 'Ausente' AND regularidad = 'Regular'");
            cb_menuAlumno_asistencia.DisplayMember = "IdCursada";
            cb_alumno_inscripcion.DataSource = CursadaDao.TraerCursadas($"SELECT * FROM cursadas");
            cb_alumno_inscripcion.DisplayMember = "IdCursada";
        }

        private void btn_menuAlumno_inscribirse_Click(object sender, EventArgs e)
        {
            ChequearInscripcion();
            dgv_menuAlumno_inscripcion.DataSource = null;
            dgv_menuAlumno_inscripcion.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND regularidad = 'Regular'");
            cb_menuAlumno_asistencia.DataSource = null;
            cb_menuAlumno_asistencia.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND asistencia = 'Ausente' AND regularidad = 'Regular'");
            cb_menuAlumno_asistencia.DisplayMember = "IdCursada";
        }

        /// <summary>
        /// Chequea si es posible la inscripcion del alumno y si es asi, lo inscribe.
        /// </summary>
        public void ChequearInscripcion()
        {
            int banderaInscripto = 0;
            List<EstadoCursada> lista = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND regularidad = 'Regular'");
            if (lista.Count < 2)
            {
                Cursada cursada = (Cursada)cb_alumno_inscripcion.SelectedItem;
                foreach (EstadoCursada estadoCursada in EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}'"))
                {
                    if (estadoCursada.IdCursada == cursada.IdCursada)
                    {
                        banderaInscripto = 1;
                        break;
                    }
                }
                if (banderaInscripto == 0)
                {
                    string nombreCorrelativa = TraerCorrelativa(cursada, MateriaDao.TraerMaterias());
                    if (nombreCorrelativa == "Sin_Correlatividad")
                    {
                        InscribirAlumno(cursada.IdCursada);
                    }
                    else if (nombreCorrelativa != null)
                    {
                        if ((EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas INNER JOIN cursadas ON cursadas.id = estado_cursadas.idCursada WHERE estado_cursadas.alumno = '{usuarioAlumno.NombreUsuario}' AND cursadas.materia = '{nombreCorrelativa}' AND estado_cursadas.regularidad = 'Aprobada'")).Count != 0)
                        {
                            InscribirAlumno(cursada.IdCursada);
                        }
                        else
                        {
                            MessageBox.Show("El Alumno no cumple con los requisitos de correlatividad.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("El alumno ya se encuentra cursando el limite de materias (2).");
            }
        }

        private void btn_menuAlumno_asistencia_Click(object sender, EventArgs e)
        {
            if (cb_menuAlumno_asistencia.SelectedIndex != -1)
            {
                if (((EstadoCursada)cb_menuAlumno_asistencia.SelectedItem).Asistencia == "Ausente")
                {
                    EstadoCursadaDao.ModificarEstadoCursada($"UPDATE estado_cursadas SET asistencia = 'Presente' WHERE alumno = '{((EstadoCursada)cb_menuAlumno_asistencia.SelectedItem).UsuarioAlumno}' AND idCursada = '{((EstadoCursada)cb_menuAlumno_asistencia.SelectedItem).IdCursada}'");
                    IActualizarEstadosCursada.ActualizarEstadosCursada();
                    dgv_menuAlumno_inscripcion.DataSource = null;
                    dgv_menuAlumno_inscripcion.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND regularidad = 'Regular'");
                    dgv_materiasAprobadas.DataSource = null;
                    dgv_materiasAprobadas.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND regularidad = 'Aprobada' AND asistencia = 'Presente'");
                }
                else
                {
                    MessageBox.Show("Ya se encuentra dada la asistencia en esa materia!");
                }
            }
            else
            {
                MessageBox.Show("Seleccione una materia!");
            }
        }

        private static string TraerCorrelativa(Cursada cursada, List<Materia> lista)
        {
            string correlativa = "";
            foreach (Materia materia in lista)
            {
                if (cursada.Materia == materia.NombreMateria)
                {
                    correlativa = materia.MateriaCorrelativa;
                    break;
                }
            }
            return correlativa;
        }

        private void InscribirAlumno(int idCursada)
        {
            EstadoCursada estadoCursada = new EstadoCursada(idCursada, usuarioAlumno.NombreUsuario, 0, "Ausente", "Regular");
            EstadoCursadaDao.AgregarEstadoCursada(estadoCursada);
            cb_menuAlumno_asistencia.DataSource = null;
            cb_menuAlumno_asistencia.DataSource = EstadoCursadaDao.TraerEstadoCursadas($"SELECT * FROM estado_cursadas WHERE alumno = '{usuarioAlumno.NombreUsuario}' AND asistencia = 'Ausente' AND regularidad = 'Regular'");
            cb_menuAlumno_asistencia.DisplayMember = "IdCursada";
        }
    }
}
