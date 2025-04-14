namespace CardioFILE
{
    partial class CF_Dialog_Other_Exams_Browser
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
            this.CF_Btn_OtherExam_Browser_Cancel = new System.Windows.Forms.Button();
            this.CF_GridView_OtherExam = new System.Windows.Forms.DataGridView();
            this.CF_Btn_OtherExam_Browser_Accept = new System.Windows.Forms.Button();
            this.DateOfConsult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.CF_GridView_OtherExam)).BeginInit();
            this.SuspendLayout();
            // 
            // CF_Btn_OtherExam_Browser_Cancel
            // 
            this.CF_Btn_OtherExam_Browser_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CF_Btn_OtherExam_Browser_Cancel.Location = new System.Drawing.Point(390, 226);
            this.CF_Btn_OtherExam_Browser_Cancel.Name = "CF_Btn_OtherExam_Browser_Cancel";
            this.CF_Btn_OtherExam_Browser_Cancel.Size = new System.Drawing.Size(75, 23);
            this.CF_Btn_OtherExam_Browser_Cancel.TabIndex = 1;
            this.CF_Btn_OtherExam_Browser_Cancel.Text = "Cancelar";
            this.CF_Btn_OtherExam_Browser_Cancel.UseVisualStyleBackColor = true;
            this.CF_Btn_OtherExam_Browser_Cancel.Click += new System.EventHandler(this.CF_Btn_OtherExam_Browser_Cancel_Click);
            // 
            // CF_GridView_OtherExam
            // 
            this.CF_GridView_OtherExam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CF_GridView_OtherExam.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DateOfConsult});
            this.CF_GridView_OtherExam.Location = new System.Drawing.Point(8, 10);
            this.CF_GridView_OtherExam.Name = "CF_GridView_OtherExam";
            this.CF_GridView_OtherExam.Size = new System.Drawing.Size(456, 208);
            this.CF_GridView_OtherExam.TabIndex = 2;
            this.CF_GridView_OtherExam.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CF_GridView_OtherExam_CellClick);
            // 
            // CF_Btn_OtherExam_Browser_Accept
            // 
            this.CF_Btn_OtherExam_Browser_Accept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CF_Btn_OtherExam_Browser_Accept.Location = new System.Drawing.Point(8, 228);
            this.CF_Btn_OtherExam_Browser_Accept.Name = "CF_Btn_OtherExam_Browser_Accept";
            this.CF_Btn_OtherExam_Browser_Accept.Size = new System.Drawing.Size(75, 23);
            this.CF_Btn_OtherExam_Browser_Accept.TabIndex = 3;
            this.CF_Btn_OtherExam_Browser_Accept.Text = "Aceptar";
            this.CF_Btn_OtherExam_Browser_Accept.UseVisualStyleBackColor = true;
            this.CF_Btn_OtherExam_Browser_Accept.Click += new System.EventHandler(this.CF_Btn_OtherExam_Browser_Accept_Click);
            // 
            // DateOfConsult
            // 
            this.DateOfConsult.HeaderText = "Fecha";
            this.DateOfConsult.Name = "DateOfConsult";
            this.DateOfConsult.ReadOnly = true;
            // 
            // CF_Dialog_Other_Exams_Browser
            // 
            this.AcceptButton = this.CF_Btn_OtherExam_Browser_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CF_Btn_OtherExam_Browser_Cancel;
            this.ClientSize = new System.Drawing.Size(474, 261);
            this.Controls.Add(this.CF_Btn_OtherExam_Browser_Accept);
            this.Controls.Add(this.CF_GridView_OtherExam);
            this.Controls.Add(this.CF_Btn_OtherExam_Browser_Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CF_Dialog_Other_Exams_Browser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Visor de Listados";
            ((System.ComponentModel.ISupportInitialize)(this.CF_GridView_OtherExam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CF_Btn_OtherExam_Browser_Cancel;
        private System.Windows.Forms.DataGridView CF_GridView_OtherExam;
        private System.Windows.Forms.Button CF_Btn_OtherExam_Browser_Accept;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateOfConsult;
    }
}