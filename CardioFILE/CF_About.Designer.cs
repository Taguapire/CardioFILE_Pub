namespace CardioFILE
{
    partial class CF_About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CF_About));
            this.CF_About_lbl_Derechos = new System.Windows.Forms.Label();
            this.CF_About_lbl_NewMail_Aviso = new System.Windows.Forms.Label();
            this.CF_About_lbl_NewMail = new System.Windows.Forms.LinkLabel();
            this.CF_About_Btn_OK = new System.Windows.Forms.Button();
            this.LOGO_PENTALPHA = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LOGO_PENTALPHA)).BeginInit();
            this.SuspendLayout();
            // 
            // CF_About_lbl_Derechos
            // 
            this.CF_About_lbl_Derechos.AutoSize = true;
            this.CF_About_lbl_Derechos.Location = new System.Drawing.Point(69, 19);
            this.CF_About_lbl_Derechos.Name = "CF_About_lbl_Derechos";
            this.CF_About_lbl_Derechos.Size = new System.Drawing.Size(338, 13);
            this.CF_About_lbl_Derechos.TabIndex = 0;
            this.CF_About_lbl_Derechos.Text = "CardioFILE  (c) por LUIS VASQUEZ/PENTALPHA E.I.R.L. 1987-2021";
            // 
            // CF_About_lbl_NewMail_Aviso
            // 
            this.CF_About_lbl_NewMail_Aviso.AutoSize = true;
            this.CF_About_lbl_NewMail_Aviso.Location = new System.Drawing.Point(83, 167);
            this.CF_About_lbl_NewMail_Aviso.Name = "CF_About_lbl_NewMail_Aviso";
            this.CF_About_lbl_NewMail_Aviso.Size = new System.Drawing.Size(123, 13);
            this.CF_About_lbl_NewMail_Aviso.TabIndex = 3;
            this.CF_About_lbl_NewMail_Aviso.Text = "Para registro contacte a:";
            // 
            // CF_About_lbl_NewMail
            // 
            this.CF_About_lbl_NewMail.AutoSize = true;
            this.CF_About_lbl_NewMail.Location = new System.Drawing.Point(266, 167);
            this.CF_About_lbl_NewMail.Name = "CF_About_lbl_NewMail";
            this.CF_About_lbl_NewMail.Size = new System.Drawing.Size(123, 13);
            this.CF_About_lbl_NewMail.TabIndex = 5;
            this.CF_About_lbl_NewMail.TabStop = true;
            this.CF_About_lbl_NewMail.Text = "ljvasquezr@outlook.com";
            this.CF_About_lbl_NewMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CF_About_lbl_NewMail_LinkClicked);
            // 
            // CF_About_Btn_OK
            // 
            this.CF_About_Btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CF_About_Btn_OK.Location = new System.Drawing.Point(170, 204);
            this.CF_About_Btn_OK.Name = "CF_About_Btn_OK";
            this.CF_About_Btn_OK.Size = new System.Drawing.Size(120, 23);
            this.CF_About_Btn_OK.TabIndex = 15;
            this.CF_About_Btn_OK.Text = "Aceptar";
            this.CF_About_Btn_OK.UseVisualStyleBackColor = true;
            // 
            // LOGO_PENTALPHA
            // 
            this.LOGO_PENTALPHA.BackColor = System.Drawing.SystemColors.Control;
            this.LOGO_PENTALPHA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LOGO_PENTALPHA.Image = ((System.Drawing.Image)(resources.GetObject("LOGO_PENTALPHA.Image")));
            this.LOGO_PENTALPHA.Location = new System.Drawing.Point(180, 59);
            this.LOGO_PENTALPHA.Name = "LOGO_PENTALPHA";
            this.LOGO_PENTALPHA.Size = new System.Drawing.Size(100, 91);
            this.LOGO_PENTALPHA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LOGO_PENTALPHA.TabIndex = 16;
            this.LOGO_PENTALPHA.TabStop = false;
            // 
            // CF_About
            // 
            this.AcceptButton = this.CF_About_Btn_OK;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 247);
            this.Controls.Add(this.LOGO_PENTALPHA);
            this.Controls.Add(this.CF_About_Btn_OK);
            this.Controls.Add(this.CF_About_lbl_NewMail);
            this.Controls.Add(this.CF_About_lbl_NewMail_Aviso);
            this.Controls.Add(this.CF_About_lbl_Derechos);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CF_About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acerca de CardioFILE para Windows";
            ((System.ComponentModel.ISupportInitialize)(this.LOGO_PENTALPHA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CF_About_lbl_Derechos;
        private System.Windows.Forms.Label CF_About_lbl_NewMail_Aviso;
        private System.Windows.Forms.LinkLabel CF_About_lbl_NewMail;
        private System.Windows.Forms.Button CF_About_Btn_OK;
        private System.Windows.Forms.PictureBox LOGO_PENTALPHA;
    }
}