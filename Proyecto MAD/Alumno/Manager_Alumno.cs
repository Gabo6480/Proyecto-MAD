using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_MAD
{
    public partial class Dar_Alta_Alumno : Form
    {
        
        private int editar;
        public Dar_Alta_Alumno()
        {
            editar = 0;
         
            InitializeComponent();
        }

        private void updateWindow()
        {
            editar = 0;
            btn_Registrar.Text = "Registrar";
            dGV_Consulta.DataSource = Global.DBManager.studentMultySelect(2);
            gB_Consulta.Enabled = dGV_Consulta.RowCount != 0;

            tB_Nombre.Text = "";
            tB_ApPaterno.Text = "";
            tB_ApMaterno.Text = "";
            rB_Hombre.Checked = false;
            rB_Mujer.Checked = false;
            dTP_Nacimiento.Value = DateTime.Today;
            cB_Carrera.SelectedIndex = -1;
            cB_Pais.SelectedIndex = -1;
        }

        private void Dar_Alta_Alumno_Load(object sender, EventArgs e)
        {
            dTP_Nacimiento.MaxDate = DateTime.Today;
            cB_Carrera.DisplayMember = "Nombre";
            cB_Carrera.DataSource = Global.DBManager.careerMultySelect(0);
            //Aquí se tiene que cargar la lista de paises
            cB_Pais.DisplayMember = "nombre";
            cB_Pais.DataSource = Global.DBManager.allCountryNames();

            if (cB_Carrera.Items.Count == 0)
            {
                MessageBox.Show("No hay registros de Carreras en la base de datos");
                this.Close();
            }
            if(cB_Pais.Items.Count == 0)
            {
                MessageBox.Show("No hay registros de Paises en la base de datos");
                this.Close();
            }
            updateWindow();
        }

        private void btn_Registrar_Click(object sender, EventArgs e)
        {
            if (cB_Pais.Text.ToString().Length != 0)
            {
                if (cB_Carrera.Text.ToString().Length != 0)
                {
                    if (rB_Hombre.Checked || rB_Mujer.Checked)
                    {
                        if (editar == 0)
                        {
                            if (Global.DBManager.insertStudent(tB_Nombre.Text.Trim(), tB_ApPaterno.Text.Trim(), tB_ApMaterno.Text.Trim(), dTP_Nacimiento.Value, cB_Carrera.Text.Trim(), cB_Pais.Text.Trim(), rB_Hombre.Checked) == 0)
                            {
                                MessageBox.Show("Error: UvU");
                            }
                        }
                        else
                        {
                            if (Global.DBManager.insertStudent(tB_Nombre.Text.Trim(), tB_ApPaterno.Text.Trim(), tB_ApMaterno.Text.Trim(), dTP_Nacimiento.Value, cB_Carrera.Text.Trim(), cB_Pais.Text.Trim(), rB_Hombre.Checked,editar) == 0)
                            {
                                MessageBox.Show("Error: OwO");
                            }
                        }
                        updateWindow();
                    }
                    else
                    {
                        MessageBox.Show("Error: Seleccione un Genero");
                    }
                }
                else
                {
                    MessageBox.Show("Error: Seleccione una Carrera");
                }
            }
            else
            {
                MessageBox.Show("Error: Seleccione un Pais");
            }
        }

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            btn_Registrar.Text = "Guardar";

            int row = dGV_Consulta.CurrentCell.RowIndex;
            editar = (int)dGV_Consulta.Rows[row].Cells[0].Value;

            DataTable result = Global.DBManager.studentMultySelect(3, editar);

            tB_Nombre.Text = result.Rows[0][1].ToString();
            tB_ApPaterno.Text = result.Rows[0][2].ToString();
            tB_ApMaterno.Text = result.Rows[0][3].ToString();
            rB_Hombre.Checked = bool.Parse(result.Rows[0][4].ToString());
            rB_Mujer.Checked = !rB_Hombre.Checked;
            dTP_Nacimiento.Value = DateTime.Parse(result.Rows[0][5].ToString());
            cB_Carrera.SelectedIndex = cB_Carrera.FindStringExact(result.Rows[0][6].ToString());
            cB_Pais.SelectedIndex = cB_Pais.FindStringExact(result.Rows[0][7].ToString());
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;

            if (MessageBox.Show("Está a punto de eliminar PERMANENTEMENTE al Alumno: \"" + dGV_Consulta.Rows[row].Cells[1].Value.ToString() + "\", ¿Está seguro que desea continuar?",
                                     "Confirmar eliminacion de Alumno",
                                     MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Global.DBManager.studentMultySelect(4, (int)dGV_Consulta.Rows[row].Cells[0].Value);
            }
            updateWindow();
        }

        private void tB_Busqueda_TextChanged(object sender, EventArgs e)
        {
            dGV_Consulta.DataSource = Global.DBManager.studentMultySelect(2);
            btn_Editar.Enabled = dGV_Consulta.RowCount != 0;
            btn_Eliminar.Enabled = dGV_Consulta.RowCount != 0;
            Global.searchInDataGridView(dGV_Consulta, tB_Busqueda.Text);
        }
    }
}
