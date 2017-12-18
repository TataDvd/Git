SELECT c."Id",c."Oborot",c."DataInvoise",m.LOOKUPFIELDKEY,m.LOOKUPID,m."VALUE",lf."Name",m.VALUEDATE
FROM "conto" c 
inner join CONTOMOVEMENT m on m.CONTOID=c."Id"
inner join "lookupsfield" lf on m.ACCFIELDKEY=lf."Id"
where c."FirmId"=13 and c."DataInvoise">='1.1.2013' and c."DataInvoise"<='31.12.2014' and c."CreditAccount"=290
order by c."Id",m.ACCFIELDKEY