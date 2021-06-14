namespace Proyecto_MAD
{
    partial class Promedios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gB_Carrera = new System.Windows.Forms.GroupBox();
            this.cB_Carreras = new System.Windows.Forms.ComboBox();
            this.btn_C_Visualizar = new System.Windows.Forms.Button();
            this.rB_C_Toda = new System.Windows.Forms.RadioButton();
            this.rB_C_Semestral = new System.Windows.Forms.RadioButton();
            this.gB_Materia = new System.Windows.Forms.GroupBox();
            this.cB_Materia = new System.Windows.Forms.ComboBox();
            this.btn_M_Visualizar = new System.Windows.Forms.Button();
            this.rB_M_General = new System.Windows.Forms.RadioButton();
            this.rB_M_Grupal = new System.Windows.Forms.RadioButton();
            this.rB_M_Semestral = new System.Windows.Forms.RadioButton();
            this.gB_Semestre = new System.Windows.Forms.GroupBox();
            this.btn_S_Visualizar = new System.Windows.Forms.Button();
            this.rB_S_Materia = new System.Windows.Forms.RadioButton();
            this.rB_S_Carrera = new System.Windows.Forms.RadioButton();
            this.rB_S_General = new System.Windows.Forms.RadioButton();
            this.rB_S_Grupal = new System.Windows.Forms.RadioButton();
            this.dGV_Consulta = new System.Windows.Forms.DataGridView();
            this.chB_Carrera = new System.Windows.Forms.CheckBox();
            this.chB_Materia = new System.Windows.Forms.CheckBox();
            this.chB_Semestre = new System.Windows.Forms.CheckBox();
            this.cB_Semestre = new System.Windows.Forms.ComboBox();
            this.gB_Carrera.SuspendLayout();
            this.gB_Materia.SuspendLayout();
            this.gB_Semestre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Consulta)).BeginInit();
            this.SuspendLayout();
            // 
            // gB_Carrera
            // 
            this.gB_Carrera.Controls.Add(this.chB_Carrera);
            this.gB_Carrera.Controls.Add(this.cB_Carreras);
            this.gB_Carrera.Controls.Add(this.btn_C_Visualizar);
            this.gB_Carrera.Controls.Add(this.rB_C_Toda);
            this.gB_Carrera.Controls.Add(this.rB_C_Semestral);
            this.gB_Carrera.Location = new System.Drawing.Point(12, 12);
            this.gB_Carrera.Name = "gB_Carrera";
            this.gB_Carrera.Size = new System.Drawing.Size(142, 163);
            this.gB_Carrera.TabIndex = 0;
            this.gB_Carrera.TabStop = false;
            this.gB_Carrera.Text = "Por Carrera";
            // 
            // cB_Carreras
            // 
            this.cB_Carreras.FormattingEnabled = true;
            this.cB_Carreras.Location = new System.Drawing.Point(6, 103);
            this.cB_Carreras.Name = "cB_Carreras";
            this.cB_Carreras.Size = new System.Drawing.Size(130, 24);
            this.cB_Carreras.TabIndex = 4;
            // 
            // btn_C_Visualizar
            // 
            this.btn_C_Visualizar.Location = new System.Drawing.Point(7, 133);
            this.btn_C_Visualizar.Name = "btn_C_Visualizar";
            this.btn_C_Visualizar.Size = new System.Drawing.Size(129, 23);
            this.btn_C_Visualizar.TabIndex = 3;
            this.btn_C_Visualizar.Text = "Visualizar";
            this.btn_C_Visualizar.UseVisualStyleBackColor = true;
            this.btn_C_Visualizar.Click += new System.EventHandler(this.btn_C_Visualizar_Click);
            // 
            // rB_C_Toda
            // 
            this.rB_C_Toda.AutoSize = true;
            this.rB_C_Toda.Location = new System.Drawing.Point(7, 49);
            this.rB_C_Toda.Name = "rB_C_Toda";
            this.rB_C_Toda.Size = new System.Drawing.Size(129, 21);
            this.rB_C_Toda.TabIndex = 1;
            this.rB_C_Toda.TabStop = true;
            this.rB_C_Toda.Text = "Toda la Historia";
            this.rB_C_Toda.UseVisualStyleBackColor = true;
            // 
            // rB_C_Semestral
            // 
            this.rB_C_Semestral.AutoSize = true;
            this.rB_C_Semestral.Location = new System.Drawing.Point(7, 22);
            this.rB_C_Semestral.Name = "rB_C_Semestral";
            this.rB_C_Semestral.Size = new System.Drawing.Size(92, 21);
            this.rB_C_Semestral.TabIndex = 0;
            this.rB_C_Semestral.TabStop = true;
            this.rB_C_Semestral.Text = "Semestral";
            this.rB_C_Semestral.UseVisualStyleBackColor = true;
            // 
            // gB_Materia
            // 
            this.gB_Materia.Controls.Add(this.chB_Materia);
            this.gB_Materia.Controls.Add(this.cB_Materia);
            this.gB_Materia.Controls.Add(this.btn_M_Visualizar);
            this.gB_Materia.Controls.Add(this.rB_M_General);
            this.gB_Materia.Controls.Add(this.rB_M_Grupal);
            this.gB_Materia.Controls.Add(this.rB_M_Semestral);
            this.gB_Materia.Location = new System.Drawing.Point(160, 12);
            this.gB_Materia.Name = "gB_Materia";
            this.gB_Materia.Size = new System.Drawing.Size(170, 163);
            this.gB_Materia.TabIndex = 1;
            this.gB_Materia.TabStop = false;
            this.gB_Materia.Text = "Por Materia";
            // 
            // cB_Materia
            // 
            this.cB_Materia.FormattingEnabled = true;
            this.cB_Materia.Location = new System.Drawing.Point(5, 103);
            this.cB_Materia.Name = "cB_Materia";
            this.cB_Materia.Size = new System.Drawing.Size(159, 24);
            this.cB_Materia.TabIndex = 6;
            // 
            // btn_M_Visualizar
            // 
            this.btn_M_Visualizar.Location = new System.Drawing.Point(6, 133);
            this.btn_M_Visualizar.Name = "btn_M_Visualizar";
            this.btn_M_Visualizar.Size = new System.Drawing.Size(158, 23);
            this.btn_M_Visualizar.TabIndex = 4;
            this.btn_M_Visualizar.Text = "Visualizar";
            this.btn_M_Visualizar.UseVisualStyleBackColor = true;
            this.btn_M_Visualizar.Click += new System.EventHandler(this.btn_M_Visualizar_Click);
            // 
            // rB_M_General
            // 
            this.rB_M_General.AutoSize = true;
            this.rB_M_General.Location = new System.Drawing.Point(84, 21);
            this.rB_M_General.Name = "rB_M_General";
            this.rB_M_General.Size = new System.Drawing.Size(80, 21);
            this.rB_M_General.TabIndex = 6;
            this.rB_M_General.TabStop = true;
            this.rB_M_General.Text = "General";
            this.rB_M_General.UseVisualStyleBackColor = true;
            // 
            // rB_M_Grupal
            // 
            this.rB_M_Grupal.AutoSize = true;
            this.rB_M_Grupal.Location = new System.Drawing.Point(6, 21);
            this.rB_M_Grupal.Name = "rB_M_Grupal";
            this.rB_M_Grupal.Size = new System.Drawing.Size(72, 21);
            this.rB_M_Grupal.TabIndex = 5;
            this.rB_M_Grupal.TabStop = true;
            this.rB_M_Grupal.Text = "Grupal";
            this.rB_M_Grupal.UseVisualStyleBackColor = true;
            // 
            // rB_M_Semestral
            // 
            this.rB_M_Semestral.AutoSize = true;
            this.rB_M_Semestral.Location = new System.Drawing.Point(6, 48);
            this.rB_M_Semestral.Name = "rB_M_Semestral";
            this.rB_M_Semestral.Size = new System.Drawing.Size(92, 21);
            this.rB_M_Semestral.TabIndex = 4;
            this.rB_M_Semestral.TabStop = true;
            this.rB_M_Semestral.Text = "Semestral";
            this.rB_M_Semestral.UseVisualStyleBackColor = true;
            // 
            // gB_Semestre
            // 
            this.gB_Semestre.Controls.Add(this.cB_Semestre);
            this.gB_Semestre.Controls.Add(this.chB_Semestre);
            this.gB_Semestre.Controls.Add(this.btn_S_Visualizar);
            this.gB_Semestre.Controls.Add(this.rB_S_Materia);
            this.gB_Semestre.Controls.Add(this.rB_S_Carrera);
            this.gB_Semestre.Controls.Add(this.rB_S_General);
            this.gB_Semestre.Controls.Add(this.rB_S_Grupal);
            this.gB_Semestre.Location = new System.Drawing.Point(336, 12);
            this.gB_Semestre.Name = "gB_Semestre";
            this.gB_Semestre.Size = new System.Drawing.Size(192, 163);
            this.gB_Semestre.TabIndex = 2;
            this.gB_Semestre.TabStop = false;
            this.gB_Semestre.Text = "Por Semestre";
            // 
            // btn_S_Visualizar
            // 
            this.btn_S_Visualizar.Location = new System.Drawing.Point(6, 133);
            this.btn_S_Visualizar.Name = "btn_S_Visualizar";
            this.btn_S_Visualizar.Size = new System.Drawing.Size(181, 23);
            this.btn_S_Visualizar.TabIndex = 7;
            this.btn_S_Visualizar.Text = "Visualizar";
            this.btn_S_Visualizar.UseVisualStyleBackColor = true;
            this.btn_S_Visualizar.Click += new System.EventHandler(this.btn_S_Visualizar_Click);
            // 
            // rB_S_Materia
            // 
            this.rB_S_Materia.AutoSize = true;
            this.rB_S_Materia.Location = new System.Drawing.Point(84, 49);
            this.rB_S_Materia.Name = "rB_S_Materia";
            this.rB_S_Materia.Size = new System.Drawing.Size(102, 21);
            this.rB_S_Materia.TabIndex = 10;
            this.rB_S_Materia.TabStop = true;
            this.rB_S_Materia.Text = "Por Materia";
            this.rB_S_Materia.UseVisualStyleBackColor = true;
            // 
            // rB_S_Carrera
            // 
            this.rB_S_Carrera.AutoSize = true;
            this.rB_S_Carrera.Location = new System.Drawing.Point(84, 22);
            this.rB_S_Carrera.Name = "rB_S_Carrera";
            this.rB_S_Carrera.Size = new System.Drawing.Size(103, 21);
            this.rB_S_Carrera.TabIndex = 9;
            this.rB_S_Carrera.TabStop = true;
            this.rB_S_Carrera.Text = "Por Carrera";
            this.rB_S_Carrera.UseVisualStyleBackColor = true;
            // 
            // rB_S_General
            // 
            this.rB_S_General.AutoSize = true;
            this.rB_S_General.Location = new System.Drawing.Point(6, 49);
            this.rB_S_General.Name = "rB_S_General";
            this.rB_S_General.Size = new System.Drawing.Size(80, 21);
            this.rB_S_General.TabIndex = 8;
            this.rB_S_General.TabStop = true;
            this.rB_S_General.Text = "General";
            this.rB_S_General.UseVisualStyleBackColor = true;
            // 
            // rB_S_Grupal
            // 
            this.rB_S_Grupal.AutoSize = true;
            this.rB_S_Grupal.Location = new System.Drawing.Point(6, 21);
            this.rB_S_Grupal.Name = "rB_S_Grupal";
            this.rB_S_Grupal.Size = new System.Drawing.Size(72, 21);
            this.rB_S_Grupal.TabIndex = 7;
            this.rB_S_Grupal.TabStop = true;
            this.rB_S_Grupal.Text = "Grupal";
            this.rB_S_Grupal.UseVisualStyleBackColor = true;
            // 
            // dGV_Consulta
            // 
            this.dGV_Consulta.AllowUserToAddRows = false;
            this.dGV_Consulta.AllowUserToDeleteRows = false;
            this.dGV_Consulta.AllowUserToResizeColumns = false;
            this.dGV_Consulta.AllowUserToResizeRows = false;
            this.dGV_Consulta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGV_Consulta.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dGV_Consulta.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dGV_Consulta.Location = new System.Drawing.Point(12, 181);
            this.dGV_Consulta.Name = "dGV_Consulta";
            this.dGV_Consulta.ReadOnly = true;
            this.dGV_Consulta.RowTemplate.Height = 24;
            this.dGV_Consulta.Size = new System.Drawing.Size(516, 226);
            this.dGV_Consulta.TabIndex = 3;
            // 
            // chB_Carrera
            // 
            this.chB_Carrera.AutoSize = true;
            this.chB_Carrera.Location = new System.Drawing.Point(7, 76);
            this.chB_Carrera.Name = "chB_Carrera";
            this.chB_Carrera.Size = new System.Drawing.Size(78, 21);
            this.chB_Carrera.TabIndex = 4;
            this.chB_Carrera.Text = "Carrera";
            this.chB_Carrera.UseVisualStyleBackColor = true;
            this.chB_Carrera.CheckedChanged += new System.EventHandler(this.chB_Carrera_CheckedChanged);
            // 
            // chB_Materia
            // 
            this.chB_Materia.AutoSize = true;
            this.chB_Materia.Location = new System.Drawing.Point(6, 76);
            this.chB_Materia.Name = "chB_Materia";
            this.chB_Materia.Size = new System.Drawing.Size(77, 21);
            this.chB_Materia.TabIndex = 5;
            this.chB_Materia.Text = "Materia";
            this.chB_Materia.UseVisualStyleBackColor = true;
            this.chB_Materia.CheckedChanged += new System.EventHandler(this.chB_Materia_CheckedChanged);
            // 
            // chB_Semestre
            // 
            this.chB_Semestre.AutoSize = true;
            this.chB_Semestre.Location = new System.Drawing.Point(6, 76);
            this.chB_Semestre.Name = "chB_Semestre";
            this.chB_Semestre.Size = new System.Drawing.Size(90, 21);
            this.chB_Semestre.TabIndex = 7;
            this.chB_Semestre.Text = "Semestre";
            this.chB_Semestre.UseVisualStyleBackColor = true;
            this.chB_Semestre.CheckedChanged += new System.EventHandler(this.chB_Semestre_CheckedChanged);
            // 
            // cB_Semestre
            // 
            this.cB_Semestre.FormattingEnabled = true;
            this.cB_Semestre.Location = new System.Drawing.Point(6, 103);
            this.cB_Semestre.Name = "cB_Semestre";
            this.cB_Semestre.Size = new System.Drawing.Size(181, 24);
            this.cB_Semestre.TabIndex = 7;
            // 
            // Promedios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 412);
            this.Controls.Add(this.dGV_Consulta);
            this.Controls.Add(this.gB_Semestre);
            this.Controls.Add(this.gB_Materia);
            this.Controls.Add(this.gB_Carrera);
            this.Name = "Promedios";
            this.Text = "Promedios";
            this.gB_Carrera.ResumeLayout(false);
            this.gB_Carrera.PerformLayout();
            this.gB_Materia.ResumeLayout(false);
            this.gB_Materia.PerformLayout();
            this.gB_Semestre.ResumeLayout(false);
            this.gB_Semestre.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Consulta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gB_Carrera;
        private System.Windows.Forms.GroupBox gB_Materia;
        private System.Windows.Forms.GroupBox gB_Semestre;
        private System.Windows.Forms.RadioButton rB_C_Semestral;
        private System.Windows.Forms.RadioButton rB_C_Toda;
        private System.Windows.Forms.Button btn_C_Visualizar;
        private System.Windows.Forms.RadioButton rB_M_Semestral;
        private System.Windows.Forms.RadioButton rB_M_Grupal;
        private System.Windows.Forms.RadioButton rB_M_General;
        private System.Windows.Forms.Button btn_M_Visualizar;
        private System.Windows.Forms.RadioButton rB_S_General;
        private System.Windows.Forms.RadioButton rB_S_Grupal;
        private System.Windows.Forms.RadioButton rB_S_Carrera;
        private System.Windows.Forms.RadioButton rB_S_Materia;
        private System.Windows.Forms.Button btn_S_Visualizar;
        private System.Windows.Forms.DataGridView dGV_Consulta;
        private System.Windows.Forms.ComboBox cB_Carreras;
        private System.Windows.Forms.ComboBox cB_Materia;
        private System.Windows.Forms.CheckBox chB_Carrera;
        private System.Windows.Forms.CheckBox chB_Materia;
        private System.Windows.Forms.CheckBox chB_Semestre;
        private System.Windows.Forms.ComboBox cB_Semestre;
    }
}