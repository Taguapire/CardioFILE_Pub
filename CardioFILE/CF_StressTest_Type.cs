using System;

namespace CardioFILE
{
    public class CF_StressTest_Type
    {
        public string V_CardID { get; set; }
        public DateTime V_Date { get; set; }
        public string V_Protocol { get; set; }
        public string V_Stage { get; set; }
        public string V_Time { get; set; }
        public int V_CardiacFrec { get; set; }
        public int V_PercentMax { get; set; }
        public string V_Arrhyth { get; set; }
        public string V_BloodPress { get; set; }
        public string V_Type { get; set; }
        public string V_FunCap { get; set; }
        public string V_ReSusp { get; set; }
        public string V_Result { get; set; }
        public string V_DescSTT { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_StressTest_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_Protocol = "";
            V_Stage = "";
            V_Time = "";
            V_CardiacFrec = 0;
            V_PercentMax = 0;
            V_Arrhyth = "";
            V_BloodPress = "";
            V_Type = "";
            V_FunCap = "";
            V_ReSusp = "";
            V_Result = "";
            V_DescSTT = "";
            V_Error_Operation = "";
        }
    }
}
