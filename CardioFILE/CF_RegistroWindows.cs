using Microsoft.Win32;

namespace CardioFILE
{
    class CF_RegistroWindows
    {
        public string ObtenerDirectorio()
        {
            RegistryKey RkCurrentUser;
            RegistryKey RkSoftware;
            RegistryKey RkPentalpha;
            RegistryKey RkCardioFILE;
            string ValorDirectorio;
            // RegistryKey RkDirectorio;
            RkCurrentUser = Registry.CurrentUser;
            RkSoftware = RkCurrentUser.OpenSubKey("SOFTWARE");
            if (RkSoftware == null)
                return "";
            RkPentalpha = RkSoftware.OpenSubKey("PENTALPHA");
            if (RkPentalpha == null)
                return "";
            RkCardioFILE = RkPentalpha.OpenSubKey("CardioFILE");
            if (RkCardioFILE == null)
                return "";
            try
            {
                ValorDirectorio = (string)RkCardioFILE.GetValue("DIRECTORY");
            }
            catch (System.NullReferenceException)
            {
                return "";
            }
            RkCardioFILE.Close();
            RkPentalpha.Close();
            RkSoftware.Close();
            RkCurrentUser.Close();
            return ValorDirectorio;
        }

        public void EscribirDirectorio(string LocalDirectorio)
        {
            RegistryKey RkCurrentUser;
            RegistryKey RkSoftware;
            RegistryKey RkPentalpha;
            RegistryKey RkCardioFILE;
            // RegistryKey RkDirectorio;
            RkCurrentUser = Registry.CurrentUser;
            RkSoftware = RkCurrentUser.OpenSubKey("SOFTWARE",true);
            RkPentalpha = RkSoftware.OpenSubKey("PENTALPHA",true);
            RkPentalpha ??= RkSoftware.CreateSubKey("PENTALPHA");
            RkCardioFILE = RkPentalpha.OpenSubKey("CardioFILE",true);
            RkCardioFILE ??= RkPentalpha.CreateSubKey("CardioFILE");
            RkCardioFILE.SetValue("DIRECTORY", LocalDirectorio);
            RkCardioFILE.Close();
            RkPentalpha.Close();
            RkSoftware.Close();
            RkCurrentUser.Close();
        }
    }
}
