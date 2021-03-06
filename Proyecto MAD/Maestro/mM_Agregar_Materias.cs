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
    public partial class mM_Agregar_Materias : Form
    {
        
        private int agregar;
        private bool remover;
        public mM_Agregar_Materias(int add, bool remove)
        {
         
            agregar = add;
            remover = remove;
            InitializeComponent();
            if (remove)
            {
                btn_Agregar.Text = "Remover";
                dGV_Materias.Columns[0].HeaderText = "Remover";
                this.Text = "Remover Materias";
                dGV_Materias.DataSource = Global.DBManager.teacherMultySelect(3, add);
            }
            else
            {
                dGV_Materias.DataSource = Global.DBManager.teacherMultySelect(5, add);
            }
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dGV_Materias.Rows)
            {
                if (row.Cells[CheckBox.Name] != null && row.Cells[CheckBox.Name].Value != null)
                {
                    if ((bool)row.Cells[CheckBox.Name].Value)
                    {
                        if(Global.DBManager.teacherSubject(agregar, (int)row.Cells[Clave.Name].Value, remover) == -1)
                        {
                            MessageBox.Show("Error: No se puede eliminar esta relacion porque existen grupos en los que este profesor imparte esta materia");
                        }
                    }
                }
            }
            this.Close();
        }
    }
}
