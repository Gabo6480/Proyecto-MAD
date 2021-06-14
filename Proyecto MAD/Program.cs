﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace Proyecto_MAD
{
    static class Global
    {
        private static SqlDBManager _manager = new SqlDBManager();

        public static SqlDBManager DBManager
        {
            get { return _manager; }
        }

        public static void searchInDataGridView(DataGridView dataGrid, string Busqueda)
        {
            if (Busqueda != "")
            {
                dataGrid.ClearSelection();
                for(int R = dataGrid.Rows.Count - 1; R >= 0 ; R--)
                {
                    bool found = false;
                    for(int C = 0; C < dataGrid.Rows[R].Cells.Count; C++)
                    {
                        if (dataGrid.Rows[R].Cells[C].Value.ToString().ToLower().Contains(Busqueda.ToLower()))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        dataGrid.Rows.Remove(dataGrid.Rows[R]);
                    }
                }
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
