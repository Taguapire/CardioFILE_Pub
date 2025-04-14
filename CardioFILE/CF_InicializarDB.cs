using System.Data.SQLite;

namespace CardioFILE
{
    class CF_InicializarDB
    {
        readonly string[] crear_Tablas = new string[20];
        readonly string[] crear_Indices = new string[20];
        // readonly string[] crear_Vistas = new string[5];
        readonly string[] crear_Triggers = new string[5];
        readonly string[] crear_Datos_Iniciales = new string[10];

        readonly SQLiteConnection CF_IniciaDB;

        public CF_InicializarDB(SQLiteConnection pCF_IniciaDB)
        {
            CF_IniciaDB = pCF_IniciaDB;
            // Tablas
            crear_Tablas[0] = "CREATE TABLE CFAPPOINTDB(ISTOPALM BIT, DATEAPPOINT DATE NOT NULL, TIMEAPPOINT TIME NOT NULL, CARDNUMBER VARCHAR NOT NULL, PATIENT VARCHAR, LOCDATE DATE)";
            crear_Tablas[1] = "CREATE TABLE CFBASDATDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, SSN VARCHAR, LASTNAME VARCHAR, FIRSTNAME VARCHAR, BDATE DATE, SEX INTEGER, ADDRESS VARCHAR, PHONENUMBER VARCHAR, ZIPCODE VARCHAR, RECONS VARCHAR, PSTATE VARCHAR, PHONETYPE INTEGER, EMAIL VARCHAR, BLOODGROUP INTEGER, BLOODFACTOR INTEGER)";
            crear_Tablas[2] = "CREATE TABLE CFCARCATDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, OBSERV VARCHAR NOT NULL)";
            crear_Tablas[3] = "CREATE TABLE CFDIATRADB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, DIAGNOSES VARCHAR, TREATMENTS VARCHAR)";
            crear_Tablas[4] = "CREATE TABLE CFECODOPDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, ECHO VARCHAR, DOPPLER VARCHAR, LABORATORY VARCHAR)";
            crear_Tablas[5] = "CREATE TABLE CFEKGRDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, OBSERV VARCHAR)";
            crear_Tablas[6] = "CREATE TABLE CFEXAHOLDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, OBSERV VARCHAR)";
            crear_Tablas[7] = "CREATE TABLE CFHEIGHTTYPE(ID INTEGER NOT NULL, DESCRIPTION VARCHAR)";
            crear_Tablas[8] = "CREATE TABLE CFPHONETYPE(ID INTEGER NOT NULL, DESCRIPTION VARCHAR)";
            crear_Tablas[9] = "CREATE TABLE CFPHYEXADB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, FAMIANT VARCHAR, PERSANT VARCHAR, BLOODPRES_MAX VARCHAR, BLOODPRES_MIN VARCHAR, PULSE VARCHAR, HEIGHT DOUBLE, WEIGHT DOUBLE, BMI DOUBLE, OBSERV VARCHAR, HEIGHT_ITEM INTEGER, WEIGHT_ITEM INTEGER)";
            crear_Tablas[10] = "CREATE TABLE CFPRONOTDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, OBSERV VARCHAR, NEXTDATEAPP DATE, NEXTTIMEAPP TIME)";
            crear_Tablas[11] = "CREATE TABLE CFRADGRADB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, OBSERV VARCHAR)";
            crear_Tablas[12] = "CREATE TABLE CFRESULT(RESULTX INTEGER NOT NULL, DESCRIPTIONX VARCHAR)";
            crear_Tablas[13] = "CREATE TABLE CFSEX(ID INTEGER, DESCRIPTION VARCHAR)";
            crear_Tablas[14] = "CREATE TABLE CFSTAGE(STAGE INTEGER, DESCRIPTION VARCHAR)";
            crear_Tablas[15] = "CREATE TABLE CFSTRTESSDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, PROTOCOL VARCHAR, STAGE VARCHAR, PTIME VARCHAR, CARDIACFREC INTEGER, PERCENTMAX INTEGER, ARRHYTH VARCHAR, BLOOD_PRESS VARCHAR, PTYPE VARCHAR, FUNCTIONALCAP VARCHAR, RE_SUSP VARCHAR, RESULT VARCHAR, DESC_STT VARCHAR)";
            crear_Tablas[16] = "CREATE TABLE CFSURGERYDB(ISTOPALM BIT, CARDNUMBER VARCHAR NOT NULL, PDATE DATE NOT NULL, OBSERV VARCHAR)";
            crear_Tablas[17] = "CREATE TABLE CFUSERDB(ISTOPALM BIT, DATEINSTALL DATE, DATELASTUPDATE DATE, DATEPAY DATE, NAME VARCHAR, ESPECIALITY VARCHAR, MDID VARCHAR, ADDRESS1 VARCHAR, ADDRESS2 VARCHAR, CITY VARCHAR, ZIPCODE VARCHAR, COUNTRY VARCHAR, EMAIL VARCHAR, PHONE VARCHAR, ORIGINPAY VARCHAR, TRANSACTIONID VARCHAR, SERIAL VARCHAR, KEYGEN VARCHAR, DATABASEPATH VARCHAR, IDIOMA VARCHAR)";
            crear_Tablas[18] = "CREATE TABLE CFWEIGHTTYPE(ID INTEGER NOT NULL, DESCRIPTION VARCHAR)";

            // Indices
            crear_Indices[0] = "CREATE UNIQUE INDEX CFBASDATDB_PK ON CFBASDATDB(CARDNUMBER)";
            crear_Indices[1] = "CREATE UNIQUE INDEX CFAPPOINTDB_PK ON CFAPPOINTDB(DATEAPPOINT,TIMEAPPOINT,CARDNUMBER)";
            crear_Indices[2] = "CREATE UNIQUE INDEX CFCARCATDB_PK ON CFCARCATDB(CARDNUMBER,PDATE)";
            crear_Indices[3] = "CREATE UNIQUE INDEX CFDIATRADB_PK ON CFDIATRADB(CARDNUMBER,PDATE)";
            crear_Indices[4] = "CREATE UNIQUE INDEX CFECODOPDB_PK ON CFECODOPDB(CARDNUMBER,PDATE)";
            crear_Indices[5] = "CREATE UNIQUE INDEX CFEKGRDB_PK ON CFEKGRDB(CARDNUMBER,PDATE)";
            crear_Indices[6] = "CREATE UNIQUE INDEX CFEXAHOLDB_PK ON CFEXAHOLDB(CARDNUMBER,PDATE)";
            crear_Indices[7] = "CREATE UNIQUE INDEX CFPHONETYPE_PK ON CFPHONETYPE(ID)";
            crear_Indices[8] = "CREATE UNIQUE INDEX CFPRONOTDB_PK ON CFPRONOTDB(CARDNUMBER,PDATE)";
            crear_Indices[9] = "CREATE UNIQUE INDEX CFRADGRADB_PK ON CFRADGRADB(CARDNUMBER,PDATE)";
            crear_Indices[10] = "CREATE UNIQUE INDEX CFRESULT_PK ON CFRESULT(RESULTX)";
            crear_Indices[11] = "CREATE UNIQUE INDEX CFSEX_PK ON CFSEX(ID)";
            crear_Indices[12] = "CREATE UNIQUE INDEX CFSTAGE_PK ON CFSTAGE(STAGE)";
            crear_Indices[13] = "CREATE UNIQUE INDEX CFSTRTESSDB_PK ON CFSTRTESSDB(CARDNUMBER,PDATE)";
            crear_Indices[14] = "CREATE UNIQUE INDEX CFSURGERYDB_PK ON CFSURGERYDB(CARDNUMBER,PDATE)";

            // Vistas
            // En estudios para reportes

            // Triggers
            crear_Triggers[0] = "CREATE TRIGGER CARDID_DELETE DELETE ON CFBASDATDB FOR EACH ROW BEGIN DELETE FROM CFPHYEXADB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFAPPOINTDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFCARCATDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFDIATRADB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFECODOPDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFEKGRDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFEXAHOLDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFPRONOTDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFRADGRADB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFSTRTESSDB WHERE CARDNUMBER = OLD.CARDNUMBER; DELETE FROM CFSURGERYDB WHERE CARDNUMBER = OLD.CARDNUMBER; END";
            crear_Triggers[1] = "CREATE TRIGGER CARDID_UPDATE UPDATE OF CARDNUMBER ON CFBASDATDB BEGIN UPDATE CFPHYEXADB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFAPPOINTDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFCARCATDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFDIATRADB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFECODOPDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFEKGRDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFEXAHOLDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFPRONOTDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFRADGRADB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFSTRTESSDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; UPDATE CFSURGERYDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; END";

            // Datos Iniciales
            crear_Datos_Iniciales[0] = "INSERT INTO CFWEIGHTTYPE (ID,DESCRIPTION) VALUES (1,'lb'), (2,'kg'), (3,'gr')";
            crear_Datos_Iniciales[1] = "INSERT INTO CFSTAGE (STAGE,DESCRIPTION) VALUES (1,'I'), (2,'II'), (3,'III'), (4,'IV')";
            crear_Datos_Iniciales[2] = "INSERT INTO CFSEX (ID,DESCRIPTION) VALUES (1,'Masculino'), (2,'Femenino')";
            crear_Datos_Iniciales[3] = "INSERT INTO CFPHONETYPE (ID,DESCRIPTION) VALUES (1,'Telefono'), (2,'Celular'), (3,'Fax')";
            crear_Datos_Iniciales[4] = "INSERT INTO CFHEIGHTTYPE (ID,DESCRIPTION) VALUES (1,'ft'), (2,'inch'), (3,'mts'), (4,'cms')";
        }

        public bool Proc_Crear_tablas()
        {
            SQLiteCommand Sql_Crear_Tablas;
            try {
                for (int i = 0; i <= 18; i++)
                {
                    Sql_Crear_Tablas = CF_IniciaDB.CreateCommand();
                    Sql_Crear_Tablas.CommandText = crear_Tablas[i];
                    Sql_Crear_Tablas.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                return false;
            }
        }

        public bool Proc_Crear_indices()
        {
            SQLiteCommand Sql_Crear_Indices;
            try
            {
                for (int i= 0; i <= 14; i++)
                {
                Sql_Crear_Indices = CF_IniciaDB.CreateCommand();
                Sql_Crear_Indices.CommandText = crear_Indices[i];
                Sql_Crear_Indices.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                return false;
            }
}

        public bool Proc_Crear_triggers()
        {
            SQLiteCommand Sql_Crear_Triggers;
            try
            {
                for (int i = 0; i <= 1; i++)
                {
                Sql_Crear_Triggers = CF_IniciaDB.CreateCommand();
                Sql_Crear_Triggers.CommandText = crear_Triggers[i];
                Sql_Crear_Triggers.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                return false;
            }
        }

        public bool Proc_Datos_Iniciales()
        {
            SQLiteCommand Sql_Datos_Iniciales;
            try
            {
                for (int i = 0; i <= 4; i++)
                {
                Sql_Datos_Iniciales = CF_IniciaDB.CreateCommand();
                Sql_Datos_Iniciales.CommandText = crear_Datos_Iniciales[i];
                Sql_Datos_Iniciales.ExecuteNonQuery();
                }
                    return true;
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                return false;
            } 
        }
    }
}

