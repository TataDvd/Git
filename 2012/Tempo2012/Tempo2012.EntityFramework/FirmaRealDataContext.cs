using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.Configuration;
using System.Data;
using System.IO;
using Tempo2012.EntityFramework.Interface;

namespace Tempo2012.EntityFramework
{
    public static partial class RealDataContext
    {
        public static IEnumerable<FirmModel> GetAllFirma()
        {

            List<FirmModel> list = new List<FirmModel>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select f.\"Address\",f.\"Address2\",f.\"Bulstad\",f.\"City\"," +
                                                          "f.EGN," + 
                                                          "f.\"City2\"," +
                                                          "f.\"Country\"," +
                                                          "f.\"DDSnum\"," +
                                                          "f.\"FirstName\"," +
                                                          "f.\"Id\"," +
                                                          "f.\"LastName\"," +
                                                          "f.\"Name\"," +
                                                          "f.\"NameBoss\"," +
                                                          "f.\"Names\"," +
                                                          "f.\"Presentor\"," +
                                                          "f.\"PresentorYN\",f.\"SurName\",f.\"SurName\",f.\"Tel\",f.\"Telefon\",f.REGISERDDS,f.NA,c.\"Name\" as CITYNAME1," +
                                                          "c1.\"Zip\" as ZIP1,c1.\"Name\" as CITYNAME2,c1.\"Zip\" as ZIP2,co.\"Name\" as COUNTRYNAME "+
                                                          "from \"firm\" f "+
                                                          "LEFT OUTER JOIN \"cities\" c  on c.\"Id\"=f.\"City\" " +
                                                          "LEFT OUTER JOIN \"cities\" c1 on c1.\"Id\"=f.\"City2\" " +
                                                          "LEFT OUTER JOIN \"countries\" co on co.\"Id\"=f.\"Country\"");
                while (dbman.DataReader.Read())
                {
                    var firma = new FirmModel();
                    firma.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    firma.Name = dbman.DataReader["Name"].ToString();
                    firma.EGN = dbman. DataReader["EGN"].ToString();
                    firma.Address = dbman. DataReader["Address"].ToString();
                    firma.Bulstad = dbman. DataReader["Bulstad"].ToString();
                    firma.Address2 = dbman. DataReader["Address2"].ToString();
                    firma.DDSnum = dbman. DataReader["DDSnum"].ToString();
                    firma.FirstName = dbman. DataReader["FirstName"].ToString();
                    firma.LastName = dbman. DataReader["LastName"].ToString();
                    firma.NameBoss = dbman. DataReader["NameBoss"].ToString();
                    firma.Names = dbman. DataReader["Names"].ToString();
                    firma.Presentor = dbman. DataReader["Presentor"].ToString();
                    firma.PresentorYN = int.Parse(dbman. DataReader["PresentorYN"].ToString());
                    firma.SurName = dbman. DataReader["SurName"].ToString();
                    firma.Tel = dbman. DataReader["Tel"].ToString();
                    firma.Telefon = dbman. DataReader["Telefon"].ToString();
                    firma.City1 = (int)dbman. DataReader["City2"];
                    firma.City = (int)dbman. DataReader["City"];
                    firma.Country = (int)dbman. DataReader["Country"];
                    firma.RegisterDds=int.Parse(dbman. DataReader["REGISERDDS"].ToString())==1;
                    firma.CityName =  dbman. DataReader["CITYNAME1"].ToString();
                    firma.CityName2 = dbman. DataReader["CITYNAME2"].ToString();
                    firma.Zip = dbman. DataReader["ZIP1"].ToString();
                    firma.Zip2= dbman. DataReader["ZIP2"].ToString();
                    firma.ContryName = dbman. DataReader["COUNTRYNAME"].ToString();
                    firma.AccType = int.Parse(dbman. DataReader["NA"].ToString());
                    list.Add(firma);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllFirma");
            }

            finally
            {
                dbman. Dispose();
            }
            return list;
        }
        public static bool UpdateFirma(FirmModel firm, bool isnew)
        {
            bool result = true;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(21);
                dbman.AddParameters(0, "@Name", firm.Name);
                dbman.AddParameters(1, "@Bulstad", firm.Bulstad);
                dbman.AddParameters(2, "@DDSnum", firm.DDSnum);
                dbman.AddParameters(3, "@City", firm.City);
                dbman.AddParameters(4, "@Country", firm.Country);
                dbman.AddParameters(5, "@Address", firm.Address);
                dbman.AddParameters(6, "@Telefon", firm.Telefon);
                dbman.AddParameters(7, "@Presentor", firm.Presentor);
                dbman.AddParameters(8, "@NameBoss", firm.NameBoss);
                dbman.AddParameters(9, "@EGN", firm.EGN);
                dbman.AddParameters(10, "@PresentorYN", firm.PresentorYN);
                dbman.AddParameters(11, "@Names", firm.Names);
                dbman.AddParameters(12, "@Tel", firm.Tel);
                dbman.AddParameters(13, "@FirstName", firm.FirstName);
                dbman.AddParameters(14, "@SurName", firm.SurName);
                dbman.AddParameters(15, "@City2", firm.City1);
                dbman.AddParameters(16, "@Address2", firm.Address2);
                dbman.AddParameters(17, "@LastName", firm.LastName);
                dbman.AddParameters(18, "@REGISERDDS", firm.RegisterDds ? 1 : 0);
                dbman.AddParameters(19, "@NA", firm.AccType);  
                if (isnew)
                {
                    dbman.AddOutputParameters(20, "@Id", firm.Id);
                    dbman.ExecuteNonQuery(CommandType.StoredProcedure, Commands.InsertFirm);
                    firm.Id = (int)dbman. Parameters[20].Value;
                }
                else
                {
                    dbman. AddParameters(20, "@Id", firm.Id);
                    dbman. ExecuteNonQuery(CommandType.Text, Commands.UpdateFirm);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "UpdateFirma");
                result = false;
            }

            finally
            {
                dbman. Dispose();
            }
            return result;
        }

        internal static List<List<string>> GetUnusableDost(bool delitem)
        {
            List<List<string>> alList = new List<List<string>>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s = "SELECT a.\"Id\",a.\"Name\",a.KONTRAGENT,a.BULSTAT, a.VAT FROM \"nom_12\" a " +
                   //"left outer join CONTOMOVEMENT cm on a.\"Name\"=cm.LOOKUPVAL " +
                   "where a.FIRMAID=" + Entrence.CurrentFirma.Id;

                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var row = new List<string>();
                    if (delitem)
                    {
                        row.Add(dbman.DataReader["Id"].ToString());
                    }
                    row.Add(dbman.DataReader["KONTRAGENT"].ToString());
                    row.Add(dbman.DataReader["Name"].ToString());
                    row.Add(dbman.DataReader["BULSTAT"].ToString());
                    row.Add(dbman.DataReader["VAT"].ToString());
                    alList.Add(row);
                }
                s = "SELECT a.\"Id\", a.KONTRAGENT,a.BULSTAT, a.VAT, a.FIRMAID,cm.NZDDS FROM \"nom_12\" a " +
                    "inner join DDSDNEV cm on a.VAT=cm.NZDDS " +
                    "where a.FIRMAID=" + Entrence.CurrentFirma.Id;
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var cont = dbman.DataReader["KONTRAGENT"].ToString();
                    if (delitem) { alList.RemoveAll(e => e[1] == cont); } else { alList.RemoveAll(e => e[0] == cont); }
                }
                s = "SELECT a.\"Id\", a.KONTRAGENT,a.BULSTAT, a.VAT, a.FIRMAID, a.\"Name\",cm.VALUENUM FROM \"nom_12\" a " +
                    "inner join CONTOMOVEMENT cm on cm.VALUENUM=a.KONTRAGENT " +
                    "where cm.LOOKUPID=12 and a.FIRMAID=" + Entrence.CurrentFirma.Id;
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var cont = dbman.DataReader["KONTRAGENT"].ToString();
                    if (delitem) { alList.RemoveAll(e => e[1] == cont); } else { alList.RemoveAll(e => e[0] == cont); }
                }
                dbman.CloseReader();
                s = "SELECT a.\"Id\", a.KONTRAGENT,a.BULSTAT, a.VAT, a.FIRMAID, a.\"Name\",cm.VALUENUM FROM \"nom_12\" a " +
                   "inner join MOVEMENT cm on cm.VALUENUM=a.KONTRAGENT " +
                   "where cm.LOOKUPID=12 and a.FIRMAID=" + Entrence.CurrentFirma.Id;
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var cont = dbman.DataReader["KONTRAGENT"].ToString();
                    if (delitem) { alList.RemoveAll(e => e[1] == cont); } else { alList.RemoveAll(e => e[0] == cont); }
                }
                dbman.CloseReader();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetUnusableDost(bool delitem)");
            }

            finally
            {
                dbman.Dispose();
            }
            if (delitem)
            {

                StringBuilder sb = new StringBuilder();
                foreach (var item in alList)
                {
                    var comm = string.Format("Delete from \"nom_12\" a where a.\"Id\"={0};", item[0]);
                    sb.AppendLine(comm);
                }
                var Rez = FbBatchExecution(sb.ToString());
                if (Rez != "")
                {

                }
            }
            return alList;
        }

        internal static IEnumerable<QuantityModel> GetAllContoQuantity(int firmid,int accid, DateTime fromDate, DateTime toDate, int v, string codeMaterial)
        {
            Dictionary<int, Dictionary<int, string>> nomen = new Dictionary<int, Dictionary<int, string>>();
            List<QuantityModel> result = new List<QuantityModel>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.PORNOM,c.DOCNUM,c.\"Oborot\",c.\"Date\" as DD,c.FOLDER,c.USERID,c.\"DebitAccount\",c.\"CreditAccount\",m.\"VALUE\",m.VALVAL,m.KURS,m.KURSM,m.KURSD,m.LOOKUPVAL,c.\"Reason\",c.\"Note\",lf.\"Name\",m.VALUEDATE FROM \"conto\" c" +
                        " inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"" +
                        " inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and m.ACCID={1} and \"Date\">='{2}.{3}.{4}' and \"Date\"<='{5}.{6}.{7}' and m.\"TYPE\"={8}) order by c.\"Id\" ",
                        firmid,
                        accid,
                        fromDate.Day,
                        fromDate.Month,
                        fromDate.Year,
                        toDate.Day,
                        toDate.Month,
                        toDate.Year,
                        v);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int newid = 0;
                int oldid = 0;
               var c = new QuantityModel();
                while (dbman.DataReader.Read())
                {
                    newid = int.Parse(dbman.DataReader["Id"].ToString());
                    string nam = dbman.DataReader["Name"].ToString();
                    if (newid != oldid)
                    {
                        oldid = newid;

                        c = new QuantityModel();
                        c.IsDebit = v == 1;
                        c.Id = dbman.DataReader["Id"].ToString();
                        c.DocNum = dbman.DataReader["DOCNUM"].ToString();
                        c.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                        c.Data = dbman.DataReader["DD"].ToString();
                        c.Folder = dbman.DataReader["FOLDER"].ToString();
                        c.User = dbman.DataReader["USERID"].ToString();
                        c.CreditAccount = int.Parse(dbman.DataReader["CreditAccount"].ToString());
                        c.DebitAccount = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                        c.Note = dbman.DataReader["Note"].ToString();
                        c.Reason = dbman.DataReader["Reason"].ToString();
                        c.PorNom = dbman.DataReader["PORNOM"].ToString();
                        result.Add(c);
                    }
                    if (nam == "Количество")
                    {
                        c.SinglePrice = decimal.Parse(dbman.DataReader["KURS"].ToString());
                        c.Quantity = decimal.Parse(dbman.DataReader["VALVAL"].ToString());
                       
                    }
                    if (nam == "Склад")
                    {
                        c.Storage = dbman.DataReader["VALUE"].ToString();
                        c.CodeStorage = dbman.DataReader["LOOKUPVAL"].ToString();
                    }
                    if (nam == "Номенклатурен номер")
                    {
                        c.StockCode = dbman.DataReader["VALUE"].ToString();
                        c.Stock = dbman.DataReader["LOOKUPVAL"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<ValutaControl> GetAllContoValuta(int FirmaId, int ValId, DateTime fromDate, DateTime toDate,string vidval,int mode=1)");


            }

            finally
            {
                dbman.Dispose();
            }
            if (!string.IsNullOrWhiteSpace(codeMaterial))
            {
                return result.Where(e => e.StockCode == codeMaterial);
            }
            return result;
        }

        
        internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAccUni(int id, int typeAccount, int kol, int val, string filter)
        {
            var filti = filter.Split('#');
            filter = filti[0];
            List<AccItemSaldo> rez = new List<AccItemSaldo>();
            List<List<string>> rez1 = new List<List<string>>();
            List<string> titles = new List<string>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.OBOROTVALUTAK,c.OBOROTVALUTA,c.OBOROTKOL,c.OBOROTKOLK,c.\"Date\",m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,c.\"DebitAccount\",m.LOOKUPVAL FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" " +
                        "where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and m.ACCID={2}",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year,
                        id);
                if (filti.Length>1 && !string.IsNullOrWhiteSpace(filti[1]))
                {
                    s = s + $" AND (c.CDETAILS like '%{filti[1].Trim()}%' OR c.DDETAILS like '%{filti[1].Trim()}%')) order by c.\"Id\",m.SORTORDER";
                }
                else
                {
                    s = s + ") order by c.\"Id\",m.SORTORDER";
                }
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                string command =
               string.Format("SELECT count(ca.\"AnaliticalNameID\") " +
                             " FROM \"accounts\" a " +
                             "inner join \"analiticalaccount\" aa on a.\"AnaliticalNum\"=aa.\"Id\"" +
                             "inner join \"analiticalaccounttype\" aat on aa.\"TypeID\"=aat.\"Id\"" +
                             "inner join \"conectoranaliticfield\" ca on aa.\"Id\"=ca.\"AnaliticalNameID\"" +
                             "inner join \"lookupsfield\" af on af.\"Id\"=ca.\"AnaliticalFieldId\" " +
                             "left outer join MAPACCTOLOOKUP l on l.ACCOUNTS_ID=a.\"Id\" and l.ANALITIC_ID=aa.\"Id\" and l.ANALITIC_FIELD_ID=ca.\"AnaliticalFieldId\"" +
                             " where a.\"Id\"={0}", id);
                int count = (int)dbman.ExecuteScalar(CommandType.Text, command);
                bool change = false;
                bool first = true;
                bool firstrow = true;
                bool ima = false;
                AccItemSaldo row = new AccItemSaldo();
                row.Type = typeAccount;
                int oldid = 0, newid = 0;
                int chikiriki = 0;
                while (dbman.DataReader.Read())
                {
                    ima = true;
                    newid = int.Parse(dbman.DataReader["Id"].ToString());
                    if (first)
                    {
                        first = false;
                        oldid = newid;
                        int smetka = 0;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == id)
                            {
                                row.IsDebit = true;
                                row.Type = typeAccount;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                if (kol == 1)
                                {
                                    row.Odk=decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                                }
                                if (val == 1)
                                {
                                    row.Odv = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                                }
                            }
                            else
                            {
                                row.Type = typeAccount;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                if (kol == 1)
                                {
                                    row.Ock = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                                }
                                if (val == 1)
                                {
                                    row.Ocv = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                                }
                            }
                        }
                    }
                    if (oldid != newid)
                    {
                        change = true;
                    }
                    if (change)
                    {
                        if (firstrow)
                        {

                            firstrow = false;
                        }
                        rez.Add(row);
                        row = new AccItemSaldo();
                        oldid = newid;
                        change = false;
                        int smetka;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == id)
                            {
                                row.IsDebit = true;
                                row.Type = typeAccount;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                if (kol == 1)
                                {
                                    row.Odk = decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                                }
                                if (val == 1)
                                {
                                    row.Odv = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                                }
                            }
                            else
                            {
                                row.Type = typeAccount;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                if (kol == 1)
                                {
                                    row.Ock = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                                }
                                if (val == 1)
                                {
                                    row.Ocv = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                                }
                            }
                        }
                        chikiriki = 0;
                    }

                    if (chikiriki < count)
                    {
                        string name = dbman.DataReader["Name"].ToString();
                        string value = dbman.DataReader["VALUE"].ToString();
                        string lookup = dbman.DataReader["LOOKUPVAL"].ToString();
                        row.Fields = string.Format("{0}|{1} ", row.Fields, string.IsNullOrWhiteSpace(lookup) ? value : value + "---" + lookup);
                        if (name == "Контрагент")
                        {
                            row.Code = value;
                        }
                        if (name == "Номер фактура")
                        {
                            row.NInvoise = value;
                        }
                        if (name == "Дата на фактура")
                        {
                            row.Data = DateTime.Parse(value);
                        }
                        if (!name.Contains("Дата ")) row.Details = string.Format("{0}|{1} ", row.Details, value);
                        if (firstrow)
                        {
                            titles.Add(name);
                        }
                        chikiriki++;
                    }
                }
                if (ima)
                {
                    rez.Add(row);
                }
                else
                {
                    //return null;
                    var atr = LoadAllAnaliticfields(id);
                    foreach (SaldoAnaliticModel saldoAnaliticModel in atr)
                    {
                        titles.Add(saldoAnaliticModel.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAcc(int id,int typ,string filter)");
                dbman.RollBackTransaction();

            }


            finally
            {
                dbman.Dispose();
            }
            if (val == 1)
            {
                titles.Add("НСВ");
                titles.Add("ОДВ");
                titles.Add("ОКВ");
                titles.Add("КСВ");
            }
            if (kol == 1)
            {
                titles.Add("НСК");
                titles.Add("ОДК");
                titles.Add("ОКК");
                titles.Add("КСК");
            }
            titles.Add("НС");
            titles.Add("ОД");
            titles.Add("ОК");
            titles.Add("КС");
            rez1.Add(titles);
            var query = (from t in rez
                         group t by new { t.Details}
                             into grp
                             select new AccItemSaldo
                             {
                                 Details=grp.Key.Details,
                                 Type=grp.First().Type,
                                 Data=grp.Last().Data,
                                 Fields=grp.Last().Fields,
                                 Ock=grp.Sum(t => t.Ock),
                                 Odk = grp.Sum(t => t.Odk),
                                 Oc = grp.Sum(t => t.Oc),
                                 Od = grp.Sum(t => t.Od),
                                 Ocv = grp.Sum(t => t.Ocv),
                                 Odv = grp.Sum(t => t.Odv),
                             }).ToList();
            //
            var rezi = GetAllAnaliticSaldos(id, Entrence.CurrentFirma.Id);
            foreach (AccItemSaldo accItemSaldo in query)
            {
                var saldo =
                    rezi.FirstOrDefault(
                        m => accItemSaldo.Details==m.Details);
                if (saldo != null)
                {
                    accItemSaldo.Nsd = saldo.BeginSaldoDebit;
                    accItemSaldo.Nsc = saldo.BeginSaldoCredit;
                    accItemSaldo.Nsdv = saldo.BeginSaldoDebitValuta;
                    accItemSaldo.Nscv = saldo.BeginSaldoCreditValuta;
                    accItemSaldo.Nsdk=  saldo.BeginSaldoDebitKol;
                    accItemSaldo.Nsck=  saldo.BeginSaldoCreditKol;
                    rezi.Remove(saldo);
                 }
            }
            foreach (var items in rezi)
            {
                var saldo =
                   query.FirstOrDefault(
                       m => items.Details.Contains(m.Details));
                if (saldo == null)
                {
                    var item = new AccItemSaldo();
                    item.Nsc = items.BeginSaldoCredit;
                    item.Nsd = items.BeginSaldoDebit;
                    item.Data = items.Date;
                    item.Type = typeAccount;
                    item.Details = items.Details;
                    item.Fields = items.Fields;
                    item.Nsdv = items.BeginSaldoDebitValuta;
                    item.Nscv = items.BeginSaldoCreditValuta;
                    item.Nsdk = items.BeginSaldoDebitKol;
                    item.Nsck = items.BeginSaldoCreditKol;
                    query.Add(item);
                }
            }
            //
            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in query.Where(e => e.Details != null && e.Fields!=null && e.Details.StartsWith(filter)).OrderBy(e => e.Details))
                {
                    var det = item.Fields.Split('|');
                    List<string> newrow = det.Skip(1).ToList();
                    decimal saldo = 0;
                    decimal ksaldo = 0;
                    if (val == 1)
                    {
                        saldo = 0;
                        if (item.Type == 1)
                        {
                            saldo = item.Nsdv - item.Nscv;
                        }
                        else
                        {
                            saldo = item.Nscv - item.Nsdv;
                        }
                        //newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2),item.Data.Month.ToZeroString(2),item.Data.Year.ToZeroString(4))); 
                        newrow.Add(saldo.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Odv.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Ocv.ToString(Vf.LevFormatUI));
                        ksaldo = 0;
                        if (item.Type == 1)
                        {
                            ksaldo = (item.Nsdv + item.Odv) - (item.Nscv + item.Ocv);
                        }
                        else
                        {
                            ksaldo = (item.Nscv + item.Ocv) - (item.Nsdv + item.Odv);
                        }
                        newrow.Add(ksaldo.ToString(Vf.ValFormatUI));
                    }
                    if (kol == 1)
                    {
                         saldo = 0;
                        if (item.Type == 1)
                        {
                            saldo = item.Nsdk - item.Nsck;
                        }
                        else
                        {
                            saldo = item.Nsck - item.Nsdk;
                        }
                        //newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2),item.Data.Month.ToZeroString(2),item.Data.Year.ToZeroString(4))); 
                        newrow.Add(saldo.ToString(Vf.KolFormatUI));
                        newrow.Add(item.Odk.ToString(Vf.KolFormatUI));
                        newrow.Add(item.Ock.ToString(Vf.KolFormatUI));
                         ksaldo = 0;
                        if (item.Type == 1)
                        {
                            ksaldo = (item.Nsdk + item.Odk) - (item.Nsck + item.Ock);
                        }
                        else
                        {
                            ksaldo = (item.Nsck + item.Ock) - (item.Nsdk + item.Odk);
                        }
                        newrow.Add(ksaldo.ToString(Vf.KolFormatUI));

                    }
                    saldo = 0;
                    ksaldo = 0;
                    if (item.Type == 1)
                    {
                        saldo = item.Nsd - item.Nsc;
                    }
                    else
                    {
                        saldo = item.Nsc - item.Nsd;
                    }
                    //newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2),item.Data.Month.ToZeroString(2),item.Data.Year.ToZeroString(4))); 
                    newrow.Add(saldo.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                    if (item.Type == 1)
                    {
                        ksaldo = (item.Nsd + item.Od) - (item.Nsc + item.Oc);
                    }
                    else
                    {
                        ksaldo = (item.Nsc + item.Oc) - (item.Nsd + item.Od);
                    }
                    newrow.Add(ksaldo.ToString(Vf.LevFormatUI));
                    rez1.Add(newrow);
                }
            }
            else
            {
                foreach (var item in query.OrderBy(e => e.Details))
                {
                    if (item.Details != null && item.Fields!=null)
                    {
                        var det = item.Fields.Split('|');
                        List<string> newrow = det.Skip(1).ToList();
                        decimal saldo = 0;
                        decimal ksaldo = 0;
                        if (val == 1)
                        {
                            saldo = 0;
                            if (item.Type == 1)
                            {
                                saldo = item.Nsdv - item.Nscv;
                            }
                            else
                            {
                                saldo = item.Nscv - item.Nsdv;
                            }
                            //newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2),item.Data.Month.ToZeroString(2),item.Data.Year.ToZeroString(4))); 
                            newrow.Add(saldo.ToString(Vf.LevFormatUI));
                            newrow.Add(item.Odv.ToString(Vf.LevFormatUI));
                            newrow.Add(item.Ocv.ToString(Vf.LevFormatUI));
                            ksaldo = 0;
                            if (item.Type == 1)
                            {
                                ksaldo = (item.Nsdv + item.Odv) - (item.Nscv + item.Ocv);
                            }
                            else
                            {
                                ksaldo = (item.Nscv + item.Ocv) - (item.Nsdv + item.Odv);
                            }
                            newrow.Add(ksaldo.ToString(Vf.ValFormatUI));
                        }
                        if (kol == 1)
                        {
                            saldo = 0;
                            if (item.Type == 1)
                            {
                                saldo = item.Nsdk - item.Nsck;
                            }
                            else
                            {
                                saldo = item.Nsck - item.Nsdk;
                            }
                            //newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2),item.Data.Month.ToZeroString(2),item.Data.Year.ToZeroString(4))); 
                            newrow.Add(saldo.ToString(Vf.LevFormatUI));
                            newrow.Add(item.Odk.ToString(Vf.LevFormatUI));
                            newrow.Add(item.Ock.ToString(Vf.LevFormatUI));
                            ksaldo = 0;
                            if (item.Type == 1)
                            {
                                ksaldo = (item.Nsdk + item.Odk) - (item.Nsck + item.Ock);
                            }
                            else
                            {
                                ksaldo = (item.Nsck + item.Ock) - (item.Nsdk + item.Odk);
                            }
                            newrow.Add(ksaldo.ToString(Vf.KolFormatUI));
                        }
                        saldo = 0;
                        if (item.Type == 1)
                        {
                            saldo = item.Nsd - item.Nsc;
                        }
                        else
                        {
                            saldo = item.Nsc - item.Nsd;
                        }
                        //newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2), item.Data.Month.ToZeroString(2), item.Data.Year.ToZeroString(4)));
                        newrow.Add(saldo.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                        ksaldo = 0;
                        if (item.Type == 1)
                        {
                            ksaldo = item.Nsd + item.Od - item.Nsc - item.Oc;
                        }
                        else
                        {
                            ksaldo = item.Nsc + item.Oc - item.Nsd - item.Od;
                        }
                        newrow.Add(ksaldo.ToString(Vf.LevFormatUI));
                        rez1.Add(newrow);
                    }
                }
            }
            return rez1;
        }

        internal static IEnumerable<Conto> GetAllContoOrfiltered(int firmaId, ISearchAcc pSearcAcc)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<Conto> allConto = new List<Conto>();
            try
            {
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from \"conto\" c");
                sb.Append(" inner join \"accounts\" a on a.\"Id\"= c.\"CreditAccount\"");
                sb.Append(" inner join \"accounts\" b on b.\"Id\"= c.\"DebitAccount\"");
                //sb.Append(" left outer join DDSDNEV d on d.NOM=c.\"Id\"");
                sb.AppendFormat(" where \"FirmId\"={0}", firmaId);
                sb.AppendFormat(
                    pSearcAcc.TypeDate == 1
                        ? " and \"Date\">='{0}.{1}.{2}' and \"Date\"<='{3}.{4}.{5}'"
                        : " AND \"Date\">='{0}.{1}.{2}' and \"Date\"<='{3}.{4}.{5}'",
                    pSearcAcc.FromDate.Day,
                    pSearcAcc.FromDate.Month,
                    pSearcAcc.FromDate.Year,
                    pSearcAcc.ToDate.Day,
                    pSearcAcc.ToDate.Month,
                    pSearcAcc.ToDate.Year);
                if (!String.IsNullOrWhiteSpace(pSearcAcc.NumDoc))
                    sb.AppendFormat(" AND DOCNUM='{0}'", pSearcAcc.NumDoc);
                
                if (pSearcAcc.DebitAcc != null)
                {
                    if (pSearcAcc.DebitAcc.Num > 0) sb.AppendFormat(" AND (c.\"CreditAccount\"={0} OR c.\"DebitAccount\"={0})", pSearcAcc.DebitAcc.Num);
                    
                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Note))
                    sb.AppendFormat(" AND UPPER(c.\"Note\") LIKE '%{0}%'", pSearcAcc.Note.ToUpper());
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Reason))
                    sb.AppendFormat(" AND UPPER(c.\"Reason\") LIKE '%{0}%'", pSearcAcc.Reason.ToUpper());

                if (pSearcAcc.DebitItems != null)
                {
                    foreach (var item in pSearcAcc.DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND (UPPER(c.\"CDETAILS\") LIKE '%{0} - {1} %' OR UPPER(c.\"DDETAILS\") LIKE '%{0} - {1} %')", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Ob))
                {
                    sb.AppendFormat(" AND c.\"NumberObject\" = '{0}'", pSearcAcc.Ob);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Folder))
                {
                    sb.AppendFormat(" AND c.FOLDER='{0}'", pSearcAcc.Folder);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Pr1))
                {
                    sb.AppendFormat(" AND c.PR1='{0}'", pSearcAcc.Pr1);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Pr2))
                {
                    sb.AppendFormat(" AND c.PR2='{0}'", pSearcAcc.Pr2);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.PorNom))
                {
                    sb.AppendFormat(" AND c.PORNOM='{0}'", pSearcAcc.PorNom);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Id))
                {
                    sb.AppendFormat(" AND c.\"Id\"='{0}'", pSearcAcc.Id);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.UserId))
                {
                    sb.AppendFormat(" AND c.USERID='{0}'", pSearcAcc.UserId);

                }
                sb.AppendFormat(" order by c.YEARR,c.MON,c.PORNOM");
                string s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text, s);
                LoadConto(allConto, dbman);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetAllConto(int p, Interface.ISearchAcc pSearcAcc)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }
        internal static IEnumerable<Conto> GetContosByContragent(int firmaId, DateTime from, DateTime to, string code,string nom)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<Conto> allConto = new List<Conto>();
            try
            {
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from \"conto\" c");
                sb.Append(" inner join \"accounts\" a on a.\"Id\"= c.\"CreditAccount\"");
                sb.Append(" inner join \"accounts\" b on b.\"Id\"= c.\"DebitAccount\"");
                sb.Append(" inner join CONTOMOVEMENT cm on cm.CONTOID = c.\"Id\"");
                //sb.Append(" left outer join DDSDNEV d on d.NOM=c.\"Id\"");
                sb.AppendFormat(" where \"FirmId\"={0}", firmaId);
                sb.AppendFormat(" AND \"Date\">='{0}.{1}.{2}' and \"Date\"<='{3}.{4}.{5}'",
                   from.Day,
                   from.Month,
                   from.Year,
                   to.Day,
                   to.Month,
                   to.Year);
                sb.AppendFormat(" AND cm.VALUENUM={0} AND cm.LOOKUPID={1}", code,nom);
                string s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text, s);
                LoadConto(allConto, dbman);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetContosByContragent(int firmaId, DateTime from, DateTime to, int code,string nom)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }
        internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAccVal(int id, int typeAccount, string filter)
        {
            List<AccItemSaldo> rez = new List<AccItemSaldo>();
            List<List<string>> rez1 = new List<List<string>>();
            List<string> titles = new List<string>();
            decimal kurs = 0;
            decimal val = 0;
            
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\",m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,c.\"DebitAccount\",m.LOOKUPVAL,m.VALVAL,m.KURS FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, id);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                string command =
               string.Format("SELECT count(ca.\"AnaliticalNameID\") " +
                             " FROM \"accounts\" a " +
                             "inner join \"analiticalaccount\" aa on a.\"AnaliticalNum\"=aa.\"Id\"" +
                             "inner join \"analiticalaccounttype\" aat on aa.\"TypeID\"=aat.\"Id\"" +
                             "inner join \"conectoranaliticfield\" ca on aa.\"Id\"=ca.\"AnaliticalNameID\"" +
                             "inner join \"lookupsfield\" af on af.\"Id\"=ca.\"AnaliticalFieldId\" " +
                             "left outer join MAPACCTOLOOKUP l on l.ACCOUNTS_ID=a.\"Id\" and l.ANALITIC_ID=aa.\"Id\" and l.ANALITIC_FIELD_ID=ca.\"AnaliticalFieldId\"" +
                             " where a.\"Id\"={0}", id);
                int count = (int)dbman.ExecuteScalar(CommandType.Text, command);
                bool change = false;
                bool first = true;
                bool firstrow = true;
                bool ima = false;
                AccItemSaldo row = new AccItemSaldo();
                row.Type = typeAccount;
                int oldid = 0, newid = 0;
                int chikiriki = 0;
                while (dbman.DataReader.Read())
                {
                    ima = true;
                    newid = int.Parse(dbman.DataReader["Id"].ToString());
                    if (first)
                    {
                        first = false;
                        oldid = newid;
                        int smetka = 0;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == id)
                            {
                                row.IsDebit = true;
                                row.Type = typeAccount;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Odv = val;
                                row.Kurs = kurs;
                            }
                            else
                            {
                                row.Type = typeAccount;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Ocv = val;
                                row.Kurs = kurs;
                            }
                        }
                    }
                    if (oldid != newid)
                    {
                        change = true;
                    }
                    if (change)
                    {
                        if (firstrow)
                        {

                            firstrow = false;
                        }
                        rez.Add(row);
                        row = new AccItemSaldo();
                        oldid = newid;
                        change = false;
                        int smetka;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == id)
                            {
                                row.IsDebit = true;
                                row.Type = typeAccount;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Odv = val;
                                row.Kurs = kurs;
                            }
                            else
                            {
                                row.Type = typeAccount;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Ocv = val;
                                row.Kurs = kurs;
                            }
                        }
                        chikiriki = 0;
                    }

                    if (chikiriki <= count)
                    {
                        string name = dbman.DataReader["Name"].ToString();
                        string value = dbman.DataReader["VALUE"].ToString();
                        string lookup = dbman.DataReader["LOOKUPVAL"].ToString();
                        row.Fields = string.Format("{0}|{1} ", row.Fields, string.IsNullOrWhiteSpace(lookup) ? value : value + "---" + lookup);
                        if (name == "Сума валута")
                        {
                            kurs= decimal.Parse(dbman.DataReader["KURS"].ToString());
                            val = decimal.Parse(dbman.DataReader["VALVAL"].ToString());
                        }
                        if (!name.Contains("Дата ")) row.Details = string.Format("{0}|{1} ", row.Details, value);
                        if (firstrow)
                        {
                            titles.Add(name);
                        }
                        chikiriki++;
                    }
                }
                if (ima)
                {
                    rez.Add(row);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAcc(int id,int typ,string filter)");
                dbman.RollBackTransaction();

            }


            finally
            {
                dbman.Dispose();
            }

            titles.Add("НС");
            titles.Add("ОД");
            titles.Add("ОК");
            titles.Add("KС");
            titles.Add("Курс");
            titles.Add("НСВ");
            titles.Add("ОВК");
            titles.Add("ОВД");
            titles.Add("КСВ");
            //titles.Add("КС");
            rez1.Add(titles);
         
            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in rez.Where(e => e.Details != null && e.Details.StartsWith(filter)).OrderBy(e => e.Details))
                {
                    var det = item.Fields.Split('|');
                    List<string> newrow = det.Skip(1).ToList();
                    newrow.Add(item.Ns.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Ns.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Kurs.ToString(Vf.ValFormatUI));
                    newrow.Add(item.Nscv.ToString(Vf.ValFormatUI));
                    newrow.Add(item.Ocv.ToString(Vf.ValFormatUI));
                    newrow.Add(item.Odv.ToString(Vf.ValFormatUI));
                    newrow.Add(item.Kurs.ToString(Vf.ValFormatUI));
                    newrow.Add(item.Kscv.ToString(Vf.ValFormatUI));
                    rez1.Add(newrow);
                }
            }
            else
            {
                foreach (var item in rez.OrderBy(e => e.Details))
                {
                    if (item.Details != null)
                    {
                        var det = item.Fields.Split('|');
                        List<string> newrow = det.Skip(1).ToList();
                        newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Ocv.ToString(Vf.ValFormatUI));
                        newrow.Add(item.Odv.ToString(Vf.ValFormatUI));
                        newrow.Add(item.Kurs.ToString(Vf.ValFormatUI));
                        rez1.Add(newrow);
                    }
                }
            }
            return rez1;
        }
        internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAccMat(int id, int typeAccount, string filter)
        {
            List<AccItemSaldo> rez = new List<AccItemSaldo>();
            List<List<string>> rez1 = new List<List<string>>();
            List<string> titles = new List<string>();
            decimal EdC = 0;
            decimal Col = 0;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\",m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,c.\"DebitAccount\",m.LOOKUPVAL,m.VALVAL,m.KURS FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, id);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                string command =
               string.Format("SELECT count(ca.\"AnaliticalNameID\") " +
                             " FROM \"accounts\" a " +
                             "inner join \"analiticalaccount\" aa on a.\"AnaliticalNum\"=aa.\"Id\"" +
                             "inner join \"analiticalaccounttype\" aat on aa.\"TypeID\"=aat.\"Id\"" +
                             "inner join \"conectoranaliticfield\" ca on aa.\"Id\"=ca.\"AnaliticalNameID\"" +
                             "inner join \"lookupsfield\" af on af.\"Id\"=ca.\"AnaliticalFieldId\" " +
                             "left outer join MAPACCTOLOOKUP l on l.ACCOUNTS_ID=a.\"Id\" and l.ANALITIC_ID=aa.\"Id\" and l.ANALITIC_FIELD_ID=ca.\"AnaliticalFieldId\"" +
                             " where a.\"Id\"={0}", id);
                int count = (int)dbman.ExecuteScalar(CommandType.Text, command);
                bool change = false;
                bool first = true;
                bool firstrow = true;
                bool ima = false;
                AccItemSaldo row = new AccItemSaldo();
                row.Type = typeAccount;
                int oldid = 0, newid = 0;
                int chikiriki = 0;
                while (dbman.DataReader.Read())
                {
                    ima = true;
                    newid = int.Parse(dbman.DataReader["Id"].ToString());
                    if (first)
                    {
                        first = false;
                        oldid = newid;
                        int smetka = 0;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == id)
                            {
                                row.IsDebit = true;
                                row.Type = typeAccount;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Col = Col;
                                row.EdC = EdC;
                            }
                            else
                            {
                                row.Type = typeAccount;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Col = Col;
                                row.EdC = EdC;
                            }
                        }
                    }
                    if (oldid != newid)
                    {
                        change = true;
                    }
                    if (change)
                    {
                        if (firstrow)
                        {

                            firstrow = false;
                        }
                        rez.Add(row);
                        row = new AccItemSaldo();
                        oldid = newid;
                        change = false;
                        int smetka;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == id)
                            {
                                row.IsDebit = true;
                                row.Type = typeAccount;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Col = Col;
                                row.EdC = EdC;
                            }
                            else
                            {
                                row.Type = typeAccount;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                                row.Col = Col;
                                row.EdC = EdC;
                            }
                        }
                        chikiriki = 0;
                    }

                    if (chikiriki <= count)
                    {
                        string name = dbman.DataReader["Name"].ToString();
                        string value = dbman.DataReader["VALUE"].ToString();
                        string lookup= dbman.DataReader["LOOKUPVAL"].ToString();
                        row.Fields = string.Format("{0}|{1} ", row.Fields,string.IsNullOrWhiteSpace(lookup)?value:value+"---"+lookup);
                        if (name == "Количество")
                        {
                           Col=decimal.Parse(dbman.DataReader["VALVAL"].ToString());
                           EdC=decimal.Parse(dbman.DataReader["KURS"].ToString());
                        }
                        
                        if (!name.Contains("Дата ")) row.Details = string.Format("{0}|{1} ", row.Details, value);
                        if (firstrow)
                        {
                            titles.Add(name);
                        }
                        chikiriki++;
                    }
                }
                if (ima)
                {
                    rez.Add(row);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAcc(int id,int typ,string filter)");
                dbman.RollBackTransaction();

            }


            finally
            {
                dbman.Dispose();
            }
              
            //titles.Add("НС");
            titles.Add("ОД");
            titles.Add("ОК");
            titles.Add("Ед.Ц");
            titles.Add("Кол.");
            //titles.Add("КС");
            rez1.Add(titles);
            //var query = (from t in rez
            //             group t by new { t.Details }
            //                 into grp
            //             select new AccItemSaldo
            //             {
            //                 Details = grp.Key.Details,
            //                 Type = grp.First().Type,
            //                 Data = grp.Last().Data,
            //                 //Nsc=grp.Sum(t => t.Nsc),
            //                 //Nsd = grp.Sum(t => t.Nsc),
            //                 Oc = grp.Sum(t => t.Oc),
            //                 Od = grp.Sum(t => t.Od),
            //                 //Ksd = grp.Sum(t => t.Ksd),
            //                 //Ksc = grp.Sum(t => t.Ksc),
            //             }).ToList();
            ////
            //var rezi = GetAllAnaliticSaldos(id, Entrence.CurrentFirma.Id);
            //foreach (AccItemSaldo accItemSaldo in query)
            //{
            //    var saldo =
            //        rezi.FirstOrDefault(
            //            m => accItemSaldo.Details == m.Details);
            //    if (saldo != null)
            //    {
            //        accItemSaldo.Nsd = saldo.BeginSaldoDebit;
            //        accItemSaldo.Nsc = saldo.BeginSaldoCredit;
            //    }
            //}
            //foreach (var items in rezi)
            //{
            //    var saldo =
            //       query.FirstOrDefault(
            //           m => items.Details.Contains(m.Details));
            //    if (saldo == null)
            //    {
            //        var item = new AccItemSaldo();
            //        item.Nsc = items.BeginSaldoCredit;
            //        item.Nsd = items.BeginSaldoDebit;
            //        item.Data = items.Date;
            //        item.Type = typeAccount;
            //        item.Details = items.Details;
            //        query.Add(item);
            //    }
            //}
            //
            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in rez.Where(e => e.Details != null && e.Details.StartsWith(filter)).OrderBy(e => e.Details))
                {
                    var det = item.Fields.Split('|');
                    List<string> newrow = det.Skip(1).ToList();
                    //decimal saldo = 0;
                    //if (item.Type == 1)
                    //{
                    //    saldo = item.Nsd - item.Nsc;
                    //}
                    //else
                    //{
                    //    saldo = item.Nsc - item.Nsd;
                    //}
                    //newrow.Add(string.Format("{0}.{1}.{2}", item.Data.Day.ToZeroString(2), item.Data.Month.ToZeroString(2), item.Data.Year.ToZeroString(4)));
                    //newrow.Add(saldo.ToString(Vf.LevFormatUI));
                    //newrow.Add(item.Col.ToString(Vf.LevFormatUI));
                    //newrow.Add(item.EdC.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Col.ToString(Vf.LevFormatUI));
                    newrow.Add(item.EdC.ToString(Vf.LevFormatUI));
                    //decimal ksaldo = 0;
                    //if (item.Type == 1)
                    //{
                    //    ksaldo = (item.Nsd + item.Od) - (item.Nsc + item.Oc);
                    //}
                    //else
                    //{
                    //    ksaldo = (item.Nsc + item.Oc) - (item.Nsd + item.Od);
                    //}
                    //newrow.Add(ksaldo.ToString(Vf.LevFormatUI));
                    rez1.Add(newrow);
                }
            }
            else
            {
                foreach (var item in rez.OrderBy(e => e.Details))
                {
                    if (item.Details != null)
                    {
                        var det = item.Fields.Split('|');
                        List<string> newrow = det.Skip(1).ToList();
                        //decimal saldo = 0;
                        //if (item.Type == 1)
                        //{
                        //    saldo = item.Nsd - item.Nsc;
                        //}
                        //else
                        //{
                        //    saldo = item.Nsc - item.Nsd;
                        //}
                        ////newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2), item.Data.Month.ToZeroString(2), item.Data.Year.ToZeroString(4)));
                        //newrow.Add(saldo.ToString(Vf.LevFormatUI));
                        //newrow.Add(item.Col.ToString(Vf.LevFormatUI));
                        //newrow.Add(item.EdC.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Col.ToString(Vf.LevFormatUI));
                        newrow.Add(item.EdC.ToString(Vf.LevFormatUI));
                        //decimal ksaldo = 0;
                        //if (item.Type == 1)
                        //{
                        //    ksaldo = item.Nsd + item.Od - item.Nsc - item.Oc;
                        //}
                        //else
                        //{
                        //    ksaldo = item.Nsc + item.Oc - item.Nsd - item.Od;
                        //}
                        //newrow.Add(ksaldo.ToString(Vf.LevFormatUI));
                        rez1.Add(newrow);
                    }
                }
            }
            return rez1;
        }
    }
}
