using System;
using System.Windows.Forms;
using System.Data.SQLite;


namespace CardioFILE
{
    public partial class CF_Dialog_BasicData_Browser : Form
    {
        SQLiteConnection dfCF_Connection;
        SQLiteCommand dfCF_Cmd_BasicData;
        SQLiteDataReader dfCF_DReader_BasicData;
        int TipoDeSalida = 0;
        string dfCf_SQL_SELECT = "";

        DataGridViewCell clickedCell;

        public CF_Dialog_BasicData_Browser()
        {
            InitializeComponent();
            dfCF_Connection = new();
            dfCF_Cmd_BasicData = new();
            dfCf_SQL_SELECT = "";
            clickedCell = new MiDataGridViewCell();
        }

        public void CF_Recuperar_Listado_Pacientes()
        {
            // Operacion SQL
            
            String[] Registro = new String[3];

            dfCf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,LASTNAME,FIRSTNAME FROM CFBASDATDB";

            dfCF_Cmd_BasicData = dfCF_Connection.CreateCommand();
            dfCF_Cmd_BasicData.CommandText = dfCf_SQL_SELECT;

            dfCF_DReader_BasicData = dfCF_Cmd_BasicData.ExecuteReader();

            // Llenar Grilla
            while (dfCF_DReader_BasicData.Read())
            {
                Registro[0] = dfCF_DReader_BasicData.GetString(dfCF_DReader_BasicData.GetOrdinal("CARDNUMBER"));
                Registro[1] = dfCF_DReader_BasicData.GetString(dfCF_DReader_BasicData.GetOrdinal("LASTNAME"));
                Registro[2] = dfCF_DReader_BasicData.GetString(dfCF_DReader_BasicData.GetOrdinal("FIRSTNAME"));
                CF_BasicData_Browser_View.Rows.Add(Registro);
            }

        }
        
        private void CF_Dialog_Btn_BasicData_OK_Click(object sender, EventArgs e)
        {
            TipoDeSalida = 1;
        }

        public void SetDbConection(SQLiteConnection pCF_Connection)
        {
            dfCF_Connection = pCF_Connection;
        }

        public String GetCardIDSeleccionado() {
            String rCardID;
            try {
                rCardID = (String) clickedCell.Value;
            }
            catch (System.NullReferenceException) {
                rCardID = "";
            }
            return rCardID;
        }

        private void CF_Dialog_Btn_BasicData_Cancel_Click(object sender, EventArgs e)
        {
            TipoDeSalida = 2;
        }

        private void CF_BasicData_Browser_View_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                clickedCell = CF_BasicData_Browser_View.Rows[e.RowIndex].Cells[0];
            }
        }

        public int GetTipoDeSalida()
        {
            return TipoDeSalida;
        }

        private void CF_Dialog_BasicData_Browser_Load(object sender, EventArgs e)
        {

        }

        class MiDataGridViewCell : DataGridViewCell
        {
        }
    }
}
