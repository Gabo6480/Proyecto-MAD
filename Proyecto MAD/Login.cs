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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable login = Global.DBManager.userValidate(tB_Usuario.Text.Trim(), tB_Constraseña.Text);
            if (login.Rows.Count != 0) {
                this.Hide();
                if (login.Rows[0][1].ToString().CompareTo("Administrador/a")==0)
                {
                    if (new AdminMain().ShowDialog() != DialogResult.OK)
                        Application.Exit();
                }
                else
                {
                    if (new UserMain().ShowDialog() != DialogResult.OK)
                        Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Usuario o contraseña invalidos");
            }
        }
    }
}
