namespace CardioFILE_Pub
{
    partial class CF_Dialog_BasicData_Browser
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
            this.CF_Dialog_Btn_BasicData_OK = new System.Windows.Forms.Button();
            this.CF_Dialog_Btn_BasicData_Cancel = new System.Windows.Forms.Button();
            this.CF_BasicData_Browser_View = new System.Windows.Forms.DataGridView();
            this.CF_Browser_BasicData_CardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CF_Browser_BasicData_LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CF_Browser_BasicData_FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.CF_BasicData_Browser_View)).BeginInit();
            this.SuspendLayout();
            // 
            // CF_Dialog_Btn_BasicData_OK
            // 
            this.CF_Dialog_Btn_BasicData_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CF_Dialog_Btn_BasicData_OK.Location = new System.Drawing.Point(6, 312);
            this.CF_Dialog_Btn_BasicData_OK.Name = "CF_Dialog_Btn_BasicData_OK";
            this.CF_Dialog_Btn_BasicData_OK.Size = new System.Drawing.Size(75, 23);
            this.CF_Dialog_Btn_BasicData_OK.TabIndex = 0;
            this.CF_Dialog_Btn_BasicData_OK.Text = "Aceptar";
            this.CF_Dialog_Btn_BasicData_OK.UseVisualStyleBackColor = true;
            this.CF_Dialog_Btn_BasicData_OK.Click += new System.EventHandler(this.CF_Dialog_Btn_BasicData_OK_Click);
            // 
            // CF_Dialog_Btn_BasicData_Cancel
            // 
            this.CF_Dialog_Btn_BasicData_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CF_Dialog_Btn_BasicData_Cancel.Location = new System.Drawing.Point(544, 312);
            this.CF_Dialog_Btn_BasicData_Cancel.Name = "CF_Dialog_Btn_BasicData_Cancel";
            this.CF_Dialog_Btn_BasicData_Cancel.Size = new System.Drawing.Size(75, 23);
            this.CF_Dialog_Btn_BasicData_Cancel.TabIndex = 1;
            this.CF_Dialog_Btn_BasicData_Cancel.Text = "Cancelar";
            this.CF_Dialog_Btn_BasicData_Cancel.UseVisualStyleBackColor = true;
            this.CF_Dialog_Btn_BasicData_Cancel.Click += new System.EventHandler(this.CF_Dialog_Btn_BasicData_Cancel_Click);
            // 
            // CF_BasicData_Browser_View
            // 
            this.CF_BasicData_Browser_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CF_BasicData_Browser_View.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CF_Browser_BasicData_CardID,
            this.CF_Browser_BasicData_LastName,
            this.CF_Browser_BasicData_FirstName});
            this.CF_BasicData_Browser_View.Location = new System.Drawing.Point(6, 6);
            this.CF_BasicData_Browser_View.Name = "CF_BasicData_Browser_View";
            this.CF_BasicData_Browser_View.Size = new System.Drawing.Size(612, 300);
            this.CF_BasicData_Browser_View.TabIndex = 2;
            this.CF_BasicData_Browser_View.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CF_BasicData_Browser_View_CellClick);
            // 
            // CF_Browser_BasicData_CardID
            // 
            this.CF_Browser_BasicData_CardID.HeaderText = "Nro de Ficha";
            this.CF_Browser_BasicData_CardID.Name = "CF_Browser_BasicData_CardID";
            this.CF_Browser_BasicData_CardID.ReadOnly = true;
            // 
            // CF_Browser_BasicData_LastName
            // 
            this.CF_Browser_BasicData_LastName.HeaderText = "Apellidos";
            this.CF_Browser_BasicData_LastName.Name = "CF_Browser_BasicData_LastName";
            this.CF_Browser_BasicData_LastName.ReadOnly = true;
            this.CF_Browser_BasicData_LastName.Width = 200;
            // 
            // CF_Browser_BasicData_FirstName
            // 
            this.CF_Browser_BasicData_FirstName.HeaderText = "Nombres";
            this.CF_Browser_BasicData_FirstName.Name = "CF_Browser_BasicData_FirstName";
            this.CF_Browser_BasicData_FirstName.ReadOnly = true;
            this.CF_Browser_BasicData_FirstName.Width = 300;
            // 
            // CF_Dialog_BasicData_Browser
            // 
            this.AcceptButton = this.CF_Dialog_Btn_BasicData_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CF_Dialog_Btn_BasicData_Cancel;
            this.ClientSize = new System.Drawing.Size(626, 339);
            this.Controls.Add(this.CF_BasicData_Browser_View);
            this.Controls.Add(this.CF_Dialog_Btn_BasicData_Cancel);
            this.Controls.Add(this.CF_Dialog_Btn_BasicData_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CF_Dialog_BasicData_Browser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Visor de Datos Basicos";
            this.Load += new System.EventHandler(this.CF_Dialog_BasicData_Browser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CF_BasicData_Browser_View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CF_Dialog_Btn_BasicData_OK;
        private System.Windows.Forms.Button CF_Dialog_Btn_BasicData_Cancel;
        private System.Windows.Forms.DataGridView CF_BasicData_Browser_View;
        private System.Windows.Forms.DataGridViewTextBoxColumn CF_Browser_BasicData_CardID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CF_Browser_BasicData_LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CF_Browser_BasicData_FirstName;
    }
}