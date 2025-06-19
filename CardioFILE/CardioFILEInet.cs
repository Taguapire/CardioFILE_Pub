using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace CardioFILE_Pub
{
    class CardioFILEInet
    {
        private readonly List<RootCardioFILE> envioJsonCardioFILEArray = new();
        private readonly RootCardioFILE envioJsonCardioFile = new();

        private List<RootCardioFILE> respuestaJsonCardioFILEArray = new();
        private RootCardioFILE respuestaJsonCardioFile = new();
        private string LvrJsonCardioFILE;

        public string GetlvrJsonCardioFILE() { return LvrJsonCardioFILE; }
        public string GetlvrJsonOperation() { return respuestaJsonCardioFile.Operation; }
        public string GetlvrJsonName() { return respuestaJsonCardioFile.Name; }
        public string GetlvrJsonEMail() { return respuestaJsonCardioFile.Email; }
        public string GetlvrJsonTransactionID() { return respuestaJsonCardioFile.Transactionid; }
        public string GetlvrJsonSerial() { return respuestaJsonCardioFile.Serial; }
        public string GetlvrJsonOriginPay() { return respuestaJsonCardioFile.Originpay; }
        public string GetlvrJsonResultado() { return respuestaJsonCardioFile.Resultado; }

        public void SetlvrJsonCardioFILE(string pvarString) { LvrJsonCardioFILE = pvarString; }
        public void SetlvrJsonOperation(string pvarString) { envioJsonCardioFile.Operation = pvarString; }
        public void SetlvrJsonName(string pvarString) { envioJsonCardioFile.Name = pvarString; }
        public void SetlvrJsonEMail(string pvarString) { envioJsonCardioFile.Email = pvarString; }
        public void SetlvrJsonTransactionID(string pvarString) { envioJsonCardioFile.Transactionid = pvarString; }
        public void SetlvrJsonSerial(string pvarString) { envioJsonCardioFile.Serial = pvarString; }
        public void SetlvrJsonOriginPay(string pvarString) { envioJsonCardioFile.Originpay = pvarString; }
        public void SetlvrJsonResultado(string pvarString) { respuestaJsonCardioFile.Resultado = pvarString; }

        private String AccesoInternet(String pParametros)
        {
            return GET("https://finanven.ddns.net/Api/CardioFILE?JSON=" + pParametros);
        }

        private string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContinueTimeout = 60000;
            request.ReadWriteTimeout = 60000;
            request.Timeout = 60000;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new(responseStream, System.Text.Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (WebException)
            {
                // Tratar la excepcion de null
                return "ERROR";
            }
        }

        public String ValidaRegistry()
        {
            // Serializar
            envioJsonCardioFILEArray.Add(envioJsonCardioFile);
            // envioJsonCardioFILEArray.Add(new RootCardioFILE());

            LvrJsonCardioFILE = JsonConvert.SerializeObject(envioJsonCardioFILEArray);

            //Envio y respuesta de Internet
            LvrJsonCardioFILE = AccesoInternet(LvrJsonCardioFILE);
            if (LvrJsonCardioFILE != null)
            {
                // Deserializar
                try
                {
                    respuestaJsonCardioFILEArray = JsonConvert.DeserializeObject<List<RootCardioFILE>>(LvrJsonCardioFILE);
                    respuestaJsonCardioFile = respuestaJsonCardioFILEArray[0];
                }
                catch (JsonException)
                {
                    respuestaJsonCardioFile.Resultado = "INCOMPLETO";
                }
                catch (ArgumentNullException)
                {
                    respuestaJsonCardioFile.Resultado = "INCOMPLETO";
                }
                catch (IndexOutOfRangeException)
                {
                    respuestaJsonCardioFile.Resultado = "INCOMPLETO";
                }
            }
            else
            {
                respuestaJsonCardioFile.Resultado = "INCOMPLETO";
            }
            return respuestaJsonCardioFile.Resultado;
        }

        public CardioFILEInet()
        {
            LvrJsonCardioFILE = "";
        }
    }
}
