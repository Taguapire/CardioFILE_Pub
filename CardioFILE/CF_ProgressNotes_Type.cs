using System;

namespace CardioFILE
{
    public class CF_ProgressNotes_Type
    {
        public string V_CardID { get; set; }
        public DateTime V_Date { get; set; }
        public string V_Observ { get; set; }
        public DateTime V_NextDateApp { get; set; }
        public DateTime V_NextTimeApp { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_ProgressNotes_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_Observ = "";
            V_NextDateApp = new();
            V_NextTimeApp = new();
            V_Error_Operation = "";
        }
    }
}
