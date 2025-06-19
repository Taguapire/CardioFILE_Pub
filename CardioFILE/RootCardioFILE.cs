namespace CardioFILE_Pub
{
    public class RootCardioFILE
    {
        public string Operation { get; set; }           // SELECT, INSERT, UPDATE, DELETE
        public string Dateinstall { get; set; }
        public string Datelastupdate { get; set; }
        public string Datepay { get; set; }
        public string Name { get; set; }
        public string Especiality { get; set; }
        public string MDid { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Originpay { get; set; }
        public string Transactionid { get; set; }
        public string Serial { get; set; }
        public string Keygen { get; set; }
        public string Databasepath { get; set; }
        public string Idioma { get; set; }
        public string Resultado { get; set; }
        public string Error_operation { get; set; }

        public RootCardioFILE()
        {
            Operation = "";           // SELECT, INSERT, UPDATE, DELETE
            Dateinstall = "";
            Datelastupdate = "";
            Datepay = "";
            Name = "";
            Especiality = "";
            MDid = "";
            Address1 = "";
            Address2 = "";
            City = "";
            Zipcode = "";
            Country = "";
            Email = "";
            Phone = "";
            Originpay = "";
            Transactionid = "";
            Serial = "";
            Keygen = "";
            Databasepath = "";
            Idioma = "";
            Resultado = "";
            Error_operation = "";
        }
    }
}
