namespace CardioFILE_Pub
{
    partial class CF_Form_Report_Historic
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
            this.CF_Report_Historic = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // CF_Report_Historic
            // 
            this.CF_Report_Historic.AutoSize = true;
            this.CF_Report_Historic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CF_Report_Historic.LocalReport.ReportEmbeddedResource = "CardioFILE.CardioFILEHist.rdlc";
            this.CF_Report_Historic.Location = new System.Drawing.Point(0, 0);
            this.CF_Report_Historic.Name = "CF_Report_Historic";
            this.CF_Report_Historic.ServerReport.BearerToken = null;
            this.CF_Report_Historic.Size = new System.Drawing.Size(898, 377);
            this.CF_Report_Historic.TabIndex = 0;
            // 
            // CF_Form_Report_Historic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 377);
            this.Controls.Add(this.CF_Report_Historic);
            this.Name = "CF_Form_Report_Historic";
            this.Text = "CardioFILE Historico";
            this.Load += new System.EventHandler(this.CF_Form_Report_Historic_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer CF_Report_Historic;

    }
}