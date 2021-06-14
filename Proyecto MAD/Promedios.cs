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
    public partial class Promedios : Form
    {
        
        public Promedios()
        {
         
            InitializeComponent();
        }

        private void updateWindow()
        {
            dGV_Consulta.DataSource = null;
            dGV_Consulta.Rows.Clear();
        }
        
        private void btn_C_Visualizar_Click(object sender, EventArgs e)
        {
            updateWindow();
            int visualizar = 0;
            if (cB_Carreras.SelectedValue != null)
            {
                visualizar = int.Parse(cB_Carreras.SelectedValue.ToString());
            }
            dGV_Consulta.DataSource = Global.DBManager.careerAverages(rB_C_Semestral.Checked, visualizar);
        }

        private void chB_Carrera_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_Carrera.Checked)
            {
                cB_Carreras.Enabled = true;
                cB_Carreras.DisplayMember = "Nombre";
                cB_Carreras.ValueMember = "Clave";
                cB_Carreras.DataSource = Global.DBManager.careerMultySelect(0);
            }
            else
            {
                cB_Carreras.DataSource = null;
                cB_Carreras.Items.Clear();
                cB_Carreras.Enabled = false;
            }
        }

        private void chB_Materia_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_Materia.Checked)
            {
                cB_Materia.Enabled = true;
                cB_Materia.DisplayMember = "Nombre";
                cB_Materia.ValueMember = "Clave";
                cB_Materia.DataSource = Global.DBManager.subjectMultySelect(4);
            }
            else
            {
                cB_Materia.DataSource = null;
                cB_Materia.Items.Clear();
                cB_Materia.Enabled = false;
            }
        }

        private void chB_Semestre_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_Semestre.Checked)
            {
                cB_Semestre.Enabled = true;
                cB_Semestre.DisplayMember = "Semestre";
                cB_Semestre.ValueMember = "id";
                cB_Semestre.DataSource = Global.DBManager.semesterMultySelect(0);
            }
            else
            {
                cB_Semestre.DataSource = null;
                cB_Semestre.Items.Clear();
                cB_Semestre.Enabled = false;
            }
        }

        private void btn_M_Visualizar_Click(object sender, EventArgs e)
        {
            updateWindow();
            int option = 0;
            if (rB_M_Semestral.Checked)
            {
                option = 1;
            }
            else if(rB_M_Grupal.Checked)
            {
                option = 2;
            }

            int visualizar = 0;
            if (cB_Materia.SelectedValue != null)
            {
                visualizar = int.Parse(cB_Materia.SelectedValue.ToString());
            }

            if (chB_Materia.Checked)
                dGV_Consulta.DataSource = Global.DBManager.subjectAverages(option, visualizar);
            else
                dGV_Consulta.DataSource = Global.DBManager.subjectAverages(option, -1);
        }

        private void btn_S_Visualizar_Click(object sender, EventArgs e)
        {
            int option = 0;
            if (rB_S_Materia.Checked)
            {
                option = 1;
            }
            else if (rB_S_Carrera.Checked)
            {
                option = 2;
            }
            else if (rB_S_General.Checked)
            {
                option = 3;
            }

            int visualizar = 0;
            if (cB_Semestre.SelectedValue != null)
            {
                visualizar = int.Parse(cB_Semestre.SelectedValue.ToString());
            }

            if (chB_Semestre.Checked)
                dGV_Consulta.DataSource = Global.DBManager.semesterAverages(option, visualizar);
            else
                dGV_Consulta.DataSource = Global.DBManager.semesterAverages(option, -1);
        }
    }
}
