using System;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CardioFILE_Pub
{
    public partial class CardioFILEPrincipal : Form
    {
        public static CF_Autor CF_Identify = new();
        public CF_Database_Class Cf_Database_Oper = new();
        public string CF_StatusReg = CF_Identify.SoftwareFull;
        public Byte[] CF_StatusRegx = new Byte[8];
        readonly CardioFILESecurity DressOfShadows = new();
        readonly CF_RegistroWindows RegWinCardioFILE = new();
        readonly string CF_DirectorioObtenido;

        // Variables Globales de Usuario
        readonly string Global_User_Name;
        readonly string Global_User_eMail;
        readonly string Global_User_TransactionID;
        readonly string Global_User_Serial;
        readonly string Global_User_OriginPay;
        readonly string Global_User_KeyGen;

        public CardioFILEPrincipal()
        {
            CF_StatusRegx[0] = 0xB4;
            InitializeComponent();
            CF_DirectorioObtenido = RegWinCardioFILE.ObtenerDirectorio();
            if (CF_DirectorioObtenido == null || CF_DirectorioObtenido == "")
            {
                RegWinCardioFILE.EscribirDirectorio(Application.LocalUserAppDataPath);
                CF_DirectorioObtenido = RegWinCardioFILE.ObtenerDirectorio();
            }
            CF_StatusRegx[1] = 0x49;
            Cf_Database_Oper.CF_OpenDatabase(CF_DirectorioObtenido);
            CF_StatusRegx[2] = 0xFB;
            Cf_BasicData_SC_BDate.Text = Cf_BasicData_SC_Calendar.Value.ToString("d");
            CF_StatusRegx[3] = 0xAB;
            Cf_PhyExam_SC_Date.Text = Cf_PhyExam_SC_Calendar.Value.ToString("d");
            CF_StatusRegx[4] = 0xC8;
            Cf_Radiography_SC_Date.Text = Cf_Radiography_SC_Calendar.Value.ToString("d");
            CF_StatusRegx[5] = 0xEB;
            Cf_EKGR_SC_Date.Text = Cf_EKGR_SC_Calendar.Value.ToString("d");
            Cf_EchoDoppler_SC_Date.Text = Cf_EchoDoppler_SC_Calendar.Value.ToString("d");
            CF_StatusRegx[6] = 0x06;
            Cf_Holter_SC_Date.Text = Cf_Holter_SC_Calendar.Value.ToString("d");
            Cf_StressTest_SC_Date.Text = Cf_StressTest_SC_Calendar.Value.ToString("d");
            Cf_CardiacCath_SC_Date.Text = Cf_CardiacCath_SC_Calendar.Value.ToString("d");
            Cf_Surgery_SC_Date.Text = Cf_Surgery_SC_Calendar.Value.ToString("d");
            Cf_DiagTreatment_SC_Date.Text = Cf_DiagTreatment_SC_Calendar.Value.ToString("d");
            Cf_ProgNotes_SC_Date.Text = Cf_ProgNotes_SC_Calendar.Value.ToString("d");
            CF_StatusRegx[7] = 0xD4;
            // Verificar estado de Registro
            // Buscar si existe Registro
            // Limpiar registro de usuario
            Global_User_Name = "";
            Global_User_eMail = "";
            Global_User_TransactionID = "";
            Global_User_Serial = "";
            Global_User_OriginPay = "";
            Global_User_KeyGen = "";
            if (Cf_Database_Oper.CF_Select_User() == true)
            {
                Cf_Database_Oper.CF_GetDb_User(Cf_Database_Oper.CF_DReader_User);
                Global_User_Name = Cf_Database_Oper.CF_User_Mem.V_Name;
                Global_User_eMail = Cf_Database_Oper.CF_User_Mem.V_eMail;
                Global_User_TransactionID = Cf_Database_Oper.CF_User_Mem.V_TransactionID;
                Global_User_Serial = Cf_Database_Oper.CF_User_Mem.V_Serial;
                Global_User_OriginPay = Cf_Database_Oper.CF_User_Mem.V_OriginPay;
                Global_User_KeyGen = Cf_Database_Oper.CF_User_Mem.V_KeyGen;

                // Prepara Blowfish con Clave
                DressOfShadows.BlowFish(ByteToHex(CF_StatusRegx));

                // Encripta lo almacenado en Base de Datos validado en Linea
                string Global_Compare = DressOfShadows.Encrypt_CBC(Global_User_Name + Global_User_eMail + Global_User_TransactionID + Global_User_Serial + Global_User_OriginPay);

                // Verificación de Checksum mediante Blowfish

                // Desencripta lo dinamico almacenado en Base de Datos
                string CF_Dinamico = DressOfShadows.Decrypt_CBC(Global_Compare);

                // Desencripta la clave almacenada en Base de Datos cuando se validó en Linea
                string CF_Estatico = DressOfShadows.Decrypt_CBC(Global_User_KeyGen);

                if (CF_Dinamico == CF_Estatico)
                {
                    CF_StatusReg = CF_Identify.SoftwareFull + " Registrado: " + Global_User_Name;
                }
                else
                {
                    MessageBox.Show("El registro del usuario: " + Global_User_Name + ", tiene diferencias con el original." + " Escriba a " + CF_Identify.Email2 + " para asistencia y correción.", "Verificación de Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CF_StatusReg = CF_Identify.SoftwareFull + " Registro con observación. Escriba a:" + CF_Identify.Email2;
                    // Blanquear Valores Globales del Usuario para definir como No Registrado
                }
            }
            else
            {
                CF_StatusReg = CF_Identify.SoftwareFull + " No Registrado";
            }
            this.Text = CF_StatusReg;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON BASIC DATA
        //**************************************************************************************************
        //**************************************************************************************************

        // Manejo de Find de ToolsBar Basic Data
        private void CF_Btn_Basic_Data_Find_Click(object sender, EventArgs e)
        {
            if (CF_Btn_Basic_Data_Find.Text == "Buscar")
            {
                CF_Btn_Basic_Data_Find.Enabled = true;
                CF_Btn_Basic_Data_Find.Text = "Aceptar";
                CF_Btn_Basic_Data_New.Enabled = false;
                CF_Btn_Basic_Data_Modify.Enabled = false;
                CF_Btn_Basic_Data_Delete.Enabled = false;
                CF_Btn_Basic_Data_Print.Enabled = false;
                CF_Btn_Basic_Data_Cancel.Enabled = true;
                CF_Panel_Btn_OtherExams.Enabled = false;
                CF_TP_Exams_Others.Enabled = false;
            }
            else if (CF_Btn_Basic_Data_Find.Text == "Aceptar")
            {
                if (Cf_Database_Oper.CF_Select_BasicData(Cf_BasicData_SC_CardID.Text))
                {
                    CF_Memory_To_Screen_Basic_Data();

                    // Procesar Examen Fisico
                    if (Cf_Database_Oper.CF_Select_PhysicalExamination(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_PhysicalExamination();
                    else
                        CF_Limpiar_PhysicalExamination();

                    // Procesar Radiography
                    if (Cf_Database_Oper.CF_Select_Radiography(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_Radiography();
                    else
                        CF_Limpiar_Radiography();

                    // Procesar EKGR
                    if (Cf_Database_Oper.CF_Select_EKGR(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_EKGR();
                    else
                        CF_Limpiar_EKGR();

                    // Procesar Echo/Doppler
                    if (Cf_Database_Oper.CF_Select_EchoDoppler(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_EchoDoppler();
                    else
                        CF_Limpiar_EchoDoppler();

                    // Procesar Holter
                    if (Cf_Database_Oper.CF_Select_Holter(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_Holter();
                    else
                        CF_Limpiar_Holter();

                    // Procesar StressTest
                    if (Cf_Database_Oper.CF_Select_StressTest(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_StressTest();
                    else
                        CF_Limpiar_StressTest();

                    // Procesar CardiacCath
                    if (Cf_Database_Oper.CF_Select_CardiacCath(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_CardiacCath();
                    else
                        CF_Limpiar_CardiacCath();

                    // Procesar Surgery
                    if (Cf_Database_Oper.CF_Select_Surgery(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_Surgery();
                    else
                        CF_Limpiar_Surgery();

                    // Procesar Diagnoses and Treatments
                    if (Cf_Database_Oper.CF_Select_DiagnosesTreatments(Cf_BasicData_SC_CardID.Text))
                        CF_Memory_To_Screen_DiagnosesTreatment();
                    else
                        CF_Limpiar_DiagnosesTreatment();

                    // Procesar Progress Notes
                    if (Cf_Database_Oper.CF_Select_ProgressNotes(Cf_BasicData_SC_CardID.Text))
                    {
                        CF_Memory_To_Screen_ProgressNotes();
                        CF_LlenadoGrilla_ProgressNotes();
                    }
                    else
                        CF_Limpiar_ProgressNotes();

                    // Activación de Opciones de Menus
                    CF_Btn_Basic_Data_Find.Enabled = true;
                    CF_Btn_Basic_Data_Find.Text = "Buscar";
                    CF_Btn_Basic_Data_New.Enabled = true;
                    CF_Btn_Basic_Data_Modify.Enabled = true;
                    CF_Btn_Basic_Data_Delete.Enabled = true;
                    CF_Btn_Basic_Data_Print.Enabled = true;
                    CF_Btn_Basic_Data_Cancel.Enabled = false;
                    CF_Panel_Btn_OtherExams.Enabled = true;
                    CF_TP_Exams_Others.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Ficha Medica no existe", "Error en Datos Basicos");
                }
            }
        }

        // Manejo de Cancel de ToolsBar de Basic Data
        private void CF_Btn_Basic_Data_Cancel_Click(object sender, EventArgs e)
        {
            Cf_BasicData_SC_CardID.Enabled = true;
            CF_Btn_Basic_Data_Find.Enabled = true;
            CF_Btn_Basic_Data_Find.Text = "Buscar";
            CF_Btn_Basic_Data_New.Enabled = true;
            CF_Btn_Basic_Data_New.Text = "Nuevo";
            CF_Btn_Basic_Data_Modify.Text = "Modificar";
            CF_Btn_Basic_Data_Modify.Enabled = true;
            CF_Btn_Basic_Data_Delete.Text = "Borrar";
            CF_Btn_Basic_Data_Delete.Enabled = true;
            CF_Btn_Basic_Data_Print.Enabled = true;
            CF_Btn_Basic_Data_Cancel.Enabled = false;
            CF_Panel_Btn_OtherExams.Enabled = true;
            CF_TP_Exams_Others.Enabled = true;
        }

        // Manejo de New de ToolsBar de Basic Data
        private void CF_Btn_Basic_Data_New_Click(object sender, EventArgs e)
        {
            if (CF_Btn_Basic_Data_New.Text == "Nuevo")
            {
                CF_Btn_Basic_Data_Find.Enabled = false;
                CF_Btn_Basic_Data_New.Text = "Aceptar";
                CF_Btn_Basic_Data_New.Enabled = true;
                CF_Btn_Basic_Data_Modify.Enabled = false;
                CF_Btn_Basic_Data_Delete.Enabled = false;
                CF_Btn_Basic_Data_Print.Enabled = false;
                CF_Btn_Basic_Data_Cancel.Enabled = true;
                CF_Panel_Btn_OtherExams.Enabled = false;
                CF_TP_Exams_Others.Enabled = false;
                CF_Limpiar_Basic_Data();
            }
            else if (CF_Btn_Basic_Data_New.Text == "Aceptar")
            {
                CF_Screen_To_Memory_Basic_Data();
                if (Cf_Database_Oper.CF_Insert_BasicData())
                {
                    CF_Btn_Basic_Data_Find.Enabled = true;
                    CF_Btn_Basic_Data_New.Text = "Nuevo";
                    CF_Btn_Basic_Data_New.Enabled = true;
                    CF_Btn_Basic_Data_Modify.Enabled = true;
                    CF_Btn_Basic_Data_Delete.Enabled = true;
                    CF_Btn_Basic_Data_Print.Enabled = true;
                    CF_Btn_Basic_Data_Cancel.Enabled = false;
                    CF_Panel_Btn_OtherExams.Enabled = true;
                    CF_TP_Exams_Others.Enabled = true;
                }
                else
                {
                    MessageBox.Show(Cf_Database_Oper.CF_BasicData_Mem.V_Error_Operation, "Error en Datos Basicos");
                }
            }
        }

        // Manejo de Cambio de Fechas de Basic Data
        private void Cf_BasicData_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_BasicData_SC_BDate.Text = Cf_BasicData_SC_Calendar.Value.ToString("d");
        }

        // Limpiar Pantalla de Basic Data
        void CF_Limpiar_Basic_Data()
        {
            Cf_BasicData_SC_Sex.Text = "";
            Cf_BasicData_SC_BloodType.Text = "";
            Cf_BasicData_SC_RH_Factor.Text = "";
            Cf_BasicData_SC_PhoneType.Text = "";
            Cf_BasicData_SC_State.Text = "";
            Cf_BasicData_SC_ReCons.Text = "";
            Cf_BasicData_SC_FirstName.Text = "";
            Cf_BasicData_SC_Phone.Text = "";
            Cf_BasicData_SC_ZipCode.Text = "";
            Cf_BasicData_SC_Address.Text = "";
            Cf_BasicData_SC_BDate.Text = Cf_BasicData_SC_Calendar.Value.ToString("d");
            Cf_BasicData_SC_LastName.Text = "";
            Cf_BasicData_SC_SSNIDCI.Text = "";
            Cf_BasicData_SC_CardID.Text = "";
            Cf_BasicData_SC_EMail.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de Basic Data
        void CF_Screen_To_Memory_Basic_Data()
        {
            Cf_Database_Oper.CF_BasicData_Mem.V_Sex = Cf_BasicData_SC_Sex.SelectedIndex;
            Cf_Database_Oper.CF_BasicData_Mem.V_BloodType = Cf_BasicData_SC_BloodType.SelectedIndex;
            Cf_Database_Oper.CF_BasicData_Mem.V_BloodFactorRH = Cf_BasicData_SC_RH_Factor.SelectedIndex;
            Cf_Database_Oper.CF_BasicData_Mem.V_PhoneType = Cf_BasicData_SC_PhoneType.SelectedIndex;
            Cf_Database_Oper.CF_BasicData_Mem.V_State = Cf_BasicData_SC_State.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_ReCons = Cf_BasicData_SC_ReCons.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_FirstName = Cf_BasicData_SC_FirstName.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_Phone = Cf_BasicData_SC_Phone.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_ZipCode = Cf_BasicData_SC_ZipCode.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_Address = Cf_BasicData_SC_Address.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_BirthDate = DateTime.Parse(Cf_BasicData_SC_BDate.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_BasicData_Mem.V_LastName = Cf_BasicData_SC_LastName.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_SSNIDCI = Cf_BasicData_SC_SSNIDCI.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_CardID = Cf_BasicData_SC_CardID.Text;
            Cf_Database_Oper.CF_BasicData_Mem.V_EMail = Cf_BasicData_SC_EMail.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de Basic Data
        void CF_Memory_To_Screen_Basic_Data()
        {
            Cf_Database_Oper.CF_GetDb_BasicData(Cf_Database_Oper.CF_DReader_BasicData);
            Cf_BasicData_SC_Sex.SelectedIndex = Cf_Database_Oper.CF_BasicData_Mem.V_Sex;
            Cf_BasicData_SC_BloodType.SelectedIndex = Cf_Database_Oper.CF_BasicData_Mem.V_BloodType;
            Cf_BasicData_SC_RH_Factor.SelectedIndex = Cf_Database_Oper.CF_BasicData_Mem.V_BloodFactorRH;
            Cf_BasicData_SC_PhoneType.SelectedIndex = Cf_Database_Oper.CF_BasicData_Mem.V_PhoneType;
            Cf_BasicData_SC_State.Text = Cf_Database_Oper.CF_BasicData_Mem.V_State;
            Cf_BasicData_SC_ReCons.Text = Cf_Database_Oper.CF_BasicData_Mem.V_ReCons;
            Cf_BasicData_SC_FirstName.Text = Cf_Database_Oper.CF_BasicData_Mem.V_FirstName;
            Cf_BasicData_SC_Phone.Text = Cf_Database_Oper.CF_BasicData_Mem.V_Phone;
            Cf_BasicData_SC_ZipCode.Text = Cf_Database_Oper.CF_BasicData_Mem.V_ZipCode;
            Cf_BasicData_SC_Address.Text = Cf_Database_Oper.CF_BasicData_Mem.V_Address;
            // En observacion
            Cf_BasicData_SC_BDate.Text = Cf_Database_Oper.CF_BasicData_Mem.V_BirthDate.ToShortDateString();
            Cf_BasicData_SC_Calendar.Value = DateTime.Parse(Cf_BasicData_SC_BDate.Text, DateTimeFormatInfo.CurrentInfo);
            // Formato de Fecha en Estudio
            Cf_BasicData_SC_LastName.Text = Cf_Database_Oper.CF_BasicData_Mem.V_LastName;
            Cf_BasicData_SC_SSNIDCI.Text = Cf_Database_Oper.CF_BasicData_Mem.V_SSNIDCI;
            Cf_BasicData_SC_CardID.Text = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_BasicData_SC_EMail.Text = Cf_Database_Oper.CF_BasicData_Mem.V_EMail;
        }

        // Actualización de Valores de Calendario en Basic Data
        private void Cf_BasicData_SC_BDate_Leave(object sender, EventArgs e)
        {
            if (Cf_BasicData_SC_BDate.Text.Length >= 10)
                Cf_BasicData_SC_Calendar.Value = DateTime.Parse(Cf_BasicData_SC_BDate.Text, DateTimeFormatInfo.CurrentInfo);
        }

        // Manejo de Delete de ToolsBar de Basic Data
        private void CF_Btn_Basic_Data_Delete_Click(object sender, EventArgs e)
        {
            if (CF_Btn_Basic_Data_Delete.Text == "Borrar")
            {
                CF_Btn_Basic_Data_Find.Enabled = false;
                CF_Btn_Basic_Data_New.Enabled = false;
                CF_Btn_Basic_Data_Modify.Enabled = false;
                CF_Btn_Basic_Data_Delete.Text = "Aceptar";
                CF_Btn_Basic_Data_Delete.Enabled = true;
                CF_Btn_Basic_Data_Print.Enabled = false;
                CF_Btn_Basic_Data_Cancel.Enabled = true;
                CF_Panel_Btn_OtherExams.Enabled = false;
                CF_TP_Exams_Others.Enabled = false;
            }
            else if (CF_Btn_Basic_Data_Delete.Text == "Aceptar")
            {
                if (Cf_Database_Oper.CF_Delete_BasicData(Cf_BasicData_SC_CardID.Text))
                {
                    CF_Btn_Basic_Data_Find.Enabled = true;
                    CF_Btn_Basic_Data_New.Enabled = true;
                    CF_Btn_Basic_Data_Modify.Enabled = true;
                    CF_Btn_Basic_Data_Delete.Enabled = true;
                    CF_Btn_Basic_Data_Delete.Text = "Borrar";
                    CF_Btn_Basic_Data_Print.Enabled = true;
                    CF_Btn_Basic_Data_Cancel.Enabled = false;
                    CF_Panel_Btn_OtherExams.Enabled = true;
                    CF_TP_Exams_Others.Enabled = true;
                }
                else
                {
                    MessageBox.Show(Cf_Database_Oper.CF_BasicData_Mem.V_Error_Operation, "Error en Datos Basicos");
                }
            }
        }

        // Manejo de Modify de ToolsBar de Basic Data
        private void CF_Btn_Basic_Data_Modify_Click(object sender, EventArgs e)
        {
            if (CF_Btn_Basic_Data_Modify.Text == "Modificar")
            {
                Cf_BasicData_SC_CardID.Enabled = false;
                CF_Btn_Basic_Data_Find.Enabled = false;
                CF_Btn_Basic_Data_New.Enabled = false;
                CF_Btn_Basic_Data_Modify.Text = "Aceptar";
                CF_Btn_Basic_Data_Modify.Enabled = true;
                CF_Btn_Basic_Data_Delete.Enabled = false;
                CF_Btn_Basic_Data_Print.Enabled = false;
                CF_Btn_Basic_Data_Cancel.Enabled = true;
                CF_Panel_Btn_OtherExams.Enabled = false;
                CF_TP_Exams_Others.Enabled = false;
            }
            else if (CF_Btn_Basic_Data_Modify.Text == "Aceptar")
            {
                CF_Screen_To_Memory_Basic_Data();
                if (Cf_Database_Oper.CF_Modify_BasicData(Cf_BasicData_SC_CardID.Text))
                {
                    Cf_BasicData_SC_CardID.Enabled = true;
                    CF_Btn_Basic_Data_Find.Enabled = true;
                    CF_Btn_Basic_Data_New.Enabled = true;
                    CF_Btn_Basic_Data_Modify.Text = "Modificar";
                    CF_Btn_Basic_Data_Modify.Enabled = true;
                    CF_Btn_Basic_Data_Delete.Enabled = true;
                    CF_Btn_Basic_Data_Print.Enabled = true;
                    CF_Btn_Basic_Data_Cancel.Enabled = false;
                    CF_Panel_Btn_OtherExams.Enabled = true;
                    CF_TP_Exams_Others.Enabled = true;
                }
                else
                {
                    MessageBox.Show(Cf_Database_Oper.CF_BasicData_Mem.V_Error_Operation, "Error en Datos Basicos");
                }
            }
        }

        // Operaciones con pantallas de Otros examenes
        private void CF_Btn_OthersExams_Find_Click(object sender, EventArgs e)
        {
            if (CF_Btn_OthersExams_Find.Text == "Buscar")
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Text = "Aceptar";
                CF_Btn_OthersExams_New.Enabled = false;
                CF_Btn_OthersExams_Modify.Enabled = false;
                CF_Btn_OthersExams_Del.Enabled = false;
                CF_Btn_OthersExams_Print.Enabled = false;
                CF_Btn_OthersExams_Cancel.Enabled = true;
            }
            else if (CF_Btn_OthersExams_Find.Text == "Aceptar")
            {
                // Verificar cual panel esta activo y ejecutar la limpieza de pantalla respectiva
                if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Physical_Examination.Text)
                {
                    if (Cf_Database_Oper.CF_Select_PhysicalExamination(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_PhyExam_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_PhysicalExamination();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de Fecha/Hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Radiography.Text)
                {
                    if (Cf_Database_Oper.CF_Select_Radiography(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_Radiography_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_Radiography();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de Fecha/Hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_EKGR.Text)
                {
                    if (Cf_Database_Oper.CF_Select_EKGR(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_EKGR_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_EKGR();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de Fecha/Hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Echo_Doppler.Text)
                {
                    if (Cf_Database_Oper.CF_Select_EchoDoppler(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_EchoDoppler_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_EchoDoppler();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de Fecha/Hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Holter.Text)
                {
                    if (Cf_Database_Oper.CF_Select_Holter(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_Holter_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_Holter();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de Fecha/Hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Stress_Test.Text)
                {
                    if (Cf_Database_Oper.CF_Select_StressTest(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_StressTest_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_StressTest();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de Fecha/Hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Cardiac_Cath.Text)
                {
                    if (Cf_Database_Oper.CF_Select_CardiacCath(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_CardiacCath_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_CardiacCath();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de fecha/hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Surgery.Text)
                {
                    if (Cf_Database_Oper.CF_Select_Surgery(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_Surgery_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_Surgery();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de fecha/hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Diagnoses_Treatments.Text)
                {
                    if (Cf_Database_Oper.CF_Select_DiagnosesTreatments(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_DiagTreatment_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_DiagnosesTreatment();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de fecha/hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Progress_Notes.Text)
                {
                    if (Cf_Database_Oper.CF_Select_ProgressNotes(Cf_BasicData_SC_CardID.Text))
                    {
                        CF_Memory_To_Screen_ProgressNotes();
                        CF_LlenadoGrilla_ProgressNotes();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error de fecha/hora");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Appointments.Text)
                {
                    if (Cf_Database_Oper.CF_Select_Appointments(DateTime.Parse(Cf_Appoint_SC_AppointDate.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Memory_To_Screen_Appointments();
                        CF_LlenadoGrilla_Appointments();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        CF_Limpiar_Appointments();
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error en la cita");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Statistics.Text)
                {
                    if (Cf_Database_Oper.CF_Select_Statistics(
                        Cf_Statistics_SC_IDiagnoses.Checked,
                        Cf_Statistics_SC_Diagnoses.Text,
                        Cf_Statistics_SC_ITreatments.Checked,
                        Cf_Statistics_SC_Treatments.Text))
                    {
                        // CF_Memory_To_Screen_Statistics();
                        CF_LlenadoGrilla_Statistics();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Text = "Buscar";
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        CF_Limpiar_Statistics();
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text + " No encontrado", "Error en estadisticas");
                    }

                    // ******************************************************************************
                    // Analizar estado de estas opciones, si las dejamos->desarrollamos y/o eliminamos
                    // ******************************************************************************
                    // Procesar Diagnostico: Cf_Statistics_SC_IDiagnoses
                    // Campo del Diagnostico: Cf_Statistics_SC_Diagnoses
                    // Procesar Tratamiento: Cf_Statistics_SC_ITreatments
                    // Campo del Tratamiento: Cf_Statistics_SC_Treatments
                    // Grilla de Resultados: Cf_Statistics_SC_GridView
                }
            }
        }

        private void CF_Btn_OthersExams_New_Click(object sender, EventArgs e)
        {
            if (CF_Btn_OthersExams_New.Text == "Nuevo")
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Enabled = false;
                CF_Btn_OthersExams_New.Text = "Aceptar";
                CF_Btn_OthersExams_Modify.Enabled = false;
                CF_Btn_OthersExams_Del.Enabled = false;
                CF_Btn_OthersExams_Print.Enabled = false;
                CF_Btn_OthersExams_Cancel.Enabled = true;
                // Verificar cual panel esta activo y ejecutar la limpieza de pantalla respectiva
                if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Physical_Examination.Text)
                    CF_Limpiar_PhysicalExamination();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Radiography.Text)
                    CF_Limpiar_Radiography();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_EKGR.Text)
                    CF_Limpiar_EKGR();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Echo_Doppler.Text)
                    CF_Limpiar_EchoDoppler();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Holter.Text)
                    CF_Limpiar_Holter();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Stress_Test.Text)
                    CF_Limpiar_StressTest();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Cardiac_Cath.Text)
                    CF_Limpiar_CardiacCath();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Surgery.Text)
                    CF_Limpiar_Surgery();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Diagnoses_Treatments.Text)
                    CF_Limpiar_DiagnosesTreatment();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Progress_Notes.Text)
                    CF_Limpiar_ProgressNotes();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Appointments.Text)
                    CF_Limpiar_Appointments();
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Statistics.Text)
                    CF_Limpiar_Statistics();
            }
            else if (CF_Btn_OthersExams_New.Text == "Aceptar")
            {
                // Verificar cual panel esta activo y ejecutar la limpieza de pantalla respectiva
                if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Physical_Examination.Text)
                {
                    CF_Screen_To_Memory_PhysicalExamination();
                    if (Cf_Database_Oper.CF_Insert_PhysicalExamination())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Radiography.Text)
                {
                    CF_Screen_To_Memory_Radiography();
                    if (Cf_Database_Oper.CF_Insert_Radiography())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Radiography_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_EKGR.Text)
                {
                    CF_Screen_To_Memory_EKGR();
                    if (Cf_Database_Oper.CF_Insert_EKGR())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_EKGR_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Echo_Doppler.Text)
                {
                    CF_Screen_To_Memory_EchoDoppler();
                    if (Cf_Database_Oper.CF_Insert_EchoDoppler())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_EchoDoppler_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Holter.Text)
                {
                    CF_Screen_To_Memory_Holter();
                    if (Cf_Database_Oper.CF_Insert_Holter())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Holter_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Stress_Test.Text)
                {
                    CF_Screen_To_Memory_StressTest();
                    if (Cf_Database_Oper.CF_Insert_StressTest())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_StressTest_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Cardiac_Cath.Text)
                {
                    CF_Screen_To_Memory_CardiacCath();
                    if (Cf_Database_Oper.CF_Insert_CardiacCath())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_CardiacCath_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Surgery.Text)
                {
                    CF_Screen_To_Memory_Surgery();
                    if (Cf_Database_Oper.CF_Insert_Surgery())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Surgery_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Diagnoses_Treatments.Text)
                {
                    CF_Screen_To_Memory_DiagnosesTreatment();
                    if (Cf_Database_Oper.CF_Insert_DiagnosesTreatments())
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Progress_Notes.Text)
                {
                    CF_Screen_To_Memory_ProgressNotes();
                    if (Cf_Database_Oper.CF_Insert_ProgressNotes())
                    {
                        CF_LlenadoGrilla_ProgressNotes();
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_ProgressNotes_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Appointments.Text)
                {
                    CF_Screen_To_Memory_Appointments();
                    if (Cf_Database_Oper.CF_Insert_Appointments())
                    {
                        if (Cf_Database_Oper.CF_Appointments_Mem.V_CardID == Cf_Database_Oper.CF_BasicData_Mem.V_CardID)
                            CF_LlenadoGrilla_ProgressNotes();
                        CF_LlenadoGrilla_Appointments();
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Text = "Nuevo";
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Appointments_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Statistics.Text)
                {
                    // ******************************************************************************
                    // Analizar estado de estas opciones, si las dejamos->desarrollamos y/o eliminamos
                    // ******************************************************************************
                    // CF_Screen_To_Memory_Statistics();
                    // MessageBox.Show("Not implement");
                }
            }
        }

        private void CF_Btn_OthersExams_Modify_Click(object sender, EventArgs e)
        {
            DateTime FechaTemporal;

            if (CF_Btn_OthersExams_Modify.Text == "Modificar")
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Enabled = false;
                CF_Btn_OthersExams_New.Enabled = false;
                CF_Btn_OthersExams_Modify.Text = "Aceptar";
                CF_Btn_OthersExams_Del.Enabled = false;
                CF_Btn_OthersExams_Print.Enabled = false;
                CF_Btn_OthersExams_Cancel.Enabled = true;
                // Evita el cambio del CARDID durante Modify en Appoint
                if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Appointments.Text)
                    Cf_Appoint_SC_CardID.Enabled = false;
            }
            else if (CF_Btn_OthersExams_Modify.Text == "Aceptar")
            {

                // Verificar cual panel esta activo y ejecutar la limpieza de pantalla respectiva
                if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Physical_Examination.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Date;
                    CF_Screen_To_Memory_PhysicalExamination();
                    if (Cf_Database_Oper.CF_Modify_PhysicalExamination(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Error_Operation, "Physical Examination Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Radiography.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_Radiography_Mem.V_Date;
                    CF_Screen_To_Memory_Radiography();
                    if (Cf_Database_Oper.CF_Modify_Radiography(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Radiography_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_EKGR.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_EKGR_Mem.V_Date;
                    CF_Screen_To_Memory_EKGR();
                    if (Cf_Database_Oper.CF_Modify_EKGR(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_EKGR_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Echo_Doppler.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_EchoDoppler_Mem.V_Date;
                    CF_Screen_To_Memory_EchoDoppler();
                    if (Cf_Database_Oper.CF_Modify_EchoDoppler(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_EchoDoppler_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Holter.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_Holter_Mem.V_Date;
                    CF_Screen_To_Memory_Holter();
                    if (Cf_Database_Oper.CF_Modify_Holter(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Holter_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Stress_Test.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_StressTest_Mem.V_Date;
                    CF_Screen_To_Memory_StressTest();
                    if (Cf_Database_Oper.CF_Modify_StressTest(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_StressTest_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Cardiac_Cath.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_CardiacCath_Mem.V_Date;
                    CF_Screen_To_Memory_CardiacCath();
                    if (Cf_Database_Oper.CF_Modify_CardiacCath(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_CardiacCath_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Surgery.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_Surgery_Mem.V_Date;
                    CF_Screen_To_Memory_Surgery();
                    if (Cf_Database_Oper.CF_Modify_Surgery(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_Surgery_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Diagnoses_Treatments.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Date;
                    CF_Screen_To_Memory_DiagnosesTreatment();
                    if (Cf_Database_Oper.CF_Modify_DiagnosesTreatments(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Progress_Notes.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_ProgressNotes_Mem.V_Date;
                    CF_Screen_To_Memory_ProgressNotes();
                    if (Cf_Database_Oper.CF_Modify_ProgressNotes(Cf_Database_Oper.CF_BasicData_Mem.V_CardID, FechaTemporal))
                    {
                        CF_LlenadoGrilla_ProgressNotes();
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_ProgressNotes_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Appointments.Text)
                {
                    FechaTemporal = Cf_Database_Oper.CF_Appointments_Mem.V_DateAppoint;
                    CF_Screen_To_Memory_Appointments();
                    if (Cf_Database_Oper.CF_Modify_Appointments(FechaTemporal, Cf_Database_Oper.CF_Appointments_Mem.V_CardID))
                    {
                        if (Cf_Database_Oper.CF_Appointments_Mem.V_CardID == Cf_Database_Oper.CF_BasicData_Mem.V_CardID)
                            CF_LlenadoGrilla_ProgressNotes();
                        CF_LlenadoGrilla_Appointments();
                        Cf_Appoint_SC_CardID.Enabled = true;
                        CF_Btn_Basic_Data_Find.Enabled = true;
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Text = "Modificar";
                        CF_Btn_OthersExams_Del.Enabled = true;
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(Cf_Database_Oper.CF_ProgressNotes_Mem.V_Error_Operation, CF_TP_Exams_Others.SelectedTab.Text + " Error");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Statistics.Text)
                {
                    // CF_Screen_To_Memory_Statistics();
                    // MessageBox.Show("Not implement");
                }

            }
        }

        private void CF_Btn_OthersExams_Del_Click(object sender, EventArgs e)
        {
            if (CF_Btn_OthersExams_Del.Text == "Borrar")
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Enabled = false;
                CF_Btn_OthersExams_New.Enabled = false;
                CF_Btn_OthersExams_Modify.Enabled = false;
                CF_Btn_OthersExams_Del.Text = "Aceptar";
                CF_Btn_OthersExams_Print.Enabled = false;
                CF_Btn_OthersExams_Cancel.Enabled = true;
            }
            else if (CF_Btn_OthersExams_Del.Text == "Aceptar")
            {
                // Verificar cual panel esta activo y ejecutar la limpieza de pantalla respectiva
                if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Physical_Examination.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_PhysicalExamination(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_PhyExam_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_PhysicalExamination();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Radiography.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_Radiography(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_Radiography_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_Radiography();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_EKGR.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_EKGR(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_EKGR_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_EKGR();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Echo_Doppler.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_EchoDoppler(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_EchoDoppler_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_EchoDoppler();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Holter.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_Holter(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_Holter_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_Holter();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Stress_Test.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_StressTest(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_StressTest_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_StressTest();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Cardiac_Cath.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_CardiacCath(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_CardiacCath_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_CardiacCath();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Surgery.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_Surgery(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_Surgery_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_Surgery();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Diagnoses_Treatments.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_DiagnosesTreatments(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_DiagTreatment_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_Limpiar_DiagnosesTreatment();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Progress_Notes.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_ProgressNotes(Cf_BasicData_SC_CardID.Text, DateTime.Parse(Cf_ProgNotes_SC_Date.Text, DateTimeFormatInfo.CurrentInfo)))
                    {
                        CF_LlenadoGrilla_ProgressNotes();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Appointments.Text)
                {
                    if (Cf_Database_Oper.CF_Delete_Appointments(DateTime.Parse(Cf_Appoint_SC_AppointDate.Text, DateTimeFormatInfo.CurrentInfo), Cf_Appoint_SC_CardID.Text))
                    {
                        if (Cf_Appoint_SC_CardID.Text == Cf_Database_Oper.CF_BasicData_Mem.V_CardID)
                            CF_LlenadoGrilla_ProgressNotes();
                        CF_LlenadoGrilla_Appointments();
                        // Activación de Opciones de Menus
                        CardioFILE_Tools.Enabled = true;
                        CF_Basic_Data_Panel.Enabled = true;
                        CF_Btn_BasicData_Browser.Enabled = true;
                        CF_Btn_OthersExams_Find.Enabled = true;
                        CF_Btn_OthersExams_New.Enabled = true;
                        CF_Btn_OthersExams_Modify.Enabled = true;
                        CF_Btn_OthersExams_Del.Text = "Borrar";
                        CF_Btn_OthersExams_Print.Enabled = true;
                        CF_Btn_OthersExams_Cancel.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(CF_TP_Exams_Others.SelectedTab.Text, "Borrar");
                    }
                }
                else if (CF_TP_Exams_Others.SelectedTab.Text == CF_TP_Statistics.Text)
                {
                    /*
                    // CF_Memory_To_Screen_Statistics();
                    MessageBox.Show("Not implement");
                     */
                }
            }
        }

        private void CF_Btn_OthersExams_Cancel_Click(object sender, EventArgs e)
        {
            CardioFILE_Tools.Enabled = true;
            CF_Basic_Data_Panel.Enabled = true;
            CF_Btn_BasicData_Browser.Enabled = true;
            Cf_Appoint_SC_CardID.Enabled = true;
            CF_Btn_OthersExams_Find.Text = "Buscar";
            CF_Btn_OthersExams_Find.Enabled = true;
            CF_Btn_OthersExams_New.Text = "Nuevo";
            CF_Btn_OthersExams_New.Enabled = true;
            CF_Btn_OthersExams_Modify.Text = "Modificar";
            CF_Btn_OthersExams_Modify.Enabled = true;
            CF_Btn_OthersExams_Del.Text = "Borrar";
            CF_Btn_OthersExams_Del.Enabled = true;
            CF_Btn_OthersExams_Print.Enabled = true;
            CF_Btn_OthersExams_Cancel.Enabled = false;
        }

        // Limpiar Pantalla de PhysicalExamination
        void CF_Limpiar_PhysicalExamination()
        {
            Cf_PhyExam_SC_Date.Text = Cf_PhyExam_SC_Calendar.Value.ToString("d");
            Cf_PhyExam_SC_Observ.Text = "";
            Cf_PhyExam_SC_BMI.Text = "";
            Cf_PhyExam_SC_Weight.Text = "";
            Cf_PhyExam_SC_Height.Text = "";
            Cf_PhyExam_SC_Weight_Item.Text = "";
            Cf_PhyExam_SC_Height_Item.Text = "";
            Cf_PhyExam_SC_Pulse.Text = "";
            Cf_PhyExam_SC_BPresMin.Text = "";
            Cf_PhyExam_SC_BPresMax.Text = "";
            Cf_PhyExam_SC_PersAnt.Text = "";
            Cf_PhyExam_SC_FamiAnt.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de Physical Examination
        void CF_Screen_To_Memory_PhysicalExamination()
        {
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Date = DateTime.Parse(Cf_PhyExam_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Observ = Cf_PhyExam_SC_Observ.Text;
            try
            {
                Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BMI = float.Parse(Cf_PhyExam_SC_BMI.Text, CultureInfo.InvariantCulture);
            }
            catch (System.FormatException)
            {
                Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BMI = (float)0;
            }
            try
            {
                Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Weight = float.Parse(Cf_PhyExam_SC_Weight.Text, CultureInfo.InvariantCulture);
            }
            catch (System.FormatException)
            {
                Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Weight = (float)0;
            }
            try
            {
                Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Height = float.Parse(Cf_PhyExam_SC_Height.Text, CultureInfo.InvariantCulture);
            }
            catch (System.FormatException)
            {
                Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Height = (float)0;
            }
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Weight_Item = Cf_PhyExam_SC_Weight_Item.SelectedIndex;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Height_Item = Cf_PhyExam_SC_Height_Item.SelectedIndex;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Pulse = Cf_PhyExam_SC_Pulse.Text;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BloodPresMin = Cf_PhyExam_SC_BPresMin.Text;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BloodPresMax = Cf_PhyExam_SC_BPresMax.Text;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_PersAnt = Cf_PhyExam_SC_PersAnt.Text;
            Cf_Database_Oper.CF_PhysicalExamination_Mem.V_FamiAnt = Cf_PhyExam_SC_FamiAnt.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de Physical Examination
        void CF_Memory_To_Screen_PhysicalExamination()
        {
            Cf_Database_Oper.CF_GetDb_PhysicalExamination(Cf_Database_Oper.CF_DReader_PhysicalExamination);
            Cf_PhyExam_SC_Date.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Date.ToShortDateString();
            Cf_PhyExam_SC_Calendar.Value = DateTime.Parse(Cf_PhyExam_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_PhyExam_SC_Observ.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Observ;
            Cf_PhyExam_SC_BMI.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BMI.ToString();
            Cf_PhyExam_SC_Weight.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Weight.ToString();
            Cf_PhyExam_SC_Height.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Height.ToString();
            Cf_PhyExam_SC_Weight_Item.SelectedIndex = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Weight_Item;
            Cf_PhyExam_SC_Height_Item.SelectedIndex = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Height_Item;
            Cf_PhyExam_SC_Pulse.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Pulse;
            Cf_PhyExam_SC_BPresMin.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BloodPresMin;
            Cf_PhyExam_SC_BPresMax.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_BloodPresMax;
            Cf_PhyExam_SC_PersAnt.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_PersAnt;
            Cf_PhyExam_SC_FamiAnt.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_FamiAnt;
        }

        // Limpiar Pantalla de Radiography
        void CF_Limpiar_Radiography()
        {
            Cf_Radiography_SC_Date.Text = Cf_Radiography_SC_Calendar.Value.ToString("d");
            Cf_Radiography_SC_Observ.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de Radiography
        void CF_Screen_To_Memory_Radiography()
        {
            Cf_Database_Oper.CF_Radiography_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_Radiography_Mem.V_Date = DateTime.Parse(Cf_Radiography_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_Radiography_Mem.V_Observ = Cf_Radiography_SC_Observ.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de Radiography
        void CF_Memory_To_Screen_Radiography()
        {
            Cf_Database_Oper.CF_GetDb_Radiography(Cf_Database_Oper.CF_DReader_Radiography);
            Cf_Radiography_SC_Date.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Date.ToShortDateString();
            Cf_Radiography_SC_Calendar.Value = DateTime.Parse(Cf_Radiography_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Radiography_SC_Observ.Text = Cf_Database_Oper.CF_Radiography_Mem.V_Observ;
        }

        // Limpiar Pantalla de EKGR
        void CF_Limpiar_EKGR()
        {
            Cf_EKGR_SC_Date.Text = Cf_EKGR_SC_Calendar.Value.ToString("d");
            Cf_EKGR_SC_Observ.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de EKGR
        void CF_Screen_To_Memory_EKGR()
        {
            Cf_Database_Oper.CF_EKGR_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_EKGR_Mem.V_Date = DateTime.Parse(Cf_EKGR_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_EKGR_Mem.V_Observ = Cf_EKGR_SC_Observ.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de EKGR
        void CF_Memory_To_Screen_EKGR()
        {
            Cf_Database_Oper.CF_GetDb_EKGR(Cf_Database_Oper.CF_DReader_EKGR);
            Cf_EKGR_SC_Date.Text = Cf_Database_Oper.CF_PhysicalExamination_Mem.V_Date.ToShortDateString();
            Cf_EKGR_SC_Calendar.Value = DateTime.Parse(Cf_EKGR_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_EKGR_SC_Observ.Text = Cf_Database_Oper.CF_EKGR_Mem.V_Observ;
        }

        // Limpiar Pantalla de EchoDoppler
        void CF_Limpiar_EchoDoppler()
        {
            Cf_EchoDoppler_SC_Date.Text = Cf_EchoDoppler_SC_Calendar.Value.ToString("d");
            Cf_EchoDoppler_SC_Echo.Text = "";
            Cf_EchoDoppler_SC_Doppler.Text = "";
            Cf_EchoDoppler_SC_Laboratory.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de EchoDoppler
        void CF_Screen_To_Memory_EchoDoppler()
        {
            Cf_Database_Oper.CF_EchoDoppler_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_EchoDoppler_Mem.V_Date = DateTime.Parse(Cf_EchoDoppler_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_EchoDoppler_Mem.V_Echo = Cf_EchoDoppler_SC_Echo.Text;
            Cf_Database_Oper.CF_EchoDoppler_Mem.V_Doppler = Cf_EchoDoppler_SC_Doppler.Text;
            Cf_Database_Oper.CF_EchoDoppler_Mem.V_Laboratory = Cf_EchoDoppler_SC_Laboratory.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de EKGR
        void CF_Memory_To_Screen_EchoDoppler()
        {
            Cf_Database_Oper.CF_GetDb_EchoDoppler(Cf_Database_Oper.CF_DReader_EchoDoppler);
            Cf_EchoDoppler_SC_Date.Text = Cf_Database_Oper.CF_EchoDoppler_Mem.V_Date.ToShortDateString();
            Cf_EchoDoppler_SC_Calendar.Value = DateTime.Parse(Cf_EchoDoppler_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_EchoDoppler_SC_Echo.Text = Cf_Database_Oper.CF_EchoDoppler_Mem.V_Echo;
            Cf_EchoDoppler_SC_Doppler.Text = Cf_Database_Oper.CF_EchoDoppler_Mem.V_Doppler;
            Cf_EchoDoppler_SC_Laboratory.Text = Cf_Database_Oper.CF_EchoDoppler_Mem.V_Laboratory;
        }

        // Limpiar Pantalla de Holter
        void CF_Limpiar_Holter()
        {
            Cf_Holter_SC_Date.Text = Cf_Holter_SC_Calendar.Value.ToString("d");
            Cf_Holter_SC_Observ.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de Holter
        void CF_Screen_To_Memory_Holter()
        {
            Cf_Database_Oper.CF_Holter_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_Holter_Mem.V_Date = DateTime.Parse(Cf_Holter_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_Holter_Mem.V_Observ = Cf_Holter_SC_Observ.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de Holter
        void CF_Memory_To_Screen_Holter()
        {
            Cf_Database_Oper.CF_GetDb_Holter(Cf_Database_Oper.CF_DReader_Holter);
            Cf_Holter_SC_Date.Text = Cf_Database_Oper.CF_Holter_Mem.V_Date.ToShortDateString();
            Cf_Holter_SC_Calendar.Value = DateTime.Parse(Cf_Holter_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Holter_SC_Observ.Text = Cf_Database_Oper.CF_Holter_Mem.V_Observ;
        }

        // Limpiar Pantalla de StressTest
        void CF_Limpiar_StressTest()
        {
            Cf_StressTest_SC_Date.Text = Cf_StressTest_SC_Calendar.Value.ToString("d");
            Cf_StressTest_SC_BloodPress.Text = "";
            Cf_StressTest_SC_Arrhyth.Text = "";
            Cf_StressTest_SC_PercentageMax.Text = "";
            Cf_StressTest_SC_CardiacFrec.Text = "";
            Cf_StressTest_SC_Time.Text = "";
            Cf_StressTest_SC_Stage.Text = "";
            Cf_StressTest_SC_Protocol.Text = "";
            Cf_StressTest_SC_DescSTT.Text = "";
            Cf_StressTest_SC_Result.Text = "";
            Cf_StressTest_SC_ReSusp.Text = "";
            Cf_StressTest_SC_FunCap.Text = "";
            Cf_StressTest_SC_Type.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de Stress Test
        void CF_Screen_To_Memory_StressTest()
        {
            Cf_Database_Oper.CF_StressTest_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_StressTest_Mem.V_Date = DateTime.Parse(Cf_StressTest_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_StressTest_Mem.V_BloodPress = Cf_StressTest_SC_BloodPress.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_Arrhyth = Cf_StressTest_SC_Arrhyth.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_PercentMax = int.Parse(Cf_StressTest_SC_PercentageMax.Text);
            Cf_Database_Oper.CF_StressTest_Mem.V_CardiacFrec = int.Parse(Cf_StressTest_SC_CardiacFrec.Text);
            // Verificar como hacer para la hora
            Cf_Database_Oper.CF_StressTest_Mem.V_Time = Cf_StressTest_SC_Time.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_Protocol = Cf_StressTest_SC_Protocol.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_DescSTT = Cf_StressTest_SC_DescSTT.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_Result = Cf_StressTest_SC_Result.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_ReSusp = Cf_StressTest_SC_ReSusp.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_FunCap = Cf_StressTest_SC_FunCap.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_Type = Cf_StressTest_SC_Type.Text;
            Cf_Database_Oper.CF_StressTest_Mem.V_Stage = Cf_StressTest_SC_Stage.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de Stress Test
        void CF_Memory_To_Screen_StressTest()
        {
            Cf_Database_Oper.CF_GetDb_StressTest(Cf_Database_Oper.CF_DReader_StressTest);
            Cf_StressTest_SC_Date.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Date.ToShortDateString();
            Cf_StressTest_SC_Calendar.Value = DateTime.Parse(Cf_StressTest_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_StressTest_SC_BloodPress.Text = Cf_Database_Oper.CF_StressTest_Mem.V_BloodPress;
            Cf_StressTest_SC_Arrhyth.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Arrhyth;
            Cf_StressTest_SC_PercentageMax.Text = Cf_Database_Oper.CF_StressTest_Mem.V_PercentMax.ToString();
            Cf_StressTest_SC_CardiacFrec.Text = Cf_Database_Oper.CF_StressTest_Mem.V_CardiacFrec.ToString();
            Cf_StressTest_SC_Time.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Time;
            Cf_StressTest_SC_Protocol.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Protocol;
            Cf_StressTest_SC_DescSTT.Text = Cf_Database_Oper.CF_StressTest_Mem.V_DescSTT;
            Cf_StressTest_SC_Result.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Result;
            Cf_StressTest_SC_ReSusp.Text = Cf_Database_Oper.CF_StressTest_Mem.V_ReSusp;
            Cf_StressTest_SC_FunCap.Text = Cf_Database_Oper.CF_StressTest_Mem.V_FunCap;
            Cf_StressTest_SC_Type.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Type;
            Cf_StressTest_SC_Stage.Text = Cf_Database_Oper.CF_StressTest_Mem.V_Stage;
        }

        // Limpiar Pantalla de CardiacCatch
        void CF_Limpiar_CardiacCath()
        {
            Cf_CardiacCath_SC_Date.Text = Cf_CardiacCath_SC_Calendar.Value.ToString("d");
            Cf_CardiacCath_SC_Observ.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de CardiacCath
        void CF_Screen_To_Memory_CardiacCath()
        {
            Cf_Database_Oper.CF_CardiacCath_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_CardiacCath_Mem.V_Date = DateTime.Parse(Cf_CardiacCath_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_CardiacCath_Mem.V_Observ = Cf_CardiacCath_SC_Observ.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de CardiacCath
        void CF_Memory_To_Screen_CardiacCath()
        {
            Cf_Database_Oper.CF_GetDb_CardiacCath(Cf_Database_Oper.CF_DReader_CardiacCath);
            Cf_CardiacCath_SC_Date.Text = Cf_Database_Oper.CF_CardiacCath_Mem.V_Date.ToShortDateString();
            Cf_CardiacCath_SC_Calendar.Value = DateTime.Parse(Cf_CardiacCath_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_CardiacCath_SC_Observ.Text = Cf_Database_Oper.CF_CardiacCath_Mem.V_Observ;
        }

        // Limpiar Pantalla de Surgery
        void CF_Limpiar_Surgery()
        {
            Cf_Surgery_SC_Date.Text = Cf_Surgery_SC_Calendar.Value.ToString("d");
            Cf_Surgery_SC_Observ.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de Surgery
        void CF_Screen_To_Memory_Surgery()
        {
            Cf_Database_Oper.CF_Surgery_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_Surgery_Mem.V_Date = DateTime.Parse(Cf_Surgery_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_Surgery_Mem.V_Observ = Cf_Surgery_SC_Observ.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de Surgery
        void CF_Memory_To_Screen_Surgery()
        {
            Cf_Database_Oper.CF_GetDb_Surgery(Cf_Database_Oper.CF_DReader_Surgery);
            Cf_Surgery_SC_Date.Text = Cf_Database_Oper.CF_Surgery_Mem.V_Date.ToShortDateString();
            Cf_Surgery_SC_Calendar.Value = DateTime.Parse(Cf_Surgery_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Surgery_SC_Observ.Text = Cf_Database_Oper.CF_Surgery_Mem.V_Observ;
        }

        // Limpiar Pantalla de DiagnosesTreatment
        void CF_Limpiar_DiagnosesTreatment()
        {
            Cf_DiagTreatment_SC_Date.Text = Cf_DiagTreatment_SC_Calendar.Value.ToString("d");
            Cf_DiagTreatment_SC_Treatment.Text = "";
            Cf_DiagTreatment_SC_Diagnoses.Text = "";
        }

        // Transferencia de Valores de Pantalla a Memoria de DiagnosesTreatment
        void CF_Screen_To_Memory_DiagnosesTreatment()
        {
            Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Date = DateTime.Parse(Cf_DiagTreatment_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Diagnoses = Cf_DiagTreatment_SC_Diagnoses.Text;
            Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Treatments = Cf_DiagTreatment_SC_Treatment.Text;
        }

        // Transferencia de Valores de Memoria a Pantalla de DiagnosesTreatment
        void CF_Memory_To_Screen_DiagnosesTreatment()
        {
            Cf_Database_Oper.CF_GetDb_DiagnosesTreatments(Cf_Database_Oper.CF_DReader_DiagnosesTreatments);
            Cf_DiagTreatment_SC_Date.Text = Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Date.ToShortDateString();
            Cf_DiagTreatment_SC_Calendar.Value = DateTime.Parse(Cf_DiagTreatment_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_DiagTreatment_SC_Diagnoses.Text = Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Diagnoses;
            Cf_DiagTreatment_SC_Treatment.Text = Cf_Database_Oper.CF_DiagnosesTreatments_Mem.V_Treatments;
        }

        // Limpiar Pantalla de ProgressNOtes
        void CF_Limpiar_ProgressNotes()
        {
            Cf_ProgNotes_SC_Date.Text = Cf_ProgNotes_SC_Calendar.Value.ToString("d");

            Cf_ProgNotes_SC_Observ.Text = "";

            Cf_ProgNotes_SC_NextDate.Text = Cf_ProgNotes_SC_NextDateCalendar.Value.ToString("d");
            Cf_ProgNotes_SC_NextTime.Text = Cf_ProgNotes_SC_NextTimeCalendar.Value.ToString("t", CultureInfo.CreateSpecificCulture("hr-HR"));

            Cf_ProgNotes_SC_GridView.SelectAll();
            Cf_ProgNotes_SC_GridView.ClearSelection();
            Cf_ProgNotes_SC_GridView.RowCount = 1;

            // Cf_ProgNotes_SC_GridDate;
            // Cf_ProgNotes_SC_GridObserv;
            // Cf_ProgNotes_SC_GridNextDate;
            // Cf_ProgNotes_SC_GridNextTime;
        }

        void CF_Memory_To_Screen_ProgressNotes()
        {
            Cf_Database_Oper.CF_GetDb_ProgressNotes(Cf_Database_Oper.CF_DReader_ProgressNotes);
            Cf_ProgNotes_SC_Date.Text = Cf_Database_Oper.CF_ProgressNotes_Mem.V_Date.ToShortDateString();
            Cf_ProgNotes_SC_Calendar.Value = DateTime.Parse(Cf_ProgNotes_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_ProgNotes_SC_Observ.Text = Cf_Database_Oper.CF_ProgressNotes_Mem.V_Observ;
            Cf_ProgNotes_SC_NextDate.Text = Cf_Database_Oper.CF_ProgressNotes_Mem.V_NextDateApp.ToShortDateString();
            Cf_ProgNotes_SC_NextDateCalendar.Value = DateTime.Parse(Cf_ProgNotes_SC_NextDate.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_ProgNotes_SC_NextTime.Text = Cf_Database_Oper.CF_ProgressNotes_Mem.V_NextTimeApp.ToString("t");
            // Cf_ProgNotes_SC_NextTimeCalendar.Value = DateTime.Parse(Cf_ProgNotes_SC_NextTime.Text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.NoCurrentDateDefault);
            Cf_ProgNotes_SC_NextTimeCalendar.Value = DateTime.Parse(Cf_ProgNotes_SC_NextTime.Text, DateTimeFormatInfo.CurrentInfo);
        }

        void CF_Screen_To_Memory_ProgressNotes()
        {
            Cf_Database_Oper.CF_ProgressNotes_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_ProgressNotes_Mem.V_Date = DateTime.Parse(Cf_ProgNotes_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_ProgressNotes_Mem.V_Observ = Cf_ProgNotes_SC_Observ.Text;
            Cf_Database_Oper.CF_ProgressNotes_Mem.V_NextDateApp = DateTime.Parse(Cf_ProgNotes_SC_NextDate.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_ProgressNotes_Mem.V_NextTimeApp = DateTime.Parse(Cf_ProgNotes_SC_NextTime.Text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.NoCurrentDateDefault);
        }

        void CF_LlenadoGrilla_ProgressNotes()
        {
            String[] RegistroProgressNotes = new String[4];

            SQLiteConnection dfCF_Connection;
            SQLiteCommand dfCF_Cmd_OtherExams;
            SQLiteDataReader dfCF_DReader_OtherExams;
            string dfCf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV,NEXTDATEAPP,NEXTTIMEAPP FROM CFPRONOTDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "' ORDER BY PDATE";

            Cf_ProgNotes_SC_GridView.SelectAll();
            Cf_ProgNotes_SC_GridView.ClearSelection();
            Cf_ProgNotes_SC_GridView.RowCount = 1;

            dfCF_Connection = Cf_Database_Oper.CF_Connection;
            dfCF_Cmd_OtherExams = dfCF_Connection.CreateCommand();
            dfCF_Cmd_OtherExams.CommandText = dfCf_SQL_SELECT;

            dfCF_DReader_OtherExams = dfCF_Cmd_OtherExams.ExecuteReader();

            // Llenar Grilla
            while (dfCF_DReader_OtherExams.Read())
            {
                RegistroProgressNotes[0] = DateTime.FromBinary(dfCF_DReader_OtherExams.GetInt64(dfCF_DReader_OtherExams.GetOrdinal("PDATE"))).ToShortDateString();
                RegistroProgressNotes[1] = dfCF_DReader_OtherExams.GetString(dfCF_DReader_OtherExams.GetOrdinal("OBSERV"));
                RegistroProgressNotes[2] = DateTime.FromBinary(dfCF_DReader_OtherExams.GetInt64(dfCF_DReader_OtherExams.GetOrdinal("NEXTDATEAPP"))).ToShortDateString();
                RegistroProgressNotes[3] = DateTime.FromBinary(dfCF_DReader_OtherExams.GetInt64(dfCF_DReader_OtherExams.GetOrdinal("NEXTTIMEAPP"))).ToString("t"); ;
                Cf_ProgNotes_SC_GridView.Rows.Add(RegistroProgressNotes);
            }
        }

        // Limpiar Pantalla de Appointments
        void CF_Limpiar_Appointments()
        {
            Cf_Appoint_SC_AppointDate.Text = Cf_Appoint_SC_APDateCalendar.Value.ToString("d");
            Cf_Appoint_SC_AppointTime.Text = Cf_Appoint_SC_APTimeCalendar.Value.ToString("t", CultureInfo.CreateSpecificCulture("hr-HR"));
            Cf_Appoint_SC_CardID.Text = "";
            Cf_Appoint_SC_Patient.Text = "";
            Cf_Appoint_SC_GridView.SelectAll();
            Cf_Appoint_SC_GridView.ClearSelection();
            Cf_Appoint_SC_GridView.RowCount = 1;
            // Cf_Appoint_SC_GridView;
            // Cf_Appoint_SC_GridTime;
            // Cf_Appoint_SC_GridCardNumber;
            // Cf_Appoint_SC_GridPatientName;
        }

        void CF_Screen_To_Memory_Appointments()
        {
            Cf_Database_Oper.CF_Appointments_Mem.V_CardID = Cf_Database_Oper.CF_BasicData_Mem.V_CardID;
            Cf_Database_Oper.CF_Appointments_Mem.V_DateAppoint = DateTime.Parse(Cf_Appoint_SC_AppointDate.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Database_Oper.CF_Appointments_Mem.V_TimeAppoint = DateTime.Parse(Cf_Appoint_SC_AppointTime.Text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.NoCurrentDateDefault);
            Cf_Database_Oper.CF_Appointments_Mem.V_Patient = Cf_Appoint_SC_Patient.Text;
        }

        void CF_Memory_To_Screen_Appointments()
        {
            Cf_Database_Oper.CF_GetDb_Appointments(Cf_Database_Oper.CF_DReader_Appointments);
            Cf_Appoint_SC_AppointDate.Text = Cf_Database_Oper.CF_Appointments_Mem.V_DateAppoint.ToShortDateString();
            Cf_Appoint_SC_APDateCalendar.Value = DateTime.Parse(Cf_Appoint_SC_AppointDate.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Appoint_SC_AppointTime.Text = Cf_Database_Oper.CF_Appointments_Mem.V_TimeAppoint.ToString("t");
            Cf_Appoint_SC_APTimeCalendar.Value = DateTime.Parse(Cf_Appoint_SC_AppointTime.Text, DateTimeFormatInfo.CurrentInfo);
            Cf_Appoint_SC_CardID.Text = Cf_Database_Oper.CF_Appointments_Mem.V_CardID;
            Cf_Appoint_SC_Patient.Text = Cf_Database_Oper.CF_Appointments_Mem.V_Patient;
        }

        void CF_LlenadoGrilla_Appointments()
        {
            String[] RegistroAppointments = new String[3];

            SQLiteConnection dfCF_Connection;
            SQLiteCommand dfCF_Cmd_OtherExams;
            SQLiteDataReader dfCF_DReader_OtherExams;
            string dfCf_SQL_SELECT = "SELECT ISTOPALM,DATEAPPOINT,TIMEAPPOINT,CARDNUMBER,PATIENT,LOCDATE FROM CFAPPOINTDB  WHERE DATEAPPOINT = " + DateTime.Parse(Cf_Appoint_SC_AppointDate.Text, DateTimeFormatInfo.CurrentInfo).ToBinary() + " ORDER BY TIMEAPPOINT";

            Cf_Appoint_SC_GridView.SelectAll();
            Cf_Appoint_SC_GridView.ClearSelection();
            Cf_Appoint_SC_GridView.RowCount = 1;

            dfCF_Connection = Cf_Database_Oper.CF_Connection;

            dfCF_Cmd_OtherExams = dfCF_Connection.CreateCommand();
            dfCF_Cmd_OtherExams.CommandText = dfCf_SQL_SELECT;

            dfCF_DReader_OtherExams = dfCF_Cmd_OtherExams.ExecuteReader();

            // Llenar Grilla
            while (dfCF_DReader_OtherExams.Read())
            {
                RegistroAppointments[0] = DateTime.FromBinary(dfCF_DReader_OtherExams.GetInt64(dfCF_DReader_OtherExams.GetOrdinal("TIMEAPPOINT"))).ToString("t");
                RegistroAppointments[1] = dfCF_DReader_OtherExams.GetString(dfCF_DReader_OtherExams.GetOrdinal("CARDNUMBER"));
                RegistroAppointments[2] = dfCF_DReader_OtherExams.GetString(dfCF_DReader_OtherExams.GetOrdinal("PATIENT"));
                Cf_Appoint_SC_GridView.Rows.Add(RegistroAppointments);
            }
        }

        // Limpiar Pantalla de Statistic
        void CF_Limpiar_Statistics()
        {
            Cf_Statistics_SC_ITreatments.Checked = true;
            Cf_Statistics_SC_IDiagnoses.Checked = true;

            Cf_Statistics_SC_Diagnoses.Text = "";
            Cf_Statistics_SC_Treatments.Text = "";

            Cf_Statistics_SC_GridView.SelectAll();
            Cf_Statistics_SC_GridView.ClearSelection();
            Cf_Statistics_SC_GridView.RowCount = 1;

            // Cf_Statistics_SC_GridView;
            // Cf_Statistics_SC_GridNumber;
            // Cf_Statistics_SC_GridCardNumber;
            // Cf_Statistics_SC_GridPatientName;
        }

        void CF_LlenadoGrilla_Statistics()
        {
            String[] RegistroStatistics = new String[4];
            int lSecuencia = 1;

            Cf_Statistics_SC_GridView.SelectAll();
            Cf_Statistics_SC_GridView.ClearSelection();
            Cf_Statistics_SC_GridView.RowCount = 1;

            // Recuperacion de Valores encontrados
            Cf_Database_Oper.CF_GetDb_Statistics(Cf_Database_Oper.CF_DReader_Statistics);
            // Llenar Grilla

            RegistroStatistics[0] = lSecuencia.ToString();
            RegistroStatistics[1] = Cf_Database_Oper.CF_Statistics_Mem.V_PDate.ToShortDateString();
            RegistroStatistics[2] = Cf_Database_Oper.CF_Statistics_Mem.V_CardID;
            RegistroStatistics[3] = Cf_Database_Oper.CF_Statistics_Mem.V_Patient;
            Cf_Statistics_SC_GridView.Rows.Add(RegistroStatistics);

            while (Cf_Database_Oper.CF_DReader_Statistics.Read())
            {
                lSecuencia++;
                RegistroStatistics[0] = lSecuencia.ToString();
                RegistroStatistics[1] = Cf_Database_Oper.CF_Statistics_Mem.V_PDate.ToShortDateString();
                RegistroStatistics[2] = Cf_Database_Oper.CF_Statistics_Mem.V_CardID;
                RegistroStatistics[3] = Cf_Database_Oper.CF_Statistics_Mem.V_Patient;
                Cf_Statistics_SC_GridView.Rows.Add(RegistroStatistics);
            }
        }

        private void CF_TP_Exams_Others_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (CF_Btn_OthersExams_Cancel.Enabled == true)
            {
                e.Cancel = true;
            }
        }

        private void CF_TP_Exams_Others_Selected(object sender, TabControlEventArgs e)
        {
            if (CF_TP_Progress_Notes.Text == e.TabPage.Text)
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Enabled = false;
                CF_Btn_OthersExams_New.Enabled = true;
                CF_Btn_OthersExams_Modify.Enabled = true;
                CF_Btn_OthersExams_Del.Enabled = true;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
            }
            else if (CF_TP_Appointments.Text == e.TabPage.Text)
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Enabled = true;
                CF_Btn_OthersExams_New.Enabled = false;
                CF_Btn_OthersExams_Modify.Enabled = false;
                CF_Btn_OthersExams_Del.Enabled = false;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
                CF_Limpiar_Appointments();
            }
            else if (CF_TP_Statistics.Text == e.TabPage.Text)
            {
                CardioFILE_Tools.Enabled = false;
                CF_Basic_Data_Panel.Enabled = false;
                CF_Btn_BasicData_Browser.Enabled = false;
                CF_Btn_OthersExams_Find.Enabled = true;
                CF_Btn_OthersExams_New.Enabled = true;
                CF_Btn_OthersExams_Modify.Enabled = false;
                CF_Btn_OthersExams_Del.Enabled = false;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
            }
            else
            {
                CardioFILE_Tools.Enabled = true;
                CF_Basic_Data_Panel.Enabled = true;
                CF_Btn_BasicData_Browser.Enabled = true;
                CF_Btn_OthersExams_Find.Enabled = true;
                CF_Btn_OthersExams_New.Enabled = true;
                CF_Btn_OthersExams_Modify.Enabled = true;
                CF_Btn_OthersExams_Del.Enabled = true;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
            }
        }

        private void CF_TP_Exams_Others_Deselected(object sender, TabControlEventArgs e)
        {
            if (CF_TP_Progress_Notes.Text == e.TabPage.Text)
            {
                CardioFILE_Tools.Enabled = true;
                CF_Basic_Data_Panel.Enabled = true;
                CF_Btn_BasicData_Browser.Enabled = true;
                CF_Btn_OthersExams_Find.Enabled = true;
                CF_Btn_OthersExams_New.Enabled = true;
                CF_Btn_OthersExams_Modify.Enabled = true;
                CF_Btn_OthersExams_Del.Enabled = true;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
            }
            else if (CF_TP_Appointments.Text == e.TabPage.Text)
            {
                CardioFILE_Tools.Enabled = true;
                Cf_Appoint_SC_CardID.Enabled = true;
                CF_Basic_Data_Panel.Enabled = true;
                CF_Btn_BasicData_Browser.Enabled = true;
                CF_Btn_OthersExams_Find.Enabled = true;
                CF_Btn_OthersExams_New.Enabled = true;
                CF_Btn_OthersExams_Modify.Enabled = true;
                CF_Btn_OthersExams_Del.Enabled = true;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
            }
            else if (CF_TP_Statistics.Text == e.TabPage.Text)
            {
                CardioFILE_Tools.Enabled = true;
                CF_Basic_Data_Panel.Enabled = true;
                CF_Btn_BasicData_Browser.Enabled = true;
                CF_Btn_OthersExams_Find.Enabled = true;
                CF_Btn_OthersExams_New.Enabled = true;
                CF_Btn_OthersExams_Modify.Enabled = true;
                CF_Btn_OthersExams_Del.Enabled = true;
                CF_Btn_OthersExams_Print.Enabled = true;
                CF_Btn_OthersExams_Cancel.Enabled = false;
            }
        }

        private void Cf_PhyExam_SC_Calendar_ValueChanged_1(object sender, EventArgs e)
        {
            Cf_PhyExam_SC_Date.Text = Cf_PhyExam_SC_Calendar.Value.ToString("d");
        }

        private void Cf_PhyExam_SC_Date_Leave(object sender, EventArgs e)
        {
            if (Cf_PhyExam_SC_Date.Text.Length >= 10)
                Cf_PhyExam_SC_Calendar.Value = DateTime.Parse(Cf_PhyExam_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
        }

        private void CF_Btn_BasicData_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_BasicData_Browser ListadoBasicData = new();
            ListadoBasicData.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoBasicData.CF_Recuperar_Listado_Pacientes();
            ListadoBasicData.ShowDialog();
            if (ListadoBasicData.GetTipoDeSalida() == 1)
                Cf_BasicData_SC_CardID.Text = ListadoBasicData.GetCardIDSeleccionado();
        }

        private void CF_Btn_PhysicalExamination_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoPhysicalExamination = new();
            ListadoPhysicalExamination.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoPhysicalExamination.SetSQL("SELECT CARDNUMBER, PDATE FROM CFPHYEXADB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoPhysicalExamination.CF_Recuperar_Listado_Examenes();
            ListadoPhysicalExamination.ShowDialog();
            if (ListadoPhysicalExamination.GetTipoDeSalida() == 1)
                Cf_PhyExam_SC_Date.Text = ListadoPhysicalExamination.GetDateSeleccionado();
        }

        private void CF_Btn_Radiography_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoRadiography = new();
            ListadoRadiography.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoRadiography.SetSQL("SELECT CARDNUMBER, PDATE FROM CFRADGRADB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoRadiography.CF_Recuperar_Listado_Examenes();
            ListadoRadiography.ShowDialog();
            if (ListadoRadiography.GetTipoDeSalida() == 1)
                Cf_Radiography_SC_Date.Text = ListadoRadiography.GetDateSeleccionado();
        }

        private void CF_Btn_EKGR_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoEKGR = new();
            ListadoEKGR.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoEKGR.SetSQL("SELECT CARDNUMBER, PDATE FROM CFEKGRDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoEKGR.CF_Recuperar_Listado_Examenes();
            ListadoEKGR.ShowDialog();
            if (ListadoEKGR.GetTipoDeSalida() == 1)
                Cf_EKGR_SC_Date.Text = ListadoEKGR.GetDateSeleccionado();
        }

        private void CF_Btn_EchoDoppler_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoEchoDoppler = new();
            ListadoEchoDoppler.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoEchoDoppler.SetSQL("SELECT CARDNUMBER, PDATE FROM CFECODOPDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoEchoDoppler.CF_Recuperar_Listado_Examenes();
            ListadoEchoDoppler.ShowDialog();
            if (ListadoEchoDoppler.GetTipoDeSalida() == 1)
                Cf_EchoDoppler_SC_Date.Text = ListadoEchoDoppler.GetDateSeleccionado();
        }

        private void CF_Btn_Holter_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoHolter = new();
            ListadoHolter.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoHolter.SetSQL("SELECT CARDNUMBER, PDATE FROM CFEXAHOLDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoHolter.CF_Recuperar_Listado_Examenes();
            ListadoHolter.ShowDialog();
            if (ListadoHolter.GetTipoDeSalida() == 1)
                Cf_Holter_SC_Date.Text = ListadoHolter.GetDateSeleccionado();
        }

        private void CF_Btn_StressTest_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoStressTest = new();
            ListadoStressTest.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoStressTest.SetSQL("SELECT CARDNUMBER, PDATE FROM CFSTRTESSDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoStressTest.CF_Recuperar_Listado_Examenes();
            ListadoStressTest.ShowDialog();
            if (ListadoStressTest.GetTipoDeSalida() == 1)
                Cf_StressTest_SC_Date.Text = ListadoStressTest.GetDateSeleccionado();
        }

        private void CF_Btn_CardiacCath_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoCardiacCath = new();
            ListadoCardiacCath.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoCardiacCath.SetSQL("SELECT CARDNUMBER, PDATE FROM CFCARCATDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoCardiacCath.CF_Recuperar_Listado_Examenes();
            ListadoCardiacCath.ShowDialog();
            if (ListadoCardiacCath.GetTipoDeSalida() == 1)
                Cf_CardiacCath_SC_Date.Text = ListadoCardiacCath.GetDateSeleccionado();
        }

        private void CF_Btn_Surgery_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoSurgery = new();
            ListadoSurgery.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoSurgery.SetSQL("SELECT CARDNUMBER, PDATE FROM CFSURGERYDB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoSurgery.CF_Recuperar_Listado_Examenes();
            ListadoSurgery.ShowDialog();
            if (ListadoSurgery.GetTipoDeSalida() == 1)
                Cf_Surgery_SC_Date.Text = ListadoSurgery.GetDateSeleccionado();
        }

        private void CF_Btn_DiagTreat_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_Other_Exams_Browser ListadoDiagTreat = new();
            ListadoDiagTreat.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoDiagTreat.SetSQL("SELECT CARDNUMBER, PDATE FROM CFDIATRADB WHERE CARDNUMBER = '" + Cf_Database_Oper.CF_BasicData_Mem.V_CardID + "'");
            ListadoDiagTreat.CF_Recuperar_Listado_Examenes();
            ListadoDiagTreat.ShowDialog();
            if (ListadoDiagTreat.GetTipoDeSalida() == 1)
                Cf_DiagTreatment_SC_Date.Text = ListadoDiagTreat.GetDateSeleccionado();
        }

        private void CF_Menu_File_Exit_Click(object sender, EventArgs e)
        {
            Cf_Database_Oper.CF_CerrarBaseDeDatos();
            System.Windows.Forms.Application.Exit();
        }

        private void CardioFILEPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cf_Database_Oper.CF_Connection.Close();
        }

        private void Cf_PhyExam_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_PhyExam_SC_Date.Text = Cf_PhyExam_SC_Calendar.Value.ToString("d");
        }

        private void Cf_Radiography_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_Radiography_SC_Date.Text = Cf_Radiography_SC_Calendar.Value.ToString("d");
        }

        private void Cf_EKGR_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_EKGR_SC_Date.Text = Cf_EKGR_SC_Calendar.Value.ToString("d");
        }

        private void Cf_EchoDoppler_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_EchoDoppler_SC_Date.Text = Cf_EchoDoppler_SC_Calendar.Value.ToString("d");
        }

        private void Cf_Holter_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_Holter_SC_Date.Text = Cf_Holter_SC_Calendar.Value.ToString("d");
        }

        private void Cf_StressTest_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_StressTest_SC_Date.Text = Cf_StressTest_SC_Calendar.Value.ToString("d");
        }

        private void Cf_CardiacCath_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_CardiacCath_SC_Date.Text = Cf_CardiacCath_SC_Calendar.Value.ToString("d");
        }

        private void Cf_Surgery_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_Surgery_SC_Date.Text = Cf_Surgery_SC_Calendar.Value.ToString("d");
        }

        private void Cf_DiagTreatment_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_DiagTreatment_SC_Date.Text = Cf_DiagTreatment_SC_Calendar.Value.ToString("d");
        }

        private void Cf_ProgNotes_SC_Calendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_ProgNotes_SC_Date.Text = Cf_ProgNotes_SC_Calendar.Value.ToString("d");
        }

        private void Cf_Appoint_SC_APCalendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_Appoint_SC_AppointDate.Text = Cf_Appoint_SC_APDateCalendar.Value.ToString("d");
        }

        private void Cf_ProgNotes_SC_NextDateCalendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_ProgNotes_SC_NextDate.Text = Cf_ProgNotes_SC_NextDateCalendar.Value.ToString("d");
        }

        private void Cf_ProgNotes_SC_NextTimeCalendar_ValueChanged(object sender, EventArgs e)
        {
            Cf_ProgNotes_SC_NextTime.Text = Cf_ProgNotes_SC_NextTimeCalendar.Value.ToString("t", CultureInfo.CreateSpecificCulture("hr-HR"));
        }

        private void CardioFILEPrincipal_Load(object sender, EventArgs e)
        {
            // Cambio a Español o Ingles
            // Debo agregarlo en alguna configuración
            // Verificar existencia de Bases de Datos
        }

        private void CF_Btn_OthersExams_Print_Click(object sender, EventArgs e)
        {
            CF_Form_Report_Recipe CF_Dialog_Report_Recipe = new();

            if (Cf_Database_Oper.CF_Select_User() == true)
            {
                Cf_Database_Oper.CF_GetDb_User(Cf_Database_Oper.CF_DReader_User);
            }

            // Datos del Medico
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_Name", Cf_Database_Oper.CF_User_Mem.V_Name);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_Especiality", Cf_Database_Oper.CF_User_Mem.V_Especiality);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_ID", Cf_Database_Oper.CF_User_Mem.V_MDID);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_Address1", Cf_Database_Oper.CF_User_Mem.V_Address1);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_Address2", Cf_Database_Oper.CF_User_Mem.V_Address2);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_Phone", Cf_Database_Oper.CF_User_Mem.V_Phone);
            // Datos del Paciente
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_Patient", Cf_BasicData_SC_FirstName.Text + ' ' + Cf_BasicData_SC_LastName.Text);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_Address", Cf_BasicData_SC_Address.Text);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_RutCiSSN", Cf_BasicData_SC_SSNIDCI.Text);

            DateTime AgePrescription = DateTime.Parse(Cf_BasicData_SC_BDate.Text, DateTimeFormatInfo.CurrentInfo);
            int YearPrescription = DateTime.Now.Year - AgePrescription.Year;
            int MonthPrescription = DateTime.Now.Month - AgePrescription.Month;
            int DayPrescription = DateTime.Now.Day - AgePrescription.Day;

            if (YearPrescription < 0)
                YearPrescription = 0;

            if (MonthPrescription < 0)
                MonthPrescription = 0;

            if (DayPrescription < 0)
                DayPrescription = 0;

            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_Age", " Y " + YearPrescription.ToString() + " M " + MonthPrescription.ToString() + " D " + DayPrescription.ToString());
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_Sex", Cf_BasicData_SC_Sex.Text);
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_Date", Cf_DiagTreatment_SC_Date.Text);
            // Datos del Tratamiento
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_Treatments", Cf_DiagTreatment_SC_Treatment.Text);
            // Datos CardioFILE
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_MD_EMail", Cf_Database_Oper.CF_User_Mem.V_eMail);
            // Informa estado de registro
            CF_Dialog_Report_Recipe.Set_CF_Parameter("Report_CF_Status", CF_StatusReg);
            CF_Dialog_Report_Recipe.ShowDialog();
        }

        private void Cf_ProgNotes_SC_GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell ColCellDate;
            DataGridViewCell ColCellObserv;
            DataGridViewCell ColCellNextDate;
            DataGridViewCell ColCellNextTime;

            string sColCellDate;
            string sColCellObserv;
            string sColCellNextDate;
            string sColCellNextTime;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                ColCellDate = Cf_ProgNotes_SC_GridView.Rows[e.RowIndex].Cells[0];
                ColCellObserv = Cf_ProgNotes_SC_GridView.Rows[e.RowIndex].Cells[1];
                ColCellNextDate = Cf_ProgNotes_SC_GridView.Rows[e.RowIndex].Cells[2];
                ColCellNextTime = Cf_ProgNotes_SC_GridView.Rows[e.RowIndex].Cells[3];

                sColCellDate = (String)ColCellDate.Value;
                sColCellObserv = (String)ColCellObserv.Value;
                sColCellNextDate = (String)ColCellNextDate.Value;
                sColCellNextTime = (String)ColCellNextTime.Value;

                try
                {
                    if (Cf_Database_Oper.CF_Select_ProgressNotes(Cf_BasicData_SC_CardID.Text, DateTime.Parse(sColCellDate, DateTimeFormatInfo.CurrentInfo)))
                    {
                        Cf_Database_Oper.CF_GetDb_ProgressNotes(Cf_Database_Oper.CF_DReader_ProgressNotes);
                        Cf_ProgNotes_SC_Date.Text = sColCellDate;
                        Cf_ProgNotes_SC_Calendar.Value = DateTime.Parse(Cf_ProgNotes_SC_Date.Text, DateTimeFormatInfo.CurrentInfo);
                        Cf_ProgNotes_SC_Observ.Text = sColCellObserv;
                        Cf_ProgNotes_SC_NextDate.Text = sColCellNextDate;
                        Cf_ProgNotes_SC_NextDateCalendar.Value = DateTime.Parse(Cf_ProgNotes_SC_NextDate.Text, DateTimeFormatInfo.CurrentInfo);
                        Cf_ProgNotes_SC_NextTime.Text = sColCellNextTime;
                        Cf_ProgNotes_SC_NextTimeCalendar.Value = DateTime.Parse(Cf_ProgNotes_SC_NextTime.Text, DateTimeFormatInfo.CurrentInfo);
                    }
                }
                catch (System.ArgumentNullException)
                {
                    ;
                }
            }
        }

        private void Cf_Appoint_SC_CardID_Leave(object sender, EventArgs e)
        {
            string pCardID_Z;
            pCardID_Z = Cf_Appoint_SC_CardID.Text;
            Cf_Appoint_SC_Patient.Text = Cf_Database_Oper.CF_Find_Patients(pCardID_Z);
        }

        private void Cf_Appoint_SC_GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell ColCellTime;
            DataGridViewCell ColCellCardID;
            DataGridViewCell ColCellPatients;

            string sColCellTime;
            string sColCellCardID;
            string sColCellPatients;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                ColCellTime = Cf_Appoint_SC_GridView.Rows[e.RowIndex].Cells[0];
                ColCellCardID = Cf_Appoint_SC_GridView.Rows[e.RowIndex].Cells[1];
                ColCellPatients = Cf_Appoint_SC_GridView.Rows[e.RowIndex].Cells[2];

                sColCellTime = (String)ColCellTime.Value;
                sColCellCardID = (String)ColCellCardID.Value;
                sColCellPatients = (String)ColCellPatients.Value;

                try
                {
                    if (Cf_Database_Oper.CF_Select_Appointments(DateTime.Parse(Cf_Appoint_SC_AppointDate.Text, DateTimeFormatInfo.CurrentInfo), sColCellCardID))
                    {
                        Cf_Database_Oper.CF_GetDb_ProgressNotes(Cf_Database_Oper.CF_DReader_ProgressNotes);
                        Cf_Appoint_SC_AppointTime.Text = sColCellTime;
                        Cf_Appoint_SC_APTimeCalendar.Value = DateTime.Parse(Cf_Appoint_SC_AppointTime.Text, DateTimeFormatInfo.CurrentInfo);
                        Cf_Appoint_SC_CardID.Text = sColCellCardID;
                        Cf_Appoint_SC_Patient.Text = sColCellPatients;
                    }
                }
                catch (System.ArgumentNullException)
                {
                    ;
                }
            }
        }

        private void CF_Btn_AppointCardID_Browser_Click(object sender, EventArgs e)
        {
            CF_Dialog_BasicData_Browser ListadoAppointsPatients = new();
            ListadoAppointsPatients.SetDbConection(Cf_Database_Oper.CF_Connection);
            ListadoAppointsPatients.CF_Recuperar_Listado_Pacientes();
            ListadoAppointsPatients.ShowDialog();
            if (ListadoAppointsPatients.GetTipoDeSalida() == 1)
                Cf_Appoint_SC_CardID.Text = ListadoAppointsPatients.GetCardIDSeleccionado();
        }

        private void Cf_Appoint_SC_CardID_TextChanged(object sender, EventArgs e)
        {

        }

        private void CF_Btn_Basic_Data_Print_Click(object sender, EventArgs e)
        {
            CF_Form_Report_Historic CF_Dialog_Report_Historic = new();
            if (Cf_Database_Oper.CF_Select_User() == true)
            {
                Cf_Database_Oper.CF_GetDb_User(Cf_Database_Oper.CF_DReader_User);
            }
            // Datos del Medico
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_MD_Name", Cf_Database_Oper.CF_User_Mem.V_Name);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_MD_Especiality", Cf_Database_Oper.CF_User_Mem.V_Especiality);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_MD_ID", Cf_Database_Oper.CF_User_Mem.V_MDID);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_MD_Address1", Cf_Database_Oper.CF_User_Mem.V_Address1);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_MD_Address2", Cf_Database_Oper.CF_User_Mem.V_Address2);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_MD_Phone", Cf_Database_Oper.CF_User_Mem.V_Phone);
            // Basic Data
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_CardID", Cf_BasicData_SC_CardID.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_SSNIDCI", Cf_BasicData_SC_SSNIDCI.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_LastName", Cf_BasicData_SC_LastName.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_FirstName", Cf_BasicData_SC_FirstName.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_BDate", Cf_BasicData_SC_BDate.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_Sex", Cf_BasicData_SC_Sex.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_BloodType", Cf_BasicData_SC_BloodType.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_RHFACTOR", Cf_BasicData_SC_RH_Factor.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_Address", Cf_BasicData_SC_Address.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_Phone", Cf_BasicData_SC_Phone.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_ZipCode", Cf_BasicData_SC_ZipCode.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_eMail", Cf_BasicData_SC_EMail.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_ReCons", Cf_BasicData_SC_ReCons.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_BasicData_State", Cf_BasicData_SC_State.Text);
            // Physical Examination
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_Date", Cf_PhyExam_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_FamiAnt", Cf_PhyExam_SC_FamiAnt.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_PersAnt", Cf_PhyExam_SC_PersAnt.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_BPressMax", Cf_PhyExam_SC_BPresMax.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_BPresMin", Cf_PhyExam_SC_BPresMin.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_Pulse", Cf_PhyExam_SC_Pulse.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_Height", Cf_PhyExam_SC_Height.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_Weight", Cf_PhyExam_SC_Weight.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_BMI", Cf_PhyExam_SC_BMI.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_PhyExam_Observ", Cf_ProgNotes_SC_Observ.Text);
            // Radiography
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_Radiography_Date", Cf_Radiography_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_Radiography_Observ", Cf_Radiography_SC_Observ.Text);
            // EKGR
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_EKGR_Date", Cf_EKGR_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_EKGR_Observ", Cf_EKGR_SC_Observ.Text);
            // Echo/Doppler/Laboratory
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_EchoDoppler_Date", Cf_EchoDoppler_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_EchoDoppler_Echo", Cf_EchoDoppler_SC_Echo.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_EchoDoppler_Doppler", Cf_EchoDoppler_SC_Doppler.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_EchoDoppler_Laboratory", Cf_EchoDoppler_SC_Laboratory.Text);
            // Holter
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_Holter_Date", Cf_Holter_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_Holter_Observ", Cf_Holter_SC_Observ.Text);
            // Stress Test
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Date", Cf_StressTest_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_BloodPress", Cf_StressTest_SC_BloodPress.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Arrhyth", Cf_StressTest_SC_Arrhyth.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_PercentageMax", Cf_StressTest_SC_PercentageMax.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_CardiacFrec", Cf_StressTest_SC_CardiacFrec.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Time", Cf_StressTest_SC_Time.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Stage", Cf_StressTest_SC_Stage.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Protocol", Cf_StressTest_SC_Protocol.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_DescSTT", Cf_StressTest_SC_DescSTT.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Result", Cf_StressTest_SC_Result.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_ReSusp", Cf_StressTest_SC_ReSusp.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_FunCap", Cf_StressTest_SC_FunCap.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_StressTest_Type", Cf_StressTest_SC_Type.Text);
            // Cardiac Cath
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_CardiacCath_Date", Cf_CardiacCath_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_CardiacCath_Observ", Cf_CardiacCath_SC_Observ.Text);
            // Surgery
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_Surgery_Date", Cf_Surgery_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_Surgery_Observ", Cf_Surgery_SC_Observ.Text);
            // Datos del Diagnoses and Tratamiento
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_DiagTreatment_Date", Cf_DiagTreatment_SC_Date.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_DiagTreatment_Diagnoses", Cf_DiagTreatment_SC_Diagnoses.Text);
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_DiagTreatment_Treatment", Cf_DiagTreatment_SC_Treatment.Text);
            // Informa estado de registro
            CF_Dialog_Report_Historic.Set_CF_Parameter("Report_CF_Status", CF_StatusReg);
            CF_Dialog_Report_Historic.ShowDialog();
        }

        private void CF_Mnu_About_Click(object sender, EventArgs e)
        {
            CF_About CF_About_CardioFILE_Dlg = new();
            CF_About_CardioFILE_Dlg.ShowDialog();
        }

        private void CF_Mnu_ConfigureDatabase_Click(object sender, EventArgs e)
        {
            string CF_DirectorioObtenido;
            Cf_Database_Oper.CF_CerrarBaseDeDatos();
            CF_ConfigureDatabase CF_ConfigureDatabase_Dlg = new();
            CF_ConfigureDatabase_Dlg.Set_Database_Directory(RegWinCardioFILE.ObtenerDirectorio());
            // CF_ConfigureDatabase_Dlg.SetLocalPrincipal(this);
            CF_ConfigureDatabase_Dlg.ShowDialog();
            CF_DirectorioObtenido = RegWinCardioFILE.ObtenerDirectorio();
            Cf_Database_Oper.CF_OpenDatabase(CF_DirectorioObtenido);
        }

        private void CF_Mnu_ConfigurePrinter_Click(object sender, EventArgs e)
        {
            CF_PrintDialog.ShowDialog();
        }

        public string ByteToHex(byte[] bytes)
        {
            StringBuilder s = new();
            foreach (byte b in bytes)
                s.Append(b.ToString("x2"));
            return s.ToString();
        }

        private void CF_Mnu_RegisterCardioFILE_Click(object sender, EventArgs e)
        {
            CF_Register_CardioFILE CF_Register_CardioFILE_Dlg = new();
            CF_Register_CardioFILE_Dlg.Set_Database_Register(Cf_Database_Oper);
            CF_Register_CardioFILE_Dlg.SetLocalPrincipal(this);
            CF_Register_CardioFILE_Dlg.ShowDialog();
        }
    }
}
