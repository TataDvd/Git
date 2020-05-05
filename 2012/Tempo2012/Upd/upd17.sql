SET TERM ^ ;
CREATE PROCEDURE FIXIMPORTDATES
AS
declare variable WORKID Integer;
declare variable WORKDATE Date;
BEGIN
for 
select c."Date",c."Id" from "conto" c 
where ((c."Id">=283219) AND (c."Id" <=285787))
    OR ((c."Id">=293205 ) AND (c."Id" <=294750)) 
 into :workdate,:workid
  do
  begin
    update CONTOMOVEMENT a
    set 
    a."VALUE"=extract(day from :workdate)||'.'||extract(month from :workdate)||'.'||extract(year from :workdate),
    a.VALUEDATE=:workdate
    where a.CONTOID=:workid AND a.ACCFIELDKEY=22;
    update DDSDNEV d
    set 
    d.DATADOC=:workdate,
    d.DATAF=:workdate
    where d.NOM=:workid;
   end
END^
SET TERM ; ^


GRANT EXECUTE
 ON PROCEDURE FIXIMPORTDATES TO  SYSDBA;