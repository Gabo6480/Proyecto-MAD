namespace Proyecto_MAD
{
    partial class mm_Agregar_Bloqueos
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
            this.btn_Agregar = new System.Windows.Forms.Button();
            this.dGV_Bloqueos = new System.Windows.Forms.DataGridView();
            this.CheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Clave = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Bloqueos)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Agregar
            // 
            this.btn_Agregar.Location = new System.Drawing.Point(12, 156);
            this.btn_Agregar.Name = "btn_Agregar";
            this.btn_Agregar.Size = new System.Drawing.Size(422, 36);
            this.btn_Agregar.TabIndex = 51;
            this.btn_Agregar.Text = "Agregar Marcados";
            this.btn_Agregar.UseVisualStyleBackColor = true;
            this.btn_Agregar.Click += new System.EventHandler(this.btn_Agregar_Click);
            // 
            // dGV_Bloqueos
            // 
            this.dGV_Bloqueos.AllowUserToAddRows = false;
            this.dGV_Bloqueos.AllowUserToResizeColumns = false;
            this.dGV_Bloqueos.AllowUserToResizeRows = false;
            this.dGV_Bloqueos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dGV_Bloqueos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Bloqueos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckBox,
            this.Clave,
            this.Nombre,
            this.Alias,
            this.Nivel});
            this.dGV_Bloqueos.Location = new System.Drawing.Point(12, 12);
            this.dGV_Bloqueos.Name = "dGV_Bloqueos";
            this.dGV_Bloqueos.RowTemplate.Height = 24;
            this.dGV_Bloqueos.Size = new System.Drawing.Size(422, 137);
            this.dGV_Bloqueos.TabIndex = 50;
            this.dGV_Bloqueos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGV_Bloqueos_CellContentClick);
            // 
            // CheckBox
            // 
            this.CheckBox.HeaderText = "Agregar";
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Width = 65;
            // 
            // Clave
            // 
            this.Clave.DataPropertyName = "Clave";
            this.Clave.HeaderText = "Clave";
            this.Clave.Name = "Clave";
            this.Clave.ReadOnly = true;
            this.Clave.Width = 72;
            // 
            // Nombre
            // 
            this.Nombre.DataPropertyName = "Nombre";
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            this.Nombre.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Nombre.Width = 87;
            // 
            // Alias
            // 
            this.Alias.DataPropertyName = "Alias";
            this.Alias.HeaderText = "Alias";
            this.Alias.Name = "Alias";
            this.Alias.Width = 67;
            // 
            // Nivel
            // 
            this.Nivel.DataPropertyName = "Nivel";
            this.Nivel.HeaderText = "Nivel";
            this.Nivel.Name = "Nivel";
            this.Nivel.Width = 68;
            // 
            // mm_Agregar_Bloqueos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 195);
            this.Controls.Add(this.btn_Agregar);
            this.Controls.Add(this.dGV_Bloqueos);
            this.Name = "mm_Agregar_Bloqueos";
            this.Text = "Agregar Bloqueos";
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Bloqueos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Agregar;
        private System.Windows.Forms.DataGridView dGV_Bloqueos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Clave;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nivel;
    }
}