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
    public partial class Dar_Alta_Carrera : Form
    {
        private int editar;
        private int materia;
        
        public Dar_Alta_Carrera()
        {
            editar = 0;
         
            InitializeComponent();
        }

        private void updateWindow()
        {
            dGV_Consulta.DataSource = Global.DBManager.careerMultySelect(0);
            gB_Consulta.Enabled = dGV_Consulta.RowCount != 0;
            btn_Registrar.Text = "Registrar";
            tB_Nombre.Text = "";          
            tB_Alias.Text = "";               
            tB_Descripcion.Text = "";
            editar = 0;

            dGV_Materias.DataSource = null;
            dGV_Materias.Rows.Clear();
            gB_Materias.Text = "Materias";
            gB_Materias.Enabled = false;
            materia = 0;
            
        }

        private void btn_Registrar_Click(object sender, EventArgs e)
        {
            if (editar == 0)
            {
                if (Global.DBManager.insertCareer(tB_Nombre.Text.Trim(), tB_Alias.Text.Trim(), tB_Descripcion.Text) == -1)
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
                switch(Global.DBManager.insertCareer(tB_Nombre.Text.Trim(), tB_Alias.Text.Trim(), tB_Descripcion.Text.Trim(),editar))
                {
                    case -1:
                        MessageBox.Show("Nombre o Alias repetidos");
                        break;
                    case -2:
                        MessageBox.Show("No puede modificar una carrera con un semestre en proceso");
                        updateWindow();
                        break;
                    default:
                        updateWindow();
                        break;
                }
            }
        }

        private void Dar_Alta_Carrera_Load(object sender, EventArgs e)
        {
            updateWindow();
        }

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            btn_Registrar.Text = "Guardar";

            int row = dGV_Consulta.CurrentCell.RowIndex;
            editar = (int)dGV_Consulta.Rows[row].Cells[0].Value;

            DataTable result = Global.DBManager.careerMultySelect(2, editar);

            tB_Nombre.Text = result.Rows[0][1].ToString();           //Nombre

            tB_Alias.Text = result.Rows[0][2].ToString();                      //Alias

            tB_Descripcion.Text = result.Rows[0][3].ToString();                      //Descripcion 
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;

            if (MessageBox.Show("Está a punto de eliminar PERMANENTEMENTE la Carrera: \"" + dGV_Consulta.Rows[row].Cells[1].Value.ToString() + "\", ¿Está seguro que desea continuar?",
                                     "Confirmar eliminacion de Carrera",
                                     MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataTable delete = Global.DBManager.careerMultySelect(3, (int)dGV_Consulta.Rows[row].Cells[0].Value);
                if (delete.Rows.Count != 0)
                {
                    MessageBox.Show(delete.Rows[0].Field<string>("ERROR"));
                }
            }
            updateWindow();
        }

        private void btn_Materias_Click(object sender, EventArgs e)
        {
            int row = dGV_Consulta.CurrentCell.RowIndex;
            gB_Materias.Text = "Materias de " + dGV_Consulta.Rows[row].Cells[1].Value.ToString();
            materia = (int)dGV_Consulta.Rows[row].Cells[0].Value;
            dGV_Materias.DataSource = Global.DBManager.careerMultySelect(4, materia);
            gB_Materias.Enabled = true;
            btn_Remover.Enabled = dGV_Materias.RowCount != 0;
            tB_BusquedaMat.Enabled = dGV_Materias.RowCount != 0;
            dGV_Materias.Enabled = dGV_Materias.RowCount != 0;
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            new mc_Agregar_Materias(materia,false).ShowDialog();
            updateWindow();
        }

        private void btn_Remover_Click(object sender, EventArgs e)
        {
            new mc_Agregar_Materias(materia, true).ShowDialog();
            updateWindow();
        }

        private void tB_BusquedaCarr_TextChanged(object sender, EventArgs e)
        {
            dGV_Consulta.DataSource = Global.DBManager.careerMultySelect(0);
            btn_Editar.Enabled = dGV_Consulta.RowCount != 0;
            btn_Eliminar.Enabled = dGV_Consulta.RowCount != 0;
            btn_Materias.Enabled = dGV_Consulta.RowCount != 0;
            Global.searchInDataGridView(dGV_Consulta, tB_BusquedaCarr.Text);
        }

        private void tB_BusquedaMat_TextChanged(object sender, EventArgs e)
        {
            dGV_Materias.DataSource = Global.DBManager.careerMultySelect(4, materia);
            Global.searchInDataGridView(dGV_Materias, tB_BusquedaMat.Text);
        }
    }
}
