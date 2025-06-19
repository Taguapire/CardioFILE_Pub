using System;
using System.IO;
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
    public partial class CF_ConfigureDatabase : Form
    {
        public CF_ConfigureDatabase()
        {
            InitializeComponent();
        }

        private void CF_ConfigDatabase_BAceptar_Click(object sender, EventArgs e)
        {
            CF_RegistroWindows RegWinCardioFILE = new();
            if (CF_ConfigDatabase_TDirActual.Text != CF_ConfigDatabase_TDirNuevo.Text)
            {
                RegWinCardioFILE.EscribirDirectorio(CF_ConfigDatabase_TDirNuevo.Text);
                File.Copy(CF_ConfigDatabase_TDirActual.Text + @"\CardioFILE.db", CF_ConfigDatabase_TDirNuevo.Text + @"\CardioFILE.db", true);
            }
            else
            {
                MessageBox.Show("Esta operación no puede ejecutarse porque los directorios son iguales!", "Error de Operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }

        private void CF_ConfigDatabase_BCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void Set_Database_Directory(string PDirectorio_Actual)
        {
            CF_ConfigDatabase_TDirActual.Text = PDirectorio_Actual;
            CF_ConfigDatabase_TDirNuevo.Text = PDirectorio_Actual;
        }

        private void CF_ConfigDatabase_BDirectorio_Click(object sender, EventArgs e)
        {
            CF_ConfigDatabase_DlgDirectorio.SelectedPath = CF_ConfigDatabase_TDirActual.Text;
            CF_ConfigDatabase_DlgDirectorio.ShowDialog();
            CF_ConfigDatabase_TDirNuevo.Text = CF_ConfigDatabase_DlgDirectorio.SelectedPath;
        }
    }
}
