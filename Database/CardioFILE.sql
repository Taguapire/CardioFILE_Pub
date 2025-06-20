BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "CFWEIGHTTYPE" (
	"ID"	INTEGER NOT NULL,
	"DESCRIPTION"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFSURGERYDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"OBSERV"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFSTRTESSDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"PROTOCOL"	VARCHAR,
	"STAGE"	INTEGER,
	"PTIME"	TIME,
	"CARDIACFREC"	INTEGER,
	"PERCENTMAX"	INTEGER,
	"ARRHYTH"	VARCHAR,
	"BLOOD_PRESS"	VARCHAR,
	"PTYPE"	VARCHAR,
	"FUNCTIONALCAP"	VARCHAR,
	"RE_SUSP"	VARCHAR,
	"RESULT"	INTEGER,
	"DESC_STT"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFSTAGE" (
	"STAGE"	INTEGER,
	"DESCRIPTION"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFSEX" (
	"ID"	INTEGER,
	"DESCRIPTION"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFRESULT" (
	"RESULTX"	INTEGER NOT NULL,
	"DESCRIPTIONX"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFRADGRADB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"OBSERV"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFPRONOTDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"OBSERV"	VARCHAR,
	"NEXTDATEAPP"	DATE,
	"NEXTTIMEAPP"	TIME
);
CREATE TABLE IF NOT EXISTS "CFPHYEXADB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"FAMIANT"	VARCHAR,
	"PERSANT"	VARCHAR,
	"BLOODPRES_MAX"	VARCHAR,
	"BLOODPRES_MIN"	VARCHAR,
	"PULSE"	VARCHAR,
	"HEIGHT"	DOUBLE,
	"WEIGHT"	DOUBLE,
	"BMI"	DOUBLE,
	"OBSERV"	VARCHAR,
	"HEIGHT_ITEM"	INTEGER,
	"WEIGHT_ITEM"	INTEGER
);
CREATE TABLE IF NOT EXISTS "CFPHONETYPE" (
	"ID"	INTEGER NOT NULL,
	"DESCRIPTION"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFHEIGHTTYPE" (
	"ID"	INTEGER NOT NULL,
	"DESCRIPTION"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFEXAHOLDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"OBSERV"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFEKGRDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"OBSERV"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFECODOPDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"ECHO"	VARCHAR,
	"DOPPLER"	VARCHAR,
	"LABORATORY"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFDIATRADB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"DIAGNOSES"	VARCHAR,
	"TREATMENTS"	VARCHAR
);
CREATE TABLE IF NOT EXISTS "CFCARCATDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PDATE"	DATE NOT NULL,
	"OBSERV"	VARCHAR NOT NULL
);
CREATE TABLE IF NOT EXISTS "CFAPPOINTDB" (
	"ISTOPALM"	BIT,
	"DATEAPPOINT"	DATE NOT NULL,
	"TIMEAPPOINT"	TIME NOT NULL,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"PATIENT"	VARCHAR,
	"LOCDATE"	DATE
);
CREATE TABLE IF NOT EXISTS "CFBASDATDB" (
	"ISTOPALM"	BIT,
	"CARDNUMBER"	VARCHAR NOT NULL,
	"SSN"	VARCHAR,
	"LASTNAME"	VARCHAR,
	"FIRSTNAME"	VARCHAR,
	"BDATE"	DATE,
	"SEX"	INTEGER,
	"ADDRESS"	VARCHAR,
	"PHONENUMBER"	VARCHAR,
	"ZIPCODE"	VARCHAR,
	"RECONS"	VARCHAR,
	"PSTATE"	VARCHAR,
	"PHONETYPE"	INTEGER,
	"EMAIL"	VARCHAR,
	"BLOODGROUP"	INTEGER,
	"BLOODFACTOR"	INTEGER
);
CREATE TABLE IF NOT EXISTS "CFUSERDB" (
	"ISTOPALM"	BIT,
	"DATEINSTALL"	DATE,
	"DATELASTUPDATE"	DATE,
	"DATEPAY"	DATE,
	"NAME"	VARCHAR,
	"ESPECIALITY"	VARCHAR,
	"MDID"	VARCHAR,
	"ADDRESS1"	VARCHAR,
	"ADDRESS2"	VARCHAR,
	"CITY"	VARCHAR,
	"ZIPCODE"	VARCHAR,
	"COUNTRY"	VARCHAR,
	"EMAIL"	VARCHAR,
	"PHONE"	VARCHAR,
	"ORIGINPAY"	VARCHAR,
	"TRANSACTIONID"	VARCHAR,
	"SERIAL"	VARCHAR,
	"KEYGEN"	VARCHAR,
	"DATABASEPATH"	VARCHAR
);
INSERT INTO "CFWEIGHTTYPE" ("ID","DESCRIPTION") VALUES (1,'lb'),
 (2,'kg'),
 (3,'gr');
INSERT INTO "CFSTAGE" ("STAGE","DESCRIPTION") VALUES (1,'I'),
 (2,'II'),
 (3,'III'),
 (4,'IV');
INSERT INTO "CFSEX" ("ID","DESCRIPTION") VALUES (1,'Male'),
 (2,'Female');
INSERT INTO "CFPHONETYPE" ("ID","DESCRIPTION") VALUES (1,'Phone'),
 (2,'Celular'),
 (3,'Fax');
INSERT INTO "CFHEIGHTTYPE" ("ID","DESCRIPTION") VALUES (1,'ft'),
 (2,'inch'),
 (3,'m'),
 (4,'cm');
CREATE UNIQUE INDEX IF NOT EXISTS "CFSURGERYDB_PK" ON "CFSURGERYDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFSTRTESSDB_PK" ON "CFSTRTESSDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFSTAGE_PK" ON "CFSTAGE" (
	"STAGE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFSEX_PK" ON "CFSEX" (
	"ID"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFRESULT_PK" ON "CFRESULT" (
	"RESULTX"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFRADGRADB_PK" ON "CFRADGRADB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFPRONOTDB_PK" ON "CFPRONOTDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFPHONETYPE_PK" ON "CFPHONETYPE" (
	"ID"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFEXAHOLDB_PK" ON "CFEXAHOLDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFEKGRDB_PK" ON "CFEKGRDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFECODOPDB_PK" ON "CFECODOPDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFDIATRADB_PK" ON "CFDIATRADB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFCARCATDB_PK" ON "CFCARCATDB" (
	"CARDNUMBER",
	"PDATE"
);
CREATE UNIQUE INDEX IF NOT EXISTS "CFAPPOINTDB_PK" ON "CFAPPOINTDB" (
	"DATEAPPOINT",
	"TIMEAPPOINT",
	"CARDNUMBER"
);
CREATE TRIGGER CARDID_DELETE DELETE ON CFBASDATDB FOR EACH ROW 
BEGIN
	DELETE FROM CFPHYEXADB WHERE CARDNUMBER = OLD.CARDNUMBER; 
	DELETE FROM CFAPPOINTDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFCARCATDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFDIATRADB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFECODOPDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFEKGRDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFEXAHOLDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFPRONOTDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFRADGRADB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFSTRTESSDB WHERE CARDNUMBER = OLD.CARDNUMBER;
	DELETE FROM CFSURGERYDB WHERE CARDNUMBER = OLD.CARDNUMBER;
END;
CREATE TRIGGER CARDID_UPDATE UPDATE OF CARDNUMBER ON CFBASDATDB
BEGIN
	UPDATE CFPHYEXADB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER; 
	UPDATE CFAPPOINTDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFCARCATDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFDIATRADB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFECODOPDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFEKGRDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFEXAHOLDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFPRONOTDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFRADGRADB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFSTRTESSDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
	UPDATE CFSURGERYDB SET CARDNUMBER = NEW.CARDNUMBER WHERE CARDNUMBER = OLD.CARDNUMBER;
END;
COMMIT;
