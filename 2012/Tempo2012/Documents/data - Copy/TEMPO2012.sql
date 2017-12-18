/********************* ROLES **********************/

CREATE ROLE RDB$ADMIN;
/********************* UDFS ***********************/

/****************** GENERATORS ********************/

CREATE GENERATOR ACCOUNTS_ID;
CREATE GENERATOR CONTOGEN;
CREATE GENERATOR CONTOMOVEMENTGEN;
CREATE GENERATOR DDSDNEVFIELDSAUTOINC;
CREATE GENERATOR FIRM_ID;
CREATE GENERATOR GENANALITICALACCOUNTTYPE;
CREATE GENERATOR GENDDSNEW;
CREATE GENERATOR GENERATORLOOKUPSFIELD;
CREATE GENERATOR GENERATORNOM_10;
CREATE GENERATOR GENERATORNOM_11;
CREATE GENERATOR GENERATORNOM_12;
CREATE GENERATOR GENERATORNOM_13;
CREATE GENERATOR GENERATORNOM_14;
CREATE GENERATOR GENERATORNOM_15;
CREATE GENERATOR GENERATORNOM_16;
CREATE GENERATOR GENERATORNOM_17;
CREATE GENERATOR GENERATORNOM_18;
CREATE GENERATOR GENERATORNOM_19;
CREATE GENERATOR GENERATORNOM_2;
CREATE GENERATOR GENERATORNOM_20;
CREATE GENERATOR GENERATORNOM_21;
CREATE GENERATOR GENERATORNOM_22;
CREATE GENERATOR GENERATORNOM_23;
CREATE GENERATOR GENERATORNOM_3;
CREATE GENERATOR GENERATORNOM_5;
CREATE GENERATOR GENERATORNOM_6;
CREATE GENERATOR GENERATORNOM_7;
CREATE GENERATOR GENERATORNOM_8;
CREATE GENERATOR GENERATORNOM_9;
CREATE GENERATOR "GEN_conectoranaliticfield_ID";
CREATE GENERATOR MOVEMENTID;
CREATE GENERATOR NEWANALITICALACC;
CREATE GENERATOR NOM_1;
CREATE GENERATOR SALDOSID;
CREATE GENERATOR SYSLOOKUP;
/******************** DOMAINS *********************/

/******************* PROCEDURES ******************/

SET TERM ^ ;
CREATE PROCEDURE ADDCONTO (
    "Date" Date,
    "Oborot" Decimal(18,4),
    "Reason" Varchar(50),
    "Note" Varchar(50),
    "DataInvoise" Date,
    "NumberObject" Integer,
    "DebitAccount" Integer,
    "CreditAccount" Integer,
    "FirmId" Integer,
    "DocumentId" Integer,
    "CartotekaDebit" Integer,
    "CartotecaCredit" Integer,
    DOCNUM Varchar(20),
    "OborotValuta" Decimal(18,4),
    "OborotKol" Decimal(18,4),
    "OborotValutaK" Decimal(18,4),
    "OborotKolK" Decimal(18,4),
    FOLDER Varchar(10) )
RETURNS (
    NEWID Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE ADDDDSDNEV (
    NOM Bigint,
    BRANCH Varchar(50),
    DOCN Varchar(20),
    DATADOC Date,
    KINDACTIVITY Integer,
    KINDDOC Integer,
    STOKE Varchar(50),
    BULSTAD Varchar(20),
    NZDDS Varchar(20),
    LOOKUPID Integer,
    LOOKUPELEMENTID Integer,
    NAMEKONTR Varchar(50),
    SUMA Numeric(18,4),
    DDSSUMA Numeric(18,4),
    CODEDOC Varchar(2) )
RETURNS (
    NEWID Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE ADDMOVENT (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer,
    VALKOLK Decimal(18,4),
    VALKOLD Decimal(18,4),
    VALVALD Decimal(18,4),
    VALVALK Decimal(18,4) )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE ADDMOVENTCONTO (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer,
    CONTOID Integer,
    "TYPE" Smallint )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE CHECKLOOKUP (
    VAL Varchar(50),
    LOOKUPID Integer )
RETURNS (
    ISIN Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE CHECKMOVENT (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer )
RETURNS (
    ISIN Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE COPYACCSYTOY (
    FY Integer,
    TY Integer,
    FID Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE DELETEACCOUNT (
    ID Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE DELETECONTO (
    CONTOID Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE DELETEDNEV (
    NOM Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE DELETEFIRMA (
    ID Integer )
RETURNS (
    CONFIRMDELETE Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE ENDSALDO (
    ACCID Integer )
RETURNS (
    ENDSALDO Decimal(18,4),
    ENDSALDOV Decimal(18,4),
    ENDSALDOK Decimal(18,4) )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE GETALLOBOROTKA (
    "DateFrom" Date,
    "DateTo" Date,
    "FirmId" Integer )
RETURNS (
    NUM Varchar(20),
    NAME Varchar(200),
    NSD Decimal(18,4),
    OBD Decimal(18,4),
    KSD Decimal(18,4),
    NSK Decimal(18,4),
    OBK Decimal(18,4),
    KSK Decimal(18,4) )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE GETOBOROTKA (
    "DateFrom" Date,
    "DateTo" Date,
    "FirmId" Integer,
    "AccId" Integer )
RETURNS (
    NSD Decimal(18,4),
    OBD Decimal(18,4),
    KSD Decimal(18,4),
    NSK Decimal(18,4),
    OBK Decimal(18,4),
    KSK Decimal(18,4) )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE IP_ACCOUNTS (
    "NameMain" Varchar(200),
    "NameMainEng" Varchar(200),
    "NameSub" Varchar(200),
    "NameSubEng" Varchar(200),
    "SubNum" Integer,
    "AnaliticalNum" Integer,
    "PartidNum" Integer,
    "TypeAccount" Integer,
    "LevelAccount" Integer,
    "TypeSaldo" Integer,
    "FirmaId" Integer,
    "IsNew" Integer,
    "Id" Integer,
    NUM Integer,
    SALDO Decimal(18,4),
    SALDOVALUTA Decimal(18,4),
    TYPEANALITICALKEY Integer,
    SALDODEBIT Decimal(18,4),
    SALDODEBITVALUTA Decimal(18,4),
    ISBUDJET Integer,
    SALDOKK Decimal(18,4),
    SALDODK Decimal(18,4),
    YY Integer )
RETURNS (
    "NewId" Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE IP_FIRM (
    "Name" Varchar(50),
    "Bulstad" Varchar(15),
    "DDSnum" Varchar(15),
    "City" Integer,
    "Country" Integer,
    "Address" Varchar(100),
    "Telefon" Varchar(20),
    "Presentor" Varchar(50),
    "NameBoss" Varchar(50),
    EGN Char(10),
    "PresentorYN" Smallint,
    "Names" Varchar(50),
    "Tel" Varchar(20),
    "FirstName" Varchar(50),
    "SurName" Varchar(50),
    "City2" Integer,
    "Address2" Varchar(100),
    "LastName" Varchar(50),
    REGISERDDS Smallint )
RETURNS (
    "Id" Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE IP_LOOKUP (
    "Name" Varchar(50),
    DESCRIPTION Varchar(50) )
RETURNS (
    "Id" Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE UPDATECONTO (
    "Date" Date,
    "Oborot" Decimal(18,4),
    "Reason" Varchar(50),
    "Note" Varchar(50),
    "DataInvoise" Date,
    "NumberObject" Integer,
    "DebitAccount" Integer,
    "CreditAccount" Integer,
    "FirmId" Integer,
    "DocumentId" Integer,
    "CartotekaDebit" Integer,
    "CartotecaCredit" Integer,
    "ContoID" Integer,
    DOCNUM Varchar(20),
    OBOROTVALUTA Decimal(18,4),
    OBOROTKOL Decimal(18,4),
    OBOROTVALUTAK Decimal(18,4),
    OBOROTKOLK Decimal(18,4),
    FOLDER Varchar(10) )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE UPDATEMOVENT (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    VALUED Decimal(18,4),
    GROUPID Integer,
    VALKOLK Decimal(18,4),
    VALKOLD Decimal(18,4),
    VALVALD Decimal(18,4),
    VALVALK Decimal(18,4) )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE UPDATEMOVENTCONTO (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer,
    CONTOID Integer,
    "TYPE" Smallint,
    ID Integer )
AS
BEGIN SUSPEND; END^
SET TERM ; ^

/******************** TABLES **********************/

CREATE TABLE CONTOMOVEMENT
(
  ACCID Integer,
  ACCFIELDKEY Integer,
  LOOKUPFIELDKEY Integer,
  DATA Date,
  ID Bigint NOT NULL,
  "VALUE" Varchar(50),
  VALUEDATE Date,
  VALUEMONEY Decimal(18,4),
  VALUENUM Integer,
  TYPEACCKEY Bigint,
  VALUED Decimal(18,4),
  CONTOID Integer,
  LOOKUPID Integer,
  "TYPE" Smallint NOT NULL,
  CONSTRAINT PK_CONTOMOVEMENT_1 PRIMARY KEY (ID)
);
CREATE TABLE DDSDNEV
(
  ID Bigint NOT NULL,
  NOM Bigint,
  BRANCH Varchar(50),
  DOCN Varchar(20),
  DATADOC Date,
  KINDACTIVITY Integer,
  KINDDOC Integer,
  STOKE Varchar(50),
  BULSTAD Varchar(20),
  NZDDS Varchar(20),
  LOOKUPID Integer,
  LOOKUPELEMENTID Integer,
  NAMEKONTR Varchar(50) NOT NULL,
  SUMA Numeric(18,4) NOT NULL,
  DDSSUMA Numeric(18,4) NOT NULL,
  CODEDOC Varchar(2) NOT NULL,
  CONSTRAINT PK_DDSDNEV PRIMARY KEY (ID)
);
CREATE TABLE DDSDNEVFIELDS
(
  ID Integer NOT NULL,
  DDSPERCENT Integer,
  NAME Varchar(1000),
  CODE Varchar(10),
  CONSTRAINT PK_DDSDNEVFIELDS PRIMARY KEY (ID)
);
CREATE TABLE DDSDNEVSELLSFIELDS
(
  ID Integer NOT NULL,
  DDSPERCENT Integer,
  NAME Varchar(1000),
  CODE Varchar(10),
  CONSTRAINT PK_DDSDNEVSELLSFIELDS PRIMARY KEY (ID)
);
CREATE TABLE DDSDNEVTOFIELDS
(
  IDDDSDNEV Integer NOT NULL,
  IDDDSFIELD Integer NOT NULL,
  SUMADDS Decimal(18,4),
  SUMAWITHDDS Decimal(18,4)
);
CREATE TABLE FIRMANALITICACCOUNTS
(
  ID Integer NOT NULL,
  DETAILSKEY Integer NOT NULL,
  CONSTRAINT PK_FIRMANALITICACCOUNTS PRIMARY KEY (ID)
);
CREATE TABLE LOOLUPTOLOOKUP
(
  MASTERFIELDID Integer,
  SLAVEFIELDID Integer,
  LOOKUPID Integer
);
CREATE TABLE MAINACCOUNTS
(
  ID Integer NOT NULL,
  NAMEMAIN Varchar(200),
  NAMEMAINENG Varchar(200),
  NUM Integer NOT NULL,
  CONSTRAINT PK_MAINACCOUNTS PRIMARY KEY (ID)
);
CREATE TABLE MAPACCTOLOOKUP
(
  ACCOUNTS_ID Integer NOT NULL,
  LOOKUP_ID Integer NOT NULL,
  FIELDLOOKUP_ID Integer NOT NULL,
  ANALITIC_ID Integer NOT NULL,
  ANALITIC_FIELD_ID Integer NOT NULL
);
CREATE TABLE MAPLOOKUPTOLOOKUP
(
  MASTERTYPELOOKUP Smallint NOT NULL,
  MASTERLOOKUP_ID Integer NOT NULL,
  MASTERFIELD_ID Integer NOT NULL,
  SLAVETYPELOOKUP Smallint NOT NULL,
  SLAVELOOKUP_ID Integer NOT NULL,
  SLAVEFIELD_ID Integer NOT NULL
);
CREATE TABLE MOVEMENT
(
  ACCID Integer,
  ACCFIELDKEY Integer,
  LOOKUPFIELDKEY Integer,
  DATA Date,
  ID Bigint NOT NULL,
  "VALUE" Varchar(50),
  VALUEDATE Date,
  VALUEMONEY Decimal(18,4),
  VALUENUM Integer,
  TYPEACCKEY Bigint,
  VALUED Decimal(18,4),
  "group" Integer,
  LOOKUPID Integer,
  VALKOLK Numeric(18,4) NOT NULL,
  VALKOLD Numeric(18,4) NOT NULL,
  VALVALD Numeric(18,4) NOT NULL,
  VALVALK Numeric(18,4) NOT NULL,
  CONSTRAINT PK_MOVEMENT_1 PRIMARY KEY (ID)
);
CREATE TABLE MOVEMENTTOACC
(
  ACCID Bigint,
  DATA Date,
  DETAILID Bigint
);
CREATE TABLE MPATYPETOAFIELD
(
  ATYPEID Integer NOT NULL,
  AFIELDID Integer NOT NULL
);
CREATE TABLE SALDOS
(
  ID Integer NOT NULL,
  SALDODEBIT Decimal(18,4),
  SALDOKREDIT Decimal(18,4),
  SALDOVALUTADEBIT Decimal(18,4),
  SALDOVALUTAKREDIT Decimal(18,4),
  ACCID Integer DEFAULT 0,
  PARTIDID Integer DEFAULT 0,
  ANALITICID Integer DEFAULT 0,
  CONSTRAINT PK_SALDO PRIMARY KEY (ID)
);
CREATE TABLE SUBACCOUNTS
(
  ID Integer NOT NULL,
  NAMEMAIN Varchar(200),
  NAMEMAINENG Varchar(200),
  NUM Integer NOT NULL,
  CONSTRAINT PK_SUBACCOUNTS PRIMARY KEY (ID)
);
CREATE TABLE "accounts"
(
  "Id" Integer NOT NULL,
  NAMEMAIN Varchar(200),
  NAMEMAINENG Varchar(200),
  NAMESUB Varchar(200),
  NAMESUBENG Varchar(200),
  "SubNum" Integer,
  "AnaliticalNum" Integer,
  "PartidNum" Integer,
  "TypeAccount" Integer,
  "LevelAccount" Integer,
  "TypeSaldo" Integer,
  "FirmaId" Integer,
  NUM Integer NOT NULL,
  SALDOVALUTA Decimal(18,4),
  SALDO Decimal(18,4),
  TYPEANALITICALKEY Integer,
  SALDODEBITVALUTA Decimal(18,4) NOT NULL,
  SALDODEBIT Decimal(18,4) NOT NULL,
  ISBUDJET Integer,
  SALDOKK Decimal(18,4),
  SALDODK Decimal(18,4),
  OBOROTL Decimal(18,4) NOT NULL,
  OBOROTDL Decimal(18,4) NOT NULL,
  OBOROTDV Decimal(18,4) NOT NULL,
  OBOROTKV Decimal(18,4) NOT NULL,
  OBOROTKK Decimal(18,4) NOT NULL,
  OBOROTDK Decimal(18,4) NOT NULL,
  MODIFY Char(1) NOT NULL,
  YY Integer NOT NULL,
  CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id")
);
CREATE TABLE "analiticalaccount"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "TypeID" Integer,
  CONSTRAINT "PK_analiticalaccount" PRIMARY KEY ("Id")
);
CREATE TABLE "analiticalaccounttype"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  SL Smallint DEFAULT 1,
  SV Smallint DEFAULT 1,
  KOL Smallint DEFAULT 1,
  CONSTRAINT "PK_analiticalaccounttype" PRIMARY KEY ("Id")
);
CREATE TABLE "analiticalfields"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "FieldType" Varchar(50),
  CONSTRAINT "PK_analiticalfields" PRIMARY KEY ("Id")
);
CREATE TABLE "cartotecacredit"
(
  "Id" Integer NOT NULL,
  "ContoId" Integer NOT NULL,
  "TitleValue" Varchar(50),
  "TypeValue" Varchar(20),
  "Value" Decimal(18,4),
  CONSTRAINT "PK_cartotecacredit" PRIMARY KEY ("Id")
);
CREATE TABLE "cartotecadebit"
(
  "Id" Integer NOT NULL,
  "ContoId" Integer NOT NULL,
  "TitleValue" Varchar(50),
  "TypeValue" Varchar(20),
  "Value" Decimal(18,4),
  CONSTRAINT "PK_cartotecadebit" PRIMARY KEY ("Id")
);
CREATE TABLE "cities"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "Zip" Integer,
  "CountryId" Integer,
  CONSTRAINT "PK_Cities" PRIMARY KEY ("Id")
);
CREATE TABLE "conectoranaliticfield"
(
  "Id" Integer NOT NULL,
  "AnaliticalNameID" Integer NOT NULL,
  "AnaliticalFieldId" Integer NOT NULL,
  REQUIRED Smallint NOT NULL,
  CONSTRAINT "PK_conectoranaliticfield" PRIMARY KEY ("Id")
);
CREATE TABLE "conto"
(
  "Id" Integer NOT NULL,
  "Oborot" Decimal(18,4),
  "Reason" Varchar(50),
  "Note" Varchar(50),
  "DataInvoise" Date,
  "NumberObject" Integer,
  "DebitAccount" Integer,
  "CreditAccount" Integer,
  "FirmId" Integer NOT NULL,
  "DocumentId" Integer,
  "CartotekaDebit" Integer,
  "CartotecaCredit" Integer,
  DOCNUM Varchar(20) NOT NULL,
  OBOROTVALUTA Decimal(18,4),
  OBOROTKOL Decimal(18,4),
  OBOROTVALUTAK Decimal(18,4) NOT NULL,
  OBOROTKOLK Decimal(18,4) NOT NULL,
  FOLDER Varchar(10),
  "Date" Date NOT NULL,
  CONSTRAINT "PK_conto" PRIMARY KEY ("Id")
);
CREATE TABLE "countries"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "Code" Varchar(10),
  CONSTRAINT "PK_countries" PRIMARY KEY ("Id")
);
CREATE TABLE "firm"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "Bulstad" Varchar(15) NOT NULL,
  "DDSnum" Varchar(15) NOT NULL,
  "City" Integer,
  "Country" Integer,
  "Address" Varchar(100),
  "Telefon" Varchar(20),
  "Presentor" Varchar(50),
  "NameBoss" Varchar(50),
  EGN Char(10),
  "PresentorYN" Smallint,
  "Names" Varchar(50),
  "Tel" Varchar(20),
  "FirstName" Varchar(50),
  "SurName" Varchar(50),
  "City2" Integer NOT NULL,
  "Address2" Varchar(100),
  "LastName" Varchar(50),
  REGISERDDS Smallint NOT NULL,
  CONSTRAINT "PK_firm" PRIMARY KEY ("Id")
);
CREATE TABLE "lookups"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "Tablename" Varchar(20),
  DESCRIPTION Varchar(50),
  NAMEENG Varchar(30),
  CONSTRAINT "PK_lookups" PRIMARY KEY ("Id")
);
CREATE TABLE "lookupsdetails"
(
  "IdLookUp" Integer NOT NULL,
  "IdLookField" Integer NOT NULL,
  SORTORDER Integer,
  ISUNIQUE Smallint NOT NULL,
  ISREQUARED Smallint NOT NULL,
  CONSTRAINT "PK_lookupsdetails" PRIMARY KEY ("IdLookUp","IdLookField")
);
CREATE TABLE "lookupsfield"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "DBField" Varchar(50),
  "IsNull" Smallint,
  NAMEENG Varchar(30) NOT NULL,
  "Length" Integer NOT NULL,
  RTABLENAME Varchar(20),
  RFIELDNAME Varchar(30),
  RFIELDKEY Varchar(30),
  RCODELOOKUP Integer,
  "GROUP" Integer NOT NULL,
  CONSTRAINT "PK_lookupsfield" PRIMARY KEY ("Id")
);
CREATE TABLE "na"
(
  "Id" Integer NOT NULL,
  "CodetId" Varchar(10),
  "Name" Varchar(100),
  AP Integer NOT NULL,
  CONSTRAINT "na" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_10"
(
  "Id" Integer NOT NULL,
  UNIT Varchar(4),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  "unit" Varchar(4),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_10" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_11"
(
  "Id" Integer NOT NULL,
  KONTRAGENT Integer,
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  BULSTAT Varchar(15),
  VAT Varchar(15),
  "Address" Varchar(50),
  MOL Varchar(30),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_11" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_12"
(
  "Id" Integer NOT NULL,
  KONTRAGENT Integer,
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  BULSTAT Varchar(15),
  VAT Varchar(15),
  "Address" Varchar(50),
  MOL Varchar(30),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_12" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_14"
(
  "Id" Integer NOT NULL,
  VIDVALUTA Varchar(3),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_14" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_15"
(
  "Id" Integer NOT NULL,
  "Document" Varchar(4),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_15" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_16"
(
  "Id" Integer NOT NULL,
  "number" Integer,
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_16" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_17"
(
  "Id" Integer NOT NULL,
  KONTRAGENT Integer,
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  BULSTAT Varchar(15),
  VAT Varchar(15),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_17" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_19"
(
  "Id" Integer NOT NULL,
  PARTIDA Varchar(14),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  "Address" Varchar(50),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_19" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_20"
(
  "Id" Integer NOT NULL,
  VIDVALUTA Varchar(3),
  DATA Date,
  KURS Decimal(15,7),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_20" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_21"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_21" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_22"
(
  "Id" Integer NOT NULL,
  NUMBER Varchar(10),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_22" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_23"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(40),
  NUMBER Integer NOT NULL,
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_23" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_7"
(
  "Id" Integer NOT NULL,
  SKLAD Integer,
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  MOL Varchar(30),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_7" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_8"
(
  "Id" Integer NOT NULL,
  "nomen number" Varchar(14),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_8" PRIMARY KEY ("Id")
);
CREATE TABLE "nom_9"
(
  "Id" Integer NOT NULL,
  "nomen number" Varchar(14),
  "Name" Varchar(40),
  NAMEENG Varchar(40),
  FIRMAID Integer NOT NULL,
  CONSTRAINT "PK_nom_9" PRIMARY KEY ("Id")
);
CREATE TABLE "nomenclatures"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "CodeId" Varchar(10),
  CONSTRAINT "PK_nomenclatures" PRIMARY KEY ("Id")
);
CREATE TABLE "sysfield"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "DBField" Varchar(50),
  "IsNull" Smallint,
  NAMEENG Varchar(30) NOT NULL,
  "Length" Integer NOT NULL,
  RTABLENAME Varchar(20),
  RFIELDNAME Varchar(30),
  RKEYNAME Varchar(30),
  RCODELOOKUP Integer,
  CONSTRAINT "PK_sysfield" PRIMARY KEY ("Id")
);
CREATE TABLE "syslookups"
(
  "Id" Integer NOT NULL,
  "Name" Varchar(50),
  "Tablename" Varchar(20),
  DESCRIPTION Varchar(50),
  NAMEENG Varchar(30),
  CONSTRAINT "PK_syslookups" PRIMARY KEY ("Id")
);
CREATE TABLE "syslookupsdetails"
(
  "IdLookUp" Integer NOT NULL,
  "IdLookField" Integer NOT NULL,
  SORTORDER Integer,
  CONSTRAINT "PK_syslookupsdetails" PRIMARY KEY ("IdLookUp","IdLookField")
);
/********************* VIEWS **********************/

CREATE VIEW DDSDNEVSALESREPORTA (BRANCH, BULSTAD, DATADOC, DOCN, NOM, NAMEKONTR, SUMA, DDSTOTAL, STOKE, CODEDOC, FID, NAMEDOC, SUMADDS, SUMAWITHDDS, NAME, DDSPERCENT, CODE, KINDACTIVITY)
AS       
select d.BRANCH,
       d.BULSTAD,
       d.DATADOC,
       d.DOCN,
       d.NOM,
       d.NAMEKONTR,
       d.SUMA,
       d.DDSSUMA as DDSTOTAL,
       d.STOKE,
       d.CODEDOC,
       c."FirmId" as FID,
       n."Name" as NAMEDOC,
       dt.SUMADDS,
       dt.SUMAWITHDDS,
       ds.NAME,
       ds.DDSPERCENT,
       ds.CODE,
       d.KINDACTIVITY 
from  DDSDNEVFIELDS ds
inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID
inner join DDSDNEV d on d.ID=dt.IDDDSDNEV
inner join "conto" c on c."Id"=d.NOM
inner join "nomenclatures" n on n."Id"=d.KINDDOC
order by d.NOM,ds.ID;
CREATE VIEW DDSDNEVSALESREPORTB (BRANCH, BULSTAD, DATADOC, DOCN, NOM, NAMEKONTR, SUMA, DDSTOTAL, STOKE, FID, NAMEDOC, SUMADDS, SUMAWITHDDS, NAME, DDSPERCENT, CODE, KINDACTIVITY)
AS       
select d.BRANCH,
       d.BULSTAD,
       d.DATADOC,
       d.DOCN,
       d.NOM,
       d.NAMEKONTR,
       d.SUMA,
       d.DDSSUMA as DDSTOTAL,
       d.STOKE,
       c."FirmId" as FID,
       n."Name" as NAMEDOC,
       dt.SUMADDS,
       dt.SUMAWITHDDS,
       ds.NAME,
       ds.DDSPERCENT,
       ds.CODE,
       d.KINDACTIVITY 
from  DDSDNEVSELLSFIELDS ds
inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID
inner join DDSDNEV d on d.ID=dt.IDDDSDNEV
inner join "conto" c on c."Id"=d.NOM
inner join "nomenclatures" n on n."Id"=d.KINDDOC
order by d.NOM,ds.ID;
CREATE VIEW SELECTMAPAFIELDTOLOOKUP (ACCOUNTS_ID, ANALITIC_FIELD_ID, ANALITIC_ID, FIELDLOOKUP_ID, LOOKUP_ID, NAMEFIELDLOOKUP, NAMELOOKUP)
AS 
SELECT a.ACCOUNTS_ID,a.ANALITIC_FIELD_ID,a.ANALITIC_ID,a.FIELDLOOKUP_ID,a.LOOKUP_ID,b."Name" as NAMEFIELDLOOKUP,c."Name" as NAMELOOKUP
FROM MAPACCTOLOOKUP a 
inner join "lookupsfield" b on a.FIELDLOOKUP_ID=b."Id"
inner join "lookups" c on c."Id"=a.LOOKUP_ID order by a.LOOKUP_ID

;
CREATE VIEW SELECTMAPAFIELDTOLOOKUPTNFN (ACCOUNTS_ID, ANALITIC_FIELD_ID, ANALITIC_ID, FIELDLOOKUP_ID, LOOKUP_ID, NAMEFIELDLOOKUP, NAMELOOKUP, TN, FN)
AS  
SELECT a.ACCOUNTS_ID,a.ANALITIC_FIELD_ID,a.ANALITIC_ID,a.FIELDLOOKUP_ID,a.LOOKUP_ID,b."Name" as NAMEFIELDLOOKUP,c."Name" as NAMELOOKUP,c."Tablename" as TN,b.NAMEENG as FN
FROM MAPACCTOLOOKUP a 
inner join "lookupsfield" b on a.FIELDLOOKUP_ID=b."Id"
inner join "lookups" c on c."Id"=a.LOOKUP_ID order by a.LOOKUP_ID

;
CREATE VIEW SHOWLOOKUPS ("Id", "Name", "Tablename", "FieldName", "DBField", LFID, SORTORDER)
AS  
select l."Id",l."Name",l."Tablename",lf."Name" as "FieldName",lf."DBField",lf."Id" as LFID,ld.SORTORDER from "lookups" l
inner join "lookupsdetails" ld on ld."IdLookUp"=l."Id"
inner join "lookupsfield" lf on lf."Id"=ld."IdLookField"
order by l."Tablename",ld.SORTORDER 
;
CREATE VIEW SYSLOOKUPS ("Id", "Name", "Tablename", FN, "DBField", "Length", RTABLENAME)
AS   
select sl."Id",sl."Name",sl."Tablename",sf."Name" as FN,sf."DBField",sf."Length",sf.RTABLENAME from "syslookups" sl 
inner join "syslookupsdetails" sd on  sl."Id"=sd."IdLookUp"
inner join "sysfield" sf on sd."IdLookField"=sf."Id" 
order by sd.SORTORDER
;
/******************* EXCEPTIONS *******************/

/******************** TRIGGERS ********************/

SET TERM ^ ;
CREATE TRIGGER ANALITICALACOUNTNEWID FOR analiticalaccount ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new."Id"=gen_id(newanaliticalacc,1);
	post_event 'acc_insert'||new."Id";
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER CONTOMOVEMENTNEW FOR CONTOMOVEMENT ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.ID=gen_id("CONTOMOVEMENTGEN",1); 
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER DDSDNEVFIELDAUTOINCTRIGER FOR DDSDNEVFIELDS ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.ID=gen_id("DDSDNEVFIELDSAUTOINC",1);
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER DDSNEWAUTOINC FOR DDSDNEV ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.ID=gen_id("GENDDSNEW",1); 
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER GRNOMENCLATURES FOR nomenclatures ACTIVE
BEFORE INSERT POSITION 0
AS 
DECLARE variable maxel integer;
BEGIN 
    select max("Id") from "nomenclatures" into :maxel;
    new."Id"=maxel+1;
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER MODECHANGE FOR accounts ACTIVE
BEFORE UPDATE POSITION 0
AS 
BEGIN 
    if (new.MODIFY<>'G') then
    begin
	   new.MODIFY='U';
	   post_event 'acc_update';
	end
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER MOVEMENT_BU0 FOR MOVEMENT ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.DATA=CURRENT_TIMESTAMP; 
	new.ID=gen_id("MOVEMENTID", 1);
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER NAI FOR na ACTIVE
BEFORE INSERT POSITION 0
AS 
DECLARE variable maxel integer;
BEGIN 
    select max("Id") from "na" into :maxel;
    new."Id"=maxel+1;
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER NEWKEY FOR conto ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new."Id"=gen_id("CONTOGEN",1);
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER SALDOS_BI0 FOR SALDOS ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.ID=gen_id(SALDOSID,1);
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER SETDATE FOR MOVEMENT ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.DATA=CURRENT_TIMESTAMP;
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER TRCITIES FOR cities ACTIVE
BEFORE INSERT POSITION 0
AS 
DECLARE variable maxel integer;
BEGIN 
    select max("Id") from "cities" into :maxel;
    new."Id"=maxel+1;
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER TRCOUNTRIES FOR countries ACTIVE
BEFORE INSERT POSITION 0
AS 
DECLARE variable maxel integer;
BEGIN 
    select max("Id") from "countries" into :maxel;
    new."Id"=maxel+1;
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER TRIGERLOOKUPSFIELD FOR lookupsfield ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
    new."Id"= gen_id(generatorlookupsfield, 1); 
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "accounts_BI0" FOR accounts ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	new.MODIFY='I';
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "analiticalaccounttype_BI0" FOR analiticalaccounttype ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
	 new."Id" = gen_id(genanaliticalaccounttype, 1);
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "conectoranaliticfield_BI0" FOR conectoranaliticfield ACTIVE
BEFORE INSERT POSITION 0
AS 
BEGIN 
		new."Id"=gen_id("GEN_conectoranaliticfield_ID",1);
END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "syslookupstr" FOR syslookups ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(SYSLOOKUP, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_10" FOR nom_10 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_10, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_11" FOR nom_11 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_11, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_12" FOR nom_12 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_12, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_14" FOR nom_14 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_14, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_15" FOR nom_15 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_15, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_16" FOR nom_16 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_16, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_17" FOR nom_17 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_17, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_19" FOR nom_19 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_19, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_20" FOR nom_20 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_20, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_21" FOR nom_21 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_21, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_22" FOR nom_22 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_22, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_23" FOR nom_23 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_23, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_7" FOR nom_7 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_7, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_8" FOR nom_8 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_8, 1);END^
SET TERM ; ^
SET TERM ^ ;
CREATE TRIGGER "trigernom_9" FOR nom_9 ACTIVE
BEFORE INSERT POSITION 0
AS BEGIN  new."Id" = gen_id(generatornom_9, 1);END^
SET TERM ; ^

SET TERM ^ ;
ALTER PROCEDURE ADDCONTO (
    "Date" Date,
    "Oborot" Decimal(18,4),
    "Reason" Varchar(50),
    "Note" Varchar(50),
    "DataInvoise" Date,
    "NumberObject" Integer,
    "DebitAccount" Integer,
    "CreditAccount" Integer,
    "FirmId" Integer,
    "DocumentId" Integer,
    "CartotekaDebit" Integer,
    "CartotecaCredit" Integer,
    DOCNUM Varchar(20),
    "OborotValuta" Decimal(18,4),
    "OborotKol" Decimal(18,4),
    "OborotValutaK" Decimal(18,4),
    "OborotKolK" Decimal(18,4),
    FOLDER Varchar(10) )
RETURNS (
    NEWID Integer )
AS
DECLARE VARIABLE oborotld Decimal(18,4);
DECLARE VARIABLE oborotvd Decimal(18,4);  
DECLARE VARIABLE oborotkd Decimal(18,4);

DECLARE VARIABLE oborotlk Decimal(18,4);  
DECLARE VARIABLE oborotvk Decimal(18,4);
DECLARE VARIABLE oborotkk Decimal(18,4); 
BEGIN
  INSERT INTO "conto" ("Date","Oborot", "Reason", "Note", "DataInvoise", "NumberObject", "DebitAccount", "CreditAccount", "FirmId", "DocumentId", "CartotekaDebit", "CartotecaCredit",DOCNUM,OBOROTVALUTA,OBOROTKOL,OBOROTVALUTAK,OBOROTKOLK,FOLDER)
 VALUES (
:"Date", 
:"Oborot", 
:"Reason", 
:"Note", 
:"DataInvoise", 
:"NumberObject", 
:"DebitAccount", 
:"CreditAccount", 
:"FirmId", 
:"DocumentId", 
:"CartotekaDebit", 
:"CartotecaCredit",
:DOCNUM,
:"OborotValuta",
:"OborotKol",
:"OborotValutaK",
:"OborotKolK",
:FOLDER
);
select sum(c."Oborot"),sum(c.OBOROTKOLK),sum(c.OBOROTVALUTAK) from "conto" c where c."CreditAccount"=:"CreditAccount" into :oborotlk,:oborotvk,:oborotkk;
select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c where c."DebitAccount"=:"DebitAccount" into :oborotld,:oborotvd,:oborotkd;
if (:oborotld IS NULL) THEN oborotld=0;
if (:oborotlk IS NULL) THEN oborotlk=0;
if (:oborotvd IS NULL) THEN oborotvd=0;
if (:oborotvk IS NULL) THEN oborotvk=0;
if (:oborotkd IS NULL) THEN oborotkd=0;
if (:oborotkk IS NULL) THEN oborotkk=0; 
Update "accounts" a set a.OBOROTDK=:oborotkd,a.OBOROTDL=:oborotld,a.OBOROTDV=:oborotvd where a."Id"=:"DebitAccount";
Update "accounts" a set a.OBOROTKK=:oborotkk,a.OBOROTL=:oborotlk,a.OBOROTKV=:oborotvk where a."Id"=:"CreditAccount";
NEWID=gen_id("CONTOGEN",0);
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE ADDDDSDNEV (
    NOM Bigint,
    BRANCH Varchar(50),
    DOCN Varchar(20),
    DATADOC Date,
    KINDACTIVITY Integer,
    KINDDOC Integer,
    STOKE Varchar(50),
    BULSTAD Varchar(20),
    NZDDS Varchar(20),
    LOOKUPID Integer,
    LOOKUPELEMENTID Integer,
    NAMEKONTR Varchar(50),
    SUMA Numeric(18,4),
    DDSSUMA Numeric(18,4),
    CODEDOC Varchar(2) )
RETURNS (
    NEWID Integer )
AS
declare variable
  testnum integer;
begin
SELECT id from DDSDNEV dn where dn.NOM=:NOM into :testnum;
if (:testnum is null) then
begin 
INSERT INTO DDSDNEV (
 NOM,
 BRANCH,
 DOCN,
 DATADOC,
 KINDACTIVITY,
 KINDDOC,
 STOKE,
 BULSTAD,
 NZDDS,
 LOOKUPID,
 LOOKUPELEMENTID,
 NAMEKONTR,
 SUMA,
 DDSSUMA,
 CODEDOC 
        )
 VALUES (
:NOM, 
:BRANCH, 
:DOCN, 
:DATADOC, 
:KINDACTIVITY, 
:KINDDOC, 
:STOKE, 
:BULSTAD, 
:NZDDS,
:LOOKUPID,
:LOOKUPELEMENTID,
:NAMEKONTR,
:SUMA,
:DDSSUMA,
:CODEDOC
);
NEWID=gen_id("GENDDSNEW",0);
end
else
begin
   Update DDSDNEV d
   Set 
     d.BRANCH=:BRANCH,
     d.DOCN=:DOCN,
     d.DATADOC=:DATADOC,
     d.BULSTAD=:BULSTAD,
     d.NZDDS=:NZDDS,
     d.KINDACTIVITY=:KINDACTIVITY,
     d.KINDDOC=:KINDDOC,
     d.LOOKUPID=:LOOKUPID,
     d.LOOKUPELEMENTID=:LOOKUPELEMENTID,
     d.NAMEKONTR=:NAMEKONTR,
     d.STOKE=:STOKE,
     d.SUMA=:SUMA,
     d.CODEDOC=:CODEDOC
  where 
     d.NOM=:NOM;
NEWID=testnum;        
end
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE ADDMOVENT (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer,
    VALKOLK Decimal(18,4),
    VALKOLD Decimal(18,4),
    VALVALD Decimal(18,4),
    VALVALK Decimal(18,4) )
AS
declare VARIABLE newgroup integer;
DECLARE VARIABLE saldok decimal(18,4);
DECLARE VARIABLE saldod decimal(18,4);
declare variable parentid integer; 
declare variable firmid integer; 
BEGIN
select max(m."group") from MOVEMENT m where m.ACCID=:ACCID and m.ACCFIELDKEY=:ACCFIELDKEY into :newgroup;
 if (:newgroup IS NULL) THEN newgroup=0;
 newgroup=newgroup+1;
INSERT INTO MOVEMENT (
ACCID, 
ACCFIELDKEY,
LOOKUPFIELDKEY,
"VALUE",
VALUEDATE,
VALUEMONEY,
VALUENUM,
TYPEACCKEY,
VALUED,
"group",
LOOKUPID,
VALKOLK,
VALKOLD,
VALVALD,
VALVALK
)
 VALUES (
:ACCID, 
:ACCFIELDKEY, 
:LOOKUPFIELDKEY, 
:VAL, 
:VALUEDATE, 
:VALUEMONEY, 
:VALUENUM, 
:TYPEACCKEY,
:VALUED,
:newgroup,
:LOOKUPID,
:VALKOLK,
:VALKOLD,
:VALVALD,
:VALVALK
); 
SELECT sum(a.VALUEMONEY) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
SELECT sum(a.VALUED) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
Update "accounts" a SET a.SALDO=:saldok,a.SALDODEBIT=:saldod where a."Id"=:ACCID;
SELECT sum(a.VALKOLD) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
SELECT sum(a.VALKOLK) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
Update "accounts" a SET a.SALDOKK=:saldok,a.SALDODK=:saldod where a."Id"=:ACCID;
SELECT sum(a.VALVALK) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
SELECT sum(a.VALVALD) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
Update "accounts" a SET a.SALDOVALUTA=:saldok,a.SALDODEBITVALUTA=:saldod where a."Id"=:ACCID;

END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE ADDMOVENTCONTO (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer,
    CONTOID Integer,
    "TYPE" Smallint )
AS
BEGIN
INSERT INTO CONTOMOVEMENT (ACCID, ACCFIELDKEY, LOOKUPFIELDKEY,"VALUE", VALUEDATE, VALUEMONEY, VALUENUM, TYPEACCKEY,VALUED,CONTOID,LOOKUPID,"TYPE") 
 VALUES (
:ACCID, 
:ACCFIELDKEY, 
:LOOKUPFIELDKEY, 
:VAL, 
:VALUEDATE, 
:VALUEMONEY, 
:VALUENUM, 
:TYPEACCKEY,
:VALUED,
:CONTOID,
:LOOKUPID,
:"TYPE"
); 
     
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE CHECKLOOKUP (
    VAL Varchar(50),
    LOOKUPID Integer )
RETURNS (
    ISIN Integer )
AS
BEGIN
select 
count(m.ID) 
from MOVEMENT m 
where m.LOOKUPID=:LOOKUPID and 
      m."VALUE"=:VAL 
into :ISIN;
 
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE CHECKMOVENT (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer )
RETURNS (
    ISIN Integer )
AS
BEGIN
select 
count(m.ID) 
from MOVEMENT m 
where m.ACCID=:ACCID and 
      m.ACCFIELDKEY=:ACCFIELDKEY and 
      m.LOOKUPFIELDKEY=:LOOKUPFIELDKEY and 
      m.LOOKUPFIELDKEY>0
into :ISIN;
 
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE COPYACCSYTOY (
    FY Integer,
    TY Integer,
    FID Integer )
AS
declare variable NAMEMAIN Varchar(200);
declare variable NAMEMAINENG Varchar(200);
declare variable NAMESUB Varchar(200);
declare variable     NAMESUBENG Varchar(200);
declare variable     "SubNum" Integer;
declare variable     "AnaliticalNum" Integer;
declare variable     "PartidNum" Integer;
declare variable     "TypeAccount" Integer;
declare variable     "LevelAccount" Integer;
declare variable     "TypeSaldo" Integer;
declare variable     "FirmaId" Integer;
declare variable     "IsNew" Integer;
declare variable     "Id" Integer;
declare variable     NUM Integer;
declare variable     SALDO Decimal(18,4);
declare variable     SALDOVALUTA Decimal(18,4);
declare variable     TYPEANALITICALKEY Integer;
declare variable     SALDODEBIT Decimal(18,4);
declare variable     SALDODEBITVALUTA Decimal(18,4);
declare variable     ISBUDJET Integer;
declare variable     SALDOKK Decimal(18,4);
declare variable     SALDODK Decimal(18,4);
declare variable     YY Integer;
declare variable     NAME Varchar(200);
declare variable     NSD Decimal(18,4);
declare variable     OBD Decimal(18,4);
declare variable     KSD Decimal(18,4);
declare variable     NSK Decimal(18,4);
declare variable     OBK Decimal(18,4);
declare variable     KSK Decimal(18,4);
declare variable     NEWID Integer;
declare variable     MODIFY char(1);
declare variable     "DateFrom" Date;
declare variable     "DateTo" Date;  
BEGIN
 "DateFrom"  = CAST('1.1.' ||:FY AS DATE);
 "DateTo"  = CAST('31.12.' ||:FY AS DATE);
 FOR SELECT 
 a."AnaliticalNum",
            a."FirmaId",
            a."Id",
            a."LevelAccount",
            a."PartidNum",
            a."SubNum",
            a."TypeAccount",
            a."TypeSaldo",
            a.ISBUDJET,
            a.MODIFY,
            a.NAMEMAIN,
            a.NAMEMAINENG,
            a.NAMESUB,
            a.NAMESUBENG,
            a.NUM,
            a.TYPEANALITICALKEY,
            a.YY 
 FROM "accounts" a 
 WHERE a."FirmaId"=:FID 
 order by a.NUM,a."SubNum"
  INTO 
            :"AnaliticalNum",
            :"FirmaId",
            :"Id",
            :"LevelAccount",
            :"PartidNum",
            :"SubNum",
            :"TypeAccount",
            :"TypeSaldo",
            :ISBUDJET,
            :MODIFY,
            :NAMEMAIN,
            :NAMEMAINENG,
            :NAMESUB,
            :NAMESUBENG,
            :NUM,
            :TYPEANALITICALKEY,
            :YY  DO 
  BEGIN 
    EXECUTE PROCEDURE GETOBOROTKA :"DateFrom",:"DateTo",:FID,:"Id"
    RETURNING_VALUES :NSD, :OBD, :KSD, :NSK, :OBK, :KSK; 
    EXECUTE PROCEDURE IP_ACCOUNTS 
    
    :NAMEMAIN,
    :NAMEMAINENG,
    :NAMESUB,
    :NAMESUBENG,
    :"SubNum" ,
    :"AnaliticalNum" ,
    :"PartidNum" ,
    :"TypeAccount" ,
    :"LevelAccount" ,
    :"TypeSaldo" ,
    :"FirmaId" ,
    :"IsNew" ,
    :"Id" ,
    :NUM ,
    :KSD ,
    0 ,
    :TYPEANALITICALKEY ,
    :KSK ,
    :SALDODEBITVALUTA ,
    :ISBUDJET ,
    0 ,
    0 ,
    :TY 
    RETURNING_VALUES :NEWID;
    SUSPEND; 
  END 
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE DELETEACCOUNT (
    ID Integer )
AS
BEGIN
  DELETE FROM "accounts" a WHERE (A."Id" =:ID); 
  DELETE FROM MAPACCTOLOOKUP a where (a.ACCOUNTS_ID=:ID);
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE DELETECONTO (
    CONTOID Integer )
AS
DECLARE VARIABLE oborotld Decimal(18,4);
DECLARE VARIABLE oborotvd Decimal(18,4);  
DECLARE VARIABLE oborotkd Decimal(18,4);

DECLARE VARIABLE oborotlk Decimal(18,4);  
DECLARE VARIABLE oborotvk Decimal(18,4);
DECLARE VARIABLE oborotkk Decimal(18,4); 

DECLARE VARIABLE acck Integer; 
DECLARE VARIABLE accd Integer;
DECLARE variable dnevId Integer; 
BEGIN
  select c."CreditAccount",c."DebitAccount" from "conto" c where c."Id"=:CONTOID into :acck,accd;
  delete from CONTOMOVEMENT cm where cm.CONTOID=:CONTOID;
  delete from "conto" c where c."Id"=:CONTOID;
  select d.ID from DDSDNEV d where d.NOM=:CONTOID into :dnevId; 
  delete from DDSDNEV d where d.NOM=:CONTOID;
  if (:oborotld IS NULL) THEN
  begin
    delete from DDSDNEVTOFIELDS d where d.IDDDSDNEV=:dnevId;
  end
  select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c where c."CreditAccount"=:acck into :oborotlk,:oborotvk,:oborotkk;
select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c where c."DebitAccount"=:accd into :oborotld,:oborotvd,:oborotkd;
if (:oborotld IS NULL) THEN oborotld=0;
if (:oborotlk IS NULL) THEN oborotlk=0;
if (:oborotvd IS NULL) THEN oborotvd=0;
if (:oborotvk IS NULL) THEN oborotvk=0;
if (:oborotkd IS NULL) THEN oborotkd=0;
if (:oborotkk IS NULL) THEN oborotkk=0; 
Update "accounts" a set a.OBOROTDK=:oborotkd,a.OBOROTDL=:oborotld,a.OBOROTDV=:oborotvd where a."Id"=:accd;
Update "accounts" a set a.OBOROTKK=:oborotkk,a.OBOROTL=:oborotlk,a.OBOROTKV=:oborotvk where a."Id"=:acck;
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE DELETEDNEV (
    NOM Integer )
AS
declare variable
  testnum integer;
BEGIN
SELECT id from DDSDNEV dn where dn.NOM=:NOM into :testnum;
if (testnum <> null) then
begin
  delete from DDSDNEV cm where cm.NOM=:NOM;
  delete from DDSDNEVTOFIELDS d where d.IDDDSDNEV=:testnum;  
end  
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE DELETEFIRMA (
    ID Integer )
RETURNS (
    CONFIRMDELETE Integer )
AS
declare variable testid integer;
BEGIN
  CONFIRMDELETE=0;
  Select count("Id") from "accounts" a where (a."FirmaId"=:ID) into :testid;
  if  (:testid=0) then
  begin
    delete from "firm" f where (f."Id"=:ID);
    CONFIRMDELETE=1;
  end
  suspend;
  /* write your code here */ 
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE ENDSALDO (
    ACCID Integer )
RETURNS (
    ENDSALDO Decimal(18,4),
    ENDSALDOV Decimal(18,4),
    ENDSALDOK Decimal(18,4) )
AS
DECLARE VARIABLE saldold Decimal(18,4);  
DECLARE VARIABLE saldolk Decimal(18,4);

DECLARE VARIABLE saldovd Decimal(18,4);  
DECLARE VARIABLE saldovk Decimal(18,4);

DECLARE VARIABLE saldokd Decimal(18,4);  
DECLARE VARIABLE saldokk Decimal(18,4);

DECLARE VARIABLE oborotld Decimal(18,4);
DECLARE VARIABLE oborotvd Decimal(18,4);  
DECLARE VARIABLE oborotkd Decimal(18,4);

DECLARE VARIABLE oborotlk Decimal(18,4);  
DECLARE VARIABLE oborotvk Decimal(18,4);
DECLARE VARIABLE oborotkk Decimal(18,4);  

DECLARE VARIABLE typeacc integer;
BEGIN
  select sum(a.SALDODEBIT),sum(a.SALDO),sum(a.SALDODEBITVALUTA),sum(a.SALDOVALUTA),sum(a.SALDODK),sum(a.SALDOKK) from "accounts" a where a."Id"=:ACCID 
  into :saldold,:saldolk,:saldovd,:saldovk,:saldokd,:saldokk;
  select a."LevelAccount" from "accounts" a where a."Id"=:ACCID into :typeacc;
  select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c where c."CreditAccount"=:ACCID into :oborotlk,:oborotvk,:oborotkk;
  select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c where c."DebitAccount"=:ACCID into :oborotld,:oborotvd,:oborotkd;
  if (:saldold IS NULL) THEN saldold=0;
  if (:saldolk IS NULL) THEN saldolk=0;
  if (:saldovd IS NULL) THEN saldovd=0;
  if (:saldovk IS NULL) THEN saldovk=0;
  if (:saldokd IS NULL) THEN saldokd=0;
  if (:saldokk IS NULL) THEN saldokk=0;
  if (:oborotld IS NULL) THEN oborotld=0;
  if (:oborotlk IS NULL) THEN oborotlk=0;
  if (:oborotvd IS NULL) THEN oborotvd=0;
  if (:oborotvk IS NULL) THEN oborotvk=0;
  if (:oborotkd IS NULL) THEN oborotkd=0;
  if (:oborotkk IS NULL) THEN oborotkk=0; 
  if (typeacc=1)THEN
  begin
     ENDSALDO=(saldolk+oborotlk)-(saldold+oborotld);
     ENDSALDOV=(saldovk+oborotvk)-(saldovd+oborotvd);
     ENDSALDOK=(saldokk+oborotkk)-(saldokd+oborotkd);
  end
  else
  begin
     ENDSALDO=(saldold+oborotld)-(saldolk+oborotlk);
     ENDSALDOV=(saldovd+oborotvd)-(saldovk+oborotvk);
     ENDSALDOK=(saldokd+oborotkd)-(saldokk+oborotkk);
  end
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE GETALLOBOROTKA (
    "DateFrom" Date,
    "DateTo" Date,
    "FirmId" Integer )
RETURNS (
    NUM Varchar(20),
    NAME Varchar(200),
    NSD Decimal(18,4),
    OBD Decimal(18,4),
    KSD Decimal(18,4),
    NSK Decimal(18,4),
    OBK Decimal(18,4),
    KSK Decimal(18,4) )
AS
DECLARE VARIABLE acc_id integer;
DECLARE VARIABLE lnum integer;
DECLARE VARIABLE lsubnum integer;

BEGIN
 FOR SELECT a.NUM,a."SubNum",a.NAMEMAIN,a."Id" 
 FROM "accounts" a 
 WHERE a."FirmaId"=:"FirmId" 
 order by a.NUM,a."SubNum"
  INTO :lnum,:lsubnum,:NAME,:acc_id DO 
  BEGIN 
    EXECUTE PROCEDURE GETOBOROTKA :"DateFrom",:"DateTo",:"FirmId",:acc_id 
    RETURNING_VALUES :NSD, :OBD, :KSD, :NSK, :OBK, :KSK; 
    IF (lsubnum=0) then
    begin
        NUM=:lnum;
    end
    else
    begin
        NUM=:lnum||'\'||:lsubnum;
    end
    SUSPEND; 
  END 
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE GETOBOROTKA (
    "DateFrom" Date,
    "DateTo" Date,
    "FirmId" Integer,
    "AccId" Integer )
RETURNS (
    NSD Decimal(18,4),
    OBD Decimal(18,4),
    KSD Decimal(18,4),
    NSK Decimal(18,4),
    OBK Decimal(18,4),
    KSK Decimal(18,4) )
AS
DECLARE VARIABLE oborotld Decimal(18,4);
DECLARE VARIABLE oborotvd Decimal(18,4);  
DECLARE VARIABLE oborotkd Decimal(18,4);

DECLARE VARIABLE oborotlk Decimal(18,4);  
DECLARE VARIABLE oborotvk Decimal(18,4);
DECLARE VARIABLE oborotkk Decimal(18,4); 

DECLARE VARIABLE nsdl Decimal(18,4);
DECLARE VARIABLE nsdll Decimal(18,4);
DECLARE VARIABLE nsdv Decimal(18,4);  
DECLARE VARIABLE nsdk Decimal(18,4);

DECLARE VARIABLE nskl Decimal(18,4);
DECLARE VARIABLE nskll Decimal(18,4);  
DECLARE VARIABLE nskv Decimal(18,4);
DECLARE VARIABLE nskk Decimal(18,4);

DECLARE VARIABLE typeacc integer;

BEGIN
select a.SALDO,a.SALDODEBIT,a."TypeAccount" from "accounts" a where a."Id"=:"AccId" into :nskll,:nsdll,:typeacc; 
select sum(c."Oborot"),sum(c.OBOROTKOLK),sum(c.OBOROTVALUTAK) from "conto" c 
       where c."CreditAccount"=:"AccId" and c."Date">=:"DateFrom" and c."Date"<=:"DateTo" 
       into :oborotlk,:oborotvk,:oborotkk;
select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c 
       where c."DebitAccount"=:"AccId" and c."Date">=:"DateFrom" and c."Date"<=:"DateTo"
       into :oborotld,:oborotvd,:oborotkd;
select sum(c."Oborot"),sum(c.OBOROTKOLK),sum(c.OBOROTVALUTAK) from "conto" c 
       where c."CreditAccount"=:"AccId" and c."Date"<:"DateFrom"  
       into :nskl,:nskv,:nskk;
select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c 
       where c."DebitAccount"=:"AccId" and c."Date"<:"DateFrom" 
       into :nsdl,:nsdv,:nsdk;       
if (:oborotld IS NULL) THEN oborotld=0;
if (:oborotlk IS NULL) THEN oborotlk=0;
if (:oborotvd IS NULL) THEN oborotvd=0;
if (:oborotvk IS NULL) THEN oborotvk=0;
if (:oborotkd IS NULL) THEN oborotkd=0;
if (:oborotkk IS NULL) THEN oborotkk=0; 
if (:nsdl IS NULL) THEN nsdl=0;
if (:nsdv IS NULL) THEN nsdv=0;
if (:nsdk IS NULL) THEN nsdk=0;
if (:nskl IS NULL) THEN nskl=0;
if (:nskv IS NULL) THEN nskv=0;
if (:nskk IS NULL) THEN nskk=0; 
if (:nskll IS NULL) THEN nskll=0;
if (:nsdll IS NULL) THEN nsdll=0; 
if (:typeacc IS NULL) THEN typeacc=1; 
if (typeacc=1) then
begin
NSD=nsdl+nsdll-nskl-nskll;
NSK=0;
if (NSD<0)then
begin
   NSK=NSD*(-1);
   NSD=0;
end
OBD=oborotld;
OBK=oborotlk;
KSD=nsdl+nsdll+oborotld-oborotlk-nskll-nskl;
KSK=0;
if (KSD<0)then
begin
   KSK=KSD*(-1);
   KSD=0;
end
end
else
begin
NSK=nskl+nskll-nsdll-nsdl;
NSD=0;
if (NSK<0)then
begin
   NSD=NSK*(-1);
   NSK=0;
end
OBD=oborotld;
OBK=oborotlk;
KSK=nskl+nskll+oborotlk-nsdl-nsdll-oborotld;
KSD=0;
if (KSK<0)then
begin
   KSD=KSK*(-1);
   KSK=0;
end
end
suspend;
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE IP_ACCOUNTS (
    "NameMain" Varchar(200),
    "NameMainEng" Varchar(200),
    "NameSub" Varchar(200),
    "NameSubEng" Varchar(200),
    "SubNum" Integer,
    "AnaliticalNum" Integer,
    "PartidNum" Integer,
    "TypeAccount" Integer,
    "LevelAccount" Integer,
    "TypeSaldo" Integer,
    "FirmaId" Integer,
    "IsNew" Integer,
    "Id" Integer,
    NUM Integer,
    SALDO Decimal(18,4),
    SALDOVALUTA Decimal(18,4),
    TYPEANALITICALKEY Integer,
    SALDODEBIT Decimal(18,4),
    SALDODEBITVALUTA Decimal(18,4),
    ISBUDJET Integer,
    SALDOKK Decimal(18,4),
    SALDODK Decimal(18,4),
    YY Integer )
RETURNS (
    "NewId" Integer )
AS
DECLARE VARIABLE 
 newid integer; 
BEGIN
if (:"IsNew"=1) then
begin 
   select a."Id" from "accounts" a where a."FirmaId"=:"FirmaId" and a."SubNum"=:"SubNum" and a.NUM=:NUM and a.YY=:YY
   into :newid;
   if (:newid is null) then
   begin
   SELECT max(a."Id") FROM "accounts" a INTO :newid; 
    if (:newid IS NULL) THEN newid=0;
   newid=newid+1;
   insert into "accounts"
(
  "Id"
, "NAMEMAIN"
, "NAMEMAINENG"
, "NAMESUB"
, "NAMESUBENG"
, "SubNum"
, "AnaliticalNum"
, "PartidNum"
, "TypeAccount"
, "LevelAccount"
, "TypeSaldo"
, "FirmaId"
, "NUM"
, "SALDO"
, "SALDOVALUTA"
, "TYPEANALITICALKEY"
, SALDODEBIT
, SALDODEBITVALUTA
, ISBUDJET
, SALDOKK
, SALDODK
, OBOROTL
, OBOROTDL
, OBOROTDV
, OBOROTKV
, OBOROTKK
, OBOROTDK
, YY
)
values
(
  :newid
, :"NameMain"
, :"NameMainEng"
, :"NameSub"
, :"NameSubEng"
, :"SubNum"
, :"AnaliticalNum"
, :"PartidNum"
, :"TypeAccount"
, :"LevelAccount"
, :"TypeSaldo"
, :"FirmaId"
, :"NUM"
, :SALDO
, :SALDOVALUTA
, :TYPEANALITICALKEY
, :SALDODEBIT
, :SALDODEBITVALUTA
, :ISBUDJET
, :SALDOKK
, :SALDODK,0,0,0,0,0,0,:YY
);
"NewId"=newid;
post_event 'acc_insert';
end
else
  begin
  "NewId"=-1; 
  end
end
else
begin
UPDATE "accounts" SET 
"NAMEMAIN" = :"NameMain", 
"NAMEMAINENG" = :"NameMainEng", 
"NAMESUB" = :"NameSub", 
"NAMESUBENG" = :"NameSubEng", 
"SubNum" =:"SubNum" , 
"AnaliticalNum" =:"AnaliticalNum", 
"PartidNum" = :"PartidNum", 
"TypeAccount" =:"TypeAccount", 
"LevelAccount" = "LevelAccount", 
"TypeSaldo" = "TypeSaldo", 
"FirmaId" = "FirmaId",
NUM = :NUM,
"SALDO"= :SALDO,
"SALDOVALUTA"=:SALDOVALUTA,
"TYPEANALITICALKEY"=:TYPEANALITICALKEY,
SALDODEBIT=:SALDODEBIT,
SALDODEBITVALUTA=:SALDODEBITVALUTA,
ISBUDJET=:ISBUDJET,
SALDOKK=:SALDOKK,
SALDODK=:SALDODK,
YY=:YY  
WHERE "Id" = :"Id";
"NewId"=:"Id";
end
suspend;

END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE IP_FIRM (
    "Name" Varchar(50),
    "Bulstad" Varchar(15),
    "DDSnum" Varchar(15),
    "City" Integer,
    "Country" Integer,
    "Address" Varchar(100),
    "Telefon" Varchar(20),
    "Presentor" Varchar(50),
    "NameBoss" Varchar(50),
    EGN Char(10),
    "PresentorYN" Smallint,
    "Names" Varchar(50),
    "Tel" Varchar(20),
    "FirstName" Varchar(50),
    "SurName" Varchar(50),
    "City2" Integer,
    "Address2" Varchar(100),
    "LastName" Varchar(50),
    REGISERDDS Smallint )
RETURNS (
    "Id" Integer )
AS
DECLARE VARIABLE 
 newid integer; 
BEGIN
   SELECT max(a."Id") FROM "firm" a INTO :newid;
    if (:newid IS NULL) THEN newid=0;
   newid=newid+1;
  insert into "firm"
(
  "Id"
, "Name"
, "Bulstad"
, "DDSnum"
, "City"
, "Country"
, "Address"
, "Telefon"
, "Presentor"
, "NameBoss"
, "EGN"
, "PresentorYN"
, "Names"
, "Tel"
, "FirstName"
, "SurName"
, "City2"
, "Address2"
, "LastName"
,  REGISERDDS
)
values
(
  :newid
, :"Name"
, :"Bulstad"
, :"DDSnum"
, :"City"
, :"Country"
, :"Address"
, :"Telefon"
, :"Presentor"
, :"NameBoss"
, :"EGN"
, :"PresentorYN"
, :"Names"
, :"Tel"
, :"FirstName"
, :"SurName"
, :"City2"
, :"Address2"
, :"LastName"
, :REGISERDDS
);
"Id"=newid;
suspend;

END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE IP_LOOKUP (
    "Name" Varchar(50),
    DESCRIPTION Varchar(50) )
RETURNS (
    "Id" Integer )
AS
DECLARE VARIABLE  newid integer; 
DECLARE VARIABLE  str varchar(20);
BEGIN
   SELECT max(a."Id") FROM "lookups" a INTO :newid;
   if (:newid IS NULL) THEN newid=0;
   newid=newid+1;
   str = 'nom_'||newid;
  INSERT INTO "lookups" ("Id", "Name", "Tablename", DESCRIPTION)
 VALUES (
:newid, 
:"Name", 
:str, 
:"DESCRIPTION"
);
"Id"=newid;
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE UPDATECONTO (
    "Date" Date,
    "Oborot" Decimal(18,4),
    "Reason" Varchar(50),
    "Note" Varchar(50),
    "DataInvoise" Date,
    "NumberObject" Integer,
    "DebitAccount" Integer,
    "CreditAccount" Integer,
    "FirmId" Integer,
    "DocumentId" Integer,
    "CartotekaDebit" Integer,
    "CartotecaCredit" Integer,
    "ContoID" Integer,
    DOCNUM Varchar(20),
    OBOROTVALUTA Decimal(18,4),
    OBOROTKOL Decimal(18,4),
    OBOROTVALUTAK Decimal(18,4),
    OBOROTKOLK Decimal(18,4),
    FOLDER Varchar(10) )
AS
DECLARE VARIABLE oborotld Decimal(18,4);
DECLARE VARIABLE oborotvd Decimal(18,4);  
DECLARE VARIABLE oborotkd Decimal(18,4);

DECLARE VARIABLE oborotlk Decimal(18,4);  
DECLARE VARIABLE oborotvk Decimal(18,4);
DECLARE VARIABLE oborotkk Decimal(18,4); 
BEGIN
  update "conto" 
  set
    "Date"=:"Date",
    "Oborot"=:"Oborot", 
    "Reason"=:"Reason", 
    "Note"=:"Note", 
    "DataInvoise"=:"DataInvoise", 
    "NumberObject"=:"NumberObject", 
    "DebitAccount"=:"DebitAccount", 
    "CreditAccount"=:"CreditAccount", 
    "FirmId"=:"FirmId", 
    "DocumentId"=:"DocumentId", 
    "CartotekaDebit"=:"CartotekaDebit", 
    "CartotecaCredit"=:"CartotecaCredit",
    DOCNUM=:DOCNUM,
    OBOROTVALUTA=:OBOROTVALUTA,
    OBOROTKOL=:OBOROTKOL,
    OBOROTVALUTAK=:OBOROTVALUTAK,
    OBOROTKOLK=:OBOROTKOLK,
    FOLDER=:FOLDER
where "Id"=:"ContoID";   
select sum(c."Oborot"),sum(c.OBOROTKOLK),sum(c.OBOROTVALUTAK) from "conto" c where c."CreditAccount"=:"CreditAccount" into :oborotlk,:oborotvk,:oborotkk;
select sum(c."Oborot"),sum(c.OBOROTKOL),sum(c.OBOROTVALUTA) from "conto" c where c."DebitAccount"=:"DebitAccount" into :oborotld,:oborotvd,:oborotkd;
if (:oborotld IS NULL) THEN oborotld=0;
if (:oborotlk IS NULL) THEN oborotlk=0;
if (:oborotvd IS NULL) THEN oborotvd=0;
if (:oborotvk IS NULL) THEN oborotvk=0;
if (:oborotkd IS NULL) THEN oborotkd=0;
if (:oborotkk IS NULL) THEN oborotkk=0; 
Update "accounts" a set a.OBOROTDK=:oborotkd,a.OBOROTDL=:oborotld,a.OBOROTDV=:oborotvd where a."Id"=:"DebitAccount";
Update "accounts" a set a.OBOROTKK=:oborotkk,a.OBOROTL=:oborotlk,a.OBOROTKV=:oborotvk where a."Id"=:"CreditAccount";
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE UPDATEMOVENT (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    VALUED Decimal(18,4),
    GROUPID Integer,
    VALKOLK Decimal(18,4),
    VALKOLD Decimal(18,4),
    VALVALD Decimal(18,4),
    VALVALK Decimal(18,4) )
AS
DECLARE VARIABLE saldok decimal(18,4);
DECLARE VARIABLE saldod decimal(18,4);
declare variable parentid integer; 
declare variable firmid integer; 
BEGIN
   UPDATE MOVEMENT
   SET
     LOOKUPFIELDKEY=:LOOKUPFIELDKEY,
     "VALUE"=:VAL,
     VALUEDATE=:VALUEDATE,
     VALUEMONEY=:VALUEMONEY,
     VALUENUM=:VALUENUM,
     VALUED=:VALUED,
     VALKOLK=:VALKOLK,
     VALKOLD=:VALKOLD,
     VALVALD=:VALVALD,
     VALVALK=:VALVALK
   WHERE 
   ACCID=:ACCID AND ACCFIELDKEY=:ACCFIELDKEY AND "group"=:GROUPID; 
       
   SELECT sum(a.VALUEMONEY) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
   SELECT sum(a.VALUED) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
   Update "accounts" a SET a.SALDO=:saldok,a.SALDODEBIT=:saldod where a."Id"=:ACCID; 
   SELECT sum(a.VALUEMONEY) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
SELECT sum(a.VALUED) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
Update "accounts" a SET a.SALDO=:saldok,a.SALDODEBIT=:saldod where a."Id"=:ACCID;
SELECT sum(a.VALKOLD) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
SELECT sum(a.VALKOLK) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
Update "accounts" a SET a.SALDOKK=:saldok,a.SALDODK=:saldod where a."Id"=:ACCID;
SELECT sum(a.VALVALK) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldok;
SELECT sum(a.VALVALD) FROM MOVEMENT a where a.ACCID=:ACCID INTO :saldod;    
Update "accounts" a SET a.SALDOVALUTA=:saldok,a.SALDODEBITVALUTA=:saldod where a."Id"=:ACCID; 
END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE UPDATEMOVENTCONTO (
    ACCID Integer,
    ACCFIELDKEY Integer,
    LOOKUPFIELDKEY Integer,
    VAL Varchar(50),
    VALUEDATE Date,
    VALUEMONEY Decimal(18,4),
    VALUENUM Integer,
    TYPEACCKEY Integer,
    VALUED Decimal(18,4),
    LOOKUPID Integer,
    CONTOID Integer,
    "TYPE" Smallint,
    ID Integer )
AS
BEGIN

Update  CONTOMOVEMENT c set
     ACCID=:ACCID,
     ACCFIELDKEY=:ACCFIELDKEY,
     LOOKUPFIELDKEY=:LOOKUPFIELDKEY,
     "VALUE"=:VAL,
     VALUEDATE=:VALUEDATE,
     VALUEMONEY=:VALUEMONEY,
     VALUENUM=:VALUENUM,
     TYPEACCKEY=:TYPEACCKEY,
     VALUED=:VALUED,
     CONTOID=:CONTOID,
     LOOKUPID=:LOOKUPID,
     "TYPE"=:"TYPE"
where c.ID=:ID; 
    
END^
SET TERM ; ^


UPDATE RDB$RELATIONS set
RDB$DESCRIPTION = '??????'
where RDB$RELATION_NAME = 'accounts';
GRANT EXECUTE
 ON PROCEDURE ADDCONTO TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE ADDDDSDNEV TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE ADDMOVENT TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE ADDMOVENTCONTO TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE CHECKLOOKUP TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE CHECKMOVENT TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE COPYACCSYTOY TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE DELETEACCOUNT TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE DELETECONTO TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE DELETEDNEV TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE DELETEFIRMA TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE ENDSALDO TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE GETALLOBOROTKA TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE GETOBOROTKA TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE IP_ACCOUNTS TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE IP_FIRM TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE IP_LOOKUP TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE UPDATECONTO TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE UPDATEMOVENT TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE UPDATEMOVENTCONTO TO  SYSDBA;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON CONTOMOVEMENT TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON DDSDNEV TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON DDSDNEVFIELDS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON DDSDNEVSELLSFIELDS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON DDSDNEVTOFIELDS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON FIRMANALITICACCOUNTS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON LOOLUPTOLOOKUP TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON MAINACCOUNTS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON MAPACCTOLOOKUP TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON MAPLOOKUPTOLOOKUP TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON MOVEMENT TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON MOVEMENTTOACC TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON MPATYPETOAFIELD TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON SALDOS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON SUBACCOUNTS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "accounts" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "analiticalaccount" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "analiticalaccounttype" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "analiticalfields" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "cartotecacredit" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "cartotecadebit" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "cities" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "conectoranaliticfield" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "conto" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "countries" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "firm" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "lookups" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "lookupsdetails" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "lookupsfield" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "na" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_10" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_11" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_12" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_14" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_15" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_16" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_17" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_19" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_20" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_21" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_22" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_23" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_7" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_8" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nom_9" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "nomenclatures" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "sysfield" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "syslookups" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON "syslookupsdetails" TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON DDSDNEVSALESREPORTA TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON DDSDNEVSALESREPORTB TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON SELECTMAPAFIELDTOLOOKUP TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON SELECTMAPAFIELDTOLOOKUPTNFN TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON SHOWLOOKUPS TO  SYSDBA WITH GRANT OPTION;

GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE
 ON SYSLOOKUPS TO  SYSDBA WITH GRANT OPTION;

