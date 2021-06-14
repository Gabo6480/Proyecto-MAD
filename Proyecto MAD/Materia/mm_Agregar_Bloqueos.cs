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
    public partial class mm_Agregar_Bloqueos : Form
    {
        
        private int bloqueo;
        private bool remover;
        public mm_Agregar_Bloqueos(int blocking, bool remove)
        {
            bloqueo = blocking;
            remover = remove;
         
            InitializeComponent();
            if (remove) {
                btn_Agregar.Text = "Remover";
                dGV_Bloqueos.Columns[0].HeaderText = "Remover";
                this.Text = "Remover Bloqueos";
                dGV_Bloqueos.DataSource = Global.DBManager.subjectMultySelect(7, blocking);
            }
            else
            {
                dGV_Bloqueos.DataSource = Global.DBManager.subjectMultySelect(8, blocking);
            }
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dGV_Bloqueos.Rows)
            {
                if (row.Cells[CheckBox.Name] != null && row.Cells[CheckBox.Name].Value != null) {
                    if ((bool)row.Cells[CheckBox.Name].Value)
                    {
                        Global.DBManager.subjectBlockSubject(bloqueo, (int)row.Cells[Clave.Name].Value, remover);
                    }
                }
            }
            this.Close();
        }

        private void dGV_Bloqueos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
