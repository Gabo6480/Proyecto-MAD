﻿using System;
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
    public partial class AdminMain : Form
    {
        public AdminMain()
        {
            InitializeComponent();
        }

        private void nuevoMaestroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dar_Alta_Maestro().ShowDialog();
        }

        private void materiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Manager_Materias().ShowDialog();
        }

        private void carrerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dar_Alta_Carrera().ShowDialog();
        }

        private void coordinadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dar_Alta_Coordinadores().ShowDialog();
        }

        private void iniciarSemestreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable result = Global.DBManager.semesterMultySelect(1);
            if (result.Columns.Count != 2)
            {
                MessageBox.Show(result.Rows[0].Field<string>("ERROR"));
            }
            else
            {
                MessageBox.Show("¡Bienvenido al semestre " + result.Rows[0].Field<string>("Semestre") + "!");
            }
        }

        private void terminarSemestreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable result = Global.DBManager.semesterMultySelect(2);
            if (result.Columns.Count != 2)
            {
                MessageBox.Show(result.Rows[0].Field<string>("ERROR"));
            }
            else
            {
                MessageBox.Show("El semestre " + result.Rows[0].Field<string>("Semestre") + " ha sido finalizado");
            }
        }

        private void promediosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Promedios().ShowDialog();
        }

        private void declararSemestresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Iniciar_Semestre().ShowDialog();
        }
    }
}
