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
    public partial class Iniciar_Semestre : Form
    {
        
        private int editar;
        public Iniciar_Semestre()
        {
         
            InitializeComponent();
        }
        private void updateWindow()
        {
            editar = -1;
            btn_Registrar.Text = "Registrar";
            dGV_Consulta.DataSource = Global.DBManager.semesterMultySelect(0);
            gB_Consulta.Enabled = dGV_Consulta.RowCount != 0;

            nUD_Minimo.Value = 1;
            nUD_Maximo.Value = 2;
            dTP_Inicio.MinDate = DateTime.Today;
            dTP_Inicio.Value = DateTime.Today;
            dTP_Final.Enabled = false;
        }

        private void Iniciar_Semestre_Load(object sender, EventArgs e)
        {
            updateWindow();
        }

        private void dTP_Inicio_ValueChanged(object sender, EventArgs e)
        {
            dTP_Final.MinDate = dTP_Inicio.Value;
            dTP_Final.Enabled = true;
        }

        private void tB_Busqueda_TextChanged(object sender, EventArgs e)
        {
            dGV_Consulta.DataSource = Global.DBManager.semesterMultySelect(0);
            gB_Consulta.Enabled = dGV_Consulta.RowCount != 0;
            Global.searchInDataGridView(dGV_Consulta, tB_Busqueda.Text);
        }

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;

            if (dGV_Consulta.Rows[row].Cells[2].Value.ToString().CompareTo("Finalizado") != 0) { 
                btn_Registrar.Text = "Guardar";
                editar = (int)dGV_Consulta.Rows[row].Cells[0].Value;

                DataTable result = Global.DBManager.semesterMultySelect(4, editar);

                nUD_Minimo.Value = result.Rows[0].Field<short>("creditos_Min");
                nUD_Maximo.Value = result.Rows[0].Field<short>("creditos_Max");
                dTP_Inicio.Value = result.Rows[0].Field<DateTime>("fecha_Inicio");
                dTP_Final.Value = result.Rows[0].Field<DateTime>("fecha_fin");
            }
            else
            {
                MessageBox.Show("No se pueden modificar semestres ya finalizados");
            }
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;
            if (dGV_Consulta.Rows[row].Cells[2].Value.ToString().CompareTo("Finalizado") != 0)
            {
                if (MessageBox.Show("Está a punto de eliminar PERMANENTEMENTE el Semestre: \"" + dGV_Consulta.Rows[row].Cells[0].Value.ToString() + "\", ¿Está seguro que desea continuar?",
                                     "Confirmar eliminacion del Semestre",
                                     MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataTable delete = Global.DBManager.semesterMultySelect(5, (int)dGV_Consulta.Rows[row].Cells[0].Value);
                    if (delete.Rows.Count != 0)
                    {
                        MessageBox.Show(delete.Rows[0].Field<string>("ERROR"));
                    }
                }
            }
            else
            {
                MessageBox.Show("No se pueden eliminar semestres ya finalizados");
            }
            updateWindow();
        }

        private void btn_Registrar_Click(object sender, EventArgs e)
        {
            if (dTP_Final.Enabled)
            {
                Global.DBManager.insertSemester((short)nUD_Minimo.Value, (short)nUD_Maximo.Value, dTP_Inicio.Value, dTP_Final.Value, editar);
                updateWindow();
            }
            else
            {
                MessageBox.Show("Error: Seleccione un rango de fechas");
            }
        }
    }
}
