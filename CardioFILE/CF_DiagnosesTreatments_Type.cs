using System;

namespace CardioFILE
{
    public class CF_DiagnosesTreatments_Type
    {
        public string V_CardID { get; set; }
        public DateTime V_Date { get; set; }
        public string V_Diagnoses { get; set; }
        public string V_Treatments { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_DiagnosesTreatments_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_Diagnoses = "";
            V_Treatments = "";
            V_Error_Operation = "";
        }
    }
}
