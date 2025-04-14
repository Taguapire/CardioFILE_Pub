using System;

namespace CardioFILE
{
    public class CF_EchoDoppler_Type
    {
        public string V_CardID { get; set; }
        public DateTime V_Date { get; set; }
        public string V_Echo { get; set; }
        public string V_Doppler { get; set; }
        public string V_Laboratory { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_EchoDoppler_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_Echo = "";
            V_Doppler = "";
            V_Laboratory = "";
            V_Error_Operation = "";
        }
    }
}
