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
 WHERE a."FirmaId"=:FID and a.YY=:FY
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
   -- EXECUTE PROCEDURE GETOBOROTKA :"DateFrom",:"DateTo",:FID,:"Id"
   --RETURNING_VALUES :NSD, :OBD, :KSD, :NSK, :OBK, :KSK; 
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
    1 ,
    -1,
    :NUM ,
    0 ,
    0 ,
    :TYPEANALITICALKEY ,
    0 ,
    0,
    :ISBUDJET ,
    0 ,
    0 ,
    :TY 
    RETURNING_VALUES :NEWID;
    EXECUTE PROCEDURE MAPACCTOL :"Id", :NEWID;
  END 
END^
SET TERM ; ^


GRANT EXECUTE
 ON PROCEDURE COPYACCSYTOY TO  SYSDBA;
