using System;
using System.Windows.Forms;
using System.Data.SQLite;


namespace CardioFILE
{

    public partial class CF_Dialog_Other_Exams_Browser : Form
    {
        private SQLiteConnection dfCF_Connection;
        private SQLiteCommand dfCF_Cmd_OtherExams;
        private SQLiteDataReader dfCF_DReader_OtherExams;
        private int TipoDeSalida = 0;
        private string dfCf_SQL_SELECT;

        DataGridViewCell clickedCell;

        public CF_Dialog_Other_Exams_Browser()
        {
            InitializeComponent();
            dfCF_Connection = new();
            dfCF_Cmd_OtherExams = new();
            dfCf_SQL_SELECT = "";
            clickedCell = new MiDataGridViewCell();
        }

        public void CF_Recuperar_Listado_Examenes()
        {
            // Operacion SQL

            String[] Registro = new String[1];

            dfCF_Cmd_OtherExams = dfCF_Connection.CreateCommand();
            dfCF_Cmd_OtherExams.CommandText = dfCf_SQL_SELECT;

            dfCF_DReader_OtherExams = dfCF_Cmd_OtherExams.ExecuteReader();

            // Llenar Grilla
            while (dfCF_DReader_OtherExams.Read())
            {
                Registro[0] = DateTime.FromBinary(dfCF_DReader_OtherExams.GetInt64(dfCF_DReader_OtherExams.GetOrdinal("PDATE"))).ToShortDateString();;
                CF_GridView_OtherExam.Rows.Add(Registro);
            }

        }

        public void SetDbConection(SQLiteConnection pCF_Connection)
        {
            dfCF_Connection = pCF_Connection;
        }

        public void SetSQL(String pSQL)
        {
            dfCf_SQL_SELECT = pSQL;
        }

        public int GetTipoDeSalida()
        {
            return TipoDeSalida;
        }

        private void CF_Btn_OtherExam_Browser_OK_Click(object sender, EventArgs e)
        {
            TipoDeSalida = 1;
        }

        private void CF_Btn_OtherExam_Browser_Cancel_Click(object sender, EventArgs e)
        {
            TipoDeSalida = 2;
        }

        private void CF_GridView_OtherExam_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                clickedCell = CF_GridView_OtherExam.Rows[e.RowIndex].Cells[0];
            }
        }

        public String GetDateSeleccionado()
        {
            String rDate;
            if (clickedCell != null) {
                try
                {
                    rDate = (String)clickedCell.Value;
                }
                catch (System.NullReferenceException)
                {
                    rDate = "";
                }
                return rDate;
            }
            else return "";
        }

        private void CF_Btn_OtherExam_Browser_Accept_Click(object sender, EventArgs e)
        {
            TipoDeSalida = 1;
        }

        class MiDataGridViewCell : DataGridViewCell
        {
        }
    }
}
