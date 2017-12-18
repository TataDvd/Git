using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;

namespace Tempo2012.EntityFramework
{
    public static partial class RealDataContext
    {
        internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAccEx(int id, int typ, string filter)
        {
            List<AccItemSaldo> rez = new List<AccItemSaldo>();
            List<List<string>> rez1 = new List<List<string>>();
            List<string> titles = new List<string>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\",m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,c.\"DebitAccount\",m.LOOKUPVAL FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
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
                row.Type = typ;
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
                                row.Type = typ;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                            else
                            {
                                row.Type = typ;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
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
                                row.Type = typ;
                                row.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                            else
                            {
                                row.Type = typ;
                                row.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                        }
                        chikiriki = 0;
                    }

                    if (chikiriki < count)
                    {
                        string value = dbman.DataReader["VALUE"].ToString();
                        string lvalue = dbman.DataReader["LOOKUPVAL"].ToString();
                        string name=dbman.DataReader["Name"].ToString();
                        row.Details = string.Format("{0}|{1} ", row.Details, value);
                        if (!String.IsNullOrWhiteSpace(lvalue) && !name.Contains("Дата ")) row.Details = string.Format("{0}|{1} ", row.Details, lvalue);
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
            titles.Add("КС");
            rez1.Add(titles);
            var query = (from t in rez
                         group t by new { t.Details, t.Type }
                             into grp
                             select new AccItemSaldo
                             {
                                 Details = grp.Key.Details,
                                 Type = grp.Key.Type,
                                 //Nsc=grp.Sum(t => t.Nsc),
                                 //Nsd = grp.Sum(t => t.Nsc),
                                 Oc = grp.Sum(t => t.Oc),
                                 Od = grp.Sum(t => t.Od),
                                 //Ksd = grp.Sum(t => t.Ksd),
                                 //Ksc = grp.Sum(t => t.Ksc),
                             }).ToList();
            //
            var rezi = GetAllAnaliticSaldos(id, Entrence.CurrentFirma.Id);
            foreach (AccItemSaldo accItemSaldo in query)
            {
                var saldo =
                    rezi.FirstOrDefault(
                        m => m.NameContragent == accItemSaldo.NameContragent && m.NumInvoise==accItemSaldo.NInvoise);
                if (saldo != null)
                {
                    accItemSaldo.Nsd = saldo.BeginSaldoDebit;
                    accItemSaldo.Nsc = saldo.BeginSaldoCredit;
                }
            }
            //foreach (var items in rezi)
            //{
            //    var saldo =
            //       query.FirstOrDefault(
            //           m => m.Details == items.Details);
            //    if (saldo == null)
            //    {
            //        var item = new AccItemSaldo();
            //        item.NInvoise = items.NumInvoise;
            //        item.NameContragent = items.NameContragent;
            //        item.Nsc = items.BeginSaldoCredit;
            //        item.Nsd = items.BeginSaldoDebit;
            //        item.Data = items.Date.ToShortDateString();
            //        item.Type = typ;
            //        item.Details = items.Details;
            //        query.Add(item);
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in query.Where(e => e.Details != null && e.Details.StartsWith(filter)).OrderBy(e => e.Details))
                {
                    var det = item.Details.Split('|');
                    List<string> newrow = det.Skip(1).ToList();
                    decimal saldo = 0;
                    if (item.Type == 1)
                    {
                        saldo = item.Nsd - item.Nsc;
                    }
                    else
                    {
                        saldo = item.Nsc - item.Nsd;
                    }
                    newrow.Add(saldo.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                    decimal ksaldo = 0;
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
            else
            {
                foreach (var item in query.OrderBy(e => e.Details))
                {
                    if (item.Details != null)
                    {
                        var det = item.Details.Split('|');
                        List<string> newrow = det.Skip(1).ToList();
                        decimal saldo = 0;
                        if (item.Type == 1)
                        {
                            saldo = item.Nsd - item.Nsc;
                        }
                        else
                        {
                            saldo = item.Nsc - item.Nsd;
                        }
                        newrow.Add(saldo.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                        newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                        decimal ksaldo = 0;
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

        internal static IEnumerable<Conto> GetNextConto(int firmaId, ISearchAcc pSearcAcc)
        {
            List<Conto> allConto = new List<Conto>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("select First 1 * from \"conto\" c"));
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
                
               
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Id))
                {
                    sb.AppendFormat(" AND c.\"Id\">'{0}'", pSearcAcc.Id);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.UserId))
                {
                    sb.AppendFormat(" AND c.USERID='{0}'", pSearcAcc.UserId);

                }
                sb.AppendFormat(" order by c.\"Id\"");
                string s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text, s);
                LoadConto(allConto,dbman);
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
    }
}
