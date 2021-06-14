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
    public partial class Manager_Materias : Form
    {
        
        private int editar;
        private int bloqueo;
        public Manager_Materias()
        {
            InitializeComponent();
            editar = 0;
         
        }

        private void updateWindow()
        {
            btn_Registrar.Text = "Registrar";
            tB_Nombre.Text = "";
            tB_Alias.Text = "";
            tB_Descripcion.Text = "";
            nUD_Creditos.Value = 0;
            nUD_Horas.Value = 0;
            nUD_Nivel.Value = 0;
            editar = 0;

            dGV_Consulta.DataSource = Global.DBManager.subjectMultySelect(5);
            gB_Consulta.Enabled = dGV_Consulta.RowCount != 0;
            dGV_Bloqueos.DataSource = null;
            dGV_Bloqueos.Rows.Clear();
            gB_Bloqueos.Text = "Bloqueos";
            bloqueo = 0;
            gB_Bloqueos.Enabled = false;
        }

        private void Manager_Materias_Load(object sender, EventArgs e)
        {
            updateWindow();
        }

        private void btn_Registrar_Click(object sender, EventArgs e)
        {
            if (editar == 0)
            {
                if (Global.DBManager.insertSubject(tB_Nombre.Text.Trim(), tB_Alias.Text.Trim(), tB_Descripcion.Text.Trim(), (short)nUD_Creditos.Value, (short)nUD_Horas.Value, (byte)nUD_Nivel.Value) != 1)
                {
                    MessageBox.Show("Nombre o Alias repetidos");
                }
                else
                {
                    updateWindow();
                }
            }
            else
            {
                switch (Global.DBManager.insertSubject(tB_Nombre.Text.Trim(), tB_Alias.Text.Trim(), tB_Descripcion.Text.Trim(), (short)nUD_Creditos.Value, (short)nUD_Horas.Value, (byte)nUD_Nivel.Value, editar))
                {
                    case 1:
                        updateWindow();
                        break;
                    case 0:
                        MessageBox.Show("Nombre o Alias repetidos");
                        break;
                    case -1:
                        MessageBox.Show("No puede modificar una Materia con un semestre en curso");
                        updateWindow();
                        break;
                    default:
                        break;
                }
            }
        }

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            btn_Registrar.Text = "Guardar";

            int row = dGV_Consulta.CurrentCell.RowIndex;
            editar = (int)dGV_Consulta.Rows[row].Cells[0].Value;

            DataTable result = Global.DBManager.subjectMultySelect(3,editar);

            tB_Nombre.Text = result.Rows[0][0].ToString();           //Nombre

            tB_Alias.Text = result.Rows[0][1].ToString();                      //Alias
           
            tB_Descripcion.Text = result.Rows[0][5].ToString();                      //Descripcion 
           
            nUD_Horas.Value = int.Parse(result.Rows[0][3].ToString());    //Medias Horas
            
            nUD_Creditos.Value = int.Parse(result.Rows[0][2].ToString());    //Creditos
          
            nUD_Nivel.Value = int.Parse(result.Rows[0][4].ToString());    //Nivel
        }

        private void btn_BuscarBlq_Click(object sender, EventArgs e)
        {
            dGV_Bloqueos.ClearSelection();
            bool found = false;
            string search = tB_BusquedaBlq.Text;
            foreach (DataGridViewRow row in dGV_Bloqueos.Rows)
            {
                if (row.Cells[1].Value.ToString().Contains(search))
                {
                    found = true;
                    row.Selected = true;
                }
            }
            if (!found)
            {
                MessageBox.Show("No se encontró en la busqueda");
            }
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;

            if (MessageBox.Show("Está a punto de eliminar PERMANENTEMENTE la Materia: \""+ dGV_Consulta.Rows[row].Cells[1].Value.ToString() + "\", ¿Está seguro que desea continuar?",
                                     "Confirmar eliminacion de Materia",
                                     MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Global.DBManager.subjectMultySelect(6, (int)dGV_Consulta.Rows[row].Cells[0].Value);
            }
            updateWindow();
        }

        private void btn_Bloqueos_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;
            gB_Bloqueos.Text = "Bloqueos de " + dGV_Consulta.Rows[row].Cells[1].Value.ToString();
            bloqueo = (int)dGV_Consulta.Rows[row].Cells[0].Value;
            dGV_Bloqueos.DataSource = Global.DBManager.subjectMultySelect(7, bloqueo);
            gB_Bloqueos.Enabled = true;
            btn_Remover.Enabled = dGV_Bloqueos.RowCount != 0;
            tB_BusquedaBlq.Enabled = dGV_Bloqueos.RowCount != 0;
            dGV_Bloqueos.Enabled = dGV_Bloqueos.RowCount != 0;
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            new mm_Agregar_Bloqueos(bloqueo,false).ShowDialog();
            updateWindow();
        }

        private void btn_Remover_Click(object sender, EventArgs e)
        {
            new mm_Agregar_Bloqueos(bloqueo, true).ShowDialog();
            updateWindow();
        }

        private void tB_Busqueda_TextChanged(object sender, EventArgs e)
        {
            dGV_Consulta.DataSource = Global.DBManager.subjectMultySelect(5);
            btn_Editar.Enabled = dGV_Consulta.RowCount != 0;
            btn_Eliminar.Enabled = dGV_Consulta.RowCount != 0;
            btn_Bloqueos.Enabled = dGV_Consulta.RowCount != 0;
            Global.searchInDataGridView(dGV_Consulta, tB_Busqueda.Text);
        }

        private void tB_BusquedaBlq_TextChanged(object sender, EventArgs e)
        {
            dGV_Bloqueos.DataSource = Global.DBManager.subjectMultySelect(7, bloqueo);
            Global.searchInDataGridView(dGV_Bloqueos, tB_BusquedaBlq.Text);
        }
    }
}
