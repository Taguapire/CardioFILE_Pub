using System;
using System.Windows.Forms;
using System.Data.SQLite;

// Procesar Acceso a la Base de Datos
namespace CardioFILE_Pub
{
    public class CF_Database_Class
    {
        // Variable de Identificación
        public static CF_Autor CF_Identify = new();

        // Variables de Bases de Datos
        public SQLiteFactory CF_Database;
        public SQLiteConnection CF_Connection;

        public SQLiteCommand CF_Cmd_BasicData;
        public SQLiteCommand CF_Cmd_PhysicalExamination;
        public SQLiteCommand CF_Cmd_Radiography;
        public SQLiteCommand CF_Cmd_EKGR;
        public SQLiteCommand CF_Cmd_EchoDoppler;
        public SQLiteCommand CF_Cmd_Holter;
        public SQLiteCommand CF_Cmd_StressTest;
        public SQLiteCommand CF_Cmd_CardiacCath;
        public SQLiteCommand CF_Cmd_Surgery;
        public SQLiteCommand CF_Cmd_DiagnosesTreatments;
        public SQLiteCommand CF_Cmd_ProgressNotes;
        public SQLiteCommand CF_Cmd_Appointments;
        public SQLiteCommand CF_Cmd_Statistics;
        public SQLiteCommand CF_Cmd_User;

        public SQLiteDataReader CF_DReader_BasicData;
        public SQLiteDataReader CF_DReader_PhysicalExamination;
        public SQLiteDataReader CF_DReader_Radiography;
        public SQLiteDataReader CF_DReader_EKGR;
        public SQLiteDataReader CF_DReader_EchoDoppler;
        public SQLiteDataReader CF_DReader_Holter;
        public SQLiteDataReader CF_DReader_StressTest;
        public SQLiteDataReader CF_DReader_CardiacCath;
        public SQLiteDataReader CF_DReader_Surgery;
        public SQLiteDataReader CF_DReader_DiagnosesTreatments;
        public SQLiteDataReader CF_DReader_ProgressNotes;
        public SQLiteDataReader CF_DReader_Appointments;
        public SQLiteDataReader CF_DReader_Statistics;
        public SQLiteDataReader CF_DReader_User;

        // Variables de Memoria
        public CF_BasicData_Type CF_BasicData_Mem = new();
        public CF_PhysicalExamination_Type CF_PhysicalExamination_Mem = new();
        public CF_Radiography_Type CF_Radiography_Mem = new();
        public CF_EKGR_Type CF_EKGR_Mem = new();
        public CF_EchoDoppler_Type CF_EchoDoppler_Mem = new();
        public CF_Holter_Type CF_Holter_Mem = new();
        public CF_StressTest_Type CF_StressTest_Mem = new();
        public CF_CardiacCath_Type CF_CardiacCath_Mem = new();
        public CF_Surgery_Type CF_Surgery_Mem = new();
        public CF_DiagnosesTreatments_Type CF_DiagnosesTreatments_Mem = new();
        public CF_ProgressNotes_Type CF_ProgressNotes_Mem = new();
        public CF_Appointments_Type CF_Appointments_Mem = new();
        public CF_Statistics_Type CF_Statistics_Mem = new();
        public CF_User_Type CF_User_Mem = new();

        // Utilitarios de Bases de Datos
        public void CF_CreateDatabase(string XCF_Directorio)
        {
            // Desde aqui debe ser llamada la creación de la Base de Datos
            string localDataFolder = XCF_Directorio;
            CF_Database = new SQLiteFactory();
            // Crear Automaticamente la Base de Datos
            SQLiteConnection.CreateFile(localDataFolder + @"\CardioFILE.db");
            //CF_Connection.SetPassword("CardioFILE");
            //CF_Connection.ChangePassword("CardioFILE");
            //Agregar Tablas y Objetos.
            //Console.WriteLine(localDataFolder);
        }

        public void CF_CerrarBaseDeDatos()
        {
            CF_Database.Dispose();
            CF_Connection.Close();
        }

        public void CF_OpenDatabase(string XCF_Directorio)
        {
            string lCF_Directorio = XCF_Directorio;
            string lCF_Consultas_Schema = "SELECT COUNT(*) FROM SQLite_MASTER WHERE type = 'table' and NAME LIKE 'CF%'";
            int lCF_Numero_Tablas = 0;
            const int lCF_NUMERO_TABLAS = 19;
            SQLiteCommand CF_Cmd_Schema;
            SQLiteDataReader CF_DReader_Schema;
            CF_Database = new SQLiteFactory();
            CF_Connection = (SQLiteConnection)CF_Database.CreateConnection();
            // *************************************************************************************
            // Se debe crear un procedimiento para la definicion de Bases de Datos y su localización
            // *************************************************************************************
            try
            {
                lCF_Directorio += @"\CardioFILE.db";
                CF_Connection.ConnectionString = "Data Source = " + lCF_Directorio;
                CF_Connection.Open();
                // Verificar si estan las tablas completas
                // SELECT * FROM SQLite_MASTER  = 'table' and NAME LIKE 'CF%';
                CF_Cmd_Schema = CF_Connection.CreateCommand();
                CF_Cmd_Schema.CommandText = lCF_Consultas_Schema;
                CF_DReader_Schema = CF_Cmd_Schema.ExecuteReader();

                if (CF_DReader_Schema.Read())
                    lCF_Numero_Tablas = CF_DReader_Schema.GetInt16(0);
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                MessageBox.Show("Error abriendo la Base de Datos", "escriba a contacto@pentalpha.net");
                // DialogResult result = MessageBox.Show("Error open Database", "Call to Support CardioFILE");
                // Consultar si debe crearse la Base de Datos
            }

            // Si hay problemas debe llamarse a CF_CreateDatabase
            // Estudiar el caso *OJO*
            // Mejorar estas tareas
            if (lCF_Numero_Tablas == (int)0)
            {
                CF_InicializarDB CF_Crear_Base_de_Datos = new(CF_Connection);
                CF_Crear_Base_de_Datos.Proc_Crear_tablas();
                CF_Crear_Base_de_Datos.Proc_Crear_indices();
                CF_Crear_Base_de_Datos.Proc_Crear_triggers();
                CF_Crear_Base_de_Datos.Proc_Datos_Iniciales();
            }
            else if (lCF_Numero_Tablas < lCF_NUMERO_TABLAS)
            {
                CF_InicializarDB CF_Crear_Base_de_Datos = new(CF_Connection);
                CF_Crear_Base_de_Datos.Proc_Crear_tablas();
                CF_Crear_Base_de_Datos.Proc_Crear_indices();
                CF_Crear_Base_de_Datos.Proc_Crear_triggers();
                CF_Crear_Base_de_Datos.Proc_Datos_Iniciales();
            }
            else if (lCF_Numero_Tablas >= lCF_NUMERO_TABLAS)
            {
                MessageBox.Show("Base de Datos Correcta", CF_Identify.SoftwareFull, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void CF_CompaqDatabase()
        {
            // VACUUM
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON BASIC DATA
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE BASIC DATA
        //**************************************************************************************************

        public string CF_Find_Patients(string pCardID)
        {
            // Operacion SQL
            SQLiteCommand CF_Cmd_Patients;
            SQLiteDataReader CF_DReader_Patients;

            string Cf_Patient_Name;

            string Cf_SQL_SELECT_Patients = "SELECT LASTNAME,FIRSTNAME FROM CFBASDATDB";
            Cf_SQL_SELECT_Patients += " WHERE CARDNUMBER = '" + pCardID + "'";

            CF_Cmd_Patients = CF_Connection.CreateCommand();
            CF_Cmd_Patients.CommandText = Cf_SQL_SELECT_Patients;

            CF_DReader_Patients = CF_Cmd_Patients.ExecuteReader();

            if (CF_DReader_Patients.Read())
            {
                Cf_Patient_Name = CF_DReader_Patients.GetString(CF_DReader_Patients.GetOrdinal("LASTNAME"));
                Cf_Patient_Name += " ";
                Cf_Patient_Name += CF_DReader_Patients.GetString(CF_DReader_Patients.GetOrdinal("FIRSTNAME"));
            }
            else
            {
                Cf_Patient_Name = "No encontrado";
            }

            return Cf_Patient_Name;
        }

        public bool CF_Select_BasicData(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,SSN,LASTNAME,FIRSTNAME,BDATE,SEX,ADDRESS,PHONENUMBER,ZIPCODE,RECONS,PSTATE,PHONETYPE,EMAIL,BLOODGROUP,BLOODFACTOR FROM CFBASDATDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_BasicData = CF_Connection.CreateCommand();
            CF_Cmd_BasicData.CommandText = Cf_SQL_SELECT;

            CF_DReader_BasicData = CF_Cmd_BasicData.ExecuteReader();

            return CF_DReader_BasicData.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_BasicData()
        {
            return CF_DReader_BasicData.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_BasicData(SQLiteDataReader pCF_DReader_BasicData)
        {
            CF_BasicData_Mem.V_CardID = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("CARDNUMBER"));
            CF_BasicData_Mem.V_SSNIDCI = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("SSN"));
            CF_BasicData_Mem.V_LastName = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("LASTNAME"));
            CF_BasicData_Mem.V_FirstName = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("FIRSTNAME"));
            CF_BasicData_Mem.V_BirthDate = DateTime.FromBinary(pCF_DReader_BasicData.GetInt64(pCF_DReader_BasicData.GetOrdinal("BDATE")));
            CF_BasicData_Mem.V_Sex = pCF_DReader_BasicData.GetInt16(pCF_DReader_BasicData.GetOrdinal("SEX"));
            CF_BasicData_Mem.V_Address = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("ADDRESS"));
            CF_BasicData_Mem.V_Phone = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("PHONENUMBER"));
            CF_BasicData_Mem.V_ZipCode = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("ZIPCODE"));
            CF_BasicData_Mem.V_ReCons = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("RECONS"));
            CF_BasicData_Mem.V_State = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("PSTATE"));
            CF_BasicData_Mem.V_PhoneType = pCF_DReader_BasicData.GetInt32(pCF_DReader_BasicData.GetOrdinal("PHONETYPE"));
            CF_BasicData_Mem.V_EMail = pCF_DReader_BasicData.GetString(pCF_DReader_BasicData.GetOrdinal("EMAIL"));
            CF_BasicData_Mem.V_BloodType = pCF_DReader_BasicData.GetInt32(pCF_DReader_BasicData.GetOrdinal("BLOODGROUP"));
            CF_BasicData_Mem.V_BloodFactorRH = pCF_DReader_BasicData.GetInt32(pCF_DReader_BasicData.GetOrdinal("BLOODFACTOR"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE BASIC DATA
        //**************************************************************************************************
        public bool CF_Insert_BasicData()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            SQLiteTransaction CF_Transaction_X;
            int Numero_Registros;

            string Cf_SQL_INSERT = "INSERT INTO CFBASDATDB(ISTOPALM,CARDNUMBER,SSN,LASTNAME,FIRSTNAME,BDATE,SEX,ADDRESS,PHONENUMBER,ZIPCODE,RECONS,PSTATE,PHONETYPE,EMAIL,BLOODGROUP,BLOODFACTOR) VALUES (";

            Cf_SQL_INSERT += "0" + ",";                                                  // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_CardID + "',";                 // CARDNUMBER
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_SSNIDCI + "',";                // SSN
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_LastName + "',";               // LASTNAME
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_FirstName + "',";              // FIRSTNAME
            Cf_SQL_INSERT += " " + CF_BasicData_Mem.V_BirthDate.ToBinary() + ",";    // BDATE
            Cf_SQL_INSERT += " " + CF_BasicData_Mem.V_Sex.ToString() + ",";          // SEX
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_Address + "',";                // ADDRESS
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_Phone + "',";                  // PHONENUMBER
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_ZipCode + "',";                // ZIPCODE
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_ReCons + "',";                 // RECONS
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_State + "',";                  // PSTATE
            Cf_SQL_INSERT += " " + CF_BasicData_Mem.V_PhoneType.ToString() + ",";    // PHONETYPE
            Cf_SQL_INSERT += "'" + CF_BasicData_Mem.V_EMail + "',";                  // EMAIL
            Cf_SQL_INSERT += " " + CF_BasicData_Mem.V_BloodType.ToString() + ",";    // BLOOD GROUP
            Cf_SQL_INSERT += " " + CF_BasicData_Mem.V_BloodFactorRH.ToString() + ")";// BLOOD FACTOR RH

            if (CF_BasicData_Mem.V_CardID == "")
            {
                CF_BasicData_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_BasicData = CF_Connection.CreateCommand();
            CF_Cmd_BasicData.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_BasicData.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_BasicData_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE BASIC DATA
        //**************************************************************************************************
        public bool CF_Delete_BasicData(string pCardID)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFBASDATDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "'";
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_BasicData = CF_Connection.CreateCommand();
            CF_Cmd_BasicData.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_BasicData.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_BasicData_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE BASIC DATA
        //**************************************************************************************************
        public bool CF_Modify_BasicData(string pCardID)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFBASDATDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_BasicData_Mem.V_CardID + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "SSN = '" + CF_BasicData_Mem.V_SSNIDCI + "',";
            Cf_SQL_UPDATE += "LASTNAME = '" + CF_BasicData_Mem.V_LastName + "',";
            Cf_SQL_UPDATE += "FIRSTNAME = '" + CF_BasicData_Mem.V_FirstName + "',";
            Cf_SQL_UPDATE += "BDATE = " + CF_BasicData_Mem.V_BirthDate.ToBinary() + ",";
            Cf_SQL_UPDATE += "SEX = " + CF_BasicData_Mem.V_Sex.ToString() + ",";
            Cf_SQL_UPDATE += "ADDRESS = '" + CF_BasicData_Mem.V_Address + "',";
            Cf_SQL_UPDATE += "PHONENUMBER = '" + CF_BasicData_Mem.V_Phone + "',";
            Cf_SQL_UPDATE += "ZIPCODE = '" + CF_BasicData_Mem.V_ZipCode + "',";
            Cf_SQL_UPDATE += "RECONS = '" + CF_BasicData_Mem.V_ReCons + "',";
            Cf_SQL_UPDATE += "PSTATE = '" + CF_BasicData_Mem.V_State + "',";
            Cf_SQL_UPDATE += "PHONETYPE = " + CF_BasicData_Mem.V_PhoneType.ToString() + ",";
            Cf_SQL_UPDATE += "EMAIL = '" + CF_BasicData_Mem.V_EMail + "',";
            Cf_SQL_UPDATE += "BLOODGROUP = " + CF_BasicData_Mem.V_BloodType.ToString() + ",";
            Cf_SQL_UPDATE += "BLOODFACTOR = " + CF_BasicData_Mem.V_BloodFactorRH.ToString();

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "'";
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_BasicData = CF_Connection.CreateCommand();
            CF_Cmd_BasicData.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_BasicData.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_BasicData_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON PHYSICAL EXAMINATION
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE PHYSICAL EXAMINATION 
        //**************************************************************************************************
        public bool CF_Select_PhysicalExamination(string pCardID)
        {
            // Operacion SQL

            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,FAMIANT,PERSANT,BLOODPRES_MAX,BLOODPRES_MIN,PULSE,HEIGHT,WEIGHT,BMI,OBSERV,HEIGHT_ITEM,WEIGHT_ITEM FROM CFPHYEXADB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_PhysicalExamination = CF_Connection.CreateCommand();
            CF_Cmd_PhysicalExamination.CommandText = Cf_SQL_SELECT;

            CF_DReader_PhysicalExamination = CF_Cmd_PhysicalExamination.ExecuteReader();
            return CF_DReader_PhysicalExamination.Read();
        }

        public bool CF_Select_PhysicalExamination(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,FAMIANT,PERSANT,BLOODPRES_MAX,BLOODPRES_MIN,PULSE,HEIGHT,WEIGHT,BMI,OBSERV,HEIGHT_ITEM,WEIGHT_ITEM FROM CFPHYEXADB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_PhysicalExamination = CF_Connection.CreateCommand();
            CF_Cmd_PhysicalExamination.CommandText = Cf_SQL_SELECT;

            CF_DReader_PhysicalExamination = CF_Cmd_PhysicalExamination.ExecuteReader();
            return CF_DReader_PhysicalExamination.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_PhysicalExamination()
        {
            return CF_DReader_PhysicalExamination.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_PhysicalExamination(SQLiteDataReader pCF_DReader_PhysicalExamination)
        {
            CF_PhysicalExamination_Mem.V_CardID = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("CARDNUMBER"));
            CF_PhysicalExamination_Mem.V_Date = DateTime.FromBinary(pCF_DReader_PhysicalExamination.GetInt64(pCF_DReader_PhysicalExamination.GetOrdinal("PDATE")));
            CF_PhysicalExamination_Mem.V_FamiAnt = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("FAMIANT"));
            CF_PhysicalExamination_Mem.V_PersAnt = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("PERSANT"));
            CF_PhysicalExamination_Mem.V_BloodPresMax = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("BLOODPRES_MAX"));
            CF_PhysicalExamination_Mem.V_BloodPresMin = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("BLOODPRES_MIN"));
            CF_PhysicalExamination_Mem.V_Pulse = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("PULSE"));
            CF_PhysicalExamination_Mem.V_Height = pCF_DReader_PhysicalExamination.GetFloat(pCF_DReader_PhysicalExamination.GetOrdinal("HEIGHT"));
            CF_PhysicalExamination_Mem.V_Weight = pCF_DReader_PhysicalExamination.GetFloat(pCF_DReader_PhysicalExamination.GetOrdinal("WEIGHT"));
            CF_PhysicalExamination_Mem.V_BMI = pCF_DReader_PhysicalExamination.GetFloat(pCF_DReader_PhysicalExamination.GetOrdinal("BMI"));
            CF_PhysicalExamination_Mem.V_Observ = pCF_DReader_PhysicalExamination.GetString(pCF_DReader_PhysicalExamination.GetOrdinal("OBSERV"));
            CF_PhysicalExamination_Mem.V_Height_Item = pCF_DReader_PhysicalExamination.GetInt16(pCF_DReader_PhysicalExamination.GetOrdinal("HEIGHT_ITEM"));
            CF_PhysicalExamination_Mem.V_Weight_Item = pCF_DReader_PhysicalExamination.GetInt16(pCF_DReader_PhysicalExamination.GetOrdinal("WEIGHT_ITEM"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE PHYSICAL EXAMINATION
        //**************************************************************************************************
        public bool CF_Insert_PhysicalExamination()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;
            string Cf_SQL_INSERT = "INSERT INTO CFPHYEXADB (ISTOPALM,CARDNUMBER,PDATE,FAMIANT,PERSANT,BLOODPRES_MAX,BLOODPRES_MIN,PULSE,HEIGHT,WEIGHT,BMI,OBSERV,HEIGHT_ITEM,WEIGHT_ITEM) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                                              // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_CardID + "',";                   // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_PhysicalExamination_Mem.V_Date.ToBinary() + ",";           // PDATE
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_FamiAnt + "',";                  // FAMIANT
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_PersAnt + "',";                  // PERSANT
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_BloodPresMax + "',";             // BLODPRES_MAX
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_BloodPresMin + "',";             // BLODPRES_MIN
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_Pulse + "',";                    // PULSE
            Cf_SQL_INSERT += " " + CF_PhysicalExamination_Mem.V_Height.ToString() + ",";         // HEIGHT
            Cf_SQL_INSERT += " " + CF_PhysicalExamination_Mem.V_Weight.ToString() + ",";         // WEIGHT
            Cf_SQL_INSERT += " " + CF_PhysicalExamination_Mem.V_BMI.ToString() + ",";            // BMI
            Cf_SQL_INSERT += "'" + CF_PhysicalExamination_Mem.V_Observ + "',";                   // OBSERV
            Cf_SQL_INSERT += " " + CF_PhysicalExamination_Mem.V_Height_Item.ToString() + ",";    // HEIGHT_ITEM
            Cf_SQL_INSERT += " " + CF_PhysicalExamination_Mem.V_Weight_Item.ToString() + ")";    // WEIGHT_ITEM

            try
            {
                int VerificarVacio = CF_PhysicalExamination_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_PhysicalExamination_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_PhysicalExamination = CF_Connection.CreateCommand();
            CF_Cmd_PhysicalExamination.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_PhysicalExamination.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_PhysicalExamination_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE PHYSICAL EXAMINATION
        //**************************************************************************************************
        public bool CF_Delete_PhysicalExamination(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFPHYEXADB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();

            CF_Cmd_PhysicalExamination = CF_Connection.CreateCommand();
            CF_Cmd_PhysicalExamination.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_PhysicalExamination.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_PhysicalExamination_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE PHYSICAL EXAMINATION
        //**************************************************************************************************
        public bool CF_Modify_PhysicalExamination(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFPHYEXADB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_PhysicalExamination_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_PhysicalExamination_Mem.V_Date.ToBinary() + ",";                // FAMIANT
            Cf_SQL_UPDATE += "FAMIANT = '" + CF_PhysicalExamination_Mem.V_FamiAnt + "',";                    // FAMIANT
            Cf_SQL_UPDATE += "PERSANT = '" + CF_PhysicalExamination_Mem.V_PersAnt + "',";                    // PERSANT
            Cf_SQL_UPDATE += "BLOODPRES_MAX = '" + CF_PhysicalExamination_Mem.V_BloodPresMax + "',";         // BLODPRES_MAX
            Cf_SQL_UPDATE += "BLOODPRES_MIN = '" + CF_PhysicalExamination_Mem.V_BloodPresMin + "',";         // BLODPRES_MIN
            Cf_SQL_UPDATE += "PULSE = '" + CF_PhysicalExamination_Mem.V_Pulse + "',";                        // PULSE
            Cf_SQL_UPDATE += "HEIGHT = " + CF_PhysicalExamination_Mem.V_Height.ToString() + ",";             // HEIGHT
            Cf_SQL_UPDATE += "WEIGHT = " + CF_PhysicalExamination_Mem.V_Weight.ToString() + ",";             // WEIGHT
            Cf_SQL_UPDATE += "BMI = " + CF_PhysicalExamination_Mem.V_BMI.ToString() + ",";                   // BMI
            Cf_SQL_UPDATE += "OBSERV = '" + CF_PhysicalExamination_Mem.V_Observ + "',";                      // OBSERV
            Cf_SQL_UPDATE += "HEIGHT_ITEM = " + CF_PhysicalExamination_Mem.V_Height_Item.ToString() + ",";   // HEIGHT_ITEM
            Cf_SQL_UPDATE += "WEIGHT_ITEM = " + CF_PhysicalExamination_Mem.V_Weight_Item.ToString();         // WEIGHT_ITEM

            try
            {
                if (pCardID == null)
                    return false;

                if (!pCardID.Equals(""))
                {
                    Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                    Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                return false; // Registro no existe
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_PhysicalExamination = CF_Connection.CreateCommand();
            CF_Cmd_PhysicalExamination.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_PhysicalExamination.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_PhysicalExamination_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON RADIOGRAPHY
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE RADIOGRAPHY
        //**************************************************************************************************
        public bool CF_Select_Radiography(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFRADGRADB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_Radiography = CF_Connection.CreateCommand();
            CF_Cmd_Radiography.CommandText = Cf_SQL_SELECT;

            CF_DReader_Radiography = CF_Cmd_Radiography.ExecuteReader();
            return CF_DReader_Radiography.Read();
        }

        public bool CF_Select_Radiography(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFRADGRADB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_Radiography = CF_Connection.CreateCommand();
            CF_Cmd_Radiography.CommandText = Cf_SQL_SELECT;

            CF_DReader_Radiography = CF_Cmd_Radiography.ExecuteReader();
            return CF_DReader_Radiography.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_Radiography()
        {
            return CF_DReader_Radiography.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_Radiography(SQLiteDataReader pCF_DReader_Radiography)
        {
            CF_Radiography_Mem.V_CardID = pCF_DReader_Radiography.GetString(pCF_DReader_Radiography.GetOrdinal("CARDNUMBER"));
            CF_Radiography_Mem.V_Date = DateTime.FromBinary(pCF_DReader_Radiography.GetInt64(pCF_DReader_Radiography.GetOrdinal("PDATE")));
            CF_Radiography_Mem.V_Observ = pCF_DReader_Radiography.GetString(pCF_DReader_Radiography.GetOrdinal("OBSERV"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE RADIOGRAPHY
        //**************************************************************************************************
        public bool CF_Insert_Radiography()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFRADGRADB (ISTOPALM,CARDNUMBER,PDATE,OBSERV) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                              // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_Radiography_Mem.V_CardID + "',";           // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_Radiography_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_INSERT += "'" + CF_Radiography_Mem.V_Observ + "')";           // OBSERV

            try
            {
                int VerificarVacio = CF_Radiography_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_Radiography_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Radiography = CF_Connection.CreateCommand();
            CF_Cmd_Radiography.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_Radiography.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Radiography_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE RADIOGRAPHY
        //**************************************************************************************************
        public bool CF_Delete_Radiography(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFRADGRADB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Radiography = CF_Connection.CreateCommand();
            CF_Cmd_Radiography.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_Radiography.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Radiography_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE RADIOGRAPHY
        //**************************************************************************************************
        public bool CF_Modify_Radiography(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFRADGRADB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_Radiography_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_Radiography_Mem.V_Date.ToBinary() + ",";    // PDATE
            Cf_SQL_UPDATE += "OBSERV = '" + CF_Radiography_Mem.V_Observ + "'";           // OBSERV

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();

            CF_Cmd_Radiography = CF_Connection.CreateCommand();
            CF_Cmd_Radiography.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_Radiography.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Radiography_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }


        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON EKGR
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE EKGR
        //**************************************************************************************************
        public bool CF_Select_EKGR(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFEKGRDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_EKGR = CF_Connection.CreateCommand();
            CF_Cmd_EKGR.CommandText = Cf_SQL_SELECT;

            CF_DReader_EKGR = CF_Cmd_EKGR.ExecuteReader();
            return CF_DReader_EKGR.Read();
        }

        public bool CF_Select_EKGR(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFEKGRDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_EKGR = CF_Connection.CreateCommand();
            CF_Cmd_EKGR.CommandText = Cf_SQL_SELECT;

            CF_DReader_EKGR = CF_Cmd_EKGR.ExecuteReader();
            return CF_DReader_EKGR.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_EKGR()
        {
            return CF_DReader_EKGR.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_EKGR(SQLiteDataReader pCF_DReader_EKGR)
        {
            CF_EKGR_Mem.V_CardID = pCF_DReader_EKGR.GetString(pCF_DReader_EKGR.GetOrdinal("CARDNUMBER"));
            CF_EKGR_Mem.V_Date = DateTime.FromBinary(pCF_DReader_EKGR.GetInt64(pCF_DReader_EKGR.GetOrdinal("PDATE")));
            CF_EKGR_Mem.V_Observ = pCF_DReader_EKGR.GetString(pCF_DReader_EKGR.GetOrdinal("OBSERV"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE EKGR
        //**************************************************************************************************
        public bool CF_Insert_EKGR()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFEKGRDB (ISTOPALM,CARDNUMBER,PDATE,OBSERV) VALUES (";
            Cf_SQL_INSERT = Cf_SQL_INSERT + "0" + ",";                                      // ISTOPALM
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_EKGR_Mem.V_CardID + "',";          // CARDNUMBER
            Cf_SQL_INSERT = Cf_SQL_INSERT + " " + CF_EKGR_Mem.V_Date.ToBinary() + ",";  // PDATE
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_EKGR_Mem.V_Observ + "')";          // OBSERV

            try
            {
                int VerificarVacio = CF_EKGR_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_EKGR_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();

            CF_Cmd_EKGR = CF_Connection.CreateCommand();
            CF_Cmd_EKGR.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_EKGR.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EKGR_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE EKGR
        //**************************************************************************************************
        public bool CF_Delete_EKGR(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFEKGRDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_EKGR = CF_Connection.CreateCommand();
            CF_Cmd_EKGR.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_EKGR.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EKGR_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE EKGR
        //**************************************************************************************************
        public bool CF_Modify_EKGR(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFEKGRDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_EKGR_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_EKGR_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_UPDATE += "OBSERV = '" + CF_EKGR_Mem.V_Observ + "'";          // OBSERV

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();

            CF_Cmd_EKGR = CF_Connection.CreateCommand();
            CF_Cmd_EKGR.CommandText = Cf_SQL_UPDATE;

            try
            {
                Modify_SI_NO = true;
                Numero_Registros = CF_Cmd_EKGR.ExecuteNonQuery();
                CF_Transaction_X.Commit();
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EKGR_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON ECHO DOPPLER
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE ECHO DOPPLER
        //**************************************************************************************************
        public bool CF_Select_EchoDoppler(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,ECHO,DOPPLER,LABORATORY FROM CFECODOPDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_EchoDoppler = CF_Connection.CreateCommand();
            CF_Cmd_EchoDoppler.CommandText = Cf_SQL_SELECT;

            CF_DReader_EchoDoppler = CF_Cmd_EchoDoppler.ExecuteReader();
            return CF_DReader_EchoDoppler.Read();
        }

        public bool CF_Select_EchoDoppler(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,ECHO,DOPPLER,LABORATORY FROM CFECODOPDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_EchoDoppler = CF_Connection.CreateCommand();
            CF_Cmd_EchoDoppler.CommandText = Cf_SQL_SELECT;

            CF_DReader_EchoDoppler = CF_Cmd_EchoDoppler.ExecuteReader();
            return CF_DReader_EchoDoppler.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_EchoDoppler()
        {
            return CF_DReader_EchoDoppler.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_EchoDoppler(SQLiteDataReader pCF_DReader_EchoDoppler)
        {
            CF_EchoDoppler_Mem.V_CardID = pCF_DReader_EchoDoppler.GetString(pCF_DReader_EchoDoppler.GetOrdinal("CARDNUMBER"));
            CF_EchoDoppler_Mem.V_Date = DateTime.FromBinary(pCF_DReader_EchoDoppler.GetInt64(pCF_DReader_EchoDoppler.GetOrdinal("PDATE")));
            CF_EchoDoppler_Mem.V_Echo = pCF_DReader_EchoDoppler.GetString(pCF_DReader_EchoDoppler.GetOrdinal("ECHO"));
            CF_EchoDoppler_Mem.V_Doppler = pCF_DReader_EchoDoppler.GetString(pCF_DReader_EchoDoppler.GetOrdinal("DOPPLER"));
            CF_EchoDoppler_Mem.V_Laboratory = pCF_DReader_EchoDoppler.GetString(pCF_DReader_EchoDoppler.GetOrdinal("LABORATORY"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE ECHO DOPPLER
        //**************************************************************************************************
        public bool CF_Insert_EchoDoppler()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFECODOPDB (ISTOPALM,CARDNUMBER,PDATE,ECHO,DOPPLER,LABORATORY) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                              // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_EchoDoppler_Mem.V_CardID + "',";           // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_EchoDoppler_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_INSERT += "'" + CF_EchoDoppler_Mem.V_Echo + "',";             // ECHO
            Cf_SQL_INSERT += "'" + CF_EchoDoppler_Mem.V_Doppler + "',";         // DOPPLER
            Cf_SQL_INSERT += "'" + CF_EchoDoppler_Mem.V_Laboratory + "')";       // LABORATORY

            try
            {
                int VerificarVacio = CF_EchoDoppler_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_EchoDoppler_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();

            CF_Cmd_EchoDoppler = CF_Connection.CreateCommand();
            CF_Cmd_EchoDoppler.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_EchoDoppler.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EchoDoppler_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE ECHO DOPPLER
        //**************************************************************************************************
        public bool CF_Delete_EchoDoppler(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFECODOPDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_EchoDoppler = CF_Connection.CreateCommand();
            CF_Cmd_EchoDoppler.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_EchoDoppler.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EchoDoppler_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE ECHO DOPPLER
        //**************************************************************************************************
        public bool CF_Modify_EchoDoppler(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFECODOPDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" +  CF_EchoDoppler_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_EchoDoppler_Mem.V_Date.ToBinary() + ",";
            Cf_SQL_UPDATE += "ECHO = '" + CF_EchoDoppler_Mem.V_Echo + "',";
            Cf_SQL_UPDATE += "DOPPLER = '" + CF_EchoDoppler_Mem.V_Doppler + "',";
            Cf_SQL_UPDATE += "LABORATORY = '" + CF_EchoDoppler_Mem.V_Laboratory + "'";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_EchoDoppler = CF_Connection.CreateCommand();
            CF_Cmd_EchoDoppler.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_EchoDoppler.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EchoDoppler_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON HOLTER
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE HOLTER
        //**************************************************************************************************
        public bool CF_Select_Holter(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFEXAHOLDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_Holter = CF_Connection.CreateCommand();
            CF_Cmd_Holter.CommandText = Cf_SQL_SELECT;

            CF_DReader_Holter = CF_Cmd_Holter.ExecuteReader();
            return CF_DReader_Holter.Read();
        }

        public bool CF_Select_Holter(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFEXAHOLDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_Holter = CF_Connection.CreateCommand();
            CF_Cmd_Holter.CommandText = Cf_SQL_SELECT;

            CF_DReader_Holter = CF_Cmd_Holter.ExecuteReader();
            return CF_DReader_Holter.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_Holter()
        {
            return CF_DReader_Holter.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_Holter(SQLiteDataReader pCF_DReader_Holter)
        {
            CF_Holter_Mem.V_CardID = pCF_DReader_Holter.GetString(pCF_DReader_Holter.GetOrdinal("CARDNUMBER"));
            CF_Holter_Mem.V_Date = DateTime.FromBinary(pCF_DReader_Holter.GetInt64(pCF_DReader_Holter.GetOrdinal("PDATE")));
            CF_Holter_Mem.V_Observ = pCF_DReader_Holter.GetString(pCF_DReader_Holter.GetOrdinal("OBSERV"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE HOLTER
        //**************************************************************************************************
        public bool CF_Insert_Holter()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFEXAHOLDB (ISTOPALM,CARDNUMBER,PDATE,OBSERV) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                        // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_Holter_Mem.V_CardID + "',";          // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_Holter_Mem.V_Date.ToBinary() + ",";  // PDATE
            Cf_SQL_INSERT += "'" + CF_Holter_Mem.V_Observ + "')";          // OBSERV

            try
            {
                int VerificarVacio = CF_Holter_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_Holter_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Holter = CF_Connection.CreateCommand();
            CF_Cmd_Holter.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_Holter.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Holter_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE HOLTER
        //**************************************************************************************************
        public bool CF_Delete_Holter(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFEXAHOLDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE = Cf_SQL_DELETE + " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE = Cf_SQL_DELETE + "       PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Holter = CF_Connection.CreateCommand();
            CF_Cmd_Holter.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_Holter.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Holter_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE HOLTER
        //**************************************************************************************************
        public bool CF_Modify_Holter(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFEXAHOLDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_Holter_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_Holter_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_UPDATE += "OBSERV = '" + CF_Holter_Mem.V_Observ + "'";          // OBSERV

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Holter = CF_Connection.CreateCommand();
            CF_Cmd_Holter.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_Holter.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_EKGR_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON STRESS TEST
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE STRESS TEST
        //**************************************************************************************************
        public bool CF_Select_StressTest(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,PROTOCOL,STAGE,PTIME,CARDIACFREC,PERCENTMAX,ARRHYTH,BLOOD_PRESS,PTYPE,FUNCTIONALCAP,RE_SUSP,RESULT,DESC_STT FROM CFSTRTESSDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT = Cf_SQL_SELECT + " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_StressTest = CF_Connection.CreateCommand();
            CF_Cmd_StressTest.CommandText = Cf_SQL_SELECT;

            CF_DReader_StressTest = CF_Cmd_StressTest.ExecuteReader();
            return CF_DReader_StressTest.Read();
        }

        public bool CF_Select_StressTest(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,PROTOCOL,STAGE,PTIME,CARDIACFREC,PERCENTMAX,ARRHYTH,BLOOD_PRESS,PTYPE,FUNCTIONALCAP,RE_SUSP,RESULT,DESC_STT FROM CFSTRTESSDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT = Cf_SQL_SELECT + " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT = Cf_SQL_SELECT + "       PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_StressTest = CF_Connection.CreateCommand();
            CF_Cmd_StressTest.CommandText = Cf_SQL_SELECT;

            CF_DReader_StressTest = CF_Cmd_StressTest.ExecuteReader();
            return CF_DReader_StressTest.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_StressTest()
        {
            return CF_DReader_StressTest.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_StressTest(SQLiteDataReader pCF_DReader_StressTest)
        {
            CF_StressTest_Mem.V_CardID = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("CARDNUMBER"));
            CF_StressTest_Mem.V_Date = DateTime.FromBinary(pCF_DReader_StressTest.GetInt64(pCF_DReader_StressTest.GetOrdinal("PDATE")));
            CF_StressTest_Mem.V_Protocol = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("PROTOCOL"));
            CF_StressTest_Mem.V_Stage = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("STAGE"));
            CF_StressTest_Mem.V_Time = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("PTIME"));
            CF_StressTest_Mem.V_CardiacFrec = pCF_DReader_StressTest.GetInt16(pCF_DReader_StressTest.GetOrdinal("CARDIACFREC"));
            CF_StressTest_Mem.V_PercentMax = pCF_DReader_StressTest.GetInt16(pCF_DReader_StressTest.GetOrdinal("PERCENTMAX"));
            CF_StressTest_Mem.V_Arrhyth = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("ARRHYTH"));
            CF_StressTest_Mem.V_BloodPress = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("BLOOD_PRESS"));
            CF_StressTest_Mem.V_Type = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("PTYPE"));
            CF_StressTest_Mem.V_FunCap = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("FUNCTIONALCAP"));
            CF_StressTest_Mem.V_ReSusp = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("RE_SUSP"));
            CF_StressTest_Mem.V_Result = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("RESULT"));
            CF_StressTest_Mem.V_DescSTT = pCF_DReader_StressTest.GetString(pCF_DReader_StressTest.GetOrdinal("DESC_STT"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE StressTest
        //**************************************************************************************************
        public bool CF_Insert_StressTest()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFSTRTESSDB (ISTOPALM,CARDNUMBER,PDATE,PROTOCOL,STAGE,PTIME,CARDIACFREC,PERCENTMAX,ARRHYTH,BLOOD_PRESS,PTYPE,FUNCTIONALCAP,RE_SUSP,RESULT,DESC_STT) VALUES (";
            Cf_SQL_INSERT = Cf_SQL_INSERT + "0" + ",";                                              // ISTOPALM
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_CardID + "',";                // CARDNUMBER
            Cf_SQL_INSERT = Cf_SQL_INSERT + " " + CF_StressTest_Mem.V_Date.ToBinary() + ",";        // PDATE
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_Protocol + "',";              // PROTOCOL
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_Stage + "',";                 // STAGE
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_Time + "',";                  // PTIME
            Cf_SQL_INSERT = Cf_SQL_INSERT + " " + CF_StressTest_Mem.V_CardiacFrec.ToString() + ","; // CARDIACFREC
            Cf_SQL_INSERT = Cf_SQL_INSERT + " " + CF_StressTest_Mem.V_PercentMax.ToString() + ",";  // PERCENTMAX
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_Arrhyth + "',";               // ARRHYTH
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_BloodPress + "',";            // BLOOD_PRESS
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_Type + "',";                  // PTYPE
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_FunCap + "',";                // FUNCTIONALCAP
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_ReSusp + "',";                // RE_SUSP
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_Result + "',";                 // RESULT
            Cf_SQL_INSERT = Cf_SQL_INSERT + "'" + CF_StressTest_Mem.V_DescSTT + "')";               // DESC_STT

            try
            {
                int VerificarVacio = CF_StressTest_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_StressTest_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_StressTest = CF_Connection.CreateCommand();
            CF_Cmd_StressTest.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_StressTest.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_StressTest_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE STRESS TEST
        //**************************************************************************************************
        public bool CF_Delete_StressTest(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFSTRTESSDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE = Cf_SQL_DELETE + " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE = Cf_SQL_DELETE + "  PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_StressTest = CF_Connection.CreateCommand();
            CF_Cmd_StressTest.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_StressTest.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_StressTest_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE STRESS TEST
        //**************************************************************************************************
        public bool CF_Modify_StressTest(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFSTRTESSDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_StressTest_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_StressTest_Mem.V_Date.ToBinary() + ",";                // PDATE
            Cf_SQL_UPDATE += "PROTOCOL = '" + CF_StressTest_Mem.V_Protocol + "',";                   // PROTOCOL
            Cf_SQL_UPDATE += "STAGE = '" + CF_StressTest_Mem.V_Stage + "',";               // STAGE
            Cf_SQL_UPDATE += "PTIME = '" + CF_StressTest_Mem.V_Time + "',";                           // PTIME
            Cf_SQL_UPDATE += "CARDIACFREC = " + CF_StressTest_Mem.V_CardiacFrec.ToString() + ",";   // CARDIACFREC
            Cf_SQL_UPDATE += "PERCENTMAX = " + CF_StressTest_Mem.V_PercentMax.ToString() + ",";     // PERCENTMAX
            Cf_SQL_UPDATE += "ARRHYTH = '" + CF_StressTest_Mem.V_Arrhyth + "',";                     // ARRHYTH
            Cf_SQL_UPDATE += "BLOOD_PRESS = '" + CF_StressTest_Mem.V_BloodPress + "',";              // BLOOD_PRESS
            Cf_SQL_UPDATE += "PTYPE = '" + CF_StressTest_Mem.V_Type + "',";                          // PTYPE
            Cf_SQL_UPDATE += "FUNCTIONALCAP = '" + CF_StressTest_Mem.V_FunCap + "',";                // FUNCTIONALCAP
            Cf_SQL_UPDATE += "RE_SUSP = '" + CF_StressTest_Mem.V_ReSusp + "',";                      // RE_SUSP
            Cf_SQL_UPDATE += "RESULT = '" + CF_StressTest_Mem.V_Result + "',";             // RESULT
            Cf_SQL_UPDATE += "DESC_STT = '" + CF_StressTest_Mem.V_DescSTT + "'";                     // DESC_STT

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += "       PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_StressTest = CF_Connection.CreateCommand();
            CF_Cmd_StressTest.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_StressTest.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_StressTest_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON CARDIAC CATH
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE CARDIAC CATH
        //**************************************************************************************************
        public bool CF_Select_CardiacCath(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFCARCATDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_CardiacCath = CF_Connection.CreateCommand();
            CF_Cmd_CardiacCath.CommandText = Cf_SQL_SELECT;

            CF_DReader_CardiacCath = CF_Cmd_CardiacCath.ExecuteReader();
            return CF_DReader_CardiacCath.Read();
        }

        public bool CF_Select_CardiacCath(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFCARCATDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_CardiacCath = CF_Connection.CreateCommand();
            CF_Cmd_CardiacCath.CommandText = Cf_SQL_SELECT;

            CF_DReader_CardiacCath = CF_Cmd_CardiacCath.ExecuteReader();
            return CF_DReader_CardiacCath.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_CardiacCath()
        {
            return CF_DReader_CardiacCath.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_CardiacCath(SQLiteDataReader pCF_DReader_CardiacCath)
        {
            CF_CardiacCath_Mem.V_CardID = pCF_DReader_CardiacCath.GetString(pCF_DReader_CardiacCath.GetOrdinal("CARDNUMBER"));
            CF_CardiacCath_Mem.V_Date = DateTime.FromBinary(pCF_DReader_CardiacCath.GetInt64(pCF_DReader_CardiacCath.GetOrdinal("PDATE")));
            CF_CardiacCath_Mem.V_Observ = pCF_DReader_CardiacCath.GetString(pCF_DReader_CardiacCath.GetOrdinal("OBSERV"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE CARDIAC CATH
        //**************************************************************************************************
        public bool CF_Insert_CardiacCath()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFCARCATDB (ISTOPALM,CARDNUMBER,PDATE,OBSERV) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                             // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_CardiacCath_Mem.V_CardID + "',";          // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_CardiacCath_Mem.V_Date.ToBinary() + ",";  // PDATE
            Cf_SQL_INSERT += "'" + CF_CardiacCath_Mem.V_Observ + "')";          // OBSERV

            try
            {
                int VerificarVacio = CF_CardiacCath_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_CardiacCath_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_CardiacCath = CF_Connection.CreateCommand();
            CF_Cmd_CardiacCath.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_CardiacCath.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_CardiacCath_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE CARDIAC CATH
        //**************************************************************************************************
        public bool CF_Delete_CardiacCath(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFCARCATDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_CardiacCath = CF_Connection.CreateCommand();
            CF_Cmd_CardiacCath.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_CardiacCath.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = true;
                CF_Transaction_X.Rollback();
                CF_CardiacCath_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE CARDIAC CATH
        //**************************************************************************************************
        public bool CF_Modify_CardiacCath(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFCARCATDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_CardiacCath_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_CardiacCath_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_UPDATE += "OBSERV = '" + CF_CardiacCath_Mem.V_Observ + "'";          // OBSERV

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_CardiacCath = CF_Connection.CreateCommand();
            CF_Cmd_CardiacCath.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_CardiacCath.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_CardiacCath_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON SURGERY
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE SURGERY
        //**************************************************************************************************
        public bool CF_Select_Surgery(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFSURGERYDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_Surgery = CF_Connection.CreateCommand();
            CF_Cmd_Surgery.CommandText = Cf_SQL_SELECT;

            CF_DReader_Surgery = CF_Cmd_Surgery.ExecuteReader();
            return CF_DReader_Surgery.Read();
        }

        public bool CF_Select_Surgery(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV FROM CFSURGERYDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_Surgery = CF_Connection.CreateCommand();
            CF_Cmd_Surgery.CommandText = Cf_SQL_SELECT;

            CF_DReader_Surgery = CF_Cmd_Surgery.ExecuteReader();
            return CF_DReader_Surgery.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_Surgery()
        {
            return CF_DReader_Surgery.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_Surgery(SQLiteDataReader pCF_DReader_Surgery)
        {
            CF_Surgery_Mem.V_CardID = pCF_DReader_Surgery.GetString(pCF_DReader_Surgery.GetOrdinal("CARDNUMBER"));
            CF_Surgery_Mem.V_Date = DateTime.FromBinary(pCF_DReader_Surgery.GetInt64(pCF_DReader_Surgery.GetOrdinal("PDATE")));
            CF_Surgery_Mem.V_Observ = pCF_DReader_Surgery.GetString(pCF_DReader_Surgery.GetOrdinal("OBSERV"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE SURGERY
        //**************************************************************************************************
        public bool CF_Insert_Surgery()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFSURGERYDB (ISTOPALM,CARDNUMBER,PDATE,OBSERV) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                         // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_Surgery_Mem.V_CardID + "',";          // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_Surgery_Mem.V_Date.ToBinary() + ",";  // PDATE
            Cf_SQL_INSERT += "'" + CF_Surgery_Mem.V_Observ + "')";          // OBSERV

            try
            {
                int VerificarVacio = CF_Surgery_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_Surgery_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Surgery = CF_Connection.CreateCommand();
            CF_Cmd_Surgery.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_Surgery.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Surgery_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE SURGERY
        //**************************************************************************************************
        public bool CF_Delete_Surgery(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFSURGERYDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Surgery = CF_Connection.CreateCommand();
            CF_Cmd_Surgery.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_Surgery.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Surgery_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE SURGERY
        //**************************************************************************************************
        public bool CF_Modify_Surgery(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFSURGERYDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_Surgery_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE = " + CF_Surgery_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_UPDATE += "OBSERV = '" + CF_Surgery_Mem.V_Observ + "'";          // OBSERV

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_Surgery = CF_Connection.CreateCommand();
            CF_Cmd_Surgery.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_Surgery.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Surgery_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON DIAGNOSES AND TREATMENTS
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE DIAGNOSES AND TREATMENTS
        //**************************************************************************************************
        public bool CF_Select_DiagnosesTreatments(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,DIAGNOSES,TREATMENTS FROM CFDIATRADB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_DiagnosesTreatments = CF_Connection.CreateCommand();
            CF_Cmd_DiagnosesTreatments.CommandText = Cf_SQL_SELECT;

            CF_DReader_DiagnosesTreatments = CF_Cmd_DiagnosesTreatments.ExecuteReader();
            return CF_DReader_DiagnosesTreatments.Read();
        }

        public bool CF_Select_DiagnosesTreatments(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,DIAGNOSES,TREATMENTS FROM CFDIATRADB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_DiagnosesTreatments = CF_Connection.CreateCommand();
            CF_Cmd_DiagnosesTreatments.CommandText = Cf_SQL_SELECT;

            CF_DReader_DiagnosesTreatments = CF_Cmd_DiagnosesTreatments.ExecuteReader();
            return CF_DReader_DiagnosesTreatments.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_DiagnosesTreatments()
        {
            return CF_DReader_DiagnosesTreatments.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_DiagnosesTreatments(SQLiteDataReader pCF_DReader_DiagnosesTreatments)
        {
            CF_DiagnosesTreatments_Mem.V_CardID = pCF_DReader_DiagnosesTreatments.GetString(pCF_DReader_DiagnosesTreatments.GetOrdinal("CARDNUMBER"));
            CF_DiagnosesTreatments_Mem.V_Date = DateTime.FromBinary(pCF_DReader_DiagnosesTreatments.GetInt64(pCF_DReader_DiagnosesTreatments.GetOrdinal("PDATE")));
            CF_DiagnosesTreatments_Mem.V_Diagnoses = pCF_DReader_DiagnosesTreatments.GetString(pCF_DReader_DiagnosesTreatments.GetOrdinal("DIAGNOSES"));
            CF_DiagnosesTreatments_Mem.V_Treatments = pCF_DReader_DiagnosesTreatments.GetString(pCF_DReader_DiagnosesTreatments.GetOrdinal("TREATMENTS"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE DIAGNOSES AND TREATMENTS
        //**************************************************************************************************
        public bool CF_Insert_DiagnosesTreatments()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFDIATRADB (ISTOPALM,CARDNUMBER,PDATE,DIAGNOSES,TREATMENTS) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                                      // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_DiagnosesTreatments_Mem.V_CardID + "',";           // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_DiagnosesTreatments_Mem.V_Date.ToBinary() + ",";   // PDATE
            Cf_SQL_INSERT += "'" + CF_DiagnosesTreatments_Mem.V_Diagnoses + "',";        // DIAGNOSES
            Cf_SQL_INSERT += "'" + CF_DiagnosesTreatments_Mem.V_Treatments + "')";       // TREATMENTS

            try
            {
                int VerificarVacio = CF_DiagnosesTreatments_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_DiagnosesTreatments_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_DiagnosesTreatments = CF_Connection.CreateCommand();
            CF_Cmd_DiagnosesTreatments.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_DiagnosesTreatments.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_DiagnosesTreatments_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE DIAGNOSES AND TREATMENTS
        //**************************************************************************************************
        public bool CF_Delete_DiagnosesTreatments(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFDIATRADB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_DiagnosesTreatments = CF_Connection.CreateCommand();
            CF_Cmd_DiagnosesTreatments.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_DiagnosesTreatments.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_DiagnosesTreatments_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE DIAGNOSES AND TREATMENTS
        //**************************************************************************************************
        public bool CF_Modify_DiagnosesTreatments(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFDIATRADB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_DiagnosesTreatments_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += " PDATE = " + CF_DiagnosesTreatments_Mem.V_Date.ToBinary() + ",";    // PDATE
            Cf_SQL_UPDATE += " DIAGNOSES = '" + CF_DiagnosesTreatments_Mem.V_Diagnoses + "',";     // OBSERV
            Cf_SQL_UPDATE += " TREATMENTS = '" + CF_DiagnosesTreatments_Mem.V_Treatments + "'";   // OBSERV

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += "       PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_DiagnosesTreatments = CF_Connection.CreateCommand();
            CF_Cmd_DiagnosesTreatments.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_DiagnosesTreatments.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_DiagnosesTreatments_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON PROGRESS NOTES
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE PROGRESS NOTES
        //**************************************************************************************************
        public bool CF_Select_ProgressNotes(string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV,NEXTDATEAPP,NEXTTIMEAPP FROM CFPRONOTDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "'";
            }

            CF_Cmd_ProgressNotes = CF_Connection.CreateCommand();
            CF_Cmd_ProgressNotes.CommandText = Cf_SQL_SELECT;

            CF_DReader_ProgressNotes = CF_Cmd_ProgressNotes.ExecuteReader();
            return CF_DReader_ProgressNotes.Read();
        }

        public bool CF_Select_ProgressNotes(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,CARDNUMBER,PDATE,OBSERV,NEXTDATEAPP,NEXTTIMEAPP FROM CFPRONOTDB";
            if (!pCardID.Equals(""))
            {
                Cf_SQL_SELECT += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_SELECT += " PDATE = " + pDate.ToBinary();
            }

            CF_Cmd_ProgressNotes = CF_Connection.CreateCommand();
            CF_Cmd_ProgressNotes.CommandText = Cf_SQL_SELECT;

            CF_DReader_ProgressNotes = CF_Cmd_ProgressNotes.ExecuteReader();
            return CF_DReader_ProgressNotes.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_ProgressNotes()
        {
            return CF_DReader_ProgressNotes.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_ProgressNotes(SQLiteDataReader pCF_DReader_ProgressNotes)
        {
            CF_ProgressNotes_Mem.V_CardID = pCF_DReader_ProgressNotes.GetString(pCF_DReader_ProgressNotes.GetOrdinal("CARDNUMBER"));
            CF_ProgressNotes_Mem.V_Date = DateTime.FromBinary(pCF_DReader_ProgressNotes.GetInt64(pCF_DReader_ProgressNotes.GetOrdinal("PDATE")));
            CF_ProgressNotes_Mem.V_NextDateApp = DateTime.FromBinary(pCF_DReader_ProgressNotes.GetInt64(pCF_DReader_ProgressNotes.GetOrdinal("NEXTDATEAPP")));
            CF_ProgressNotes_Mem.V_NextTimeApp = DateTime.FromBinary(pCF_DReader_ProgressNotes.GetInt64(pCF_DReader_ProgressNotes.GetOrdinal("NEXTTIMEAPP")));
            CF_ProgressNotes_Mem.V_Observ = pCF_DReader_ProgressNotes.GetString(pCF_DReader_ProgressNotes.GetOrdinal("OBSERV"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE PROGRESS NOTES
        //**************************************************************************************************
        public bool CF_Insert_ProgressNotes()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            string Cf_SQL_INSERT_X;
            SQLiteCommand CF_Cmd_Appointments_X;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_INSERT = "INSERT INTO CFPRONOTDB (ISTOPALM,CARDNUMBER,PDATE,OBSERV,NEXTDATEAPP,NEXTTIMEAPP) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                                      // ISTOPALM
            Cf_SQL_INSERT += "'" + CF_ProgressNotes_Mem.V_CardID + "',";                 // CARDNUMBER
            Cf_SQL_INSERT += " " + CF_ProgressNotes_Mem.V_Date.ToBinary() + ",";         // PDATE
            Cf_SQL_INSERT += "'" + CF_ProgressNotes_Mem.V_Observ + "',";                 // OBSERV
            Cf_SQL_INSERT += " " + CF_ProgressNotes_Mem.V_NextDateApp.ToBinary() + ",";  // NEXTDATEAPP
            Cf_SQL_INSERT += " " + CF_ProgressNotes_Mem.V_NextTimeApp.ToBinary() + ")";  // NEXTTIMEAPP

            try
            {
                int VerificarVacio = CF_ProgressNotes_Mem.V_CardID.Length;
            }
            catch (System.NullReferenceException)
            {
                CF_ProgressNotes_Mem.V_Error_Operation = "Nro de Ficha vacio";
                return false;
            }


            Cf_SQL_INSERT_X = "INSERT INTO CFAPPOINTDB (ISTOPALM,DATEAPPOINT,TIMEAPPOINT,CARDNUMBER,PATIENT,LOCDATE) VALUES (";
            Cf_SQL_INSERT_X += "0" + ",";                                                                              // ISTOPALM
            Cf_SQL_INSERT_X += " " + CF_ProgressNotes_Mem.V_NextDateApp.ToBinary() + ",";                          // DATEAPPOINT
            Cf_SQL_INSERT_X += " " + CF_ProgressNotes_Mem.V_NextTimeApp.ToBinary() + ",";                          // TIMEAPPOINT
            Cf_SQL_INSERT_X += "'" + CF_ProgressNotes_Mem.V_CardID + "',";                                         // CARDNUMBER
            Cf_SQL_INSERT_X += "'" + CF_BasicData_Mem.V_LastName + " " + CF_BasicData_Mem.V_FirstName + "',";  // PATIENT
            Cf_SQL_INSERT_X += " " + CF_ProgressNotes_Mem.V_Date.ToBinary() + ")";                                 // LOCDATE

            CF_Transaction_X = CF_Connection.BeginTransaction();

            try
            {
                // ***************************************************************************************************************************
                // Insertar en Progress Notes
                // ***************************************************************************************************************************
                CF_Cmd_ProgressNotes = CF_Connection.CreateCommand();
                CF_Cmd_ProgressNotes.CommandText = Cf_SQL_INSERT;
                CF_Cmd_ProgressNotes.ExecuteNonQuery();

                // ***************************************************************************************************************************
                // Insertar en Appoint
                // ***************************************************************************************************************************
                CF_Cmd_Appointments_X = CF_Connection.CreateCommand();
                CF_Cmd_Appointments_X.CommandText = Cf_SQL_INSERT_X;
                CF_Cmd_Appointments_X.ExecuteNonQuery();

                CF_Transaction_X.Commit();

                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_ProgressNotes_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE PROGRESS NOTES
        //**************************************************************************************************
        public bool CF_Delete_ProgressNotes(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_DELETE_X;
            bool Delete_SI_NO;

            SQLiteTransaction CF_Transaction_X;
            SQLiteCommand CF_Cmd_Appointments_X;

            string Cf_SQL_DELETE = "DELETE FROM CFPRONOTDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_DELETE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            Cf_SQL_DELETE_X = "DELETE FROM CFAPPOINTDB WHERE LOCDATE = " + pDate.ToBinary() + "' AND CARDNUMBER = '" + pCardID + "'";

            CF_Transaction_X = CF_Connection.BeginTransaction();

            try
            {
                CF_Cmd_ProgressNotes = CF_Connection.CreateCommand();
                CF_Cmd_ProgressNotes.CommandText = Cf_SQL_DELETE;

                CF_Cmd_Appointments_X = CF_Connection.CreateCommand();
                CF_Cmd_Appointments_X.CommandText = Cf_SQL_DELETE_X;

                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_ProgressNotes_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE PROGRESS NOTES
        //**************************************************************************************************
        public bool CF_Modify_ProgressNotes(string pCardID, DateTime pDate)
        {
            // Operacion SQL
            bool Modify_SI_NO;

            SQLiteTransaction CF_Transaction_X;
            SQLiteCommand CF_Cmd_Appointments_X;

            string Cf_SQL_UPDATE_X;
            string Cf_SQL_UPDATE = "UPDATE CFPRONOTDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_ProgressNotes_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "PDATE =  " + CF_ProgressNotes_Mem.V_Date.ToBinary() + ",";                 // PDATE
            Cf_SQL_UPDATE += "OBSERV = '" + CF_ProgressNotes_Mem.V_Observ + "',";                        // OBSERV
            Cf_SQL_UPDATE += "NEXTDATEAPP =  " + CF_ProgressNotes_Mem.V_NextDateApp.ToBinary() + ",";    // NEXTDATEAPP
            Cf_SQL_UPDATE += "NEXTTIMEAPP =  " + CF_ProgressNotes_Mem.V_NextTimeApp.ToBinary();          // NEXTTIMEAPP

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
                Cf_SQL_UPDATE += " PDATE = " + pDate.ToBinary();
            }
            else
            {
                return false;
            }

            Cf_SQL_UPDATE_X = "UPDATE CFAPPOINTDB SET ";
            Cf_SQL_UPDATE_X += "ISTOPALM = 0,";
            Cf_SQL_UPDATE_X += "DATEAPPOINT = " + CF_ProgressNotes_Mem.V_NextDateApp.ToBinary() + ",";                         // DATEAPPOINT
            Cf_SQL_UPDATE_X += "TIMEAPPOINT = " + CF_ProgressNotes_Mem.V_NextTimeApp.ToBinary() + ",";                         // TIMEAPPOINT
            Cf_SQL_UPDATE_X += "PATIENT = '" + CF_BasicData_Mem.V_LastName + " " + CF_BasicData_Mem.V_FirstName + "',";    // PATIENTS
            Cf_SQL_UPDATE_X += "LOCDATE = " + CF_ProgressNotes_Mem.V_Date.ToBinary();                                          // LOCDATE
            Cf_SQL_UPDATE_X += " WHERE LOCDATE = " + pDate.ToBinary() + " AND ";
            Cf_SQL_UPDATE_X += " CARDNUMBER = '" + pCardID + "'";

            CF_Transaction_X = CF_Connection.BeginTransaction();

            try
            {
                CF_Cmd_ProgressNotes = CF_Connection.CreateCommand();
                CF_Cmd_ProgressNotes.CommandText = Cf_SQL_UPDATE;

                CF_Cmd_Appointments_X = CF_Connection.CreateCommand();
                CF_Cmd_Appointments_X.CommandText = Cf_SQL_UPDATE_X;

                CF_Transaction_X.Commit();

                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_ProgressNotes_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON APPOINTMENTS
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE APPOINTMENTS
        //**************************************************************************************************
        public bool CF_Select_Appointments(DateTime pDate)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,DATEAPPOINT,TIMEAPPOINT,CARDNUMBER,PATIENT,LOCDATE FROM CFAPPOINTDB";
            Cf_SQL_SELECT += " WHERE DATEAPPOINT = " + pDate.ToBinary() + " ORDER BY TIMEAPPOINT";

            CF_Cmd_Appointments = CF_Connection.CreateCommand();

            CF_Cmd_Appointments.CommandText = Cf_SQL_SELECT;

            CF_DReader_Appointments = CF_Cmd_Appointments.ExecuteReader();
            return CF_DReader_Appointments.Read();
        }

        public bool CF_Select_Appointments(DateTime pDate, string pCardID)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,DATEAPPOINT,TIMEAPPOINT,CARDNUMBER,PATIENT,LOCDATE FROM CFAPPOINTDB";
            Cf_SQL_SELECT += " WHERE DATEAPPOINT = " + pDate.ToBinary() + " AND ";
            Cf_SQL_SELECT += " CARDNUMBER = '" + pCardID + "'";
            CF_Cmd_Appointments = CF_Connection.CreateCommand();

            CF_Cmd_Appointments.CommandText = Cf_SQL_SELECT;

            CF_DReader_Appointments = CF_Cmd_Appointments.ExecuteReader();
            return CF_DReader_Appointments.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_Appointments()
        {
            return CF_DReader_Appointments.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_Appointments(SQLiteDataReader pCF_DReader_Appointments)
        {
            CF_Appointments_Mem.V_CardID = pCF_DReader_Appointments.GetString(pCF_DReader_Appointments.GetOrdinal("CARDNUMBER"));
            CF_Appointments_Mem.V_DateAppoint = DateTime.FromBinary(pCF_DReader_Appointments.GetInt64(pCF_DReader_Appointments.GetOrdinal("DATEAPPOINT")));
            CF_Appointments_Mem.V_TimeAppoint = DateTime.FromBinary(pCF_DReader_Appointments.GetInt64(pCF_DReader_Appointments.GetOrdinal("TIMEAPPOINT")));
            CF_Appointments_Mem.V_LocDate = DateTime.FromBinary(pCF_DReader_Appointments.GetInt64(pCF_DReader_Appointments.GetOrdinal("LOCDATE")));
            CF_Appointments_Mem.V_Patient = pCF_DReader_Appointments.GetString(pCF_DReader_Appointments.GetOrdinal("PATIENT"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE APPOINTMENTS
        //**************************************************************************************************
        public bool CF_Insert_Appointments()
        {
            // Operacion SQL
            bool Insertado_SI_NO;

            SQLiteTransaction CF_Transaction_X;
            SQLiteCommand CF_Cmd_ProgressNotes_X;

            string Cf_SQL_INSERT_X;

            string Cf_SQL_INSERT = "INSERT INTO CFAPPOINTDB (ISTOPALM,DATEAPPOINT,TIMEAPPOINT,CARDNUMBER,PATIENT,LOCDATE) VALUES (";
            Cf_SQL_INSERT += "0" + ",";                                                      // ISTOPALM
            Cf_SQL_INSERT += " " + CF_Appointments_Mem.V_DateAppoint.ToBinary() + ",";   // DATEAPPOINT
            Cf_SQL_INSERT += " " + CF_Appointments_Mem.V_TimeAppoint.ToBinary() + ",";   // TIMEAPPOINT
            Cf_SQL_INSERT += "'" + CF_Appointments_Mem.V_CardID + "',";                  // CARDNUMBER
            Cf_SQL_INSERT += "'" + CF_Appointments_Mem.V_Patient + "',";                 // PATIENT
            Cf_SQL_INSERT += " " + CF_Appointments_Mem.V_LocDate.ToBinary() + ")";       // LOCDATE

            Cf_SQL_INSERT_X = "INSERT INTO CFPRONOTDB (ISTOPALM,CARDNUMBER,PDATE,OBSERV,NEXTDATEAPP,NEXTTIMEAPP) VALUES (";
            Cf_SQL_INSERT_X += "0" + ",";                                                      // ISTOPALM
            Cf_SQL_INSERT_X += "'" + CF_Appointments_Mem.V_CardID + "',";                  // CARDNUMBER
            Cf_SQL_INSERT_X += " " + CF_Appointments_Mem.V_LocDate.ToBinary() + ",";       // PDATE
            Cf_SQL_INSERT_X += "'From Appoints',";                                             // OBSERV
            Cf_SQL_INSERT_X += " " + CF_Appointments_Mem.V_DateAppoint.ToBinary() + ",";   // NEXTDATEAPP
            Cf_SQL_INSERT_X += " " + CF_Appointments_Mem.V_TimeAppoint.ToBinary() + ")";   // NEXTTIMEAPP

            CF_Transaction_X = CF_Connection.BeginTransaction();

            try
            {
                CF_Cmd_Appointments = CF_Connection.CreateCommand();
                CF_Cmd_Appointments.CommandText = Cf_SQL_INSERT;

                CF_Cmd_ProgressNotes_X = CF_Connection.CreateCommand();
                CF_Cmd_ProgressNotes_X.CommandText = Cf_SQL_INSERT_X;

                CF_Transaction_X.Commit();

                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Appointments_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE APPOINTMENTS
        //**************************************************************************************************
        public bool CF_Delete_Appointments(DateTime pDate, string pCardID)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteCommand CF_Cmd_ProgressNotes_X;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE_X;

            string Cf_SQL_DELETE = "DELETE FROM CFAPPOINTDB ";

            if (!pCardID.Equals(""))
            {
                Cf_SQL_DELETE += " WHERE DATEAPPOINT = " + pDate.ToBinary() + "' AND ";
                Cf_SQL_DELETE += " CARDNUMBER = '" + pCardID + "'";
            }
            else
            {
                return false;
            }

            Cf_SQL_DELETE_X = "DELETE FROM CFPRONOTDB ";
            Cf_SQL_DELETE_X += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
            Cf_SQL_DELETE_X += " NEXTDATEAPP = " + pDate.ToBinary();

            CF_Transaction_X = CF_Connection.BeginTransaction();

            try
            {
                CF_Cmd_Appointments = CF_Connection.CreateCommand();
                CF_Cmd_Appointments.CommandText = Cf_SQL_DELETE;
                Numero_Registros = CF_Cmd_Appointments.ExecuteNonQuery();

                CF_Cmd_ProgressNotes_X = CF_Connection.CreateCommand();
                CF_Cmd_ProgressNotes_X.CommandText = Cf_SQL_DELETE_X;
                Numero_Registros = CF_Cmd_ProgressNotes_X.ExecuteNonQuery();

                CF_Transaction_X.Commit();

                Delete_SI_NO = true;
            }
            catch (SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Appointments_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE APPOINTMENTS
        //**************************************************************************************************
        public bool CF_Modify_Appointments(DateTime pDate, string pCardID)
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;

            SQLiteCommand CF_Cmd_ProgressNotes_X;
            SQLiteTransaction CF_Transaction_X;
            string Cf_SQL_UPDATE_X;
            string Cf_SQL_UPDATE = "UPDATE CFAPPOINTDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            // *******************************************************************************************
            // Procedimiento en Fichas no Modifica la clave primaria o CARDNUMBER
            // Cf_SQL_UPDATE = Cf_SQL_UPDATE + "CARDNUMBER = '" + CF_Appointments_Mem.V_CardID() + "',";
            // *******************************************************************************************
            Cf_SQL_UPDATE += "DATEAPPOINT = " + CF_Appointments_Mem.V_DateAppoint.ToBinary() + ",";  // DATEAPPOINT
            Cf_SQL_UPDATE += "TIMEAPPOINT = " + CF_Appointments_Mem.V_TimeAppoint.ToBinary() + ",";  // TIMEAPPOINT
            Cf_SQL_UPDATE += "CARDNUMBER = '" + CF_Appointments_Mem.V_CardID + "',";                 // CARDNUMBER
            Cf_SQL_UPDATE += "PATIENT = '" + CF_Appointments_Mem.V_Patient + "',";                   // PATIENTS
            Cf_SQL_UPDATE += "LOCDATE = " + CF_Appointments_Mem.V_LocDate.ToBinary();                // LOCDATE

            if (!pCardID.Equals(""))
            {
                Cf_SQL_UPDATE += " WHERE DATEAPPOINT = " + pDate.ToBinary() + " AND ";
                Cf_SQL_UPDATE += " CARDNUMBER = '" + pCardID + "'";
            }
            else
            {
                return false;
            }

            Cf_SQL_UPDATE_X = "UPDATE CFPRONOTDB SET ";
            Cf_SQL_UPDATE_X += "PDATE =  " + CF_Appointments_Mem.V_LocDate.ToBinary() + ",";              // PDATE
            Cf_SQL_UPDATE_X += "NEXTDATEAPP =  " + CF_Appointments_Mem.V_DateAppoint.ToBinary() + ",";    // NEXTDATEAPP
            Cf_SQL_UPDATE_X += "NEXTTIMEAPP =  " + CF_Appointments_Mem.V_TimeAppoint.ToBinary();          // NEXTTIMEAPP
            Cf_SQL_UPDATE_X += " WHERE CARDNUMBER = '" + pCardID + "' AND ";
            Cf_SQL_UPDATE_X += " NEXTDATEAPP = " + pDate.ToBinary();

            CF_Transaction_X = CF_Connection.BeginTransaction();

            try
            {
                CF_Cmd_Appointments = CF_Connection.CreateCommand();
                CF_Cmd_Appointments.CommandText = Cf_SQL_UPDATE;
                Numero_Registros = CF_Cmd_Appointments.ExecuteNonQuery();

                CF_Cmd_ProgressNotes_X = CF_Connection.CreateCommand();
                CF_Cmd_ProgressNotes_X.CommandText = Cf_SQL_UPDATE_X;
                Numero_Registros = CF_Cmd_ProgressNotes_X.ExecuteNonQuery();

                CF_Transaction_X.Commit();

                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_Appointments_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION CON STATISTICS
        //**************************************************************************************************
        //**************************************************************************************************
        // OPERACION SELECT DE STATISTICS
        //**************************************************************************************************

        public bool CF_Select_Statistics(bool cDiagnoses, string tDiagnoses, bool cTreatments, string tTreatments)
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT PDATE, CARDNUMBER FROM CFDIATRADB ";
            if ((cDiagnoses) || (cTreatments))
            {

                Cf_SQL_SELECT += " WHERE ";
            }

            if (cDiagnoses)
            {
                Cf_SQL_SELECT += " DIAGNOSES LIKE '%" + tDiagnoses + "%' ";
            }

            if ((cDiagnoses) && (cTreatments))
            {
                Cf_SQL_SELECT += " AND ";
            }

            if (cTreatments)
            {
                Cf_SQL_SELECT += " TREATMENTS LIKE '%" + tTreatments + "%' ";
            }

            CF_Cmd_Statistics = CF_Connection.CreateCommand();
            CF_Cmd_Statistics.CommandText = Cf_SQL_SELECT;

            CF_DReader_Statistics = CF_Cmd_Statistics.ExecuteReader();
            return CF_DReader_Statistics.Read();
        }

        // Lectura de Proximo Registro
        public bool CF_Next_Statistics()
        {
            return CF_DReader_Statistics.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_Statistics(SQLiteDataReader pCF_DReader_Statistics)
        {
            string lTemp_CardID;
            CF_Statistics_Mem.V_PDate = DateTime.FromBinary(pCF_DReader_Statistics.GetInt64(pCF_DReader_Statistics.GetOrdinal("PDATE")));
            lTemp_CardID = pCF_DReader_Statistics.GetString(pCF_DReader_Statistics.GetOrdinal("CARDNUMBER"));
            CF_Statistics_Mem.V_CardID = lTemp_CardID;
            CF_Statistics_Mem.V_Patient = CF_Find_Patients(lTemp_CardID);
        }

        // Operación con Users
        //**************************************************************************************************
        // OPERACION SELECT DE USER
        //**************************************************************************************************

        public bool CF_Select_User()
        {
            // Operacion SQL
            string Cf_SQL_SELECT = "SELECT ISTOPALM,DATEINSTALL,DATELASTUPDATE,DATEPAY,NAME,ESPECIALITY,MDID,ADDRESS1,ADDRESS2,CITY,ZIPCODE,COUNTRY,EMAIL,PHONE,ORIGINPAY,TRANSACTIONID,SERIAL,KEYGEN,DATABASEPATH,IDIOMA FROM CFUSERDB";

            CF_Cmd_User = CF_Connection.CreateCommand();
            CF_Cmd_User.CommandText = Cf_SQL_SELECT;

            try
            {
                CF_DReader_User = CF_Cmd_User.ExecuteReader();
                return CF_DReader_User.Read();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                return false;
            }
        }

        // Lectura de Proximo Registro
        public bool CF_Next_User()
        {
            return CF_DReader_User.Read();
        }

        // Lectura de Tabla a Memoria
        public void CF_GetDb_User(SQLiteDataReader pCF_DReader_User)
        {
            CF_User_Mem.V_DateInstall = DateTime.FromBinary(pCF_DReader_User.GetInt64(pCF_DReader_User.GetOrdinal("DATEINSTALL")));
            CF_User_Mem.V_DateLastUpdate = DateTime.FromBinary(pCF_DReader_User.GetInt64(pCF_DReader_User.GetOrdinal("DATELASTUPDATE")));
            CF_User_Mem.V_DatePay = DateTime.FromBinary(pCF_DReader_User.GetInt64(pCF_DReader_User.GetOrdinal("DATEPAY")));
            CF_User_Mem.V_Name = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("NAME"));
            CF_User_Mem.V_Especiality = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("ESPECIALITY"));
            CF_User_Mem.V_MDID = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("MDID"));
            CF_User_Mem.V_Address1 = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("ADDRESS1"));
            CF_User_Mem.V_Address2 = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("ADDRESS2"));
            CF_User_Mem.V_City = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("CITY"));
            CF_User_Mem.V_ZipCode = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("ZIPCODE"));
            CF_User_Mem.V_Country = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("COUNTRY"));
            CF_User_Mem.V_eMail = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("EMAIL"));
            CF_User_Mem.V_Phone = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("PHONE"));
            CF_User_Mem.V_OriginPay = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("ORIGINPAY"));
            CF_User_Mem.V_TransactionID = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("TRANSACTIONID"));
            CF_User_Mem.V_Serial = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("SERIAL"));
            CF_User_Mem.V_KeyGen = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("KEYGEN"));
            CF_User_Mem.V_DatabasePath = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("DATABASEPATH"));
            CF_User_Mem.V_Idioma = pCF_DReader_User.GetString(pCF_DReader_User.GetOrdinal("IDIOMA"));
        }

        //**************************************************************************************************
        // OPERACION INSERT DE USER
        //**************************************************************************************************
        public bool CF_Insert_User()
        {
            // Operacion SQL
            bool Insertado_SI_NO;
            SQLiteTransaction CF_Transaction_X;
            int Numero_Registros;

            string Cf_SQL_INSERT = "INSERT INTO CFUSERDB(ISTOPALM,DATEINSTALL,DATELASTUPDATE,DATEPAY,NAME,ESPECIALITY,MDID,ADDRESS1,ADDRESS2,CITY,ZIPCODE,COUNTRY,EMAIL,PHONE,ORIGINPAY,TRANSACTIONID,SERIAL,KEYGEN,DATABASEPATH,IDIOMA) VALUES (";

            Cf_SQL_INSERT += "0" + ",";                                             // ISTOPALM
            Cf_SQL_INSERT += " " + CF_User_Mem.V_DateInstall.ToBinary() + ",";
            Cf_SQL_INSERT += " " + CF_User_Mem.V_DateLastUpdate.ToBinary() + ",";
            Cf_SQL_INSERT += " " + CF_User_Mem.V_DatePay.ToBinary() + ",";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Name + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Especiality + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_MDID + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Address1 + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Address2 + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_City + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_ZipCode + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Country + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_eMail + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Phone + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_OriginPay + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_TransactionID + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Serial + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_KeyGen + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_DatabasePath + "',";
            Cf_SQL_INSERT += "'" + CF_User_Mem.V_Idioma + "')";

            if (CF_User_Mem.V_MDID == "")
            {
                CF_User_Mem.V_Error_Operation = "MDID es nulo";
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_User = CF_Connection.CreateCommand();
            CF_Cmd_User.CommandText = Cf_SQL_INSERT;

            try
            {
                Numero_Registros = CF_Cmd_User.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Insertado_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Insertado_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_User_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Insertado_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION DELETE DE USER
        //**************************************************************************************************
        public bool CF_Delete_User(string pMDID)
        {
            // Operacion SQL
            bool Delete_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_DELETE = "DELETE FROM CFUSERDB ";

            if (!pMDID.Equals(""))
            {
                Cf_SQL_DELETE = Cf_SQL_DELETE + " WHERE MDID = '" + pMDID + "'";
            }
            else
            {
                return false;
            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_User = CF_Connection.CreateCommand();
            CF_Cmd_User.CommandText = Cf_SQL_DELETE;

            try
            {
                Numero_Registros = CF_Cmd_User.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Delete_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Delete_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_User_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Delete_SI_NO;
        }

        //**************************************************************************************************
        // OPERACION UPDATE DE USER
        //**************************************************************************************************
        public bool CF_Modify_User()
        {
            // Operacion SQL
            bool Modify_SI_NO;
            int Numero_Registros;
            SQLiteTransaction CF_Transaction_X;

            string Cf_SQL_UPDATE = "UPDATE CFUSERDB SET ";
            Cf_SQL_UPDATE += "ISTOPALM = 0,";
            Cf_SQL_UPDATE += "DATEINSTALL = " + CF_User_Mem.V_DateInstall.ToBinary() + ",";
            Cf_SQL_UPDATE += "DATELASTUPDATE = " + CF_User_Mem.V_DateLastUpdate.ToBinary() + ",";
            Cf_SQL_UPDATE += "DATEPAY = " + CF_User_Mem.V_DatePay.ToBinary() + ",";
            Cf_SQL_UPDATE += "NAME = '" + CF_User_Mem.V_Name + "',";
            Cf_SQL_UPDATE += "ESPECIALITY = '" + CF_User_Mem.V_Especiality + "',";
            Cf_SQL_UPDATE += "MDID = '" + CF_User_Mem.V_MDID + "',";
            Cf_SQL_UPDATE += "ADDRESS1 = '" + CF_User_Mem.V_Address1 + "',";
            Cf_SQL_UPDATE += "ADDRESS2 = '" + CF_User_Mem.V_Address2 + "',";
            Cf_SQL_UPDATE += "CITY = '" + CF_User_Mem.V_City + "',";
            Cf_SQL_UPDATE += "ZIPCODE = '" + CF_User_Mem.V_ZipCode + "',";
            Cf_SQL_UPDATE += "COUNTRY = '" + CF_User_Mem.V_Country + "',";
            Cf_SQL_UPDATE += "EMAIL = '" + CF_User_Mem.V_eMail + "',";
            Cf_SQL_UPDATE += "PHONE = '" + CF_User_Mem.V_Phone + "',";
            Cf_SQL_UPDATE += "ORIGINPAY = '" + CF_User_Mem.V_OriginPay + "',";
            Cf_SQL_UPDATE += "TRANSACTIONID = '" + CF_User_Mem.V_TransactionID + "',";
            Cf_SQL_UPDATE += "SERIAL = '" + CF_User_Mem.V_Serial + "',";
            Cf_SQL_UPDATE += "KEYGEN = '" + CF_User_Mem.V_KeyGen + "',";
            Cf_SQL_UPDATE += "DATABASEPATH = '" + CF_User_Mem.V_DatabasePath + "',";
            Cf_SQL_UPDATE += "IDIOMA = '" + CF_User_Mem.V_Idioma + "'";

            //            if (!pMDID.Equals(""))
            //            {
            //                Cf_SQL_UPDATE = Cf_SQL_UPDATE + " WHERE MDID = '" + pMDID + "'";
            //            }
            //            else
            //            {
            //                return false;
            //            }

            CF_Transaction_X = CF_Connection.BeginTransaction();
            CF_Cmd_User = CF_Connection.CreateCommand();
            CF_Cmd_User.CommandText = Cf_SQL_UPDATE;

            try
            {
                Numero_Registros = CF_Cmd_User.ExecuteNonQuery();
                CF_Transaction_X.Commit();
                Modify_SI_NO = true;
            }
            catch (System.Data.SQLite.SQLiteException ErrorSQLite)
            {
                Modify_SI_NO = false;
                CF_Transaction_X.Rollback();
                CF_User_Mem.V_Error_Operation = ErrorSQLite.Message;
            }

            return Modify_SI_NO;
        }

        public CF_Database_Class()
        {
            CF_Database = new();
            CF_Connection = new();

            CF_Cmd_BasicData = new();
            CF_Cmd_PhysicalExamination = new();
            CF_Cmd_Radiography = new();
            CF_Cmd_EKGR = new();
            CF_Cmd_EchoDoppler = new();
            CF_Cmd_Holter = new();
            CF_Cmd_StressTest = new();
            CF_Cmd_CardiacCath = new();
            CF_Cmd_Surgery = new();
            CF_Cmd_DiagnosesTreatments = new();
            CF_Cmd_ProgressNotes = new();
            CF_Cmd_Appointments = new();
            CF_Cmd_Statistics = new();
            CF_Cmd_User = new();
        }
    }
}
