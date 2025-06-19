using System;

namespace CardioFILE_Pub
{
    public class CF_Holter_Type
    {
        public string V_CardID { get; set; }
        public DateTime V_Date { get; set; }
        public string V_Observ { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_Holter_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_Observ = "";
            V_Error_Operation = "";
        }
    }
}
