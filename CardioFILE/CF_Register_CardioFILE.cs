using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Data.SQLite;

// using System.Text;

namespace CardioFILE
{
    public partial class CF_Register_CardioFILE : Form
    {
        public static CF_Autor CF_Identify = new();
        CF_Database_Class Cf_Database_Register;
        CardioFILEPrincipal localPrincipal;
        readonly CardioFILESecurity DressOfShadows = new();
  
        Boolean CF_Registro_Validado = false;

        Boolean Register_Insertar = false;

        public CF_Register_CardioFILE()
        {
            InitializeComponent();
            // Cf_Database_Register = new();
            // localPrincipal = new();
        }

        private void CF_RegCard_Validate_Click(object sender, EventArgs e)
        {
            string vlOperation;
            string vlName;
            string vlEMail;
            string vlTransactionID;
            string vlSerial;
            string vlOriginPay;
            string vlResultado;
            CardioFILEInet inetVerified = new();
            // Verificar que los campos siguientes esten llenos
            //
            // Verificación de Campos Llenos
            if (CF_RegCard_Name.Text == "" || 
                CF_RegCard_eMail.Text == "" ||
                CF_RegCard_TransactionID.Text == "" ||
                CF_RegCard_Serial.Text == "" ||
                CF_RegCard_OriginPayment.Text == "")
            {
                MessageBox.Show("Los campos Nombre, eMail, Serial #, ID de Transacción y Origen del Pago son obligatorios para validar", "Proceso de Validación", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            vlOperation = "VALIDAR";

            vlName = CF_RegCard_Name.Text;
            // eMail
            vlEMail = CF_RegCard_eMail.Text;
            // Transaction ID #
            vlTransactionID = CF_RegCard_TransactionID.Text;
            // Serial
            vlSerial = CF_RegCard_Serial.Text;
            // Origin Payment
            vlOriginPay = CF_RegCard_OriginPayment.Text;

            // Name
            //
            // Pasarlos como Cadena JSON a ValidaRegistry
            try
            {
                inetVerified.SetlvrJsonOperation(vlOperation);
                inetVerified.SetlvrJsonName(vlName);
                inetVerified.SetlvrJsonEMail(vlEMail);
                inetVerified.SetlvrJsonTransactionID(vlTransactionID);
                inetVerified.SetlvrJsonSerial(vlSerial);
                inetVerified.SetlvrJsonOriginPay(vlOriginPay);
                vlResultado = inetVerified.ValidaRegistry();

                if (vlResultado == "REGISTRADO")
                {
                    localPrincipal.Text = CF_Identify.SoftwareFull + " Registrado: " + vlName;
                    localPrincipal.CF_StatusReg = CF_Identify.SoftwareFull +" Registrado: " + vlName;
                    CF_Registro_Validado = true;
                    MessageBox.Show("La validación es correcta", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // MessageBox.Show("Registered Succesfull!", "Validation");
                    CF_RegCard_Btn_OK.Enabled = true;
                }
                else if (vlResultado == "NOREGISTRADO")
                {
                    MessageBox.Show("Existen inconsistencias en la validación debe contactar a soporte en contacto@pentalpha.net", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    localPrincipal.Text = CF_Identify.SoftwareFull + " Unregistered";
                    localPrincipal.CF_StatusReg = CF_Identify.SoftwareFull + " Unregistered";
                    CF_RegCard_Btn_OK.Enabled = false;
                }
                else
                {
                    localPrincipal.Text = CF_Identify.SoftwareFull + " NO REGISTRADO";
                    localPrincipal.CF_StatusReg = CF_Identify.SoftwareFull + "NOREGISTRADO";
                    CF_RegCard_Btn_OK.Enabled = false;
                    MessageBox.Show("No existe registro de este usuario, consulte a soporte en contacto@pentalpha.net", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.NullReferenceException)
            {
                CF_RegCard_Btn_OK.Enabled = false;
                MessageBox.Show("Error de conección a la Base de Datos, debe contactar a soporte en contacto@pentalpha.net", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void Set_Database_Register (CF_Database_Class pCf_Database_Register)
        {
            Cf_Database_Register = pCf_Database_Register;
        }
       
        private void CF_RegCard_Btn_OK_Click(object sender, EventArgs e)
        {
            Cf_Database_Register.CF_User_Mem.V_Name = CF_RegCard_Name.Text;
            Cf_Database_Register.CF_User_Mem.V_Especiality = CF_RegCard_Especiality.Text;
            Cf_Database_Register.CF_User_Mem.V_MDID = CF_RegCard_MDID.Text;
            Cf_Database_Register.CF_User_Mem.V_Address1 = CF_RegCard_Address1.Text;
            Cf_Database_Register.CF_User_Mem.V_Address2 = CF_RegCard_Address2.Text;
            Cf_Database_Register.CF_User_Mem.V_City = CF_RegCard_City.Text;
            Cf_Database_Register.CF_User_Mem.V_ZipCode = CF_RegCard_ZipCode.Text;
            Cf_Database_Register.CF_User_Mem.V_Country = CF_RegCard_Country.Text;
            Cf_Database_Register.CF_User_Mem.V_eMail = CF_RegCard_eMail.Text;
            Cf_Database_Register.CF_User_Mem.V_Phone = CF_RegCard_Phone.Text;
            Cf_Database_Register.CF_User_Mem.V_TransactionID = CF_RegCard_TransactionID.Text;
            Cf_Database_Register.CF_User_Mem.V_Serial = CF_RegCard_Serial.Text;
            Cf_Database_Register.CF_User_Mem.V_OriginPay = CF_RegCard_OriginPayment.Text;
            Cf_Database_Register.CF_User_Mem.V_Idioma = "Spanish";

            if (CF_Registro_Validado == true)
            {
                DressOfShadows.BlowFish(localPrincipal.ByteToHex(localPrincipal.CF_StatusRegx));
                string Global_Compare = DressOfShadows.Encrypt_CBC(Cf_Database_Register.CF_User_Mem.V_Name + Cf_Database_Register.CF_User_Mem.V_eMail + Cf_Database_Register.CF_User_Mem.V_TransactionID + Cf_Database_Register.CF_User_Mem.V_Serial + Cf_Database_Register.CF_User_Mem.V_OriginPay);
                Cf_Database_Register.CF_User_Mem.V_KeyGen = Global_Compare;
            }
            else
            {
                Cf_Database_Register.CF_User_Mem.V_KeyGen = "";
            }
            // MessageBox.Show(localPrincipal.ByteToHex(localPrincipal.CF_StatusRegx));
            if (Register_Insertar == true)
            {
                if (!(Cf_Database_Register.CF_Insert_User()))
                {
                    MessageBox.Show(Cf_Database_Register.CF_User_Mem.V_Error_Operation, "Insert User Error");
                }
            }
            else
            {
                if (!(Cf_Database_Register.CF_Modify_User()))
                {
                    MessageBox.Show(Cf_Database_Register.CF_User_Mem.V_Error_Operation, "Modify User Error");
                }
            }
        }

        private void CF_RegCard_TransactionID_TextChanged(object sender, EventArgs e)
        {

        }

        private void CF_RegCard_Serial_TextChanged(object sender, EventArgs e)
        {

        }

        private void CF_Register_CardioFILE_Load(object sender, EventArgs e)
        {
            if (Cf_Database_Register.CF_Select_User() == true)
            {
                Cf_Database_Register.CF_GetDb_User(Cf_Database_Register.CF_DReader_User);
                CF_RegCard_Name.Text = Cf_Database_Register.CF_User_Mem.V_Name;
                CF_RegCard_Especiality.Text = Cf_Database_Register.CF_User_Mem.V_Especiality;
                CF_RegCard_MDID.Text = Cf_Database_Register.CF_User_Mem.V_MDID;
                CF_RegCard_Address1.Text = Cf_Database_Register.CF_User_Mem.V_Address1;
                CF_RegCard_Address2.Text = Cf_Database_Register.CF_User_Mem.V_Address2;
                CF_RegCard_City.Text = Cf_Database_Register.CF_User_Mem.V_City;
                CF_RegCard_ZipCode.Text = Cf_Database_Register.CF_User_Mem.V_ZipCode;
                CF_RegCard_Country.Text = Cf_Database_Register.CF_User_Mem.V_Country;
                CF_RegCard_eMail.Text = Cf_Database_Register.CF_User_Mem.V_eMail;
                CF_RegCard_Phone.Text = Cf_Database_Register.CF_User_Mem.V_Phone;
                CF_RegCard_TransactionID.Text = Cf_Database_Register.CF_User_Mem.V_TransactionID;
                CF_RegCard_Serial.Text = Cf_Database_Register.CF_User_Mem.V_Serial;
                CF_RegCard_OriginPayment.Text = Cf_Database_Register.CF_User_Mem.V_OriginPay;
                // CF_RegCard_Idioma.Text = Cf_Database_Register.CF_User_Mem.V_Idioma();
                Register_Insertar = false;
            }
            else
            {
                CF_RegCard_Name.Text = "";
                CF_RegCard_Especiality.Text = "";
                CF_RegCard_MDID.Text = "";
                CF_RegCard_Address1.Text = "";
                CF_RegCard_Address2.Text = "";
                CF_RegCard_City.Text = "";
                CF_RegCard_ZipCode.Text = "";
                CF_RegCard_Country.Text = "";
                CF_RegCard_eMail.Text = "";
                CF_RegCard_Phone.Text = "";
                CF_RegCard_TransactionID.Text = "";
                CF_RegCard_Serial.Text = "";
                CF_RegCard_OriginPayment.Text = "";
                // CF_RegCard_Idioma.Text = "";
                Register_Insertar = true;
            }
        }

        public void SetLocalPrincipal(CardioFILEPrincipal vlocalPrincipal)
        {
           localPrincipal = vlocalPrincipal;;
        }
    }
}
