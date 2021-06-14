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
    public partial class Inscripcion_Grupos : Form
    {
        
        private int grupo;
        public Inscripcion_Grupos()
        {
         
            InitializeComponent();
        }

        private void updateWindow()
        {
            grupo = -1;

            dGV_Grupos.DataSource = Global.DBManager.groupMultySelect(1);
            gB_Grupos.Enabled = dGV_Grupos.RowCount != 0;

            dGV_Alumnos.DataSource = null;
            dGV_Alumnos.Rows.Clear();
            gB_Alumnos.Enabled = false;
            gB_Alumnos.Text = "Alumnos";
        }

        private void Inscripcion_Grupos_Load(object sender, EventArgs e)
        {
            updateWindow();
        }

        private void btn_Lista_Click(object sender, EventArgs e)
        {
            int row = dGV_Grupos.CurrentCell.RowIndex;
            gB_Alumnos.Text = "Alumnos del Grupo " + dGV_Grupos.Rows[row].Cells[0].Value.ToString() + ": " + dGV_Grupos.Rows[row].Cells[1].Value.ToString();
            grupo = (int)dGV_Grupos.Rows[row].Cells[0].Value;
            dGV_Alumnos.DataSource = Global.DBManager.groupMultySelect(4, grupo);
            gB_Alumnos.Enabled = true;
            btn_Remover.Enabled = dGV_Alumnos.RowCount != 0;
            tB_BusquedaAlu.Enabled = dGV_Alumnos.RowCount != 0;
            dGV_Alumnos.Enabled = dGV_Alumnos.RowCount != 0;
        }

        private void btn_Remover_Click(object sender, EventArgs e)
        {
            new mg_Agregar_Alumnos(grupo, true).ShowDialog();
            updateWindow();
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            new mg_Agregar_Alumnos(grupo,false).ShowDialog();
            updateWindow();
        }

        private void btn_Actualizar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dGV_Alumnos.Rows)
            {
                if (row.Cells[Calificacion.Name] != null && row.Cells[Calificacion.Name].Value != null)
                {   
                    if (Global.DBManager.updateCalification(grupo, (int)row.Cells[Matricula.Name].Value, float.Parse(row.Cells[Calificacion.Name].Value.ToString())) == 0)
                    {
                 
                    }
                }
            }
            updateWindow();
        }

        private void tB_Busqueda_TextChanged(object sender, EventArgs e)
        {
            dGV_Grupos.DataSource = Global.DBManager.groupMultySelect(1);
            gB_Grupos.Enabled = dGV_Grupos.RowCount != 0;
            Global.searchInDataGridView(dGV_Grupos, tB_Busqueda.Text);
        }

        private void tB_BusquedaAlu_TextChanged(object sender, EventArgs e)
        {
            dGV_Alumnos.DataSource = Global.DBManager.groupMultySelect(4, grupo);
            Global.searchInDataGridView(dGV_Alumnos, tB_BusquedaAlu.Text);
        }
    }
}
