update CONTOMOVEMENT a
set a."VALUE"=extract(day from a.VALUEDATE)||'.'||extract(month from a.VALUEDATE)||'.'||extract(year from a.VALUEDATE)
where a."VALUE" like '%25/%'