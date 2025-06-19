using System;

//****************************************************************************************
//****************************************************************************************
// Manejo de Valores de Memoria de Basic Data
//****************************************************************************************
//****************************************************************************************
namespace CardioFILE_Pub
{
    public class CF_BasicData_Type
    {
        public string V_CardID { get; set; }
        public int V_Sex { get; set; }
        public int V_PhoneType { get; set; }
        public string V_State { get; set; }
        public string V_ReCons { get; set; }
        public string V_FirstName { get; set; }
        public string V_Phone { get; set; }
        public string V_Address { get; set; }
        public string V_ZipCode { get; set; }
        public DateTime V_BirthDate { get; set; }
        public string V_LastName { get; set; }
        public string V_SSNIDCI { get; set; }
        public string V_EMail { get; set; }
        public int V_BloodType { get; set; }
        public int V_BloodFactorRH { get; set; }
        public string V_Error_Operation { get; set; }

        public CF_BasicData_Type()
        {
            V_CardID = "";
            V_Sex = 0;
            V_PhoneType = 0;
            V_State = "";
            V_ReCons = "";
            V_FirstName = "";
            V_Phone = "";
            V_Address = "";
            V_ZipCode = "";
            V_BirthDate = new();
            V_LastName = "";
            V_SSNIDCI = "";
            V_EMail = "";
            V_BloodType = 0;
            V_BloodFactorRH = 0;
            V_Error_Operation = "";
        }
    }
}
