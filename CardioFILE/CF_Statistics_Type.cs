using System;

namespace CardioFILE
{
    public class CF_Statistics_Type
    {
        public DateTime V_PDate { get; set; }
        public string V_CardID { get; set; }
        public string V_Patient { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_Statistics_Type()
        {
            V_PDate = new();
            V_CardID = "";
            V_Patient = "";
            V_Error_Operation = "";
        }
    }
}
