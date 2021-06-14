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
    public partial class Dar_Alta_Coordinadores : Form
    {
        
        private string editar;
        public Dar_Alta_Coordinadores()
        {
         
            editar = "";
            InitializeComponent();
        }

        private void updateWindow()
        {
            tB_Usuario.Text = "";
            tB_Contraseña.Text = "";
            tB_ConVericicacion.Text = "";

            tB_Nombre.Text = "";
            tB_ApPaterno.Text = "";
            tB_ApMaterno.Text = "";
            rB_Hombre.Checked = false;
            rB_Mujer.Checked = false;
            dTP_Nacimiento.Value = DateTime.Today;
            tB_Nomina.Text = "";
            tB_Titulo.Text = "";

            editar = "";
            tB_ConVericicacion.Enabled = false;

            cB_Carrera.DataSource = Global.DBManager.careerMultySelect(1);
            cB_Carrera.DisplayMember = "Nombre";
            cB_Carrera.Enabled = cB_Carrera.Items.Count != 0;
            cB_Carrera.Text = "";

            dGV_Consulta.DataSource = Global.DBManager.coordinatorMultySelect(0);
            gB_Consulta.Enabled = dGV_Consulta.RowCount != 0;
            btn_Registrar.Text = "Registrar";

            gB_Usuario.Enabled = true;
        }

        private void tB_Contraseña_TextChanged(object sender, EventArgs e)
        {
            if (tB_Contraseña.TextLength != 0)
            {
                tB_ConVericicacion.Enabled = true;
            }
            else
            {
                tB_ConVericicacion.Enabled = false;
            }
        }

        private void Dar_Alta_Coordinadores_Load(object sender, EventArgs e)
        {
            dTP_Nacimiento.MaxDate = DateTime.Today;
            tB_ConVericicacion.Enabled = false;
            updateWindow();
        }

        private void btn_Registrar_Click(object sender, EventArgs e)
        {
            if(tB_Contraseña.ToString().CompareTo(tB_ConVericicacion.ToString()) != 0)
            {
                MessageBox.Show("Error: Las contraseñas no coinciden");
            }
            else
            {
                if (cB_Carrera.Text.ToString().Length != 0)
                {
                    if (rB_Hombre.Checked || rB_Mujer.Checked)
                    {
                        if (Global.DBManager.insertCoordinator(tB_Nombre.Text.Trim(), tB_ApPaterno.Text.Trim(), tB_ApMaterno.Text.Trim(), dTP_Nacimiento.Value, cB_Carrera.Text.Trim(), tB_Nomina.Text.Trim(), tB_Usuario.Text.Trim(), tB_Contraseña.Text.Trim(), tB_Titulo.Text.Trim(), rB_Hombre.Checked,editar.Length > 0) == 0)
                        {
                            MessageBox.Show("Error: Nombre de usuaro ya existe");
                        }
                        else
                        {
                            updateWindow();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Seleccione un genero");
                    }
                }
                else
                {
                    MessageBox.Show("Error: Seleccione una carrera");
                }
            }
        }

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            btn_Registrar.Text = "Guardar";

            int row = dGV_Consulta.CurrentCell.RowIndex;
            editar = dGV_Consulta.Rows[row].Cells[0].Value.ToString();
            gB_Usuario.Enabled = false;
            DataTable result = Global.DBManager.coordinatorMultySelect(1, editar);

            
            tB_Usuario.Text = result.Rows[0][0].ToString();
            tB_Contraseña.Text = result.Rows[0][1].ToString();
            tB_ConVericicacion.Text = result.Rows[0][1].ToString();

            tB_Nombre.Text = result.Rows[0][2].ToString();
            tB_ApPaterno.Text = result.Rows[0][3].ToString();
            tB_ApMaterno.Text = result.Rows[0][4].ToString();
            rB_Hombre.Checked = bool.Parse(result.Rows[0][5].ToString());
            rB_Mujer.Checked = !rB_Hombre.Checked;
            dTP_Nacimiento.Value = DateTime.Parse(result.Rows[0][6].ToString());
            cB_Carrera.Text = result.Rows[0][7].ToString();
            cB_Carrera.Enabled = false;
            tB_Nomina.Text = result.Rows[0][8].ToString();
            tB_Titulo.Text = result.Rows[0][9].ToString();
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;

            if (MessageBox.Show("Está a punto de eliminar PERMANENTEMENTE al Coordinador: \"" + dGV_Consulta.Rows[row].Cells[1].Value.ToString() + "\", ¿Está seguro que desea continuar?",
                                     "Confirmar eliminacion de Coordinador",
                                     MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Global.DBManager.coordinatorMultySelect(2, dGV_Consulta.Rows[row].Cells[0].Value.ToString());
            }
            updateWindow();
        }

        private void tB_Busqueda_TextChanged(object sender, EventArgs e)
        {
            dGV_Consulta.DataSource = Global.DBManager.coordinatorMultySelect(0);
            btn_Editar.Enabled = dGV_Consulta.RowCount != 0;
            btn_Eliminar.Enabled = dGV_Consulta.RowCount != 0;
            Global.searchInDataGridView(dGV_Consulta, tB_Busqueda.Text);
        }
    }
}
