using System;

//****************************************************************************************
//****************************************************************************************
// Manejo de Valores de Memoria de Basic Data
//****************************************************************************************
//****************************************************************************************
namespace CardioFILE
{
    public class CF_PhysicalExamination_Type
    {
        public string V_CardID { get; set; }
        public DateTime V_Date { get; set; }
        public string V_FamiAnt { get; set; }
        public string V_PersAnt { get; set; }
        public string V_BloodPresMax { get; set; }
        public string V_BloodPresMin { get; set; }
        public string V_Pulse { get; set; }
        public float V_Height { get; set; }
        public float V_Weight { get; set; }
        public int V_Height_Item { get; set; }
        public int V_Weight_Item { get; set; }
        public float V_BMI { get; set; }
        public string V_Observ { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_PhysicalExamination_Type()
        {
            V_CardID = "";
            V_Date = new();
            V_FamiAnt = "";
            V_PersAnt = "";
            V_BloodPresMax = "";
            V_BloodPresMin = "";
            V_Pulse = "";
            V_Height = (float)0;
            V_Weight = (float)0;
            V_Height_Item = (int)0;
            V_Weight_Item = (int)0;
            V_BMI = (float)0;
            V_Observ = "";
            V_Error_Operation = "";
        }
    }
}
