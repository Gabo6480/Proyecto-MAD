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
    public partial class mg_Agregar_Alumnos : Form
    {
        
        private int agregar;
        private bool remover;
        public mg_Agregar_Alumnos(int add, bool remove)
        {
         
            agregar = add;
            remover = remove;
            InitializeComponent();
            if (remove)
            {
                btn_Agregar.Text = "Remover";
                dGV_Alumnos.Columns[0].HeaderText = "Remover";
                this.Text = "Remover Alumnos";
                dGV_Alumnos.DataSource = Global.DBManager.groupMultySelect(4, add);
            }
            else
            {
                dGV_Alumnos.DataSource = Global.DBManager.groupMultySelect(5, add);
            }
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dGV_Alumnos.Rows)
            {
                if (row.Cells[CheckBox.Name] != null && row.Cells[CheckBox.Name].Value != null)
                {
                    if ((bool)row.Cells[CheckBox.Name].Value)
                    {
                        switch (Global.DBManager.enrollStudentToGroup(agregar, (int)row.Cells[Matricula.Name].Value, remover))
                        {
                            case -1:
                                MessageBox.Show("Error: No se puede inscribir al alumno " + row.Cells[Nombre.Name].Value.ToString() + " porque exedería el cupo del aula");
                                break;
                            case -2:
                                MessageBox.Show("Error: No se puede inscribir al alumno " + row.Cells[Nombre.Name].Value.ToString() + " porque exedería el maximo de creditos");
                                break;
                            default:
                                break;
                        }
                        
                    }
                }
            }
            this.Close();
        }

        private void tB_Busqueda_TextChanged(object sender, EventArgs e)
        {
            if (remover)
            {
                dGV_Alumnos.DataSource = Global.DBManager.groupMultySelect(4, agregar);
            }
            else
            {
                dGV_Alumnos.DataSource = Global.DBManager.groupMultySelect(5, agregar);
            }
            Global.searchInDataGridView(dGV_Alumnos, tB_Busqueda.Text);
        }
    }
}
