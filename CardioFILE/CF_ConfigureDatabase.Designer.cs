
namespace CardioFILE_Pub
{
    partial class CF_ConfigureDatabase
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
            this.CF_ConfigDatabase_LDirActual = new System.Windows.Forms.Label();
            this.CF_ConfigDatabase_TDirActual = new System.Windows.Forms.TextBox();
            this.CF_ConfigDatabase_BAceptar = new System.Windows.Forms.Button();
            this.CF_ConfigDatabase_BCancelar = new System.Windows.Forms.Button();
            this.CF_ConfigDatabase_TDirNuevo = new System.Windows.Forms.TextBox();
            this.CF_ConfigDatabase_LDirNuevo = new System.Windows.Forms.Label();
            this.CF_ConfigDatabase_DlgDirectorio = new System.Windows.Forms.FolderBrowserDialog();
            this.CF_ConfigDatabase_BDirectorio = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CF_ConfigDatabase_LDirActual
            // 
            this.CF_ConfigDatabase_LDirActual.AutoSize = true;
            this.CF_ConfigDatabase_LDirActual.Location = new System.Drawing.Point(10, 9);
            this.CF_ConfigDatabase_LDirActual.Name = "CF_ConfigDatabase_LDirActual";
            this.CF_ConfigDatabase_LDirActual.Size = new System.Drawing.Size(85, 13);
            this.CF_ConfigDatabase_LDirActual.TabIndex = 0;
            this.CF_ConfigDatabase_LDirActual.Text = "Directorio Actual";
            // 
            // CF_ConfigDatabase_TDirActual
            // 
            this.CF_ConfigDatabase_TDirActual.Location = new System.Drawing.Point(13, 37);
            this.CF_ConfigDatabase_TDirActual.Name = "CF_ConfigDatabase_TDirActual";
            this.CF_ConfigDatabase_TDirActual.ReadOnly = true;
            this.CF_ConfigDatabase_TDirActual.Size = new System.Drawing.Size(696, 20);
            this.CF_ConfigDatabase_TDirActual.TabIndex = 1;
            // 
            // CF_ConfigDatabase_BAceptar
            // 
            this.CF_ConfigDatabase_BAceptar.Location = new System.Drawing.Point(13, 164);
            this.CF_ConfigDatabase_BAceptar.Name = "CF_ConfigDatabase_BAceptar";
            this.CF_ConfigDatabase_BAceptar.Size = new System.Drawing.Size(75, 23);
            this.CF_ConfigDatabase_BAceptar.TabIndex = 2;
            this.CF_ConfigDatabase_BAceptar.Text = "Aceptar";
            this.CF_ConfigDatabase_BAceptar.UseVisualStyleBackColor = true;
            this.CF_ConfigDatabase_BAceptar.Click += new System.EventHandler(this.CF_ConfigDatabase_BAceptar_Click);
            // 
            // CF_ConfigDatabase_BCancelar
            // 
            this.CF_ConfigDatabase_BCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CF_ConfigDatabase_BCancelar.Location = new System.Drawing.Point(633, 164);
            this.CF_ConfigDatabase_BCancelar.Name = "CF_ConfigDatabase_BCancelar";
            this.CF_ConfigDatabase_BCancelar.Size = new System.Drawing.Size(75, 23);
            this.CF_ConfigDatabase_BCancelar.TabIndex = 3;
            this.CF_ConfigDatabase_BCancelar.Text = "Cancelar";
            this.CF_ConfigDatabase_BCancelar.UseVisualStyleBackColor = true;
            this.CF_ConfigDatabase_BCancelar.Click += new System.EventHandler(this.CF_ConfigDatabase_BCancelar_Click);
            // 
            // CF_ConfigDatabase_TDirNuevo
            // 
            this.CF_ConfigDatabase_TDirNuevo.Location = new System.Drawing.Point(14, 103);
            this.CF_ConfigDatabase_TDirNuevo.Name = "CF_ConfigDatabase_TDirNuevo";
            this.CF_ConfigDatabase_TDirNuevo.ReadOnly = true;
            this.CF_ConfigDatabase_TDirNuevo.Size = new System.Drawing.Size(696, 20);
            this.CF_ConfigDatabase_TDirNuevo.TabIndex = 5;
            // 
            // CF_ConfigDatabase_LDirNuevo
            // 
            this.CF_ConfigDatabase_LDirNuevo.AutoSize = true;
            this.CF_ConfigDatabase_LDirNuevo.Location = new System.Drawing.Point(11, 75);
            this.CF_ConfigDatabase_LDirNuevo.Name = "CF_ConfigDatabase_LDirNuevo";
            this.CF_ConfigDatabase_LDirNuevo.Size = new System.Drawing.Size(87, 13);
            this.CF_ConfigDatabase_LDirNuevo.TabIndex = 4;
            this.CF_ConfigDatabase_LDirNuevo.Text = "Directorio Nuevo";
            // 
            // CF_ConfigDatabase_BDirectorio
            // 
            this.CF_ConfigDatabase_BDirectorio.Location = new System.Drawing.Point(277, 164);
            this.CF_ConfigDatabase_BDirectorio.Name = "CF_ConfigDatabase_BDirectorio";
            this.CF_ConfigDatabase_BDirectorio.Size = new System.Drawing.Size(145, 23);
            this.CF_ConfigDatabase_BDirectorio.TabIndex = 6;
            this.CF_ConfigDatabase_BDirectorio.Text = "Buscar Nuevo Directorio";
            this.CF_ConfigDatabase_BDirectorio.UseVisualStyleBackColor = true;
            this.CF_ConfigDatabase_BDirectorio.Click += new System.EventHandler(this.CF_ConfigDatabase_BDirectorio_Click);
            // 
            // CF_ConfigureDatabase
            // 
            this.AcceptButton = this.CF_ConfigDatabase_BAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CF_ConfigDatabase_BCancelar;
            this.ClientSize = new System.Drawing.Size(720, 199);
            this.Controls.Add(this.CF_ConfigDatabase_BDirectorio);
            this.Controls.Add(this.CF_ConfigDatabase_TDirNuevo);
            this.Controls.Add(this.CF_ConfigDatabase_LDirNuevo);
            this.Controls.Add(this.CF_ConfigDatabase_BCancelar);
            this.Controls.Add(this.CF_ConfigDatabase_BAceptar);
            this.Controls.Add(this.CF_ConfigDatabase_TDirActual);
            this.Controls.Add(this.CF_ConfigDatabase_LDirActual);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CF_ConfigureDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Directorio de Base de Datos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CF_ConfigDatabase_LDirActual;
        private System.Windows.Forms.TextBox CF_ConfigDatabase_TDirActual;
        private System.Windows.Forms.Button CF_ConfigDatabase_BAceptar;
        private System.Windows.Forms.Button CF_ConfigDatabase_BCancelar;
        private System.Windows.Forms.TextBox CF_ConfigDatabase_TDirNuevo;
        private System.Windows.Forms.Label CF_ConfigDatabase_LDirNuevo;
        private System.Windows.Forms.FolderBrowserDialog CF_ConfigDatabase_DlgDirectorio;
        private System.Windows.Forms.Button CF_ConfigDatabase_BDirectorio;
    }
}