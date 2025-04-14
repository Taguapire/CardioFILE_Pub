using System;

namespace CardioFILE
{
    public class CF_Radiography_Type
    {
        public string V_CardID;
        public DateTime V_Date;
        public string V_Observ;
        public string V_Error_Operation;

        public CF_Radiography_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_Observ = "";
            V_Error_Operation = "";
        }
    }
}
