namespace CardioFILE_Pub
{
    partial class CF_Form_Report_Recipe
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
            this.CF_Report_Recipe = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // CF_Report_Recipe
            // 
            this.CF_Report_Recipe.AutoScroll = true;
            this.CF_Report_Recipe.AutoSize = true;
            this.CF_Report_Recipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CF_Report_Recipe.LocalReport.ReportEmbeddedResource = "CardioFILE.CardioFILERec.rdlc";
            this.CF_Report_Recipe.LocalReport.ReportPath = "";
            this.CF_Report_Recipe.Location = new System.Drawing.Point(0, 0);
            this.CF_Report_Recipe.Name = "CF_Report_Recipe";
            this.CF_Report_Recipe.ServerReport.BearerToken = null;
            this.CF_Report_Recipe.Size = new System.Drawing.Size(848, 359);
            this.CF_Report_Recipe.TabIndex = 0;
            this.CF_Report_Recipe.Load += new System.EventHandler(this.CF_Report_Recipe_Load);
            // 
            // CF_Form_Report_Recipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 359);
            this.Controls.Add(this.CF_Report_Recipe);
            this.Name = "CF_Form_Report_Recipe";
            this.Text = "CardioFILE Receta";
            this.Load += new System.EventHandler(this.CF_Form_Report_Recipe_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer CF_Report_Recipe;


    }
}