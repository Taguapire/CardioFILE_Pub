using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CardioFILE
{
    public partial class CF_Form_Report_Historic : Form
    {
        public CF_Form_Report_Historic()
        {
            InitializeComponent();
        }

        private void CF_Form_Report_Historic_Load(object sender, EventArgs e)
        {
            this.CF_Report_Historic.RefreshReport();
        }

        public void Set_CF_Parameter(String pVariable, String pValor)
        {
            ReportParameter CF_ReportParameter = new();
            CF_ReportParameter.Name = pVariable;
            CF_ReportParameter.Values.Add(pValor);
            CF_Report_Historic.LocalReport.SetParameters(new ReportParameter[] { CF_ReportParameter });
        }
    }
}
