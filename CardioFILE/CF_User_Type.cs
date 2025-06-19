using System;

namespace CardioFILE_Pub
{
    public class CF_User_Type
    {
        public string V_Operacion { get; set; }
        public DateTime V_DateInstall { get; set; }
        public DateTime V_DateLastUpdate { get; set; }
        public DateTime V_DatePay { get; set; }
        public string V_Name { get; set; }
        public string V_Especiality { get; set; }
        public string V_MDID { get; set; }
        public string V_Address1 { get; set; }
        public string V_Address2 { get; set; }
        public string V_City { get; set; }
        public string V_ZipCode { get; set; }
        public string V_Country { get; set; }
        public string V_eMail { get; set; }
        public string V_Phone { get; set; }
        public string V_OriginPay { get; set; }
        public string V_TransactionID { get; set; }
        public string V_Serial { get; set; }
        public string V_KeyGen { get; set; }
        public string V_DatabasePath { get; set; }
        public string V_Error_Operation { get; set; }
        public string V_Resultado { get; set; }
        public string V_Idioma { get; set; }

        public CF_User_Type()
        {
            V_Operacion = "";
            V_DateInstall = new();
            V_DateLastUpdate = new();
            V_DatePay = new();
            V_Name = "";
            V_Especiality = "";
            V_MDID = "";
            V_Address1 = "";
            V_Address2 = "";
            V_City = "";
            V_ZipCode = "";
            V_Country = "";
            V_eMail = "";
            V_Phone = "";
            V_OriginPay = "";
            V_TransactionID = "";
            V_Serial = "";
            V_KeyGen = "";
            V_DatabasePath = "";
            V_Error_Operation = "";
            V_Resultado = "";
            V_Idioma = "";
        }
    }
}
