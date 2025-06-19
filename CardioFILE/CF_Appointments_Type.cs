using System;

namespace CardioFILE_Pub
{
    public class CF_Appointments_Type
    {
        public string V_CardID { get; set; }
        public string V_Patient { get; set; }
        public DateTime V_DateAppoint { get; set; }
        public DateTime V_TimeAppoint { get; set; }
        public DateTime V_LocDate { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_Appointments_Type()
        {
            V_CardID = "";
            V_Patient = "";
            V_DateAppoint = new();
            V_TimeAppoint = new();
            V_LocDate = new();
            V_Error_Operation = "";
        }
    }
}
