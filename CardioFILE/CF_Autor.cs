namespace CardioFILE_Pub
{
    public class CF_Autor
    {
        public string Software { private set; get; }
        public string Version { private set; get; } 
        public string Release { private set; get; }
        public string Autor { private set; get; }
        public string Derechos { private set; get; }
        public string Empresa1 { private set; get; }
        public string Empresa2 { private set; get; }
        public string Inicio { private set; get; }
        public string Final { private set; get; }
        public string Email1 { private set; get; }
        public string Email2 { private set; get; }
        public string SoftwareFull { private set; get; }

        public CF_Autor()
        {
            Software = "CardioFILE";
            Version = "1";
            Release = "00";
            Autor = "Luis Vásquez";
            Derechos = "(c) por Luis Vasquez";
            Empresa1 = "PENTALPHA EIRL";
            Empresa2 = "Luis Vásquez Consultores EIRL";
            Inicio = "1987";
            Final = "2021";
            Email1 = "ljvasquezr@outlook.com";
            Email2 = "sqlbiker@yahoo.com";
            SoftwareFull = Software + " " + Version + "." + Release;
        }
    }
}
