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
if (ISIN=0) then
begin
   select 
    count(m.ID) 
    from CONTOMOVEMENT m 
    where m.LOOKUPID=:LOOKUPID and 
       m."VALUE"=:VAL 
    into :ISIN;
end 
END^
SET TERM ; ^


GRANT EXECUTE
 ON PROCEDURE CHECKLOOKUP TO  SYSDBA;