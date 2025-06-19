using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardioFILE_Pub
{
    public partial class CF_About : Form
    {
        public CF_About()
        {
            InitializeComponent();
        }

        private void CF_About_Btn_Register_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:cardiofile@pentalpha.net");
        }

        private void CF_About_lbl_OldMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:cardiofile@pentalpha.net");
        }

        private void CF_About_lbl_NewMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:cardiofile@pentalpha.net");
        }

        private void CF_About_lbl_SupportMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:cardiofile@pentalpha.net");
        }
    }
}
