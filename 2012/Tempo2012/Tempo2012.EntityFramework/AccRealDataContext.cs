﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using PasswordHashMaker;
using Tempo2012.EntityFramework.Models;
using System.Configuration;
using System.Data;
using System.IO;
using FirebirdSql.Data.Isql;
using Tempo2012.EntityFramework.Interface;
using System.ComponentModel;
using System.Diagnostics;



namespace Tempo2012.EntityFramework
{
    public static class IntHelper
    {
        public static string ToZeroString(this int d, int size)
        {
            var s = d.ToString();
            while (s.Length < size)
            {
                s = string.Format("0{0}", s);
            }
            return s;
        }
        public static string ToZeroString(this DateTime d)
        {
            string s = string.Format("{0}.{1}.{2}",d.Day.ToZeroString(2), d.Month.ToZeroString(2),d.Year.ToZeroString(4));
            return s;
        }
    }
    public class OboronaVed
    {
        public string Name;
        public int Id;
        public string Num;
        public string SubNum;
        public int Num1;
        public int SubNum1;
        public decimal NSD;
        public decimal NSK;
        public decimal OD;
        public decimal OK;
        public decimal KSD;
        public decimal KSK;
        public string Contagent;
        public int numContagent;

        public int LookupId { get; internal set; }
        public string Bulstad { get; internal set; }

        public override string ToString()
        {
            if (SubNum !="0")
            {
                 return string.Format("{0}/{1} {2}",Num,SubNum,Name);
            }
            if (Num == "0")
            {
                return string.Format("{0} {1}","000", Name); ;
            }
            return string.Format("{0} {1}",Num,Name);
        }

        public string ToShortString()
        {
            if (SubNum != "0")
            {
                return string.Format("{0}/{1}", Num, SubNum);
            }
            if (Num == "0")
            {
                return "000";
            }
            return string.Format("{0}", Num);
        }
        public void GetTemplateKeys(Dictionary<string, string> keys)
        {
            keys.Add(string.Format("@{0}KSD",ToShortString()),KSD.ToString(Vf.LevFormatUI));
            keys.Add(string.Format("@{0}KSK", ToShortString()), KSK.ToString(Vf.LevFormatUI));
            keys.Add(string.Format("@{0}OD", ToShortString()), OD.ToString(Vf.LevFormatUI));
            keys.Add(string.Format("@{0}OK", ToShortString()), OK.ToString(Vf.LevFormatUI));
            keys.Add(string.Format("@{0}NSD", ToShortString()), NSD.ToString(Vf.LevFormatUI));
            keys.Add(string.Format("@{0}NSK", ToShortString()), NSK.ToString(Vf.LevFormatUI));
        }

    }
    public static partial class RealDataContext
    {

        public static IEnumerable<AnaliticalAccount> GetAllAnaliticalAccount()
        {

            List<AnaliticalAccount> aa = new List<AnaliticalAccount>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                
               
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"analiticalaccount\"");
                while (dbman.DataReader.Read())
                {
                    aa.Add(new AnaliticalAccount
                    {
                        Id = int.Parse(dbman.DataReader["Id"].ToString()),
                        Name = dbman.DataReader["Name"].ToString(),
                        TypeID = int.Parse(dbman.DataReader["TypeID"].ToString())
                    });

                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllAnaliticalAccount");
            }

            finally
            {
                dbman.Dispose();
            }

            #region default

            if (aa.Count == 0)
            {

                aa.AddRange(new List<AnaliticalAccount>
                {
                    new AnaliticalAccount {Id = 1, Name = "Аналитична сметка", TypeID = 1},
                    new AnaliticalAccount {Id = 2, Name = "Аналитична сметка с валута", TypeID = 2},
                    new AnaliticalAccount {Id = 3, Name = "Дълготрайни активи", TypeID = 1},
                    new AnaliticalAccount {Id = 4, Name = "Материали", TypeID = 5},
                    new AnaliticalAccount {Id = 5, Name = "Материаали с валута", TypeID = 6},
                    new AnaliticalAccount {Id = 6, Name = "Контрагенти", TypeID = 1},
                    new AnaliticalAccount {Id = 7, Name = "Контрагенти с валута", TypeID = 2},
                    new AnaliticalAccount {Id = 8, Name = "Контрагенти с падеж", TypeID = 3},
                    new AnaliticalAccount {Id = 9, Name = "Контрагенти с валута и с падеж", TypeID = 4}
                });

            }

            #endregion

            return aa;
        }

        public static IEnumerable<AnaliticalAccountType> GetAllAnaliticalAccountType()
        {
            List<AnaliticalAccountType> aat = new List<AnaliticalAccountType>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
               
                
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"analiticalaccounttype\"");
                while (dbman.DataReader.Read())
                {
                    aat.Add(new AnaliticalAccountType
                    {
                        Id = int.Parse(dbman.DataReader["Id"].ToString()),
                        Name = dbman.DataReader["Name"].ToString(),
                        Kol = int.Parse(dbman.DataReader["KOL"].ToString()) == 0 ? false : true,
                        Sl = int.Parse(dbman.DataReader["SL"].ToString()) == 0 ? false : true,
                        Sv = int.Parse(dbman.DataReader["SV"].ToString()) == 0 ? false : true,
                    });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllAnaliticalAccountType");
            }

            finally
            {
                dbman.Dispose();
            }
            if (aat.Count == 0)
            {
                aat.AddRange(new List<AnaliticalAccountType>
                {
                    new AnaliticalAccountType {Id = 1, Name = "Стойностна"},
                    new AnaliticalAccountType {Id = 2, Name = "Валутна"},
                    new AnaliticalAccountType {Id = 3, Name = "Стойностна с падеж"},
                    new AnaliticalAccountType {Id = 4, Name = "Валутна с падеж"},
                    new AnaliticalAccountType {Id = 5, Name = "Материална"},
                    new AnaliticalAccountType {Id = 6, Name = "Материална с валута"},
                    new AnaliticalAccountType {Id = 7, Name = "Дълготрайни активи"}
                });
            }
            return aat;
        }

        public static IEnumerable<AnaliticalFields> GetAllAnaliticalFields()
        {
            List<AnaliticalFields> allanal = new List<AnaliticalFields>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"lookupsfield\"");
                while (dbman.DataReader.Read())
                {
                    int test;
                    allanal.Add(new AnaliticalFields
                    {
                        Id = int.Parse(dbman.DataReader["Id"].ToString()),
                        Name = dbman.DataReader["Name"].ToString(),
                        FieldType = dbman.DataReader["DBField"].ToString(),
                        RFIELDKEY = dbman.DataReader["RFIELDKEY"].ToString(),
                        RFIELDNAME = dbman.DataReader["RFIELDNAME"].ToString(),
                        RTABLENAME = dbman.DataReader["RTABLENAME"].ToString(),
                        RCODELOOKUP =
                            int.TryParse(dbman.DataReader["RCODELOOKUP"].ToString(), out test)
                                ? test
                                : 0,
                        Group =
                            int.TryParse(dbman.DataReader["GROUP"].ToString(), out test) ? test : 0
                        
                    });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllAnaliticalFields");
            }

            finally
            {
                dbman.Dispose();
            }
            //if (allanal.Count==0)
            //{
            //    allanal.AddRange(new List<AnaliticalFields>
            //    {

            //        new AnaliticalFields{Id=1,Name="Сума лв.",FieldType="money"},
            //        new AnaliticalFields{Id=2,Name="Сума валута",FieldType="money"},
            //        new AnaliticalFields{Id=3,Name="Дата падеж",FieldType="data"},
            //        new AnaliticalFields{Id=4,Name="Количество",FieldType="int"},
            //        new AnaliticalFields{Id=5,Name="Мярка",FieldType="string"}
            //    });

            //}
            return allanal;
        }

        public static IEnumerable<AnaliticalFields> GetAnaliticalFields()
        {
            List<AnaliticalFields> allanal = new List<AnaliticalFields>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"analiticalfields\"");
                while (dbman.DataReader.Read())
                {
                    allanal.Add(new AnaliticalFields
                    {
                        Id = int.Parse(dbman.DataReader["Id"].ToString()),
                        Name = dbman.DataReader["Name"].ToString(),
                        FieldType = dbman.DataReader["FieldType"].ToString(),
                        Requared = int.Parse(dbman.DataReader["REQUIRED"].ToString())==1,
                        SortOrder = int.Parse(dbman.DataReader["SORTORDER"].ToString())
                    });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAnaliticalFields");
            }

            finally
            {
                dbman.Dispose();
            }
            //if (allanal.Count==0)
            //{
            //    allanal.AddRange(new List<AnaliticalFields>
            //    {

            //        new AnaliticalFields{Id=1,Name="Сума лв.",FieldType="money"},
            //        new AnaliticalFields{Id=2,Name="Сума валута",FieldType="money"},
            //        new AnaliticalFields{Id=3,Name="Дата падеж",FieldType="data"},
            //        new AnaliticalFields{Id=4,Name="Количество",FieldType="int"},
            //        new AnaliticalFields{Id=5,Name="Мярка",FieldType="string"}
            //    });

            //}
            return allanal;
        }

        public static bool SaveAa(AnaliticalAccount CurrentAnaliticalAccount,
            IEnumerable<AnaliticalFields> CurrentFieldSelected)
        {
            bool result = true;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;

            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(2);
                dbman.AddParameters(0, "@Name", CurrentAnaliticalAccount.Name);
                dbman.AddParameters(1, "@TypeID", CurrentAnaliticalAccount.TypeID);
                dbman.ExecuteNonQuery(CommandType.Text, Commands.insertAA);
                var newid = dbman.ExecuteScalar(CommandType.Text,
                    "select gen_id(NEWANALITICALACC, 0) from rdb$database");
                CurrentAnaliticalAccount.Id = ((long?) newid).GetValueOrDefault();
                foreach (var field in CurrentFieldSelected)
                {
                    string commands =
                        string.Format(
                            "INSERT INTO \"conectoranaliticfield\" (\"AnaliticalNameID\",\"AnaliticalFieldId\",REQUIRED,SORTORDER) VALUES ({0},{1},{2},{3})",
                            CurrentAnaliticalAccount.Id,
                            field.Id, field.Requared ? 1 : 0,field.SortOrder);
                    dbman.ExecuteNonQuery(CommandType.Text, commands);

                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"SaveAa(AnaliticalAccount CurrentAnaliticalAccount,IEnumerable<AnaliticalFields> CurrentFieldSelected)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        public static bool UpdateAa(AnaliticalAccount CurrentAnaliticalAccount,
            IEnumerable<AnaliticalFields> CurrentFieldSelected)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(3);
                dbman.AddParameters(0, "@Name", CurrentAnaliticalAccount.Name);
                dbman.AddParameters(1, "@TypeID", CurrentAnaliticalAccount.TypeID);
                dbman.AddParameters(2, "@Id", CurrentAnaliticalAccount.Id);
                dbman.ExecuteNonQuery(CommandType.Text, Commands.UpdateAA);
                dbman.ExecuteNonQuery(CommandType.Text,
                    string.Format(Commands.DeleteAAConector, CurrentAnaliticalAccount.Id));

                foreach (var field in CurrentFieldSelected)
                {
                    string commands = string.Format(
                        "INSERT INTO \"conectoranaliticfield\" (\"AnaliticalNameID\",\"AnaliticalFieldId\",REQUIRED,SORTORDER) VALUES ({0},{1},{2},{3})",
                        CurrentAnaliticalAccount.Id,
                        field.Id, field.Requared ? 1 : 0,field.SortOrder);
                    dbman.ExecuteNonQuery(CommandType.Text, commands);

                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"UpdateAa(AnaliticalAccount CurrentAnaliticalAccount,IEnumerable<AnaliticalFields> CurrentFieldSelected)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        public static bool DeleteAa(long p)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.ExecuteNonQuery(CommandType.Text, string.Format(Commands.DeleteAAConector, p));
                dbman.ExecuteNonQuery(CommandType.Text, string.Format(Commands.DeleteAA, p));
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message," DeleteAa(long p)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }



        public static bool DeleteFirma(int firmid)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            bool result = true;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(2);
                dbman.AddParameters(0, "@ID", firmid);
                dbman.AddOutputParameters(1, "@CONFIRMDELETE", firmid);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "DELETEFIRMA");
                int confirm = (int) dbman.Parameters[1].Value;
                if (confirm == 0)
                {
                    result = false;
                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "DeleteFirma(int firmid)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }



        public static bool CheckLookup(int lookupId, string val)
        {
            bool result = true;
            int isin = 0;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(3);
                dbman.AddParameters(0, "@VAL", val);
                dbman.AddParameters(1, "@LOOKUPID", lookupId);
                dbman.AddOutputParameters(2, "@ISIN", isin);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "CHECKLOOKUP");
                isin = (int) dbman.Parameters[2].Value;
                if (isin > 0)
                {
                    result = false;
                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, " CheckLookup(int lookupId, string val)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }



        public static void GetAccMovement(AccountsModel _acc, ObservableCollection<string> OborotDt,
            ObservableCollection<string> OborotKt)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text,
                    string.Format(
                        "SELECT a.\"Oborot\",a.OBOROTVALUTA, a.OBOROTKOL,a.\"DebitAccount\",a.\"CreditAccount\" FROM \"conto\"" +
                        " a where a.\"DebitAccount\"={0} or a.\"CreditAccount\"={0} and \"Date\"=>'1.1.{1}' and \"Date\"<='31.12.{1}'",
                        _acc.Id, ConfigTempoSinglenton.GetInstance().WorkDate.Year));
                while (dbman.DataReader.Read())
                {
                    var t = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                    if (t == _acc.Id)
                    {
                        OborotDt.Add(dbman.DataReader["Oborot"].ToString());
                        OborotKt.Add("");
                    }
                    else
                    {
                        OborotKt.Add(dbman.DataReader["Oborot"].ToString());
                        OborotDt.Add("");
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"GetAccMovement(AccountsModel _acc, ObservableCollection<string> OborotDt,ObservableCollection<string> OborotKt)");
            }

            finally
            {
                dbman.Dispose();
            }

        }

        internal static List<List<string>> GetDnevItem(int KindActivity, int month, int year)
        {
            return GetDnevItem(KindActivity, new DateTime(year, month, 1), new DateTime(year, month, GetEndDate(month,year)));
        }

        private static string add10o(string p)
        {
            string s = "";
            if (p.Count() < 10)
            {
                for (var i = p.Count(); i < 10; i++)
                {
                    s = string.Format("0{0}", s);
                }
            }
            return string.Format("{0}{1}", s, p);


        }

        private static decimal CaseGsuma(int KindActivity, decimal gsuma, List<string> row, Purchases pocupki,
            decimal sumdds,
            Sells prodazbi, string code, decimal suma, ref decimal gumdds, string codedoc)
        {
            if (codedoc.Equals("11") || codedoc.Equals("12") || codedoc.Equals("13") || codedoc.Equals("94"))
                return gsuma;
            if (KindActivity == 2)
            {
                switch (code)
                {
                    case "ДК":
                        prodazbi.Kol11 += suma;
                        prodazbi.Kol12 += sumdds;
                        gsuma += suma;
                        gumdds += sumdds;
                        row[11] = suma.ToString(Vf.LevFormatUI);
                        row[12] = sumdds.ToString(Vf.LevFormatUI);
                        break;
                    case "ВОП":
                        prodazbi.Kol13 += suma;
                        prodazbi.Kol15 += sumdds;
                        row[13] = suma.ToString(Vf.LevFormatUI);
                        row[15] = sumdds.ToString(Vf.LevFormatUI);
                        gsuma += suma;
                        gumdds += sumdds;
                        break;
                    case "ЧЛ82":
                        prodazbi.Kol14 += suma;
                        prodazbi.Kol15 += sumdds;
                        row[14] = suma.ToString(Vf.LevFormatUI);
                        row[15] = sumdds.ToString(Vf.LevFormatUI);
                        gsuma += suma;
                        gumdds += sumdds;
                        break;
                    case "ДРУГИ":
                        prodazbi.Kol16 += sumdds;
                        row[16] = sumdds.ToString(Vf.LevFormatUI);
                        gumdds += sumdds;
                        break;
                    case "ТУРИСТ":
                        prodazbi.Kol17 += suma;
                        prodazbi.Kol18 += sumdds;
                        row[17] = suma.ToString(Vf.LevFormatUI);
                        row[18] = sumdds.ToString(Vf.LevFormatUI);
                        gsuma += suma;
                        gumdds += sumdds;
                        break;
                    case "ГЛ3":
                        prodazbi.Kol19 += suma;
                        row[19] = suma.ToString(Vf.LevFormatUI);
                        gsuma += suma;
                        break;
                    case "ВОД":
                        prodazbi.Kol20 += suma;
                        row[20] = suma.ToString(Vf.LevFormatUI);
                        gsuma += suma;
                        break;
                    case "ЧЛ140":
                        prodazbi.Kol21 += suma;
                        row[21] = suma.ToString(Vf.LevFormatUI);
                        gsuma += suma;
                        break;
                    case "ДРЧЛЕНКА":
                        prodazbi.Kol22 += suma;
                        row[22] = suma.ToString(Vf.LevFormatUI);
                        break;
                    case "ДИСТ":
                        prodazbi.Kol23 += suma;
                        row[23] = suma.ToString(Vf.LevFormatUI);
                        break;
                    case "ОСВ":
                        prodazbi.Kol24 += suma;
                        row[24] = suma.ToString(Vf.LevFormatUI);
                        break;
                    case "ПОСР":
                        prodazbi.Kol25 += suma;
                        row[25] = suma.ToString(Vf.LevFormatUI);
                        break;
                }
            }
            else
            {
                switch (code)
                {
                    case "ОСВ":
                        pocupki.Kol9 += suma;
                        row[9] = suma.ToString(Vf.LevFormatUI);
                        break;
                    case "ДК":
                        pocupki.Kol10 += suma;
                        pocupki.Kol11 += sumdds;
                        row[10] = suma.ToString(Vf.LevFormatUI);
                        row[11] = sumdds.ToString(Vf.LevFormatUI);
                        break;
                    case "ЧДК":
                        pocupki.Kol12 += suma;
                        pocupki.Kol13 += sumdds;
                        row[12] = suma.ToString(Vf.LevFormatUI);
                        row[13] = sumdds.ToString(Vf.LevFormatUI);
                        break;
                    case "ПОСР":
                        pocupki.Kol15 += suma;
                        row[15] = suma.ToString(Vf.LevFormatUI);
                        break;
                    case "ГК":
                        pocupki.Kol14 += suma;
                        row[14] = suma.ToString(Vf.LevFormatUI);
                        break;
                }
            }
            return gsuma;
        }

        private static List<string> Cherta(int p)
        {
            List<string> row = new List<string>();
            for (int i = 0; i < p; i++)
            {
                row.Add("---------------------------");
            }
            return row;
        }

        internal static void ExecuteQuery(string getQuery, DataTable items)
        {
            List<Conto> allConto = new List<Conto>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, getQuery);
                //"SELECT a.\"Id\", a.\"Date\", a.\"Oborot\", a.\"Reason\", a.\"Note\", a.\"DataInvoise\", a.\"NumberObject\", a.\"DebitAccount\", a.\"CreditAccount\", a.\"FirmId\", a.\"DocumentId\", a.\"CartotekaDebit\", a.\"CartotecaCredit\", a.DOCNUM,d.NAMEMAIN dname, d.NUM dnum,d.\"SubNum\" dsubnum,c.NAMEMAIN cname,c.NUM cnum, c.\"SubNum\" csubnum FROM \"conto\" a inner join \"accounts\" d on d.\"Id\"=a.\"CreditAccount\" inner join \"accounts\" c on c.\"Id\"=a.\"DebitAccount\""
                while (dbman.DataReader.Read())
                {
                    DataRow dr = items.NewRow();
                    for (int i = 0; i < dbman.DataReader.FieldCount; i++)
                    {
                        dr[items.Columns[i]] = dbman.DataReader.GetValue(i).ToString();
                    }
                    items.Rows.Add(dr);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "ExecuteQuery(string getQuery, DataTable items)");
            }

            finally
            {
                dbman.Dispose();
            }


        }

        public static Purchases GetPurchases(int month, int year)
        {
            Purchases purchases = new Purchases();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                var str = string.Format(" select d.BRANCH,d.BULSTAD,d.DATADOC,d.DATAF,d.DOCN," +
                                        "d.NOM,d.NAMEKONTR,d.SUMA,d.DDSSUMA as DDSTOTAL,d.STOKE,d.CODEDOC," +
                                        "c.\"FirmId\" as FID,n.\"Name\" as NAMEDOC,dt.SUMADDS,dt.DDS as SUMAWITHDDS,ds.NAME," +
                                        "ds.DDSPERCENT" +
                                        ",ds.CODE,d.KINDACTIVITY,d.A8 as AA,c.PORNOM" +
                                        " from  DDSDNEVSELLSFIELDS ds" +
                                        " inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID" +
                                        " inner join DDSDNEV d on d.ID=dt.IDDDSDNEV" +
                                        " inner join \"conto\" c on c.\"Id\"=d.NOM" +
                                        " inner join \"nomenclatures\" n on n.\"Id\"=d.KINDDOC" +
                                        " where c.\"FirmId\"={0}" +
                                        " and d.KINDACTIVITY={1}" +
                                        " and EXTRACT(MONTH FROM d.DATADOC) = {2}" +
                                        " and EXTRACT(YEAR FROM d.DATADOC) = {3}" +
                                        " order by d.DATADOC",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id, 1, month,
                    year);
                dbman.ExecuteReader(CommandType.Text, str);

                string tester = "";

                while (dbman.DataReader.Read())
                {
                    string codedoc = dbman.DataReader["CODEDOC"].ToString();
                    if (codedoc.Equals("11") || codedoc.Equals("12") || codedoc.Equals("13") || codedoc.Equals("94"))
                        continue;
                    string code = dbman.DataReader["CODE"].ToString();
                    decimal sum = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                    decimal sumdds = decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                    string newtester = dbman.DataReader["NOM"].ToString();
                    if (!newtester.Equals(tester))
                    {
                        tester = newtester;
                        purchases.Count++;
                    }
                    switch (code)
                    {
                        case "ОСВ":
                            purchases.Kol9 += sum;

                            break;
                        case "ДК":
                            purchases.Kol10 += sum;
                            purchases.Kol11 += sumdds;

                            break;
                        case "ЧДК":
                            purchases.Kol12 += sum;
                            purchases.Kol13 += sumdds;

                            break;
                        case "ПОСР":
                            purchases.Kol15 += sum;

                            break;
                        case "ГК":
                            purchases.Kol14 += sum;

                            break;

                    }



                }

            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static Purchases GetPurchases(int month, int year)");
            }

            finally
            {
                dbman.Dispose();
            }
            return purchases;

        }

        public static Sells GetSales(int month, int year)
        {
            Sells sells = new Sells();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                var str = string.Format("select d.BRANCH,d.BULSTAD,d.DATADOC,d.DATAF,d.DOCN," +
                                        "d.NOM,d.NAMEKONTR,d.SUMA,d.DDSSUMA as DDSTOTAL,d.STOKE,d.CODEDOC," +
                                        "c.\"FirmId\" as FID,n.\"Name\" as NAMEDOC,dt.SUMADDS,dt.DDS as SUMAWITHDDS,ds.NAME," +
                                        "ds.DDSPERCENT" +
                                        ",ds.CODE,d.KINDACTIVITY,d.A8 as AA,c.PORNOM" +
                                        " from  DDSDNEVFIELDS ds" +
                                        " inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID" +
                                        " inner join DDSDNEV d on d.ID=dt.IDDDSDNEV" +
                                        " inner join \"conto\" c on c.\"Id\"=d.NOM" +
                                        " inner join \"nomenclatures\" n on n.\"Id\"=d.KINDDOC" +
                                        " where c.\"FirmId\"={0}" +
                                        " and d.KINDACTIVITY={1}" +
                                        " and EXTRACT(MONTH FROM d.DATADOC) = {2}" +
                                        " and EXTRACT(YEAR FROM d.DATADOC) = {3}" +
                                        " order by d.DATADOC",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    2,
                    month,
                    year);
                dbman.ExecuteReader(CommandType.Text, str);
                string tester = "";
                while (dbman.DataReader.Read())
                {
                    string codedoc = dbman.DataReader["CODEDOC"].ToString();
                    if (codedoc.Equals("11") || codedoc.Equals("12") || codedoc.Equals("13") || codedoc.Equals("94"))
                        continue;
                    decimal suma = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                    decimal sumdds = decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                    decimal dds = sumdds;
                    string code = dbman.DataReader["CODE"].ToString();
                    string newtester = dbman.DataReader["NOM"].ToString();
                    if (!newtester.Equals(tester))
                    {
                        tester = newtester;
                        sells.Count++;
                    }
                    switch (code)
                    {
                        case "ДК":
                            sells.Kol11 += suma;
                            sells.Kol12 += dds;
                            break;
                        case "ВОП":
                            sells.Kol13 += suma;
                            sells.Kol15 += dds;
                            break;
                        case "ЧЛ82":
                            sells.Kol14 += suma;
                            sells.Kol15 += dds;
                            break;
                        case "ДРУГИ":
                            sells.Kol16 += dds;
                            break;
                        case "ТУРИСТ":
                            sells.Kol17 += suma;
                            sells.Kol18 += dds;
                            break;
                        case "ГЛ3":
                            sells.Kol19 += suma;
                            break;
                        case "ВОД":
                            sells.Kol20 += suma;
                            break;
                        case "ЧЛ140":
                            sells.Kol21 += suma;
                            break;
                        case "ДРЧЛЕНКА":
                            sells.Kol22 += suma;
                            break;
                        case "ДИСТ":
                            sells.Kol23 += suma;
                            break;
                        case "ОСВ":
                            sells.Kol24 += suma;
                            break;
                        case "ПОСР":
                            sells.Kol25 += suma;
                            break;

                    }



                }

            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, " public static Sells GetSales(int month, int year)");
            }

            finally
            {
                dbman.Dispose();
            }
            return sells;
        }



        public static IEnumerable<Conto> GetAllConto(int firmaId, int startingIndex, int numberOfRecords)
        {
            List<Conto> allConto = new List<Conto>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();

                //var c = dbman.ExecuteScalar(CommandType.Text,
                //                                String.Format(
                //                                    "select count(*) from \"conto\" where \"FirmId\"={0} and \"Date\">='1.1.{1}' and \"Date\"<='31.12.{1}'",
                //                                    firmaId, ConfigTempoSinglenton.GetInstance().WorkDate.Year));
                //long count = (long) startingIndex + 1;
                //int totalcount = (int) Convert.ToInt64(c);
                //long cpage = startingIndex/numberOfRecords;
                dbman.ExecuteReader(CommandType.Text,
                    String.Format(
                        "select FIRST {0} SKIP {1} * from \"conto\" a where \"FirmId\"={2} and \"Date\">='1.{4}.{3}' and \"Date\"<='{5}.{4}.{3}' order by a.\"Id\" DESC",
                        numberOfRecords, startingIndex, firmaId,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Month,
                        GetEndDate(ConfigTempoSinglenton.GetInstance().WorkDate.Month,
                            ConfigTempoSinglenton.GetInstance().WorkDate.Year)));

                LoadConto(allConto,dbman);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<Conto> GetAllConto(int firmaId, int startingIndex, int numberOfRecords)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }

        private static int GetEndDate(int toMonth, int currentYear)
        {
            int rez = 30;
            switch (toMonth)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                {
                    rez = 31;
                }
                    break;
                case 2:
                    rez = IsYearBig(currentYear) ? 29 : 28;
                    break;
            }
            return rez;
        }

        private static bool IsYearBig(int currentYear)
        {
            return currentYear%4 == 0;
        }

        private static void LoadConto(List<Conto> allConto,DBManager dbman)
        {
            while (dbman.DataReader.Read())
            {
                var c = new Conto();
                c.Id = int.Parse(dbman.DataReader["Id"].ToString());
                c.CartotecaCredit = int.Parse(dbman.DataReader["CartotecaCredit"].ToString());
                c.CartotekaDebit = int.Parse(dbman.DataReader["CartotekaDebit"].ToString());
                c.CreditAccount = int.Parse(dbman.DataReader["CreditAccount"].ToString());
                c.Data = DateTime.Parse(dbman.DataReader["Date"].ToString());
                c.DataInvoise = DateTime.Parse(dbman.DataReader["DataInvoise"].ToString());
                c.DebitAccount = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                c.DocumentId = int.Parse(dbman.DataReader["DocumentId"].ToString());
                c.FirmId = int.Parse(dbman.DataReader["FirmId"].ToString());
                c.Reason = dbman.DataReader["Reason"].ToString();
                c.Note = dbman.DataReader["Note"].ToString();
                c.NumberObject = int.Parse(dbman.DataReader["NumberObject"].ToString());
                c.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                c.DocNum = dbman.DataReader["DOCNUM"].ToString();
                c.OborotValutaD = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                c.OborotKolD = decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                c.OborotValutaK = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                c.OborotKolK = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                c.Folder = dbman.DataReader["FOLDER"].ToString();
                c.Nd = int.Parse(dbman.DataReader["PORNOM"].ToString());
                c.IsDdsPurchases = int.Parse(dbman.DataReader["ISDDSPURCHASES"].ToString());
                c.IsDdsSales = int.Parse(dbman.DataReader["ISDDSSALES"].ToString());
                c.IsDdsPurchasesIncluded = int.Parse(dbman.DataReader["ISDDSPURCHASESINCLUDED"].ToString());
                c.IsDdsSalesIncluded = int.Parse(dbman.DataReader["ISDDSSALESINCLUDED"].ToString());
                c.VopPurchases = dbman.DataReader["VOPPURCHASES"].ToString();
                c.VopSales = dbman.DataReader["VOPSALES"].ToString();
                c.IsSales = int.Parse(dbman.DataReader["ISSALES"].ToString());
                c.IsPurchases = int.Parse(dbman.DataReader["ISPURCHASES"].ToString());
                c.DDetails = dbman.DataReader["DDETAILS"].ToString();
                c.CDetails = dbman.DataReader["CDETAILS"].ToString();
                c.UserId = int.Parse(dbman.DataReader["USERID"].ToString());
                c.Pr1 = dbman.DataReader["PR1"].ToString();
                c.Pr2 = dbman.DataReader["PR2"].ToString();
                c.KD = dbman.DataReader["KD"].ToString();
                allConto.Add(c);
            }
        }

        internal static IEnumerable<Conto> GetAllConto(int p, Interface.ISearchAcc pSearcAcc)
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
                sb.AppendFormat(" where \"FirmId\"={0}", p);
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
                if (pSearcAcc.CreditAcc != null)
                {
                    if (pSearcAcc.CreditAcc.Num > 0)
                        sb.AppendFormat(" AND a.NUM={0}", pSearcAcc.CreditAcc.Num);
                    if (pSearcAcc.CreditAcc.SubNum > -1)
                        sb.AppendFormat(" AND a.\"SubNum\"={0}", pSearcAcc.CreditAcc.SubNum);
                }
                if (pSearcAcc.DebitAcc != null)
                {
                    if (pSearcAcc.DebitAcc.Num > 0)
                        sb.AppendFormat(" AND b.NUM={0}", pSearcAcc.DebitAcc.Num);
                    if (pSearcAcc.DebitAcc.SubNum > -1)
                        sb.AppendFormat(" AND b.\"SubNum\"={0}", pSearcAcc.DebitAcc.SubNum);
                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Note))
                    sb.AppendFormat(" AND UPPER(c.\"Note\") LIKE '%{0}%'", pSearcAcc.Note.ToUpper());
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Reason))
                    sb.AppendFormat(" AND UPPER(c.\"Reason\") LIKE '%{0}%'", pSearcAcc.Reason.ToUpper());
               
                    if (pSearcAcc.CreditItems != null)
                    {
                        foreach (var item in pSearcAcc.CreditItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                        {
                            sb.AppendFormat(" AND UPPER(c.\"CDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                        }
                    }
                    if (pSearcAcc.DebitItems != null)
                    {
                        foreach (var item in pSearcAcc.DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                        {
                            sb.AppendFormat(" AND UPPER(c.\"DDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
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

        internal static List<List<string>> GetOborotnaVed(DateTime ToDate, DateTime FromDate)
        {
            var Allacc = new List<AccountsModel>(GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            List<List<string>> result = new List<List<string>>();
            List<OboronaVed> gruper = new List<OboronaVed>();
            foreach (var item in Allacc)
            {
                var obor = new OboronaVed();
                obor.Id =item.Id;
                obor.Num =item.Num.ToString();
                obor.SubNum = item.SubNum.ToString();
                obor.Name = item.NameMain;
                gruper.Add(obor);
            }
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                string s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",sum(a.\"Oborot\") as debit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}') " +
                   "group by b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\" " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year
                   );
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    
                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    var ite = gruper.FirstOrDefault(e => e.Id == id);
                    if (ite == null)
                    {
                        var obor = new OboronaVed();
                        obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                        obor.Num = dbman.DataReader["NUM"].ToString();
                        obor.SubNum = dbman.DataReader["SubNum"].ToString();
                        obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                        obor.OD = decimal.Parse(dbman.DataReader["debit"].ToString());
                        gruper.Add(obor);
                    }
                    else
                    {
                        ite.OD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    }
                    
                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",sum(a.\"Oborot\") as debit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<'{4}.{5}.{6}') " +
                   "group by b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\" " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    ToDate.Year,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int id = int.Parse(dbman.DataReader["Id"].ToString());
                    var ite = gruper.FirstOrDefault(e => e.Id == id);
                    if (ite == null)
                    {
                        var obor = new OboronaVed();
                        obor.Id = id;
                        obor.Num = dbman.DataReader["NUM"].ToString();
                        obor.SubNum = dbman.DataReader["SubNum"].ToString();
                        obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                        obor.NSD = decimal.Parse(dbman.DataReader["debit"].ToString());
                        gruper.Add(obor);
                    }
                    else
                    {
                        ite.NSD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    }
                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",sum(a.\"Oborot\") as credit FROM \"conto\" a "+
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}')"+
                   "group by b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\" "+
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int id=int.Parse(dbman.DataReader["Id"].ToString());
                     var ite=gruper.FirstOrDefault(e=>e.Id==id);
                     if (ite == null)
                     {
                         var obor = new OboronaVed();
                         obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                         obor.Num = dbman.DataReader["NUM"].ToString();
                         obor.SubNum = dbman.DataReader["SubNum"].ToString();
                         obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                         obor.OK = decimal.Parse(dbman.DataReader["credit"].ToString());
                         gruper.Add(obor);
                     }
                     else
                     {
                         ite.OK = decimal.Parse(dbman.DataReader["credit"].ToString());
                     }
                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",sum(a.\"Oborot\") as credit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\"" +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<'{4}.{5}.{6}')" +
                   "group by b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\" " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    ToDate.Year,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                 while (dbman.DataReader.Read())
                 {
                     int id=int.Parse(dbman.DataReader["Id"].ToString());
                     var ite=gruper.FirstOrDefault(e=>e.Id==id);
                     if (ite == null)
                     {
                         var obor = new OboronaVed();
                         obor.Id = id;
                         obor.Num = dbman.DataReader["NUM"].ToString();
                         obor.SubNum = dbman.DataReader["SubNum"].ToString();
                         obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                         obor.NSK = decimal.Parse(dbman.DataReader["credit"].ToString());
                         gruper.Add(obor);
                     }
                     else
                     {
                         ite.NSK = decimal.Parse(dbman.DataReader["credit"].ToString());
                     }
                 }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetOborotnaVed(DateTime ToDate, DateTime FromDate)");
            }

            finally
            {
                dbman.Dispose();
            }
            foreach (var item in gruper)
            {
                var sm = Allacc.FirstOrDefault(e => e.Id == item.Id);
                if (sm != null)
                {
                    if (Math.Abs(item.NSD+sm.SaldoDL) > Math.Abs(item.NSK+sm.SaldoKL))
                    {
                        item.NSD = (item.NSD+sm.SaldoDL) - (item.NSK+sm.SaldoKL);
                        item.NSK = 0;
                    }
                    else
                    {
                        item.NSK = (item.NSK+sm.SaldoKL) - (item.NSD+sm.SaldoDL);
                        item.NSD = 0;
                    }
                    var sdebit = item.NSD + item.OD;
                    var scredit = item.NSK + item.OK;
                    if (Math.Abs(sdebit) > Math.Abs(scredit))
                    {
                        item.KSD = sdebit - scredit;
                    }
                    else
                    {
                        item.KSK = scredit - sdebit;
                    }
                    //if (sm.TypeAccount == 1)
                    //{
                    //    item.NSD += sm.BeginSaldoL;
                    //    item.KSD = item.NSD + item.OD - item.NSK-item.OK;
                    //}
                    //else
                    //{
                    //    item.NSK += sm.BeginSaldoL;
                    //    item.KSD = item.NSK + item.OK - item.NSD-item.OD;
                    //}
                                    }
                var row = new List<string>();
                row.Add(item.ToShortString());
                row.Add(item.Name);
                row.Add(item.NSD.ToString(Vf.LevFormatUI));
                row.Add(item.NSK.ToString(Vf.LevFormatUI));
                row.Add(item.OD.ToString(Vf.LevFormatUI));
                row.Add(item.OK.ToString(Vf.LevFormatUI));
                row.Add(item.KSD.ToString(Vf.LevFormatUI));
                row.Add(item.KSK.ToString(Vf.LevFormatUI));
                result.Add(row);
            }
            return result;
        }

        internal static List<List<string>> GetOborotnaFullDetailed(DateTime ToDate, DateTime FromDate)
        {
            var Allacc = new List<AccountsModel>(GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            var Oborotka = GetOborotnaVed(ToDate, FromDate);
            List<List<string>> result = new List<List<string>>();
            foreach (var item in Allacc)
            {
                if (item.LevelAccount == 2)
                {
                    //var row = Oborotka.FirstOrDefault(e => e[0] == item.ShortName);
                    //if (row != null) result.Add(row);
                    var rows = GetOborotnaVedDetailed(ToDate, FromDate, item.Id);
                    foreach (var r in rows)
                    {
                        List<String> toadd = new List<string>();
                        toadd.Add(r[0]);
                        toadd.Add(string.Format("{0} {1} {2} {3}",r[1], r[2], r[3],r[4]));
                        toadd.Add(r[5]);
                        toadd.Add(r[6]);
                        toadd.Add(r[7]);
                        toadd.Add(r[8]);
                        toadd.Add(r[9]);
                        toadd.Add(r[10]);
                        result.Add(toadd);
                    }
                        
                }
                else
                {
                    var row = Oborotka.FirstOrDefault(e => e[0] == item.Short);
                    if (row != null) result.Add(row);   
                }
            }
            return result;
        }
        internal static List<List<string>> GetOborotnaVedDetailed(DateTime ToDate, DateTime FromDate,int accid=-1)
        {
            var Allacc = new List<AccountsModel>(GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            List<List<string>> result = new List<List<string>>();
            List<OboronaVed> gruper = new List<OboronaVed>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                string s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.DDETAILS,a.\"Oborot\" as debit,m.LOOKUP_ID FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "inner join MAPACCTOLOOKUP m on b.\"Id\"=m.ACCOUNTS_ID " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}' and m.ANALITIC_FIELD_ID=28 and b.\"LevelAccount\"=2{7} " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year,
                    accid == -1 ? ")" : string.Format(" and b.\"Id\"={0}) ", accid)
                   );
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["DDETAILS"].ToString();
                    if (contr != null)
                    {
                        SetNomandFact(ref nomer, ref contr);
                    }

                    var obor = new OboronaVed();
                    obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.OD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    obor.Contagent = contr;
                    obor.numContagent = nomer;
                    obor.LookupId = int.Parse(dbman.DataReader["LOOKUP_ID"].ToString());
                    gruper.Add(obor);


                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.DDETAILS,m.LOOKUP_ID,a.\"Oborot\" as debit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "inner join MAPACCTOLOOKUP m on b.\"Id\"=m.ACCOUNTS_ID " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<'{4}.{5}.{6}' and m.ANALITIC_FIELD_ID=28 and b.\"LevelAccount\"=2{7} " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    ToDate.Year,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    accid == -1 ? ")" : string.Format(" and b.\"Id\"={0}) ", accid)
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    int id = int.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["DDETAILS"].ToString();
                    if (contr != null)
                    {
                        SetNomandFact(ref nomer, ref contr);
                    }

                    var obor = new OboronaVed();
                    obor.Id = id;
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.NSD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    obor.Contagent = contr;
                    obor.numContagent = nomer;
                    obor.LookupId = int.Parse(dbman.DataReader["LOOKUP_ID"].ToString());
                    gruper.Add(obor);

                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.CDETAILS,m.LOOKUP_ID,a.\"Oborot\" as credit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\" " +
                   "inner join MAPACCTOLOOKUP m on b.\"Id\"=m.ACCOUNTS_ID " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}' and m.ANALITIC_FIELD_ID=28 and b.\"LevelAccount\"=2 {7}" +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year,
                    accid == -1 ? ")" : string.Format(" and b.\"Id\"={0}) ", accid)
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["CDETAILS"].ToString();
                    if (contr != null)
                    {
                        SetNomandFact(ref nomer, ref contr);
                    }

                    var obor = new OboronaVed();
                    obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.Contagent = contr;
                    obor.OK = decimal.Parse(dbman.DataReader["credit"].ToString());
                    obor.numContagent = nomer;
                    obor.LookupId = int.Parse(dbman.DataReader["LOOKUP_ID"].ToString());
                    gruper.Add(obor);

                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.CDETAILS,m.LOOKUP_ID,a.\"Oborot\" as credit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\"" +
                   "inner join MAPACCTOLOOKUP m on b.\"Id\"=m.ACCOUNTS_ID " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<'{4}.{5}.{6}' and m.ANALITIC_FIELD_ID=28  and b.\"LevelAccount\"=2{7} " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    ToDate.Year,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    accid == -1 ? ")" : string.Format(" and b.\"Id\"={0}) ", accid)
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    int id = int.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["CDETAILS"].ToString();
                    if (contr != null)
                    {
                        SetNomandFact(ref nomer, ref contr);
                    }

                    var obor = new OboronaVed();
                    obor.Id = id;
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.NSK = decimal.Parse(dbman.DataReader["credit"].ToString());
                    obor.Contagent = contr;
                    obor.numContagent = nomer;
                    obor.LookupId = int.Parse(dbman.DataReader["LOOKUP_ID"].ToString());
                    gruper.Add(obor);

                }


                var results = from p in gruper
                              group p by new { p.Num, p.SubNum, p.Name, p.numContagent } into gcs
                              select new OboronaVed()
                              {
                                  Num = gcs.Key.Num,
                                  SubNum = gcs.Key.SubNum,
                                  Num1 = int.Parse(gcs.Key.Num),
                                  SubNum1 = int.Parse(gcs.Key.SubNum),
                                  Name = gcs.Key.Name,
                                  Id = gcs.First().Id,
                                  Contagent = gcs.First().Contagent,
                                  LookupId = gcs.First().LookupId,
                                  numContagent = gcs.Key.numContagent,
                                  NSD = gcs.Sum(x => x.NSD),
                                  NSK = gcs.Sum(x => x.NSK),
                                  OD = gcs.Sum(x => x.OD),
                                  OK = gcs.Sum(x => x.OK),
                              };

                int olditemid = -1;
                OboronaVed olditem = null;
                int lookupid = 12;
                var lookupModel = GetLookup(lookupid);
                var activelookup = GetLookupDictionary(lookupModel.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                List<SaldoFactura> rezi = new List<SaldoFactura>();
                if (results.Count() > 0)
                {
                    foreach (var item in results.OrderBy(e => e.Num1).ThenBy(e => e.SubNum1).ThenBy(e => e.numContagent))
                    {
                        if (item.LookupId != 0 && lookupid != item.LookupId)
                        {
                            lookupModel = GetLookup(item.LookupId);
                            activelookup = GetLookupDictionary(lookupModel.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                            lookupid = item.LookupId;
                        }
                        if (olditemid != item.Id)
                        {
                            if (rezi != null && rezi.Count > 0)
                            {
                                foreach (var it in rezi)
                                {
                                    var itemp = new OboronaVed();
                                    if (olditem != null)
                                    {
                                        itemp.Id = olditem.Id;
                                        itemp.Num = olditem.Num;
                                        itemp.SubNum = olditem.SubNum;
                                        itemp.Name = olditem.Name;
                                    }
                                    itemp.numContagent = int.Parse(it.Code);
                                    var bul = activelookup.FirstOrDefault(e => e.ContainsKey("KONTRAGENT") && e["KONTRAGENT"].ToString() == itemp.numContagent.ToString());
                                    if (bul != null)
                                    {
                                        if (bul.ContainsKey("BULSTAT")) itemp.Bulstad = bul["BULSTAT"].ToString();
                                        if (bul.ContainsKey("Name")) itemp.Contagent = bul["Name"].ToString();
                                    }
                                    decimal d = 0;
                                    decimal c = 0;
                                    d = it.BeginSaldoDebit;
                                    c = it.BeginSaldoCredit;
                                    if (Math.Abs(itemp.NSD + d) > Math.Abs(itemp.NSK + c))
                                    {
                                        itemp.NSD = (itemp.NSD + d) - (itemp.NSK + c);
                                        itemp.NSK = 0;
                                    }
                                    else
                                    {
                                        itemp.NSK = (itemp.NSK + c) - (itemp.NSD + d);
                                        itemp.NSD = 0;
                                    }
                                    var sd = itemp.NSD + itemp.OD;
                                    var sc = itemp.NSK + itemp.OK;
                                    if (Math.Abs(sd) > Math.Abs(sc))
                                    {
                                        itemp.KSD = sd - sc;
                                    }
                                    else
                                    {
                                        itemp.KSK = sc - sd;
                                    }
                                    var row1 = new List<string>();
                                    row1.Add(itemp.ToShortString());
                                    row1.Add(itemp.Name);
                                    row1.Add(itemp.numContagent==0?"":itemp.numContagent.ToString());
                                    row1.Add(itemp.Bulstad ?? " ");
                                    row1.Add(itemp.Contagent ?? " ");
                                    row1.Add(itemp.NSD.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.NSK.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.OD.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.OK.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.KSD.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.KSK.ToString(Vf.LevFormatUI));
                                    result.Add(row1);


                                }
                            }
                            rezi = GetAllAnaliticSaldos(item.Id, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                            rezi = new List<SaldoFactura>(from p in rezi
                                                          group p by new { p.Code } into gcs
                                                          select new SaldoFactura()
                                                          {
                                                              Code = gcs.Key.Code,
                                                              BeginSaldoCredit = gcs.Sum(x => x.BeginSaldoCredit),
                                                              BeginSaldoDebit = gcs.Sum(x => x.BeginSaldoDebit)
                                                          });
                            olditemid = item.Id;
                            olditem = item;
                        }
                        var ns = rezi.FirstOrDefault(e => e.Code == item.numContagent.ToString());
                        if (item.LookupId != 0 && item.numContagent != 0)
                        {
                            var bul = activelookup.FirstOrDefault(e => e.ContainsKey("KONTRAGENT") && e["KONTRAGENT"].ToString() == item.numContagent.ToString());
                            if (bul != null)
                            {
                                if (bul.ContainsKey("BULSTAT")) item.Bulstad = bul["BULSTAT"].ToString();
                            }
                        }
                        decimal nsd = 0;
                        decimal nsc = 0;
                        if (ns != null)
                        {
                            nsd = ns.BeginSaldoDebit;
                            nsc = ns.BeginSaldoCredit;
                            rezi.Remove(ns);
                        }
                        if (Math.Abs(item.NSD + nsd) > Math.Abs(item.NSK + nsc))
                        {
                            item.NSD = (item.NSD + nsd) - (item.NSK + nsc);
                            item.NSK = 0;
                        }
                        else
                        {
                            item.NSK = (item.NSK + nsc) - (item.NSD + nsd);
                            item.NSD = 0;
                        }
                        var sdebit = item.NSD + item.OD;
                        var scredit = item.NSK + item.OK;
                        if (Math.Abs(sdebit) > Math.Abs(scredit))
                        {
                            item.KSD = sdebit - scredit;
                        }
                        else
                        {
                            item.KSK = scredit - sdebit;
                        }
                        //if (sm.TypeAccount == 1)
                        //{
                        //    item.NSD += sm.BeginSaldoL;
                        //    item.KSD = item.NSD + item.OD - item.NSK-item.OK;
                        //}
                        //else
                        //{
                        //    item.NSK += sm.BeginSaldoL;
                        //    item.KSD = item.NSK + item.OK - item.NSD-item.OD;
                        //}

                        var row = new List<string>();
                        row.Add(item.ToShortString());
                        row.Add(item.Name);
                        row.Add(item.numContagent.ToString());
                        row.Add(item.Bulstad ?? " ");
                        row.Add(item.Contagent);
                        row.Add(item.NSD.ToString(Vf.LevFormatUI));
                        row.Add(item.NSK.ToString(Vf.LevFormatUI));
                        row.Add(item.OD.ToString(Vf.LevFormatUI));
                        row.Add(item.OK.ToString(Vf.LevFormatUI));
                        row.Add(item.KSD.ToString(Vf.LevFormatUI));
                        row.Add(item.KSK.ToString(Vf.LevFormatUI));
                        result.Add(row);
                    }
                    var item1 = results.OrderBy(e => e.Num1).ThenBy(e => e.SubNum1).ThenBy(e => e.numContagent).Last();
                    if (rezi != null && rezi.Count > 0)
                    {
                        foreach (var it in rezi)
                        {
                            var itemp = new OboronaVed();
                            itemp.Id = item1.Id;
                            itemp.Num = item1.Num;
                            itemp.SubNum = item1.SubNum;
                            itemp.Name = item1.Name;
                            itemp.numContagent = int.Parse(it.Code);
                            var bul = activelookup.FirstOrDefault(e => e.ContainsKey("KONTRAGENT") && e["KONTRAGENT"].ToString() == itemp.numContagent.ToString());
                            if (bul != null)
                            {
                                if (bul.ContainsKey("BULSTAT")) itemp.Bulstad = bul["BULSTAT"].ToString();
                                if (bul.ContainsKey("Name")) itemp.Contagent = bul["Name"].ToString();
                            }
                            decimal d = 0;
                            decimal c = 0;
                            d = it.BeginSaldoDebit;
                            c = it.BeginSaldoCredit;
                            if (Math.Abs(itemp.NSD + d) > Math.Abs(itemp.NSK + c))
                            {
                                itemp.NSD = (itemp.NSD + d) - (itemp.NSK + c);
                                itemp.NSK = 0;
                            }
                            else
                            {
                                itemp.NSK = (itemp.NSK + c) - (itemp.NSD + d);
                                itemp.NSD = 0;
                            }
                            var sd = itemp.NSD + itemp.OD;
                            var sc = itemp.NSK + itemp.OK;
                            if (Math.Abs(sd) > Math.Abs(sc))
                            {
                                itemp.KSD = sd - sc;
                            }
                            else
                            {
                                itemp.KSK = sc - sd;
                            }
                            var row1 = new List<string>();
                            row1.Add(itemp.ToShortString());
                            row1.Add(itemp.Name);
                            row1.Add(itemp.numContagent.ToString());
                            row1.Add(itemp.Bulstad ?? " ");
                            row1.Add(itemp.Contagent);
                            row1.Add(itemp.NSD.ToString(Vf.LevFormatUI));
                            row1.Add(itemp.NSK.ToString(Vf.LevFormatUI));
                            row1.Add(itemp.OD.ToString(Vf.LevFormatUI));
                            row1.Add(itemp.OK.ToString(Vf.LevFormatUI));
                            row1.Add(itemp.KSD.ToString(Vf.LevFormatUI));
                            row1.Add(itemp.KSK.ToString(Vf.LevFormatUI));
                            result.Add(row1);
                        }
                    }
                }
                else
                {
                    if (accid == -1)
                    {
                        foreach (var currentacc in Allacc)
                        {
                            rezi = GetAllAnaliticSaldos(currentacc.Id, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                            rezi = new List<SaldoFactura>(from p in rezi
                                                          group p by new { p.Code } into gcs
                                                          select new SaldoFactura()
                                                          {
                                                              Code = gcs.Key.Code,
                                                              BeginSaldoCredit = gcs.Sum(x => x.BeginSaldoCredit),
                                                              BeginSaldoDebit = gcs.Sum(x => x.BeginSaldoDebit)
                                                          });
                            var ss = string.Format("SELECT a.ACCOUNTS_ID,a.LOOKUP_ID, a.FIELDLOOKUP_ID FROM MAPACCTOLOOKUP a where a.FIELDLOOKUP_ID = 28 and a.ACCOUNTS_ID = {0}", currentacc.Id);
                            var dbman1 = new DBManager(DataProvider.Firebird);
                            dbman1.ConnectionString = Entrence.ConnectionString;
                            dbman1.Open();
                            dbman1.ExecuteReader(CommandType.Text, ss);
                            lookupid = -1;
                            if (dbman1.DataReader.Read())
                            {
                                var ll = dbman1.DataReader["LOOKUP_ID"].ToString();
                                int lid = -1;
                                if (int.TryParse(ll, out lid))
                                {
                                    lookupid = lid;
                                }
                            }
                            dbman1.DataReader.Close();
                            dbman1.Dispose();
                            if (lookupid != -1)
                            {
                                lookupModel = GetLookup(lookupid);
                                activelookup = GetLookupDictionary(lookupModel.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                            }
                            foreach (var it in rezi)
                            {
                                var itemp = new OboronaVed();
                                itemp.Id = currentacc.Id;
                                itemp.Num = currentacc.Num.ToString();
                                itemp.SubNum = currentacc.SubNum.ToString();
                                itemp.Name = currentacc.NameMain;
                                itemp.numContagent = int.Parse(it.Code);
                                var bul = activelookup.FirstOrDefault(e => e.ContainsKey("KONTRAGENT") && e["KONTRAGENT"].ToString() == itemp.numContagent.ToString());
                                if (bul != null)
                                {
                                    if (bul.ContainsKey("BULSTAT")) itemp.Bulstad = bul["BULSTAT"].ToString();
                                    if (bul.ContainsKey("Name")) itemp.Contagent = bul["Name"].ToString();
                                }
                                decimal d = 0;
                                decimal c = 0;
                                d = it.BeginSaldoDebit;
                                c = it.BeginSaldoCredit;
                                if (Math.Abs(itemp.NSD + d) > Math.Abs(itemp.NSK + c))
                                {
                                    itemp.NSD = (itemp.NSD + d) - (itemp.NSK + c);
                                    itemp.NSK = 0;
                                }
                                else
                                {
                                    itemp.NSK = (itemp.NSK + c) - (itemp.NSD + d);
                                    itemp.NSD = 0;
                                }
                                var sd = itemp.NSD + itemp.OD;
                                var sc = itemp.NSK + itemp.OK;
                                if (Math.Abs(sd) > Math.Abs(sc))
                                {
                                    itemp.KSD = sd - sc;
                                }
                                else
                                {
                                    itemp.KSK = sc - sd;
                                }
                                var row1 = new List<string>();
                                row1.Add(itemp.ToShortString());
                                row1.Add(itemp.Name);
                                row1.Add(itemp.numContagent.ToString());
                                row1.Add(itemp.Bulstad ?? " ");
                                row1.Add(itemp.Contagent);
                                row1.Add(itemp.NSD.ToString(Vf.LevFormatUI));
                                row1.Add(itemp.NSK.ToString(Vf.LevFormatUI));
                                row1.Add(itemp.OD.ToString(Vf.LevFormatUI));
                                row1.Add(itemp.OK.ToString(Vf.LevFormatUI));
                                row1.Add(itemp.KSD.ToString(Vf.LevFormatUI));
                                row1.Add(itemp.KSK.ToString(Vf.LevFormatUI));
                                result.Add(row1);
                            }
                        }
                    }
                    else
                    {
                        var currentacc = Allacc.FirstOrDefault(e => e.Id == accid);
                        
                        if (currentacc != null)
                        {
                            var ss = string.Format("SELECT a.ACCOUNTS_ID,a.LOOKUP_ID, a.FIELDLOOKUP_ID FROM MAPACCTOLOOKUP a where a.FIELDLOOKUP_ID = 28 and a.ACCOUNTS_ID = {0}", currentacc.Id);
                            var dbman1 = new DBManager(DataProvider.Firebird);
                            dbman1.ConnectionString = Entrence.ConnectionString;
                            dbman1.Open();
                            dbman1.ExecuteReader(CommandType.Text, ss);
                            lookupid = -1;
                            if (dbman1.DataReader.Read())
                            {
                                var ll = dbman1.DataReader["LOOKUP_ID"].ToString();
                                int lid = -1;
                                if (int.TryParse(ll, out lid))
                                {
                                     lookupid= lid;   
                                }
                            }
                            dbman1.DataReader.Close();
                            dbman1.Dispose();
                            if (lookupid != -1)
                            {
                                lookupModel = GetLookup(lookupid);
                                activelookup = GetLookupDictionary(lookupModel.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                            }
                            rezi = GetAllAnaliticSaldos(currentacc.Id, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                            rezi = new List<SaldoFactura>(from p in rezi
                                                          group p by new { p.Code } into gcs
                                                          select new SaldoFactura()
                                                          {
                                                              Code = gcs.Key.Code,
                                                              BeginSaldoCredit = gcs.Sum(x => x.BeginSaldoCredit),
                                                              BeginSaldoDebit = gcs.Sum(x => x.BeginSaldoDebit)
                                                          });
                          
                            foreach (var it in rezi)
                            {
                                var itemp = new OboronaVed();
                                itemp.Id = currentacc.Id;
                                itemp.Num = currentacc.Num.ToString();
                                itemp.SubNum = currentacc.SubNum.ToString();
                                itemp.Name = currentacc.NameMain;
                                if (it.Code != null)
                                {
                                    itemp.numContagent = int.Parse(it.Code);
                                }
                                 var bul = activelookup.FirstOrDefault(e => e.ContainsKey("KONTRAGENT") && e["KONTRAGENT"].ToString() == itemp.numContagent.ToString());
                                if (bul != null)
                                {
                                    if (bul.ContainsKey("BULSTAT")) itemp.Bulstad = bul["BULSTAT"].ToString();
                                    if (bul.ContainsKey("Name")) itemp.Contagent = bul["Name"].ToString();
                                }
                                decimal d = 0;
                                decimal c = 0;
                                d = it.BeginSaldoDebit;
                                c = it.BeginSaldoCredit;
                                if (Math.Abs(itemp.NSD + d) > Math.Abs(itemp.NSK + c))
                                {
                                    itemp.NSD = (itemp.NSD + d) - (itemp.NSK + c);
                                    itemp.NSK = 0;
                                }
                                else
                                {
                                    itemp.NSK = (itemp.NSK + c) - (itemp.NSD + d);
                                    itemp.NSD = 0;
                                }
                                var sd = itemp.NSD + itemp.OD;
                                var sc = itemp.NSK + itemp.OK;
                                if (Math.Abs(sd) > Math.Abs(sc))
                                {
                                    itemp.KSD = sd - sc;
                                }
                                else
                                {
                                    itemp.KSK = sc - sd;
                                }
                                    var row1 = new List<string>();
                                    row1.Add(itemp.ToShortString());
                                    row1.Add(itemp.Name);
                                    row1.Add(itemp.numContagent.ToString());
                                    row1.Add(itemp.Bulstad ?? " ");
                                    row1.Add(itemp.Contagent);
                                    row1.Add(itemp.NSD.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.NSK.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.OD.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.OK.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.KSD.ToString(Vf.LevFormatUI));
                                    row1.Add(itemp.KSK.ToString(Vf.LevFormatUI));
                                    result.Add(row1);
                                
                            }
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetOborotnaVedDetail(DateTime ToDate, DateTime FromDate)");
            }

            finally
            {
                dbman.Dispose();
            }

            return result;
        }
        internal static OboronaVed GetOborotnaVedSaldo(DateTime FromDate, int accid)
        {
            var result = new OboronaVed();
            if (FromDate.Month==1)
            {
                return result;
            }
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                string s = string.Format(
                   "SELECT sum(a.\"Oborot\") as debit FROM \"conto\" a " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}' and a.\"DebitAccount\"={7}) ",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    FromDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year,
                    accid
                   );
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                if (dbman.DataReader.Read())
                {
                    result.NSD = decimal.Parse(dbman.DataReader["debit"].ToString());
                }
                dbman.CloseReader();
              s = string.Format(
                   "SELECT sum(a.\"Oborot\") as credit FROM \"conto\" a " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}' and a.\"CreditAccount\"={7}) ",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    FromDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year,
                    accid
                   );
                dbman.ExecuteReader(CommandType.Text, s);
                if (dbman.DataReader.Read())
                {
                    result.NSD = decimal.Parse(dbman.DataReader["debit"].ToString());
                }
                    
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetOborotnaVedSaldo(DateTime ToDate, DateTime FromDate)");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        private static void SetNomandFact(ref int nomer, ref string contr)
        {
            var ss = contr.Split('\n');
            if (ss != null && ss.Length > 0)
            {
                contr = ss[0];
                if (contr.Contains("Контрагент"))
                {
                    var sss = contr.Split(' ');
                    if (sss.Length > 2)
                    {
                        nomer = int.Parse(sss[2]);
                    }
                    contr = contr.Replace("Контрагент - " + sss[2], "");
                }
            }
        }

        internal static List<List<string>> GetOborotnaVedDetailedFactura(DateTime ToDate, DateTime FromDate)
        {
            var Allacc = new List<AccountsModel>(GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            List<List<string>> result = new List<List<string>>();
            List<OboronaVed> gruper = new List<OboronaVed>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                string s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.DDETAILS,a.\"Oborot\" as debit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}') " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year
                   );
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["DDETAILS"].ToString();
                    if (contr != null)
                    {
                        var ss = contr.Split('\n');
                        if (ss != null && ss.Length > 0)
                        {
                            contr = ss[0];
                            if (contr.Contains("Контрагент"))
                            {
                                var sss = contr.Split(' ');
                                if (sss.Length > 2)
                                {
                                    nomer = int.Parse(sss[2]);
                                }
                            }
                        }
                    }

                    var obor = new OboronaVed();
                    obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.OD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    obor.Contagent = contr;
                    obor.numContagent = nomer;
                    gruper.Add(obor);


                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.DDETAILS,a.\"Oborot\" as debit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<'{4}.{5}.{6}') " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    ToDate.Year,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    int id = int.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["DDETAILS"].ToString();
                    if (contr != null)
                    {
                        var ss = contr.Split('\n');
                        if (ss != null && ss.Length > 0)
                        {
                            contr = ss[0];
                            if (contr.Contains("Контрагент"))
                            {
                                var sss = contr.Split(' ');
                                if (sss.Length > 2)
                                {
                                    nomer = int.Parse(sss[2]);
                                }
                            }
                        }
                    }

                    var obor = new OboronaVed();
                    obor.Id = id;
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.NSD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    obor.Contagent = contr;
                    obor.numContagent = nomer;
                    gruper.Add(obor);

                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.CDETAILS,a.\"Oborot\" as credit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}')" +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["CDETAILS"].ToString();
                    if (contr != null)
                    {
                        var ss = contr.Split('\n');
                        if (ss != null && ss.Length > 0)
                        {
                            contr = ss[0];
                            if (contr.Contains("Контрагент"))
                            {
                                var sss = contr.Split(' ');
                                if (sss.Length > 2)
                                {
                                    nomer = int.Parse(sss[2]);
                                }
                            }
                        }
                    }

                    var obor = new OboronaVed();
                    obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.Contagent = contr;
                    obor.OK = decimal.Parse(dbman.DataReader["credit"].ToString());
                    obor.numContagent = nomer;
                    gruper.Add(obor);

                }
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",a.CDETAILS,a.\"Oborot\" as credit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\"" +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<'{4}.{5}.{6}')" +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    1,
                    1,
                    ToDate.Year,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    int nomer = 0;
                    int id = int.Parse(dbman.DataReader["Id"].ToString());
                    string contr = dbman.DataReader["CDETAILS"].ToString();
                    if (contr != null)
                    {
                        var ss = contr.Split('\n');
                        if (ss != null && ss.Length > 0)
                        {
                            contr = ss[0];
                            if (contr.Contains("Контрагент"))
                            {
                                var sss = contr.Split(' ');
                                if (sss.Length > 2)
                                {
                                    nomer = int.Parse(sss[2]);
                                }
                            }
                        }
                    }

                    var obor = new OboronaVed();
                    obor.Id = id;
                    obor.Num = dbman.DataReader["NUM"].ToString();
                    obor.SubNum = dbman.DataReader["SubNum"].ToString();
                    obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                    obor.NSK = decimal.Parse(dbman.DataReader["credit"].ToString());
                    obor.Contagent = contr;
                    obor.numContagent = nomer;
                    gruper.Add(obor);

                }

            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetOborotnaVedDetail(DateTime ToDate, DateTime FromDate)");
            }

            finally
            {
                dbman.Dispose();
            }

            var results = from p in gruper
                          group p by new { p.Num, p.SubNum, p.Name, p.numContagent } into gcs
                          select new OboronaVed()
                          {
                              Num = gcs.Key.Num,
                              SubNum = gcs.Key.SubNum,
                              Num1 = int.Parse(gcs.Key.Num),
                              SubNum1 = int.Parse(gcs.Key.SubNum),
                              Name = gcs.Key.Name,
                              Id = gcs.First().Id,
                              Contagent = gcs.First().Contagent,
                              numContagent = gcs.Key.numContagent,
                              NSD = gcs.Sum(x => x.NSD),
                              NSK = gcs.Sum(x => x.NSK),
                              OD = gcs.Sum(x => x.OD),
                              OK = gcs.Sum(x => x.OK),
                          };

            int olditemid = -1;
            List<SaldoFactura> rezi = new List<SaldoFactura>();
            foreach (var item in results.OrderBy(e => e.Num1).ThenBy(e => e.SubNum1).ThenBy(e => e.numContagent))
            {
                if (olditemid != item.Id)
                {
                    rezi = GetAllAnaliticSaldos(item.Id, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                    rezi = new List<SaldoFactura>(from p in rezi
                                                  group p by new { p.Code } into gcs
                                                  select new SaldoFactura()
                                                  {
                                                      Code = gcs.Key.Code,
                                                      BeginSaldoCredit = gcs.Sum(x => x.BeginSaldoCredit),
                                                      BeginSaldoDebit = gcs.Sum(x => x.BeginSaldoDebit)
                                                  });
                }
                var ns = rezi.FirstOrDefault(e => e.Code == item.numContagent.ToString());
                decimal nsd = 0;
                decimal nsc = 0;
                if (ns != null)
                {
                    nsd = ns.BeginSaldoDebit;
                    nsc = ns.BeginSaldoCredit;
                }
                if (Math.Abs(item.NSD + nsd) > Math.Abs(item.NSK + nsc))
                {
                    item.NSD = (item.NSD + nsd) - (item.NSK + nsc);
                    item.NSK = 0;
                }
                else
                {
                    item.NSK = (item.NSK + nsc) - (item.NSD + nsd);
                    item.NSD = 0;
                }
                var sdebit = item.NSD + item.OD;
                var scredit = item.NSK + item.OK;
                if (Math.Abs(sdebit) > Math.Abs(scredit))
                {
                    item.KSD = sdebit - scredit;
                }
                else
                {
                    item.KSK = scredit - sdebit;
                }
                //if (sm.TypeAccount == 1)
                //{
                //    item.NSD += sm.BeginSaldoL;
                //    item.KSD = item.NSD + item.OD - item.NSK-item.OK;
                //}
                //else
                //{
                //    item.NSK += sm.BeginSaldoL;
                //    item.KSD = item.NSK + item.OK - item.NSD-item.OD;
                //}

                var row = new List<string>();
                row.Add(item.ToShortString());
                row.Add(item.Name);
                row.Add(item.Contagent);
                row.Add(item.NSD.ToString(Vf.LevFormatUI));
                row.Add(item.NSK.ToString(Vf.LevFormatUI));
                row.Add(item.OD.ToString(Vf.LevFormatUI));
                row.Add(item.OK.ToString(Vf.LevFormatUI));
                row.Add(item.KSD.ToString(Vf.LevFormatUI));
                row.Add(item.KSK.ToString(Vf.LevFormatUI));
                result.Add(row);
            }
            return result;
        }
        private static List<AnaliticalFields> LoadAccFieldsMetaData(AccountsModel acc)
        {
            var AllAnaliticTypes = new ObservableCollection<AnaliticalAccountType>(GetAllAnaliticalAccountType());
            var AllAnaliticalAccount = new ObservableCollection<AnaliticalAccount>(GetAllAnaliticalAccount());
            var AllAnaliticalFields = new ObservableCollection<AnaliticalFields>(GetAllAnaliticalFields());
            var AllConnectors =
                new ObservableCollection<MapAnanaliticAccToAnaliticField>(GetAllConnectorAnaliticField());
            var AlaMapToType = new ObservableCollection<MapAnanaliticAccToAnaliticField>(GetAllConnectorTypeField());
            var SelectedAnaliticalFields = new ObservableCollection<AnaliticalFields>();
            var SelectedAnaliticalTypeFields = new ObservableCollection<AnaliticalFields>();

            var CurrentAllAnaliticalAccount = AllAnaliticalAccount.FirstOrDefault(e => e.Id == acc.AnaliticalNum);
            //AnaliticalAccountType CurrentAllTypeAccount = AllAnaliticTypes.FirstOrDefault(e => e.Id == CurrentAllAnaliticalAccount.Id);
            var SelectedConnectors =
                       new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                           AllConnectors.Where(e => CurrentAllAnaliticalAccount != null && e.AnaliticalNameID == CurrentAllAnaliticalAccount.Id));
            foreach (var curr in SelectedConnectors.OrderBy(e => e.SortOrder))
            {
                var addfield = AllAnaliticalFields.Where(e => e.Id == curr.AnaliticalFieldId).FirstOrDefault();
                if (addfield != null)
                {
                    addfield.Requared = curr.Required;
                    if (addfield != null) SelectedAnaliticalFields.Add(addfield);
                }
            }
            LoadMapToLookUps(SelectedAnaliticalFields, acc.Id, acc.AnaliticalNum);
            SelectedConnectors =
                new ObservableCollection<MapAnanaliticAccToAnaliticField>(
                    AlaMapToType.Where(e => e.AnaliticalFieldId == CurrentAllAnaliticalAccount.TypeID));
            foreach (var curr in SelectedConnectors)
            {
                var addfield = AllAnaliticalFields.FirstOrDefault(e => e.Id == curr.AnaliticalNameID);
                if (addfield != null)
                {
                    addfield.Requared = curr.Required;
                    SelectedAnaliticalTypeFields.Add(addfield);
                }
            }
            List<AnaliticalFields> sFieldses = new List<AnaliticalFields>();
            var CurrentAllTypeAccount = AllAnaliticTypes.FirstOrDefault(e => e.Id == CurrentAllAnaliticalAccount.TypeID);
            if (CurrentAllTypeAccount != null)
            {
                if (CurrentAllTypeAccount.Sl)
                {
                    sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Сума лв."));
                }
                if (CurrentAllTypeAccount.Sv)
                {
                    sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Сума валута"));
                }
                if (CurrentAllTypeAccount.Kol)
                {
                    sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Количество"));
                }

            }
            else
            {
                sFieldses.Add(AllAnaliticalFields.FirstOrDefault(f => f.Name == "Сума лв."));

            }
            foreach (var item in SelectedAnaliticalFields.OrderBy(e=>e.SortOrder))
            {
                sFieldses.Add(item);
            }
            return sFieldses;
        }
        public static void CopyAccFromYtoY(int firmaId, int fromYear, int toYear,bool et1,bool et2, bool et3, BackgroundWorker bw)
        {
            if (et1)
            {
                var dbman = new DBManager(DataProvider.Firebird);
                dbman.ConnectionString = Entrence.ConnectionString;
                try
                {

                    dbman.Open();
                    dbman.CreateParameters(3);
                    dbman.AddParameters(0, "@FY", fromYear);
                    dbman.AddParameters(1, "@TY", toYear);
                    dbman.AddParameters(2, "@FID", firmaId);
                    dbman.BeginTransaction();
                    dbman.ExecuteNonQuery(CommandType.StoredProcedure, "COPYACCSYTOY");
                    dbman.CommitTransaction();
                }
                catch (Exception ex)
                {
                    dbman.RollBackTransaction();
                    Logger.Instance().WriteLogError(ex.Message, "public static void CopyAccFromYtoY(int firmaId, int fromYear, int toYear)");
                }

                finally
                {
                    dbman.Dispose();
                }
            }
            bw.ReportProgress(33);
            var acc = new List<AccountsModel>(GetAllAccounts(firmaId, toYear));
            bw.ReportProgress(40);
            if (et2)
            {
                var list = GetOborotnaVed(new DateTime(fromYear, 1, 1), new DateTime(fromYear, 12, 31));
                bw.ReportProgress(55);
                foreach (var item in list)
                {
                    var accforsaldo = acc.FirstOrDefault(e => e.ShortName ==string.Format("{0} {1}",item[0],item[1]));
                    {
                        if (accforsaldo != null)
                        {
                            accforsaldo.SaldoDL = decimal.Parse(item[6]);
                            accforsaldo.SaldoKL = decimal.Parse(item[7]);
                            string mess;
                            UpdateAccount(accforsaldo, false, null, out mess, toYear);
                        }
                        else
                        {
                            var a = "nothing";
                        }
                    }
                }
            }
            bw.ReportProgress(60);
            if (et3)
            {
                var list2 = GetOborotnaVedDetailed(new DateTime(fromYear, 1, 1), new DateTime(fromYear, 12, 31));
                bw.ReportProgress(80);
                AccountsModel accforsaldo = null;
                List<AnaliticalFields> accfields = null;
                int gr = 0;
                StringBuilder sb = new StringBuilder();
                foreach (var item in list2)
                {
                    var a = decimal.Parse(item[9]);
                    var b = decimal.Parse(item[8]);
                    if (a == 0 && b== 0)
                    {
                        continue;
                    }
                    gr++;
                    if (accforsaldo != null && accforsaldo.ShortName == string.Format("{0} {1}", item[0], item[1]))
                    {
                        if (accfields != null)
                        {
                            int sortorder = 0;
                            foreach (var it in accfields)
                            {
                                SaldoAnaliticModel saldoAnaliticModel;
                                bool include;
                                gr = calc(accforsaldo, gr, item, sortorder, it, out saldoAnaliticModel, out include);
                                if (include)
                                {
                                    NewMethod(sb, saldoAnaliticModel);

                                    //SaveMovement(saldoAnaliticModel);
                                }
                                sortorder++;
                            }
                        }
                    }
                    else
                    {
                        gr = 1;
                        accforsaldo = acc.FirstOrDefault(e => e.ShortName == string.Format("{0} {1}", item[0], item[1]));
                        if (accforsaldo!=null)
                        {
                            accfields = LoadAccFieldsMetaData(accforsaldo);
                            if (accfields != null)
                            {
                                int sortorder = 0;
                                foreach (var it in accfields)
                                {
                                    SaldoAnaliticModel saldoAnaliticModel;
                                    bool include;
                                    gr = calc(accforsaldo, gr, item, sortorder, it, out saldoAnaliticModel, out include);
                                    if (include)
                                    {
                                        NewMethod(sb, saldoAnaliticModel);
                                        //SaveMovement(saldoAnaliticModel);
                                    }
                                    sortorder++;
                                }
                            }
                        }
                        
                    }
                    
                }
                bw.ReportProgress(95);
                FbBatchExecution(sb.ToString());
                bw.ReportProgress(98);
                var path = Path.Combine(Entrence.CurrentFirmaPathReport, "export" + DateTime.Now.ToString("ddMMyyyy") + ".txt");
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    sw.Write(sb.ToString());
                }
                Process.Start(path);
            }
            bw.ReportProgress(100);
        }

        private static int calc(AccountsModel accforsaldo, int gr, List<string> item, int sortorder, AnaliticalFields it, out SaldoAnaliticModel saldoAnaliticModel, out bool include)
        {
            saldoAnaliticModel = new SaldoAnaliticModel();
            saldoAnaliticModel.ACCID = accforsaldo.Id;
            saldoAnaliticModel.ACCFIELDKEY = it.Id;
            saldoAnaliticModel.LOOKUPFIELDKEY = 0;
            saldoAnaliticModel.VAL = "";
            saldoAnaliticModel.VALUEDATE = DateTime.Now;
            saldoAnaliticModel.VALUEMONEY = 0;
            saldoAnaliticModel.VALUENUM = 0;
            saldoAnaliticModel.TYPEACCKEY = accforsaldo.LevelAccount;
            saldoAnaliticModel.VALUED = 0;
            saldoAnaliticModel.LOOKUPID = it.RCODELOOKUP;
            saldoAnaliticModel.VALKOLK = 0;
            saldoAnaliticModel.VALKOLD = 0;
            saldoAnaliticModel.VALVALK = 0;
            saldoAnaliticModel.VALVALD = 0;
            saldoAnaliticModel.GROUP = gr;
            saldoAnaliticModel.SORTORDER = sortorder;
            if (it.Name == "Контрагент")
            {
                saldoAnaliticModel.VALS = item[4];
                saldoAnaliticModel.VAL = item[2];
            }
            if (it.Name.Contains("Дата"))
            {
                saldoAnaliticModel.VAL = DateTime.Now.ToShortDateString();
            }
            if (it.Name==("Номер фактура"))
            {
                saldoAnaliticModel.VAL = "0";
            }
            include = true;
            if (it.Name.Contains("Сума лв."))
            {
                saldoAnaliticModel.VALUED = decimal.Parse(item[9]);
                saldoAnaliticModel.VALUEMONEY = decimal.Parse(item[10]);
                
            }

            return gr;
        }

        private static void NewMethod(StringBuilder sb, SaldoAnaliticModel saldoAnaliticModel)
        {
            var client = saldoAnaliticModel.VALS!=null ? saldoAnaliticModel.VALS.Replace("'", "''"):" ";
            sb.Append("INSERT INTO MOVEMENT(ACCID,ACCFIELDKEY,LOOKUPFIELDKEY,\"VALUE\",");
            sb.Append("VALUEDATE,VALUEMONEY,VALUENUM,TYPEACCKEY,VALUED,\"group\",LOOKUPID,VALKOLK,VALKOLD,VALVALD,VALVALK,");
            sb.Append("VALS,SORTORDER) VALUES(");
            sb.AppendFormat("'{0}',", saldoAnaliticModel.ACCID);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.ACCFIELDKEY);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.LOOKUPFIELDKEY);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VAL);
            sb.AppendFormat("'{0}.{1}.{2}',", saldoAnaliticModel.VALUEDATE.Day,saldoAnaliticModel.VALUEDATE.Month,saldoAnaliticModel.VALUEDATE.Year);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALUEMONEY.ToString("0.00").Replace(',', '.'));
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALUENUM);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.TYPEACCKEY);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALUED.ToString("0.00").Replace(',','.'));
            sb.AppendFormat("'{0}',", saldoAnaliticModel.GROUP);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.LOOKUPID);
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALKOLK.ToString("0.00").Replace(',', '.'));
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALKOLD.ToString("0.00").Replace(',', '.'));
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALVALD.ToString("0.00").Replace(',', '.'));
            sb.AppendFormat("'{0}',", saldoAnaliticModel.VALVALK.ToString("0.00").Replace(',', '.'));
            sb.AppendFormat("'{0}',", client);
            sb.AppendFormat("'{0}'", saldoAnaliticModel.SORTORDER);
            sb.Append(");"); sb.AppendLine();

        }

        public static IEnumerable<InvoiseControl> GetFullInvoiseContoDebit(int AccID,bool withoutsuma=false)
        {
            Dictionary<int, Dictionary<int, string>> nomen = new Dictionary<int, Dictionary<int, string>>();
            List<InvoiseControl> result = new List<InvoiseControl>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.FOLDER,c.\"Reason\",c.DOCNUM,c.\"Date\" as DD,m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,m.LOOKUPVAL FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and c.\"DebitAccount\"={2} and m.\"TYPE\"=1 and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, AccID);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int oldid = 0;
                bool first = true;

                while (dbman.DataReader.Read())
                {
                    var ic = new InvoiseControl();
                    int newid = int.Parse(dbman.DataReader["Id"].ToString());
                    ic.Id = dbman.DataReader["Id"].ToString();
                    ic.LOOKUPFIELDKEY = int.Parse(dbman.DataReader["LOOKUPFIELDKEY"].ToString());
                    ic.LOOKUPID = int.Parse(dbman.DataReader["LOOKUPID"].ToString());
                    
                    ic.NameField = dbman.DataReader["NAME"].ToString();
                    ic.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                    ic.VALUE = dbman.DataReader["VALUE"].ToString();
                    var dd = DateTime.Parse(dbman.DataReader["DD"].ToString());
                    ic.DataInvoise = dd;
                    ic.Folder=dbman.DataReader["Folder"].ToString();
                    ic.DocNumber = dbman.DataReader["DOCNUM"].ToString();
                    ic.Reason=dbman.DataReader["Reason"].ToString();
                    if (ic.NameField == "Номер фактура")
                    {
                        ic.NInvoise = ic.VALUE;
                    }
                    if (ic.NameField == "Контрагент")
                    {
                        ic.CID = ic.LOOKUPID;
                        ic.CKEY = ic.LOOKUPFIELDKEY;
                        ic.CodeContragent = ic.VALUE;
                        ic.NameContragent = dbman.DataReader["LOOKUPVAL"].ToString();
                    }
                    result.Add(ic);
                }

            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<InvoiseControl> GetFullInvoiseContoDebit(int AccID)");
            }

            finally
            {
                dbman.Dispose();
            }
            bool f1 = true;
            Dictionary<int, string> list = new Dictionary<int, string>();
            List<InvoiseControl> result2 = new List<InvoiseControl>();
            List<InvoiseControl> result3 = new List<InvoiseControl>();
            int old = 0;
            InvoiseControl ici = new InvoiseControl();
            foreach (var ic1 in result)
            {

                var newid = int.Parse(ic1.Id);
                if (f1)
                {
                    old = newid;
                    ici = ic1.Clone();
                    f1 = false;
                }
                if (old != newid)
                {
                    result2.Add(ici.Clone());
                    ici = ic1.Clone();
                    old = newid;
                }
                if (ic1.NInvoise != null)
                {
                    ici.NInvoise = ic1.NInvoise;
                }
               
                    if (ic1.NameField != "Номер фактура")
                    {
                        ici.Details = string.Format("{0}|{1}-{2}", ici.Details, ic1.NameField, ic1.VALUE);
                    }
                    if (ic1.NameField == "Дата на фактура")
                    {
                        ici.DataInvoise = ic1.DataInvoise;
                    }
                    if (ic1.NameField == "Контрагент")
                    {
                        ici.CodeContragent = ic1.CodeContragent;
                    }
                

            }
            result2.Add(ici);
            if (withoutsuma)
            {
                return result2;
            }
            var query = (from t in result2
                         group t by new { t.NInvoise, t.CodeContragent }
                into grp
                         select new InvoiseControl
                         {
                             NInvoise = grp.Key.NInvoise,
                             NameContragent = grp.Last().NameContragent,
                             CodeContragent = grp.Key.CodeContragent,
                             Oborot = grp.Sum(t => t.Oborot),
                             DataInvoise = grp.Max(t => t.DataInvoise),
                             Reason = grp.First().Reason,
                             Folder = grp.First().Folder,
                             DocNumber = grp.First().DocNumber
                         });
            //foreach (var VARIABLE in query)
            //{
            //    result3.Add(new InvoiseControl
            //    {
            //        DataInvoise = VARIABLE.Data,
            //        NInvoise = VARIABLE.NInvoise,
            //        NameContragent = VARIABLE.NameContragent,
            //        Oborot = VARIABLE.Quantity,
            //        CodeContragent = VARIABLE.CodeContragent
            //    });
            //}
            return new List<InvoiseControl>(query);
           
        }

        private static string Fi(LookupModel lm)
        {
            StringBuilder fields = new StringBuilder();
            foreach (var fild in lm.Fields)
            {
                fields.Append("\"");
                fields.Append(fild.NameEng);
                fields.Append("\"");
                fields.Append(",");
            }
            string fi = fields.ToString();
            fi = fi.Remove(fi.Length - 1, 1) + " ";
            return fi;
        }

        public static IEnumerable<ValutaControl> GetFullValutaDebit(int AccID)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            var rez = new List<ValutaControl>();
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\" as DD,m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and c.\"CreditAccount\"={2} and m.\"TYPE\"=2 and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, AccID);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int oldid = 0;
                bool first = true;

                while (dbman.DataReader.Read())
                {
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<ValutaControl> GetFullValutaDebit(int AccID)");
            }

            finally
            {
                dbman.Dispose();
            }
            return rez;
        }

        public static IEnumerable<InvoiseControl> GetFullInvoiseContoCredit(int AccID,bool withoutsuma=false)
        {
            Dictionary<int, Dictionary<int, string>> nomen = new Dictionary<int, Dictionary<int, string>>();
            List<InvoiseControl> result = new List<InvoiseControl>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\" as DD,c.FOLDER,c.\"Reason\",c.DOCNUM,m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,m.LOOKUPVAL FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and c.\"CreditAccount\"={2} and m.\"TYPE\"=2 and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, AccID);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int oldid = 0;
                bool first = true;

                while (dbman.DataReader.Read())
                {
                    var ic = new InvoiseControl();
                    int newid = int.Parse(dbman.DataReader["Id"].ToString());
                    ic.Id = dbman.DataReader["Id"].ToString();
                    ic.LOOKUPFIELDKEY = int.Parse(dbman.DataReader["LOOKUPFIELDKEY"].ToString());
                    ic.LOOKUPID = int.Parse(dbman.DataReader["LOOKUPID"].ToString());
                    ic.NameField = dbman.DataReader["NAME"].ToString();
                    ic.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                    ic.VALUE = dbman.DataReader["VALUE"].ToString();
                    ic.DataInvoise = DateTime.Parse(dbman.DataReader["DD"].ToString());
                    ic.Folder = dbman.DataReader["Folder"].ToString();
                    ic.DocNumber = dbman.DataReader["DOCNUM"].ToString();
                    ic.Reason = dbman.DataReader["Reason"].ToString();
                    if (ic.NameField == "Номер фактура")
                    {
                        ic.NInvoise = ic.VALUE;
                    }
                    if (ic.NameField == "Контрагент")
                    {
                        ic.CID = ic.LOOKUPID;
                        ic.CKEY = ic.LOOKUPFIELDKEY;
                        ic.CodeContragent = ic.VALUE;
                        ic.NameContragent = dbman.DataReader["LOOKUPVAL"].ToString();
                    }
                    result.Add(ic);
                }

            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<InvoiseControl> GetFullInvoiseContoCredit(int AccID)");
            }

            finally
            {
                dbman.Dispose();
            }
            bool f = true;
            bool f1 = true;
            Dictionary<int, string> list = new Dictionary<int, string>();
            List<InvoiseControl> result2 = new List<InvoiseControl>();
            List<InvoiseControl> result3 = new List<InvoiseControl>();
            int old = 0;
            InvoiseControl ici = new InvoiseControl();
            foreach (var ic1 in result)
            {

                var newid = int.Parse(ic1.Id);
                if (f1)
                {
                    old = newid;
                    ici = ic1.Clone();
                    f1 = false;
                }
                if (old != newid)
                {
                    result2.Add(ici.Clone());
                    ici = ic1.Clone();
                    old = newid;
                }
                if (ic1.NInvoise != null)
                {
                    ici.NInvoise = ic1.NInvoise;
                }
               
                    if (ic1.NameField != "Номер фактура")
                    {
                        ici.Details = string.Format("{0}|{1}-{2}", ici.Details, ic1.NameField, ic1.VALUE);
                    }
                    if (ic1.NameField == "Дата на фактура")
                    {
                        ici.DataInvoise = ic1.DataInvoise;
                    }
                    if (ic1.NameField == "Контрагент")
                    {
                        ici.CodeContragent = ic1.CodeContragent;
                        ici.NameContragent = ic1.NameContragent;
                    }
                

            }
            result2.Add(ici);
            if (withoutsuma)
            {
                return result2;
            }
            var query = (from t in result2
                         group t by new { t.NInvoise, t.CodeContragent }
                into grp
                         select new InvoiseControl
                         {
                             NInvoise = grp.Key.NInvoise,
                             NameContragent = grp.Last().NameContragent,
                             CodeContragent = grp.Key.CodeContragent,
                             Oborot = grp.Sum(t => t.Oborot),
                             DataInvoise = grp.Max(t => t.DataInvoise),
                             Reason = grp.First().Reason,
                             Folder=grp.First().Folder,
                             DocNumber=grp.First().DocNumber
                     });
            //foreach (var VARIABLE in query)
            //{
            //    result3.Add(new InvoiseControl
            //    {
            //        DataInvoise = VARIABLE.Data,
            //        NInvoise = VARIABLE.NInvoise,
            //        NameContragent = VARIABLE.NameContragent,
            //        Oborot = VARIABLE.Quantity,
            //        CodeContragent = VARIABLE.CodeContragent
            //    });
            //}
            return new List<InvoiseControl>(query);
        }

        internal static AccSaldo GetSaldoAcc(int accId)
        {
            AccSaldo result = new AccSaldo();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {

                dbman.Open();
                dbman.CreateParameters(10);
                dbman.AddParameters(0, "@DateFrom",
                    new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Month, 1));
                dbman.AddParameters(1, "@DateTo",
                    new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Month, 1).AddMonths(1));
                dbman.AddParameters(2, "@FirmId", ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                dbman.AddParameters(3, "@AccId", accId);
                dbman.AddOutputParameters(4, "@NSD", result.Bsd);
                dbman.AddOutputParameters(5, "@OBD", result.Od);
                dbman.AddOutputParameters(6, "@KSD", result.Ksd);
                dbman.AddOutputParameters(7, "@NSK", result.Bsc);
                dbman.AddOutputParameters(8, "@OBK", result.Oc);
                dbman.AddOutputParameters(9, "@KSK", result.Ksc);
                dbman.BeginTransaction();
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "GETOBOROTKA");
                result.Bsd = (decimal) dbman.Parameters[4].Value;
                result.Od = (decimal) dbman.Parameters[5].Value;
                result.Ksd = (decimal) dbman.Parameters[6].Value;
                result.Bsc = (decimal) dbman.Parameters[7].Value;
                result.Oc = (decimal) dbman.Parameters[8].Value;
                result.Ksc = (decimal) dbman.Parameters[9].Value;
                dbman.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "internal static AccSaldo GetSaldoAcc(int accId)");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        internal static bool CheckLookup(List<string> list, LookupModel lookup)
        {
            bool result = true;
            int i = 0;
            bool isUnik = false;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Select * from \"{0}\" where FIRMAID={1} and ", lookup.LookUpMetaData.Tablename,
                ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
            foreach (var field in lookup.Fields)
            {
                if (field.IsUnique)
                {
                    if (field.DbField.ToLower().Contains("char"))
                    {
                        sb.AppendFormat(" \"{0}\"='{1}' or", field.NameEng, list[i]);
                    }
                    else
                    {
                        sb.AppendFormat(" \"{0}\"={1} or", field.NameEng, list[i]);
                    }
                    isUnik = true;
                }
                i++;
            }
            sb.Remove(sb.Length - 2, 2);
            if (!isUnik) return true;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                var reader = dbman.ExecuteReader(CommandType.Text, sb.ToString());
                if (reader.Read())
                {
                    result = false;
                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static bool CheckLookup(List<string> list, LookupModel lookup)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        internal static string GetSettings(string key)
        {
            string result = "0.00";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Select * from SETINGS where NAME='{0}' and FIRMAID={1}", key,
                ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, sb.ToString());
                if (dbman.DataReader.Read())
                {
                    result = dbman.DataReader["SVALUE"].ToString();

                }
                
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, " internal static string GetSettings(string key)");

            }

            finally
            {
                dbman.Dispose();
            }
            return result;

        }

        internal static void SaveNuberFormats()
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {

                dbman.Open();
                dbman.CreateParameters(3);

                dbman.BeginTransaction();
                dbman.AddParameters(0, "@NAME", "LV");
                dbman.AddParameters(1, "@VAL", Vf.LevFormatUI);
                dbman.AddParameters(2, "@FIRMAID", ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);

                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "MANAGESETTINGS");
                dbman.Parameters[0].Value = "KURS";
                dbman.Parameters[1].Value = Vf.KursFormatUI;
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "MANAGESETTINGS");
                dbman.Parameters[0].Value = "VAL";
                dbman.Parameters[1].Value = Vf.ValFormatUI;
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "MANAGESETTINGS");
                dbman.Parameters[0].Value = "KOL";
                dbman.Parameters[1].Value = Vf.KolFormatUI;
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "MANAGESETTINGS");
                dbman.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "internal static void SaveNuberFormats()");
            }

            finally
            {
                dbman.Dispose();
            }
        }

        public static void SaveKurs(DateTime Data, string Code, decimal Kurs)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {

                dbman.Open();
                dbman.CreateParameters(3);
                dbman.BeginTransaction();
                dbman.AddParameters(0, "@DATA", Data);
                dbman.AddParameters(1, "@CODE", Code);
                dbman.AddParameters(2, "@KURS", Kurs);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "MANAGEKURS");
                dbman.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "public static void SaveKurs(DateTime Data, string Code, decimal Kurs)");
            }

            finally
            {
                dbman.Dispose();
            }
        }

        public static IEnumerable<ValutaEntity> GetCurRates(string codevaluta, DateTime fromDate, DateTime toDate)
        {

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            dbman.Open();
            dbman.CreateParameters(3);
            dbman.ExecuteReader(CommandType.Text,
                string.Format(
                    "Select * from VK v where v.VIDVALUTA='{0}' AND v.DATA>='{1}' AND v.DATA<='{2}' order by v.DATA",
                    codevaluta
                    , string.Format("{0}.{1}.{2}", fromDate.Day, fromDate.Month, fromDate.Year),
                    string.Format("{0}.{1}.{2}", toDate.Day, toDate.Month, toDate.Year)));
            while (dbman.DataReader.Read())
            {
                yield return new ValutaEntity
                {
                    CodeVal = dbman.DataReader["VIDVALUTA"].ToString(),
                    Date = DateTime.Parse(dbman.DataReader["DATA"].ToString()),
                    State = 0,
                    Value = decimal.Parse(dbman.DataReader["KURS"].ToString())
                };

            }
            dbman.Dispose();
        }

        internal static void DeleteKurs(List<ValutaEntity> itemsfordelete)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {

                dbman.Open();
                dbman.CreateParameters(2);
                dbman.BeginTransaction();
                dbman.AddParameters(0, "@DATA", null);
                dbman.AddParameters(1, "@CODE", null);
                foreach (var valutaEntity in itemsfordelete)
                {
                    dbman.Parameters[0].Value = valutaEntity.Date;
                    dbman.Parameters[1].Value = valutaEntity.CodeVal;
                    dbman.ExecuteNonQuery(CommandType.StoredProcedure, "DELETEKURS");
                }
                dbman.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "internal static void DeleteKurs(List<ValutaEntity> itemsfordelete)");
            }

            finally
            {
                dbman.Dispose();
            }
        }

        internal static decimal? LoadKursForaDay(DateTime fromDate, string codevaluta)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            dbman.Open();
            dbman.CreateParameters(3);
            dbman.ExecuteReader(CommandType.Text,
                string.Format("Select * from VK v where v.VIDVALUTA='{0}' AND v.DATA='{1}'",
                    codevaluta
                    ,
                    string.Format("{0}.{1}.{2}", fromDate.Day, fromDate.Month,
                        fromDate.Year)));
            if (dbman.DataReader.Read())
            {
                return decimal.Parse(dbman.DataReader["KURS"].ToString());
            }
            dbman.Dispose();
            return null;
        }

        internal static bool DeleteAllConto(int firmaId)
        {
            bool rez = true;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(1);
                dbman.AddParameters(0, "@FIRMAID", firmaId);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "CLEARFIRMA");
                dbman.CommitTransaction();
            }
            catch (Exception ex)
            {
                rez = false;
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "internal static void DeleteAllConto(int firmaId)");
            }
            finally
            {
                dbman.Dispose();
            }
            return rez;
        }

        internal static List<ViesRow> GetVies(int month, int year, Dictionary<string, string> declar)
        {
            List<ViesRow> list = new List<ViesRow>();
            var rez = GetDnevItemVies(2, new DateTime(year, month, 1), new DateTime(year, month, GetEndDate(month, year)));
         int k = 1;
           decimal sumak3 = 0, sumak1k2k3 = 0;
            foreach (List<string> list1 in rez)
            {
                
                if ((list1[20] != Vf.LevFormatUI || list1[25] != Vf.LevFormatUI || list1[22] != Vf.LevFormatUI))
                {
                    var lis = new ViesRow();
                    lis.PorNom=k; //k1
                    lis.Name=list1[5]; //k2
                   
                    //lis.Add(string.Format("{0:D2}/{1}", month, year));
                    k++;
                    list.Add(lis);
                    decimal val1;
                    decimal val2;
                    decimal val3;
                    if (decimal.TryParse(list1[20], out val1) && decimal.TryParse(list1[25], out val2) &&
                        decimal.TryParse(list1[22], out val3))
                    {
                        sumak1k2k3 += val1 + val2 + val3;
                        lis.K3 = val1;
                        lis.K4 = val2;
                        lis.K5 = val3;
                    }
                    if (decimal.TryParse(list1[20], out val1))
                    {
                        lis.K3 = val1;
                        sumak3 += val1;
                    }
                }
               
            }
           
            declar.Add("sumak3", sumak3.ToString());
            declar.Add("sumak1k2k3", sumak1k2k3.ToString());
            var results = list.GroupBy(e => e.Name).Select(g => new ViesRow
            {
                Name=g.Key,
                K3 = g.Sum(p => p.K3),
                K4 = g.Sum(p => p.K4),
                K5 = g.Sum(p => p.K5),
            });
            List<ViesRow> r=new List<ViesRow>();
            int k1=1;
            foreach (ViesRow row in results)
            {
                r.Add(new ViesRow {K3 = row.K3, K4 = row.K4, K5 = row.K5, Name = row.Name, PorNom = k1});
                k1++;
            }
            declar.Add("count", (k1 - 1).ToString());
            return r;
        }

        internal static int GetAllContoCount(int id, int year, int month)
        {
            int c = 0;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                c = (int) dbman.ExecuteScalar(CommandType.Text,
                    String.Format(
                        "select count(*) from \"conto\" where \"FirmId\"={0} and \"Date\">='1.{1}.{2}' and \"Date\"<='{3}.{1}.{2}'",
                        id, month, year, GetEndDate(month, year)));
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static int GetAllContoCount(int id, int year, int month)");

            }
            finally
            {
                dbman.Dispose();
            }
            return c;
        }

        internal static int GetAllContoCount(int firmaId, ISearchAcc pSearcAcc)
        {
            int c = 0;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("select count(*) from \"conto\" c");
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
                    sb.AppendFormat(" AND DOCNUM LIKE '%{0}%'", pSearcAcc.NumDoc);
                if (pSearcAcc.CreditAcc != null)
                {
                    if (pSearcAcc.CreditAcc.Num > 0)
                        sb.AppendFormat(" AND a.NUM={0}", pSearcAcc.CreditAcc.Num);
                    if (pSearcAcc.CreditAcc.SubNum > -1)
                        sb.AppendFormat(" AND a.\"SubNum\"={0}", pSearcAcc.CreditAcc.SubNum);
                }
                if (pSearcAcc.DebitAcc != null)
                {
                    if (pSearcAcc.DebitAcc.Num > 0)
                        sb.AppendFormat(" AND b.NUM={0}", pSearcAcc.DebitAcc.Num);
                    if (pSearcAcc.DebitAcc.SubNum > -1)
                        sb.AppendFormat(" AND b.\"SubNum\"={0}", pSearcAcc.DebitAcc.SubNum);
                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Note))
                    sb.AppendFormat(" AND UPPER(c.\"Note\") LIKE '%{0}%'", pSearcAcc.Note.ToUpper());
                if (!String.IsNullOrWhiteSpace(pSearcAcc.Reason))
                    sb.AppendFormat(" AND UPPER(c.\"Reason\") LIKE '%{0}%'", pSearcAcc.Reason.ToUpper());
                if (pSearcAcc.CreditItems != null)
                {
                    foreach (var item in pSearcAcc.CreditItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"CDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (pSearcAcc.DebitItems != null)
                {
                    foreach (var item in pSearcAcc.DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"DDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
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
                string s = sb.ToString();
                c = (int)dbman.ExecuteScalar(CommandType.Text, s);
                //LoadConto(allConto);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetAllConto(int p, Interface.ISearchAcc pSearcAcc)");
            }

            finally
            {
                dbman.Dispose();
            }
            return c;
        }

        

        public static IEnumerable<AccItemSaldo> GetInfoFactura(int AccID, int acctype, string contragent)
        {
            Dictionary<int, Dictionary<int, string>> nomen = new Dictionary<int, Dictionary<int, string>>();
            List<AccItemSaldo> result = new List<AccItemSaldo>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\",m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,c.\"DebitAccount\",m.LOOKUPVAL FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and (c.\"DebitAccount\"={2} or c.\"CreditAccount\"={2}) and m.ACCID={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, AccID);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int oldid = 0;
                bool first = true;
                var ic = new AccItemSaldo();
                ic.Type = acctype;
                bool load = true;
                while (dbman.DataReader.Read())
                {

                    string value = dbman.DataReader["VALUE"].ToString();
                    string field = dbman.DataReader["Name"].ToString();
                    int newid = int.Parse(dbman.DataReader["Id"].ToString());
                    if (first)
                    {
                        first = false;
                        oldid = newid;
                    }

                    if (oldid != newid)
                    {
                        if (ic.NameContragent == contragent)
                        {

                            result.Add(ic.Clone());
                        }

                        ic = new AccItemSaldo {Type = acctype};
                        oldid = newid;
                        load = true;
                    }



                    if (field == "Номер фактура")
                    {
                        ic.NInvoise = value;
                    }
                    if (field == "Контрагент")
                    {
                        ic.NameContragent = dbman.DataReader["LOOKUPVAL"].ToString();

                    }
                    if (field == "Дата на фактура")
                    {
                        ic.Data=DateTime.Parse(value);
                    }

                    if (load)
                    {
                        int smetka;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == AccID)
                            {
                                ic.IsDebit = true;
                                ic.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                            else
                            {
                                ic.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                        }
                        load = false;
                    }
                }
                if (ic.NameContragent == contragent) result.Add(ic);

            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<AccItemSaldo> GetInfoFactura(int AccID, int acctype, string contragent)");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        public static List<List<string>> GetDnevItem(int kindActivity, DateTime fromDate, DateTime toDate)
        {
            List<List<string>> result = new List<List<string>>();
            Purchases pocupki = new Purchases();
            Sells prodazbi = new Sells();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                if (kindActivity == 2)
                {
                    var str = string.Format("select d.BRANCH,d.BULSTAD,d.NZDDS,d.DATADOC,d.DATAF,d.DOCN," +
                                            "d.NOM,d.NAMEKONTR,d.SUMA,d.DDSSUMA as DDSTOTAL,d.STOKE,d.CODEDOC," +
                                            "c.\"FirmId\" as FID,c.\"Id\" as TR,n.\"Name\" as NAMEDOC,dt.SUMADDS,dt.DDS as SUMAWITHDDS,ds.NAME," +
                                            "ds.DDSPERCENT" +
                                            ",ds.CODE,d.KINDACTIVITY,d.A8 as AA,c.PORNOM,c.FOLDER as FOL,c.\"NumberObject\" as OB,EXTRACT(MONTH FROM d.DATADOC) as MON " +
                                            " from  DDSDNEVFIELDS ds" +
                                            " inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID" +
                                            " inner join DDSDNEV d on d.ID=dt.IDDDSDNEV" +
                                            " inner join \"conto\" c on c.\"Id\"=d.NOM" +
                                            " inner join \"nomenclatures\" n on n.\"Id\"=d.KINDDOC" +
                                            " where c.\"FirmId\"={0}" +
                                            " and d.KINDACTIVITY={1}" +
                                            " and d.DATADOC >= '{2}'" +
                                            " and d.DATADOC <= '{3}'" +
                                            " order by MON,c.PORNOM",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        kindActivity,
                        string.Format("{0}.{1}.{2}", fromDate.Day, fromDate.Month, fromDate.Year),
                        string.Format("{0}.{1}.{2}", toDate.Day, toDate.Month, toDate.Year));
                    dbman.ExecuteReader(CommandType.Text, str);
                }
                else
                {
                    var str = string.Format(" select d.BRANCH,d.BULSTAD,d.NZDDS,d.DATADOC,d.DATAF,d.DOCN," +
                                            "d.NOM,d.NAMEKONTR,d.SUMA,d.DDSSUMA as DDSTOTAL,d.STOKE,d.CODEDOC," +
                                            "c.\"FirmId\" as FID,c.\"Id\" as TR,n.\"Name\" as NAMEDOC,dt.SUMADDS,dt.DDS as SUMAWITHDDS,ds.NAME," +
                                            "ds.DDSPERCENT" +
                                            ",ds.CODE,d.KINDACTIVITY,d.A8 as AA,c.PORNOM,c.FOLDER as FOL,c.\"NumberObject\" as OB,EXTRACT(MONTH FROM d.DATADOC) as MON" +
                                            " from  DDSDNEVSELLSFIELDS ds" +
                                            " inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID" +
                                            " inner join DDSDNEV d on d.ID=dt.IDDDSDNEV" +
                                            " inner join \"conto\" c on c.\"Id\"=d.NOM" +
                                            " inner join \"nomenclatures\" n on n.\"Id\"=d.KINDDOC" +
                                            " where c.\"FirmId\"={0}" +
                                            " and d.KINDACTIVITY={1}" +
                                            " and d.DATADOC >= '{2}'" +
                                            " and d.DATADOC <= '{3}'" +
                                            " order by MON,c.PORNOM",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        kindActivity,
                        string.Format("{0}.{1}.{2}", fromDate.Day, fromDate.Month, fromDate.Year),
                        string.Format("{0}.{1}.{2}", toDate.Day, toDate.Month, toDate.Year));
                    dbman.ExecuteReader(CommandType.Text, str);
                }
                string tester = "";
                List<string> row = null;
                int i = 1;
                string ssuma = "", ssumdds = "";
                decimal suma = 0, gumdds = 0;
                decimal gsuma = 0;
                string codedoc = "";
                while (dbman.DataReader.Read())
                {
                    string newtester = dbman.DataReader["NOM"].ToString();
                    if (!newtester.Equals(tester))
                    {
                        tester = newtester;
                        if (row != null)
                        {
                            if (kindActivity == 2)
                            {
                                row[9] = gsuma.ToString(Vf.LevFormatUI);
                                row[10] = gumdds.ToString(Vf.LevFormatUI);
                            }
                            result.Add(new List<string>(row));
                            gsuma = 0;
                            gumdds = 0;
                        }
                        row = new List<string>();
                        row.Add(i.ToString(CultureInfo.InvariantCulture));
                        i++;
                        row.Add(dbman.DataReader["BRANCH"].ToString());
                        codedoc = dbman.DataReader["CODEDOC"].ToString();
                        row.Add(codedoc);
                        string dn = add10o(dbman.DataReader["DOCN"].ToString());
                        row.Add(dn);
                        DateTime d = DateTime.Parse(dbman.DataReader["DATAF"].ToString());
                        row.Add(string.Format("{0:D2}/{1:D2}/{2:D4}", d.Day, d.Month, d.Year));
                        row.Add(dbman.DataReader["NZDDS"].ToString());
                        row.Add(dbman.DataReader["NAMEKONTR"].ToString());
                        row.Add(dbman.DataReader["STOKE"].ToString());
                        string a8 = dbman.DataReader["AA"].ToString();
                        string pornom = dbman.DataReader["TR"].ToString();
                        string folder= dbman.DataReader["FOL"].ToString();
                        string ob= dbman.DataReader["OB"].ToString();
                        row.Add(a8);
                        if (kindActivity == 2)
                        {
                            row.Add(ssuma);
                            row.Add(ssumdds);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(pornom);
                            row.Add(folder);
                            row.Add(ob);
                        }
                        else
                        {
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(pornom);
                            row.Add(folder);
                            row.Add(ob);
                        }
                        suma = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());

                        decimal sumdds = decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                        decimal prddc = decimal.Parse(dbman.DataReader["DDSPERCENT"].ToString());
                        string code = dbman.DataReader["CODE"].ToString();
                        //Sells.Kol12 + Sells.Kol15 + Sells.Kol16 + Sells.Kol18
                        gsuma = CaseGsuma(kindActivity, gsuma, row, pocupki, sumdds, prodazbi, code, suma, ref gumdds,
                            codedoc);
                    }
                    else
                    {
                        decimal sum = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                        decimal sumdds = decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                        string code = dbman.DataReader["CODE"].ToString();
                        gsuma = CaseGsuma(kindActivity, gsuma, row, pocupki, sumdds, prodazbi, code, sum, ref gumdds,
                            codedoc);
                    }


                }
                if (row != null)
                {
                    if (kindActivity == 2)
                    {
                        row[9] = gsuma.ToString(Vf.LevFormatUI);
                        row[10] = gumdds.ToString(Vf.LevFormatUI);
                    }
                    result.Add(row);
                }
                //if (kindActivity == 2)
                //{
                //    result.Add(Cherta(27));
                //}
                //else
                //{
                //    result.Add(Cherta(17));
                //}
                //row = new List<string>();
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //if (kindActivity == 2)
                //{
                //    //row.Add(gsuma.ToString(Vf.LevFormatUI));
                //    //row.Add(gumdds.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol9.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol10.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol11.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol12.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol13.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol14.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol15.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol16.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol17.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol18.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol19.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol20.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol21.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol22.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol23.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol24.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol25.ToString(Vf.LevFormatUI));
                //    row.Add("");
                //}
                //else
                //{
                //    row.Add(pocupki.Kol9.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol10.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol11.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol12.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol13.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol14.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol15.ToString(Vf.LevFormatUI));
                //    row.Add("");
                //}
                //result.Add(row);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static List<List<string>> GetDnevItem(int kindActivity, DateTime fromDate, DateTime toDate)");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        public static List<List<string>> GetDnevItemVies(int kindActivity, DateTime fromDate, DateTime toDate)
        {
            List<List<string>> result = new List<List<string>>();
            Purchases pocupki = new Purchases();
            Sells prodazbi = new Sells();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                if (kindActivity == 2)
                {
                    var str = string.Format("select d.BRANCH,d.BULSTAD,d.NZDDS,d.DATADOC,d.DATAF,d.DOCN," +
                                            "d.NOM,d.NAMEKONTR,d.SUMA,d.DDSSUMA as DDSTOTAL,d.STOKE,d.CODEDOC," +
                                            "c.\"FirmId\" as FID,c.\"Id\" as TR,n.\"Name\" as NAMEDOC,dt.SUMADDS,dt.DDS as SUMAWITHDDS,ds.NAME," +
                                            "ds.DDSPERCENT" +
                                            ",ds.CODE,d.KINDACTIVITY,d.A8 as AA,c.PORNOM,c.FOLDER as FOL,c.\"NumberObject\" as OB" +
                                            " from  DDSDNEVFIELDS ds" +
                                            " inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID" +
                                            " inner join DDSDNEV d on d.ID=dt.IDDDSDNEV" +
                                            " inner join \"conto\" c on c.\"Id\"=d.NOM" +
                                            " inner join \"nomenclatures\" n on n.\"Id\"=d.KINDDOC" +
                                            " where c.\"FirmId\"={0}" +
                                            " and d.KINDACTIVITY={1}" +
                                            " and d.DATADOC >= '{2}'" +
                                            " and d.DATADOC <= '{3}'" +
                                            " and d.NZDDS not SIMILAR TO '[0-9]%' and d.NZDDS not like 'BG%'"+
                                            " order by d.DATADOC",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        kindActivity,
                        string.Format("{0}.{1}.{2}", fromDate.Day, fromDate.Month, fromDate.Year),
                        string.Format("{0}.{1}.{2}", toDate.Day, toDate.Month, toDate.Year));
                    dbman.ExecuteReader(CommandType.Text, str);
                }
                else
                {
                    var str = string.Format(" select d.BRANCH,d.BULSTAD,d.NZDDS,d.DATADOC,d.DATAF,d.DOCN," +
                                            "d.NOM,d.NAMEKONTR,d.SUMA,d.DDSSUMA as DDSTOTAL,d.STOKE,d.CODEDOC," +
                                            "c.\"FirmId\" as FID,c.\"Id\" as TR,n.\"Name\" as NAMEDOC,dt.SUMADDS,dt.DDS as SUMAWITHDDS,ds.NAME," +
                                            "ds.DDSPERCENT" +
                                            ",ds.CODE,d.KINDACTIVITY,d.A8 as AA,c.PORNOM,c.FOLDER as FOL,c.\"NumberObject\" as OB" +
                                            " from  DDSDNEVSELLSFIELDS ds" +
                                            " inner join DDSDNEVTOFIELDS dt on dt.IDDDSFIELD=ds.ID" +
                                            " inner join DDSDNEV d on d.ID=dt.IDDDSDNEV" +
                                            " inner join \"conto\" c on c.\"Id\"=d.NOM" +
                                            " inner join \"nomenclatures\" n on n.\"Id\"=d.KINDDOC" +
                                            " where c.\"FirmId\"={0}" +
                                            " and d.KINDACTIVITY={1}" +
                                            " and d.DATADOC >= '{2}'" +
                                            " and d.DATADOC <= '{3}'" +
                                            " and d.NZDDS not SIMILAR TO '[0-9]%' and d.NZDDS not like 'BG%'" +
                                            " order by d.DATADOC",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        kindActivity,
                        string.Format("{0}.{1}.{2}", fromDate.Day, fromDate.Month, fromDate.Year),
                        string.Format("{0}.{1}.{2}", toDate.Day, toDate.Month, toDate.Year));
                    dbman.ExecuteReader(CommandType.Text, str);
                }
                string tester = "";
                List<string> row = null;
                int i = 1;
                string ssuma = "", ssumdds = "";
                decimal suma = 0, gumdds = 0;
                decimal gsuma = 0;
                string codedoc = "";
                while (dbman.DataReader.Read())
                {
                    string newtester = dbman.DataReader["NOM"].ToString();
                    if (!newtester.Equals(tester))
                    {
                        tester = newtester;
                        if (row != null)
                        {
                            if (kindActivity == 2)
                            {
                                row[9] = gsuma.ToString(Vf.LevFormatUI);
                                row[10] = gumdds.ToString(Vf.LevFormatUI);
                            }
                            result.Add(new List<string>(row));
                            gsuma = 0;
                            gumdds = 0;
                        }
                        row = new List<string>();
                        row.Add(i.ToString(CultureInfo.InvariantCulture));
                        i++;
                        row.Add(dbman.DataReader["BRANCH"].ToString());
                        codedoc = dbman.DataReader["CODEDOC"].ToString();
                        row.Add(codedoc);
                        string dn = add10o(dbman.DataReader["DOCN"].ToString());
                        row.Add(dn);
                        DateTime d = DateTime.Parse(dbman.DataReader["DATAF"].ToString());
                        row.Add(string.Format("{0:D2}/{1:D2}/{2:D4}", d.Day, d.Month, d.Year));
                        row.Add(dbman.DataReader["NZDDS"].ToString());
                        row.Add(dbman.DataReader["NAMEKONTR"].ToString());
                        row.Add(dbman.DataReader["STOKE"].ToString());
                        string a8 = dbman.DataReader["AA"].ToString();
                        string pornom = dbman.DataReader["TR"].ToString();
                        string folder = dbman.DataReader["FOL"].ToString();
                        string ob = dbman.DataReader["OB"].ToString();
                        row.Add(a8);
                        if (kindActivity == 2)
                        {
                            row.Add(ssuma);
                            row.Add(ssumdds);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(pornom);
                            row.Add(folder);
                            row.Add(ob);
                        }
                        else
                        {
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(Vf.LevFormatUI);
                            row.Add(pornom);
                            row.Add(folder);
                            row.Add(ob);
                        }
                        suma = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());

                        decimal sumdds = decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                        decimal prddc = decimal.Parse(dbman.DataReader["DDSPERCENT"].ToString());
                        string code = dbman.DataReader["CODE"].ToString();
                        //Sells.Kol12 + Sells.Kol15 + Sells.Kol16 + Sells.Kol18
                        gsuma = CaseGsuma(kindActivity, gsuma, row, pocupki, sumdds, prodazbi, code, suma, ref gumdds,
                            codedoc);
                    }
                    else
                    {
                        decimal sum = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                        decimal sumdds = decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                        string code = dbman.DataReader["CODE"].ToString();
                        gsuma = CaseGsuma(kindActivity, gsuma, row, pocupki, sumdds, prodazbi, code, sum, ref gumdds,
                            codedoc);
                    }


                }
                if (row != null)
                {
                    if (kindActivity == 2)
                    {
                        row[9] = gsuma.ToString(Vf.LevFormatUI);
                        row[10] = gumdds.ToString(Vf.LevFormatUI);
                    }
                    result.Add(row);
                }
                //if (kindActivity == 2)
                //{
                //    result.Add(Cherta(27));
                //}
                //else
                //{
                //    result.Add(Cherta(17));
                //}
                //row = new List<string>();
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //row.Add("");
                //if (kindActivity == 2)
                //{
                //    //row.Add(gsuma.ToString(Vf.LevFormatUI));
                //    //row.Add(gumdds.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol9.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol10.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol11.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol12.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol13.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol14.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol15.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol16.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol17.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol18.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol19.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol20.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol21.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol22.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol23.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol24.ToString(Vf.LevFormatUI));
                //    row.Add(prodazbi.Kol25.ToString(Vf.LevFormatUI));
                //    row.Add("");
                //}
                //else
                //{
                //    row.Add(pocupki.Kol9.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol10.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol11.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol12.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol13.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol14.ToString(Vf.LevFormatUI));
                //    row.Add(pocupki.Kol15.ToString(Vf.LevFormatUI));
                //    row.Add("");
                //}
                //result.Add(row);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static List<List<string>> GetDnevItemVies(int kindActivity, DateTime fromDate, DateTime toDate)");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        internal static List<Conto> GetAccMovent(int accId)
        {
            List<Conto> allConto = new List<Conto>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text,
                    String.Format(
                        "select  * from \"conto\" a where a.\"DebitAccount\"={0} or a.\"CreditAccount\"={0} and \"Date\"=>'1.1.{1}' and \"Date\"<='31.12.{1}' order by a.\"Id\"",
                        accId,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year));
                LoadConto(allConto,dbman);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<Conto> GetAccMovent(int accId)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }

        internal static void SaveUser(User currentUser)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string pas = Hash(currentUser.PassWord);
                dbman.Open();
                dbman.CreateParameters(6);
                dbman.BeginTransaction();
                dbman.AddParameters(0, "@ID", currentUser.Id);
                dbman.AddParameters(1, "@USERNAME", currentUser.UserName);
                dbman.AddParameters(2, "@PASSWORDS", pas);
                dbman.AddParameters(3, "@RIGHTS", currentUser.Rights);
                dbman.AddParameters(4, "@NAME", currentUser.Name);
                dbman.AddOutputParameters(5, "@NEWID", currentUser.Id);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "SAVEUSER");
                currentUser.Id = (int) dbman.Parameters[5].Value;
                dbman.CommitTransaction();

            }
            catch (Exception ex)
            {
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "internal static void SaveUser(User currentUser)");
            }

            finally
            {
                dbman.Dispose();
            }
        }

        private static string Hash(string rassword)
        {
            return PasswordHash.CreateHash(rassword);
        }

        public static IEnumerable<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select  * from USERS");

                while (dbman.DataReader.Read())
                {
                    var c = new User();
                    c.Id = int.Parse(dbman.DataReader["ID"].ToString());
                    c.UserName = dbman.DataReader["USERNAME"].ToString();
                    c.PassHash = dbman.DataReader["PASSWORDS"].ToString();
                    c.Rights = uint.Parse(dbman.DataReader["RIGHTS"].ToString());
                    c.Name = dbman.DataReader["NAME"].ToString();
                    allUsers.Add(c);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<User> GetAllUsers()");
            }

            finally
            {
                dbman.Dispose();
            }

            return allUsers;
        }



        internal static void SaveContoWithOutTransaction(Conto CurrentConto)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                CreateParameters(CurrentConto, true,dbman);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "ADDCONTO");
                CurrentConto.Id = (int) dbman.Parameters[29].Value;
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static void SaveContoWithOutTransaction(Conto CurrentConto)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }

        internal static void DeleteContos(int firmaId, DateTime fromDate, DateTime toDate)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(3);
                dbman.BeginTransaction();
                dbman.AddParameters(0, "@FROMD", fromDate);
                dbman.AddParameters(1, "@TODATE", toDate);
                dbman.AddParameters(2, "@FIRMAID", firmaId);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "DELETECONTOS");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static void DeleteContos(int firmaId, DateTime fromDate, DateTime toDate)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }

        public static void UpdateRowSys(List<string> list, LookupModel lookup)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<List<string>> result = new List<List<string>>();
            try
            {
                dbman.Open();
                dbman.CreateParameters(lookup.Fields.Count);
                if (lookup.LookUpMetaData.Tablename == "DDSDNEVSELLSFIELDS" ||
                    lookup.LookUpMetaData.Tablename == "DDSDNEVFIELDS")
                {
                    lookup.Fields[0].DbField = "ID";
                    lookup.Fields[0].NameEng = "ID";
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE \"{0}\" SET ", lookup.LookUpMetaData.Tablename);
                foreach (var field in lookup.Fields)
                {
                    if (field.NameEng != "ID" || field.NameEng != "Id")
                    {
                        sb.AppendFormat("\"{0}\"=@{1},", field.NameEng, field.NameEng.Replace(' ', '_'));
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                if (lookup.LookUpMetaData.Tablename == "DDSDNEVSELLSFIELDS" ||
                    lookup.LookUpMetaData.Tablename == "DDSDNEVFIELDS")
                {
                    sb.AppendFormat(" where \"ID\"={0}", list.ElementAt(0));
                }
                else
                {
                    sb.AppendFormat(" where \"Id\"={0}", list.ElementAt(0));
                }

                var i = 0;
                foreach (var field in lookup.Fields)
                {

                    if (field.DbField.ToLower() == "integer")
                    {
                        int ii = 0;
                        if (int.TryParse(list.ElementAt(i), out ii))
                        {
                            dbman.AddParameters(i, "@" + field.NameEng.Replace(' ', '_'), ii);
                        }
                        else
                        {
                            dbman.AddParameters(i, "@" + field.NameEng.Replace(' ', '_'), 0);
                        }
                    }
                    else
                    {
                        if (field.DbField.ToUpper().Contains("DECIMAL"))
                        {
                            dbman.AddParameters(i, "@" + field.NameEng.Replace(' ', '_'),
                                decimal.Parse(list.ElementAt(i)));
                        }
                        else
                        {
                            if (field.DbField.ToUpper() == ("CHAR(38)"))
                            {
                                if (field.Name != "Id" || field.Name != "ID")
                                {
                                    dbman.AddParameters(i, "@" + field.NameEng.Replace(' ', '_'),
                                        Guid.NewGuid().ToString());
                                }
                            }
                            else
                            {
                                if (field.DbField.ToUpper() == ("DATE"))
                                {
                                    dbman.AddParameters(i, "@" + field.NameEng.Replace(' ', '_'),
                                        DateTime.Parse(list.ElementAt(i)));
                                }
                                else
                                {
                                    dbman.AddParameters(i, "@" + field.NameEng.Replace(' ', '_'), list.ElementAt(i));
                                }
                            }
                        }

                    }
                    i++;
                }
                string s = sb.ToString();
                dbman.ExecuteNonQuery(CommandType.Text, s);
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void UpdateRowSys(List<string> list, LookupModel lookup)");
            }

            finally
            {
                dbman.Dispose();
            }

        }

        public static Dictionary<int, Dictionary<int, string>> LoadConfig()
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            var mDictionary = new Dictionary<int, Dictionary<int, string>>();
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "SELECT A.FID, A.LID, A.SF FROM LOOKUPSEARCHFIELD A");

                while (dbman.DataReader.Read())
                {
                    int fn = int.Parse(dbman.DataReader["FID"].ToString());
                    int ln = int.Parse(dbman.DataReader["LID"].ToString());
                    string fieldn = dbman.DataReader["SF"].ToString();
                    if (mDictionary.ContainsKey(fn))
                    {
                        var ld = mDictionary[fn];
                        if (ld.ContainsKey(ln))
                        {
                            ld[ln] = fieldn;
                        }
                        else
                        {
                            ld.Add(ln, fieldn);
                        }
                    }
                    else
                    {
                        mDictionary.Add(fn, new Dictionary<int, string> {{ln, fieldn}});
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static Dictionary<int, Dictionary<int, string>> LoadConfig()");


            }

            finally
            {
                dbman.Dispose();
            }
            return mDictionary;
        }

        internal static IEnumerable<ObservableCollection<string>> GetAllSearches()
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<ObservableCollection<string>> rez = new List<ObservableCollection<string>>();
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text,string.Format(
                    "SELECT F.\"Id\" FID,F.\"Name\" FNAME,L.\"Id\" LID,L.\"Name\" LNAME, A.SF SF FROM LOOKUPSEARCHFIELD A inner join \"firm\" F on A.FID=F.\"Id\" inner join \"lookups\" L on A.LID=L.\"Id\" where  A.FID={0}",ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
                var ob1 = new ObservableCollection<string> {"N Фирма", "Фирма", "N Номен", "Номенклатура", "Поле"};
                rez.Add(ob1);
                while (dbman.DataReader.Read())
                {
                    string fid = dbman.DataReader["FID"].ToString();
                    string fn = dbman.DataReader["FNAME"].ToString();
                    string lid = dbman.DataReader["LID"].ToString();
                    string ln = dbman.DataReader["LNAME"].ToString();
                    string fieldn = dbman.DataReader["SF"].ToString();
                    var ob = new ObservableCollection<string> {fid, fn, lid, ln, fieldn};
                    rez.Add(ob);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<ObservableCollection<string>> GetAllSearches()");


            }

            finally
            {
                dbman.Dispose();
            }
            return rez;

        }

        public static void SaveMap(int fid, int lid, string field)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(3);
                dbman.BeginTransaction();
                dbman.AddParameters(0, "@FID", fid);
                dbman.AddParameters(1, "@LID", lid);
                dbman.AddParameters(2, "@SF", field);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure,
                    "INSERTMAP");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void SaveMap(int fid, int lid, string field)");
                dbman.RollBackTransaction();

            }


            finally
            {
                dbman.Dispose();
            }
        }

        public static void DeleteMap(int fid, int lid)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(2);
                dbman.BeginTransaction();
                dbman.AddParameters(0, "@FID", fid);
                dbman.AddParameters(1, "@LID", lid);
                dbman.ExecuteNonQuery(CommandType.Text,
                    "DELETE FROM LOOKUPSEARCHFIELD WHERE FID=@FID AND LID=@LID");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void DeleteMap(int fid, int lid)");
                dbman.RollBackTransaction();

            }


            finally
            {
                dbman.Dispose();
            }

        }
        public static IEnumerable<AccItemSaldo> GetInfoFactura(int AccID, int acctype, string invoiseNumber,string contragent)
        {
            Dictionary<int, Dictionary<int, string>> nomen = new Dictionary<int, Dictionary<int, string>>();
            List<AccItemSaldo> result = new List<AccItemSaldo>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Date\",m.LOOKUPFIELDKEY,m.LOOKUPID,m.\"VALUE\",lf.\"Name\",m.VALUEDATE,c.\"DebitAccount\",m.LOOKUPVAL FROM \"conto\" c inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and c.\"Date\">='1.1.{1}' and c.\"Date\"<='31.12.{1}' and c.\"DebitAccount\"={2} or c.\"CreditAccount\"={2}) order by c.\"Id\",m.SORTORDER",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ConfigTempoSinglenton.GetInstance().WorkDate.Year, AccID);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int oldid = 0;
                bool first = true;
                var ic = new AccItemSaldo();
                ic.Type = acctype;
                bool load = true;
                while (dbman.DataReader.Read())
                {

                    string value = dbman.DataReader["VALUE"].ToString();
                    string field = dbman.DataReader["Name"].ToString();
                    int newid = int.Parse(dbman.DataReader["Id"].ToString());
                    if (first)
                    {
                        first = false;
                        oldid = newid;
                    }

                    if (oldid != newid)
                    {
                        if (ic.NameContragent == contragent && ic.NInvoise == invoiseNumber)
                        {

                            result.Add(ic.Clone());
                        }

                        ic = new AccItemSaldo { Type = acctype };
                        oldid = newid;
                        load = true;
                    }



                    if (field == "Номер фактура")
                    {
                        ic.NInvoise = value;
                    }
                    if (field == "Контрагент")
                    {
                        ic.NameContragent = dbman.DataReader["LOOKUPVAL"].ToString();

                    }
                    if (field == "Дата на фактура")
                    {
                        ic.Data = DateTime.Parse(value);
                    }

                    if (load)
                    {
                        int smetka;
                        if (int.TryParse(dbman.DataReader["DebitAccount"].ToString(), out smetka))
                        {
                            if (smetka == AccID)
                            {
                                ic.IsDebit = true;
                                ic.Od = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                            else
                            {
                                ic.Oc = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                            }
                        }
                        load = false;
                    }
                }
                if (ic.NameContragent == contragent && ic.NInvoise == invoiseNumber) result.Add(ic);

            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<AccItemSaldo> GetInfoFactura(int AccID, int acctype, string invoiseNumber,string contragent)");
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        internal static IEnumerable<IEnumerable<string>> GetDetailsContoToAcc(int id,int typ,string filter)
        {
           List<AccItemSaldo> rez=new List<AccItemSaldo>();
           List<List<string>> rez1=new List<List<string>>();
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
                             " where a.\"Id\"={0}",id);
                int count=(int)dbman.ExecuteScalar(CommandType.Text, command);
                bool change = false;
                bool first = true;
                bool firstrow = true;
                bool ima = false;
                AccItemSaldo row = new AccItemSaldo();
                row.Type = typ;
                int oldid=0,newid = 0;
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
                                row.Type =typ;
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
                        string name = dbman.DataReader["Name"].ToString();
                        string value = dbman.DataReader["VALUE"].ToString();
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
                         group t by new { t.Code,t.NInvoise}
                             into grp
                             select new AccItemSaldo
                             {
                                 Code = grp.Key.Code,
                                 NInvoise = grp.Key.NInvoise,
                                 Details=grp.First().Details,
                                 Type=grp.First().Type,
                                 Data=grp.Last().Data,
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
                        m => accItemSaldo.Code==m.Code && accItemSaldo.NInvoise==m.NumInvoise);
                if (saldo != null)
                {
                    accItemSaldo.Nsd = saldo.BeginSaldoDebit;
                    accItemSaldo.Nsc = saldo.BeginSaldoCredit;
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
                    item.NInvoise = items.NumInvoise;
                    item.NameContragent = items.NameContragent;
                    item.Nsc = items.BeginSaldoCredit;
                    item.Nsd = items.BeginSaldoDebit;
                    item.Data = items.Date;
                    item.Type = typ;
                    item.Details = items.Details;
                    query.Add(item);
                }
            }
            //
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
                    newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2),item.Data.Month.ToZeroString(2),item.Data.Year.ToZeroString(4))); 
                    newrow.Add(saldo.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Od.ToString(Vf.LevFormatUI));
                    newrow.Add(item.Oc.ToString(Vf.LevFormatUI));
                    decimal ksaldo = 0;
                    if (item.Type == 1)
                    {
                        ksaldo = (item.Nsd +item.Od) - (item.Nsc + item.Oc);
                    }
                    else
                    {
                        ksaldo = (item.Nsc + item.Oc) -(item.Nsd + item.Od);
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
                        newrow.Add(string.Format("{0}.{1}.{2}",item.Data.Day.ToZeroString(2), item.Data.Month.ToZeroString(2), item.Data.Year.ToZeroString(4)));
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

        internal static string SelectMax(string tableName, string fieldName)
        {
            string rez = "1";
            int k=1;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(2);
                string s = string.Format("SELECT MAX(\"{0}\") AS M FROM  \"{1}\" WHERE FIRMAID={2}", fieldName, tableName, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                dbman.ExecuteReader(CommandType.Text, s);
                if (dbman.DataReader.Read())
                {

                    int i;
                    if (int.TryParse(dbman.DataReader["M"].ToString(), out i))
                    {
                        k = i + 1;
                    }
                }
                rez = k.ToString();
            }
            
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static string SelectMax(string tableName, string fieldName)");
 
            }
            finally
            {
                dbman.Dispose();
            }
            return rez;
        }

        internal static string FbBatchExecution(string sql)
        {
            string rez = "";
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                //dbman.BeginTransaction();
                FbScript script = new FbScript(sql);
                script.Parse();
                FirebirdSql.Data.Isql.FbBatchExecution fbe =
                    new FirebirdSql.Data.Isql.FbBatchExecution(dbman.Connection as FbConnection);
                foreach (string cmd in script.Results)
                {
                    fbe.SqlStatements.Add(cmd);
                }
                fbe.Execute(true);
                //dbman.CommitTransaction();
            }
            catch (Exception ex)
            {
                //Logger.Instance().WriteLogError(ex.Message, "internal static string FbBatchExecution(string sql)");
                rez = ex.Message;
                //dbman.RollBackTransaction();
            }
            finally
            {
                dbman.Dispose();
            }
            return rez;
        }

        internal static IEnumerable<Conto> GetAllContoWithDds(int firmaId, Interface.CSearchAcc cSearchAcc,int tipdnev)
        {
            List<Conto> allConto = new List<Conto>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s =
                    String.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Reason\",c.\"Note\",c.\"DataInvoise\",c.\"NumberObject\",c.\"DebitAccount\",c.\"CreditAccount\",c.\"FirmId\",c.\"DocumentId\",c.DOCNUM,c.OBOROTVALUTA,c.OBOROTKOL,c.OBOROTVALUTAK,c.OBOROTKOLK,c.FOLDER,c.\"Date\",c.ISDDSSALES,c.ISDDSPURCHASES,c.ISSALES,c.ISPURCHASES,c.VOPPURCHASES,c.VOPSALES,c.PORNOM,c.DDETAILS,c.USERID,c.CDETAILS,c.PR1,d.NAMEKONTR,d.CODEDOC,d.NZDDS,d.DOCN,d.DATAF,sum(ds.DDS) as DDS,sum(ds.SUMADDS) as SUMADDS " +
                        "FROM \"conto\" c "+ 
                        "inner join DDSDNEV d on d.NOM=c.\"Id\" "+
                        "inner join DDSDNEVTOFIELDS ds on ds.IDDDSDNEV=d.ID "+
                        "inner join \"accounts\" b on b.\"Id\"= c.\"DebitAccount\" "+
                        "inner join \"accounts\" a on a.\"Id\"= c.\"CreditAccount\" "+
                        "where \"FirmId\"={0} and \"Date\">='{1}.{2}.{3}' and \"Date\"<='{4}.{5}.{6}' and d.KINDACTIVITY={7} ",
                        firmaId,
                        cSearchAcc.FromDate.Day,
                        cSearchAcc.FromDate.Month,
                        cSearchAcc.FromDate.Year,
                        cSearchAcc.ToDate.Day,
                        cSearchAcc.ToDate.Month,
                        cSearchAcc.ToDate.Year,
                        tipdnev
                        );
                StringBuilder sb=new StringBuilder();
                sb.Append(s);

                if (!String.IsNullOrWhiteSpace(cSearchAcc.NumDoc))
                    sb.AppendFormat(" AND DOCNUM LIKE '%{0}%'", cSearchAcc.NumDoc);
                if (cSearchAcc.CreditAcc != null)
                {
                    if (cSearchAcc.CreditAcc.Num > 0)
                        sb.AppendFormat(" AND a.NUM={0}", cSearchAcc.CreditAcc.Num);
                    if (cSearchAcc.CreditAcc.SubNum > -1)
                        sb.AppendFormat(" AND a.\"SubNum\"={0}", cSearchAcc.CreditAcc.SubNum);
                }
                if (cSearchAcc.DebitAcc != null)
                {
                    if (cSearchAcc.DebitAcc.Num > 0)
                        sb.AppendFormat(" AND b.NUM={0}", cSearchAcc.DebitAcc.Num);
                    if (cSearchAcc.DebitAcc.SubNum > -1)
                        sb.AppendFormat(" AND b.\"SubNum\"={0}", cSearchAcc.DebitAcc.SubNum);
                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Note))
                    sb.AppendFormat(" AND UPPER(c.\"Note\") LIKE '%{0}%'", cSearchAcc.Note.ToUpper());
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Reason))
                    sb.AppendFormat(" AND UPPER(c.\"Reason\") LIKE '%{0}%'", cSearchAcc.Reason.ToUpper());
                if (cSearchAcc.CreditItems != null)
                {
                    foreach (var item in cSearchAcc.CreditItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"CDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (cSearchAcc.DebitItems != null)
                {
                    foreach (var item in cSearchAcc.DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"DDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Ob))
                {
                    sb.AppendFormat(" AND c.\"NumberObject\" = '{0}'", cSearchAcc.Ob);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Folder))
                {
                    sb.AppendFormat(" AND c.FOLDER='{0}'", cSearchAcc.Folder);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Pr1))
                {
                    sb.AppendFormat(" AND c.PR1='{0}'", cSearchAcc.Pr1);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Pr2))
                {
                    sb.AppendFormat(" AND c.PR2='{0}'", cSearchAcc.Pr2);

                }
                sb.AppendFormat(" group by c.\"Id\",c.\"Oborot\",c.\"Reason\",c.\"Note\",c.\"DataInvoise\",c.\"NumberObject\",c.\"DebitAccount\",c.\"CreditAccount\",c.\"FirmId\",c.\"DocumentId\",c.DOCNUM,c.OBOROTVALUTA,c.OBOROTKOL,c.OBOROTVALUTAK,c.OBOROTKOLK,c.FOLDER,c.\"Date\",c.ISDDSSALES,c.ISDDSPURCHASES,c.ISSALES,c.ISPURCHASES,c.VOPPURCHASES,c.VOPSALES,c.PORNOM,c.DDETAILS,c.CDETAILS,c.USERID,c.PR1,d.NAMEKONTR,d.CODEDOC,d.NZDDS,d.DOCN,d.DATAF");
                s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text,s);
                while (dbman.DataReader.Read())
                {
                    var c = new Conto();
                    c.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    //c.CartotecaCredit = int.Parse(dbman.DataReader["CartotecaCredit"].ToString());
                    //c.CartotekaDebit = int.Parse(dbman.DataReader["CartotekaDebit"].ToString());
                    c.CreditAccount = int.Parse(dbman.DataReader["CreditAccount"].ToString());
                    c.Data = DateTime.Parse(dbman.DataReader["Date"].ToString());
                    c.DataInvoise = DateTime.Parse(dbman.DataReader["DataInvoise"].ToString());
                    c.DebitAccount = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                    c.DocumentId = int.Parse(dbman.DataReader["DocumentId"].ToString());
                    c.FirmId = int.Parse(dbman.DataReader["FirmId"].ToString());
                    c.Reason = dbman.DataReader["Reason"].ToString();
                    c.Note = dbman.DataReader["Note"].ToString();
                    c.NumberObject = int.Parse(dbman.DataReader["NumberObject"].ToString());
                    c.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                    c.DocNum = dbman.DataReader["DOCNUM"].ToString();
                    //c.OborotValutaD = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                    //c.OborotKolD = decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                    //c.OborotValutaK = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                    //c.OborotKolK = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                    c.Folder = dbman.DataReader["FOLDER"].ToString();
                    c.Nd = int.Parse(dbman.DataReader["PORNOM"].ToString());
                    c.IsDdsPurchases = int.Parse(dbman.DataReader["ISDDSPURCHASES"].ToString());
                    c.IsDdsSales = int.Parse(dbman.DataReader["ISDDSSALES"].ToString());
                    //c.IsDdsPurchasesIncluded = int.Parse(dbman.DataReader["ISDDSPURCHASESINCLUDED"].ToString());
                    //c.IsDdsSalesIncluded = int.Parse(dbman.DataReader["ISDDSSALESINCLUDED"].ToString());
                    c.VopPurchases = dbman.DataReader["VOPPURCHASES"].ToString();
                    c.VopSales = dbman.DataReader["VOPSALES"].ToString();
                    c.IsSales = int.Parse(dbman.DataReader["ISSALES"].ToString());
                    c.IsPurchases = int.Parse(dbman.DataReader["ISPURCHASES"].ToString());
                    c.DDetails = dbman.DataReader["DDETAILS"].ToString();
                    c.CDetails = dbman.DataReader["CDETAILS"].ToString();
                    c.UserId = int.Parse(dbman.DataReader["USERID"].ToString());
                    c.Pr1 = dbman.DataReader["PR1"].ToString();
                    //c.Pr2 = dbman.DataReader["PR2"].ToString();
                    //c.KD = dbman.DataReader["KD"].ToString();
                    c.DataInvoiseDnev = dbman.DataReader["DATAF"].ToString();
                    c.NomInvoise = dbman.DataReader["DOCN"].ToString();
                    c.Contragent = dbman.DataReader["NAMEKONTR"].ToString();
                    c.KindDoc = dbman.DataReader["CODEDOC"].ToString();
                    c.Vat = dbman.DataReader["NZDDS"].ToString();
                    c.SumDds = Decimal.Parse( dbman.DataReader["DDS"].ToString());
                    c.Sum=Decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                    allConto.Add(c);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetAllContoWithDds(int firmaId, Interface.CSearchAcc cSearchAcc,int tipdnev)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }

        internal static IEnumerable<Conto> GetAllContoWithDdsAndNot(int firmaId, Interface.CSearchAcc cSearchAcc,string clnum,string nom)
        {
            List<Conto> allConto = new List<Conto>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s =
                    String.Format(
                        "SELECT c.\"Id\",c.\"Oborot\",c.\"Reason\",c.\"Note\",c.\"DataInvoise\",c.\"NumberObject\",c.\"DebitAccount\",c.\"CreditAccount\",c.\"FirmId\",c.\"DocumentId\",c.DOCNUM,c.OBOROTVALUTA,c.OBOROTKOL,c.OBOROTVALUTAK,c.OBOROTKOLK,c.FOLDER,c.\"Date\",c.ISDDSSALES,c.ISDDSPURCHASES,c.ISSALES,c.ISPURCHASES,c.VOPPURCHASES,c.VOPSALES,c.PORNOM,c.DDETAILS,c.CDETAILS,c.USERID,c.PR1,d.NAMEKONTR,d.CODEDOC,d.NZDDS,d.DOCN,d.DATAF,d.CLNUM,f.DDS,f.SUMAWITHDDS as SUMADDS " +
                        "FROM \"conto\" c " +
                        "inner join \"accounts\" b on b.\"Id\"= c.\"DebitAccount\" " +
                        "inner join \"accounts\" a on a.\"Id\"= c.\"CreditAccount\" " +
                        "inner join DDSDNEV d on d.NOM=c.\"Id\" " +
                        "inner join DDSDNEVTOFIELDS f on f.IDDDSDNEV=d.ID " + 
                        "where \"FirmId\"={0} and \"Date\">='{1}.{2}.{3}' and \"Date\"<='{4}.{5}.{6}'",
                        firmaId,
                        cSearchAcc.FromDate.Day,
                        cSearchAcc.FromDate.Month,
                        cSearchAcc.FromDate.Year,
                        cSearchAcc.ToDate.Day,
                        cSearchAcc.ToDate.Month,
                        cSearchAcc.ToDate.Year
                        );
                StringBuilder sb = new StringBuilder();
                sb.Append(s);

                if (!String.IsNullOrWhiteSpace(cSearchAcc.NumDoc))
                    sb.AppendFormat(" AND DOCNUM LIKE '%{0}%'", cSearchAcc.NumDoc);
                if (cSearchAcc.CreditAcc != null)
                {
                    if (cSearchAcc.CreditAcc.Num > 0)
                        sb.AppendFormat(" AND a.NUM={0}", cSearchAcc.CreditAcc.Num);
                    if (cSearchAcc.CreditAcc.SubNum > -1)
                        sb.AppendFormat(" AND a.\"SubNum\"={0}", cSearchAcc.CreditAcc.SubNum);
                }
                if (cSearchAcc.DebitAcc != null)
                {
                    if (cSearchAcc.DebitAcc.Num > 0)
                        sb.AppendFormat(" AND b.NUM={0}", cSearchAcc.DebitAcc.Num);
                    if (cSearchAcc.DebitAcc.SubNum > -1)
                        sb.AppendFormat(" AND b.\"SubNum\"={0}", cSearchAcc.DebitAcc.SubNum);
                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Note))
                    sb.AppendFormat(" AND UPPER(c.\"Note\") LIKE '%{0}%'", cSearchAcc.Note.ToUpper());
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Reason))
                    sb.AppendFormat(" AND UPPER(c.\"Reason\") LIKE '%{0}%'", cSearchAcc.Reason.ToUpper());
                if (cSearchAcc.CreditItems != null)
                {
                    foreach (var item in cSearchAcc.CreditItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"CDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (cSearchAcc.DebitItems != null)
                {
                    foreach (var item in cSearchAcc.DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"DDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Ob))
                {
                    sb.AppendFormat(" AND c.\"NumberObject\" = '{0}'", cSearchAcc.Ob);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Folder))
                {
                    sb.AppendFormat(" AND c.FOLDER='{0}'", cSearchAcc.Folder);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Pr1))
                {
                    sb.AppendFormat(" AND c.PR1='{0}'", cSearchAcc.Pr1);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Pr2))
                {
                    sb.AppendFormat(" AND c.PR2='{0}'", cSearchAcc.Pr2);

                }
                sb.AppendFormat(" AND d.CLNUM='{0}' AND d.LOOKUPID='{1}'", clnum,nom);
                s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var c = new Conto();
                    c.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    //c.CartotecaCredit = int.Parse(dbman.DataReader["CartotecaCredit"].ToString());
                    //c.CartotekaDebit = int.Parse(dbman.DataReader["CartotekaDebit"].ToString());
                    c.CreditAccount = int.Parse(dbman.DataReader["CreditAccount"].ToString());
                    c.Data = DateTime.Parse(dbman.DataReader["Date"].ToString());
                    c.DataInvoise = DateTime.Parse(dbman.DataReader["DataInvoise"].ToString());
                    c.DebitAccount = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                    c.DocumentId = int.Parse(dbman.DataReader["DocumentId"].ToString());
                    c.FirmId = int.Parse(dbman.DataReader["FirmId"].ToString());
                    c.Reason = dbman.DataReader["Reason"].ToString();
                    c.Note = dbman.DataReader["Note"].ToString();
                    c.NumberObject = int.Parse(dbman.DataReader["NumberObject"].ToString());
                    c.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                    c.DocNum = dbman.DataReader["DOCNUM"].ToString();
                    //c.OborotValutaD = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                    //c.OborotKolD = decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                    //c.OborotValutaK = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                    //c.OborotKolK = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                    c.Folder = dbman.DataReader["FOLDER"].ToString();
                    c.Nd = int.Parse(dbman.DataReader["PORNOM"].ToString());
                    c.IsDdsPurchases = int.Parse(dbman.DataReader["ISDDSPURCHASES"].ToString());
                    c.IsDdsSales = int.Parse(dbman.DataReader["ISDDSSALES"].ToString());
                    //c.IsDdsPurchasesIncluded = int.Parse(dbman.DataReader["ISDDSPURCHASESINCLUDED"].ToString());
                    //c.IsDdsSalesIncluded = int.Parse(dbman.DataReader["ISDDSSALESINCLUDED"].ToString());
                    c.VopPurchases = dbman.DataReader["VOPPURCHASES"].ToString();
                    c.VopSales = dbman.DataReader["VOPSALES"].ToString();
                    c.IsSales = int.Parse(dbman.DataReader["ISSALES"].ToString());
                    c.IsPurchases = int.Parse(dbman.DataReader["ISPURCHASES"].ToString());
                    c.DDetails = dbman.DataReader["DDETAILS"].ToString();
                    c.CDetails = dbman.DataReader["CDETAILS"].ToString();
                    c.UserId = int.Parse(dbman.DataReader["USERID"].ToString());
                    c.Pr1 = dbman.DataReader["PR1"].ToString();
                    //c.Pr2 = dbman.DataReader["PR2"].ToString();
                    //c.KD = dbman.DataReader["KD"].ToString();
                    c.DataInvoiseDnev = dbman.DataReader["DATAF"].ToString();
                    c.NomInvoise = dbman.DataReader["DOCN"].ToString();
                    c.Contragent = dbman.DataReader["NAMEKONTR"].ToString();
                    c.KindDoc = dbman.DataReader["CODEDOC"].ToString();
                    c.Vat = dbman.DataReader["NZDDS"].ToString();
                    c.ClientNumDds=dbman.DataReader["CLNUM"].ToString();
                    c.DName = dbman.DataReader["DOCN"].ToString();
                    c.SumDds = Decimal.Parse(dbman.DataReader["DDS"].ToString());
                    c.Sum = Decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                    allConto.Add(c);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetAllContoWithDds(int firmaId, Interface.CSearchAcc cSearchAcc,int tipdnev)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }

        internal static List<List<string>> GetDebit(DateTime FromDate, DateTime ToDate, int accId, int firmId)
        {
            List<List<string>> alList = new List<List<string>>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;

            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text,
                    String.Format("SELECT ac.NUM,ac.\"SubNum\",sum(c.\"Oborot\") as ob1 "+
                                  "FROM \"conto\" c "+
                                  "inner join \"accounts\" ad on ad.\"Id\"=c.\"DebitAccount\""+
                                  "inner join \"accounts\" ac on ac.\"Id\"=c.\"CreditAccount\""+   
                                  "where c.\"Date\">='{0}.{1}.{2}' and c.\"Date\"<='{3}.{4}.{5}' and c.\"FirmId\"={6} and c.\"DebitAccount\"={7}"+
                                  "group by c.\"CreditAccount\",ad.NUM,ad.\"SubNum\",ac.NUM,ac.\"SubNum\""+ 
                                  "order by ac.NUM,ac.\"SubNum\"",
                                   FromDate.Day,FromDate.Month,FromDate.Year,
                                   ToDate.Day,ToDate.Month,ToDate.Year,firmId,accId));
                while (dbman.DataReader.Read())
                {
                    var row = new List<string>();
                    string s = dbman.DataReader["Num"].ToString();
                    if (dbman.DataReader["SubNum"].ToString() != "0")
                    {
                        s = string.Format("{0}/{1}", s, dbman.DataReader["SubNum"]);
                    }
                    row.Add(s);
                    row.Add(string.Format(decimal.Parse(dbman.DataReader["ob1"].ToString()).ToString(Vf.LevFormatUI)));
                    alList.Add(row);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetDebit(DateTime FromDate, DateTime ToDate, int accId, int firmId)");
            }

            finally
            {
                dbman.Dispose();
            }

            return alList;
        }

        internal static List<List<string>> GetCredit(DateTime fromDate, DateTime toDate, int accId, int firmId)
        {
            List<List<string>> alList = new List<List<string>>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
              dbman.Open();
                dbman.ExecuteReader(CommandType.Text,
                    String.Format("SELECT ad.NUM,ad.\"SubNum\",sum(c.\"Oborot\") as ob1 "+
                                  "FROM \"conto\" c "+
                                  "inner join \"accounts\" ad on ad.\"Id\"=c.\"DebitAccount\""+
                                  "inner join \"accounts\" ac on ac.\"Id\"=c.\"CreditAccount\""+
                                  "where c.\"Date\">='{0}.{1}.{2}' and c.\"Date\"<='{3}.{4}.{5}' and c.\"FirmId\"={6} and c.\"CreditAccount\"={7}" +
                                  "group by c.\"CreditAccount\",ad.NUM,ad.\"SubNum\",ac.NUM,ac.\"SubNum\""+ 
                                  "order by ad.NUM,ad.\"SubNum\"",
                                   fromDate.Day,fromDate.Month,fromDate.Year,
                                   toDate.Day,toDate.Month,toDate.Year,firmId,accId));
                while (dbman.DataReader.Read())
                {
                    var row = new List<string>();
                    string s = dbman.DataReader["Num"].ToString();
                    if (dbman.DataReader["SubNum"].ToString() != "0")
                    {
                        s = string.Format("{0}/{1}", s, dbman.DataReader["SubNum"]);
                    }
                    row.Add(s);
                    row.Add(string.Format(decimal.Parse(dbman.DataReader["ob1"].ToString()).ToString(Vf.LevFormatUI)));
                    alList.Add(row);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GeCredit(DateTime fromDate, DateTime toDate, int accId, int firmId)");
            }

            finally
            {
                dbman.Dispose();
            }

            return alList;
        }

        internal static IEnumerable<Conto> GetAllContoWithoutDds(int firmaId, Interface.CSearchAcc cSearchAcc, int tipdnev)
        {

            List<Conto> allConto = new List<Conto>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s =
                    String.Format(
                        "SELECT " +
                                "c.\"Id\"," +
                                "c.\"Oborot\"," +
                                "c.\"Reason\"," +
                                "c.\"Note\"," +
                                "c.\"DataInvoise\"," +
                                "c.\"NumberObject\"," +
                                "c.\"DebitAccount\"," +
                                "c.\"CreditAccount\"," +
                                "c.\"FirmId\"," +
                                "c.\"DocumentId\"," +
                                "c.DOCNUM," +
                                "c.OBOROTVALUTA," +
                                "c.OBOROTKOL," +
                                "c.OBOROTVALUTAK," +
                                "c.OBOROTKOLK," +
                                "c.FOLDER," +
                                "c.\"Date\"," +
                                "c.ISDDSSALES," +
                                "c.ISDDSPURCHASES," +
                                "c.VOPPURCHASES," +
                                "c.VOPSALES," +
                                "c.PORNOM," +
                                "c.DDETAILS," +
                                "c.CDETAILS," +
                                "c.PR1,c.USERID  FROM \"conto\" c " +
                                "inner join \"accounts\" b on b.\"Id\"= c.\"DebitAccount\"" +
                                "inner join \"accounts\" a on a.\"Id\"= c.\"CreditAccount\"" +
                        "where \"FirmId\"={0} and \"Date\">='{1}.{2}.{3}' and \"Date\"<='{4}.{5}.{6}' and c.ISPURCHASES=0 and c.ISSALES=0",
                        firmaId,
                        cSearchAcc.FromDate.Day,
                        cSearchAcc.FromDate.Month,
                        cSearchAcc.FromDate.Year,
                        cSearchAcc.ToDate.Day,
                        cSearchAcc.ToDate.Month,
                        cSearchAcc.ToDate.Year
                        );
                StringBuilder sb = new StringBuilder();
                sb.Append(s);

                if (!String.IsNullOrWhiteSpace(cSearchAcc.NumDoc))
                    sb.AppendFormat(" AND DOCNUM LIKE '%{0}%'", cSearchAcc.NumDoc);
                if (cSearchAcc.CreditAcc != null)
                {
                    if (cSearchAcc.CreditAcc.Num > 0)
                        sb.AppendFormat(" AND a.NUM={0}", cSearchAcc.CreditAcc.Num);
                    if (cSearchAcc.CreditAcc.SubNum > -1)
                        sb.AppendFormat(" AND a.\"SubNum\"={0}", cSearchAcc.CreditAcc.SubNum);
                }
                if (cSearchAcc.DebitAcc != null)
                {
                    if (cSearchAcc.DebitAcc.Num > 0)
                        sb.AppendFormat(" AND b.NUM={0}", cSearchAcc.DebitAcc.Num);
                    if (cSearchAcc.DebitAcc.SubNum > -1)
                        sb.AppendFormat(" AND b.\"SubNum\"={0}", cSearchAcc.DebitAcc.SubNum);
                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Note))
                    sb.AppendFormat(" AND UPPER(c.\"Note\") LIKE '%{0}%'", cSearchAcc.Note.ToUpper());
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Reason))
                    sb.AppendFormat(" AND UPPER(c.\"Reason\") LIKE '%{0}%'", cSearchAcc.Reason.ToUpper());
                if (cSearchAcc.CreditItems != null)
                {
                    foreach (var item in cSearchAcc.CreditItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"CDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (cSearchAcc.DebitItems != null)
                {
                    foreach (var item in cSearchAcc.DebitItems.Where(item => !String.IsNullOrWhiteSpace(item.Value)))
                    {
                        sb.AppendFormat(" AND UPPER(c.\"DDETAILS\") LIKE '%{0} - {1} %'", item.Name.ToUpper(), item.Value.ToUpper());
                    }
                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Ob))
                {
                    sb.AppendFormat(" AND c.\"NumberObject\" = '{0}'", cSearchAcc.Ob);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Folder))
                {
                    sb.AppendFormat(" AND c.FOLDER='{0}'", cSearchAcc.Folder);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Pr1))
                {
                    sb.AppendFormat(" AND c.PR1='{0}'", cSearchAcc.Pr1);

                }
                if (!String.IsNullOrWhiteSpace(cSearchAcc.Pr2))
                {
                    sb.AppendFormat(" AND c.PR2='{0}'", cSearchAcc.Pr2);

                }
                sb.AppendFormat(" order by c.YEARR,c.MON,c.PORNOM");
                s = sb.ToString();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    var c = new Conto();
                    c.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    //c.CartotecaCredit = int.Parse(dbman.DataReader["CartotecaCredit"].ToString());
                    //c.CartotekaDebit = int.Parse(dbman.DataReader["CartotekaDebit"].ToString());
                    c.CreditAccount = int.Parse(dbman.DataReader["CreditAccount"].ToString());
                    c.Data = DateTime.Parse(dbman.DataReader["Date"].ToString());
                    c.DataInvoise = DateTime.Parse(dbman.DataReader["DataInvoise"].ToString());
                    c.DebitAccount = int.Parse(dbman.DataReader["DebitAccount"].ToString());
                    c.DocumentId = int.Parse(dbman.DataReader["DocumentId"].ToString());
                    c.FirmId = int.Parse(dbman.DataReader["FirmId"].ToString());
                    c.Reason = dbman.DataReader["Reason"].ToString();
                    c.Note = dbman.DataReader["Note"].ToString();
                    c.NumberObject = int.Parse(dbman.DataReader["NumberObject"].ToString());
                    c.Oborot = decimal.Parse(dbman.DataReader["Oborot"].ToString());
                    c.DocNum = dbman.DataReader["DOCNUM"].ToString();
                    //c.OborotValutaD = decimal.Parse(dbman.DataReader["OBOROTVALUTA"].ToString());
                    //c.OborotKolD = decimal.Parse(dbman.DataReader["OBOROTKOL"].ToString());
                    //c.OborotValutaK = decimal.Parse(dbman.DataReader["OBOROTVALUTAK"].ToString());
                    //c.OborotKolK = decimal.Parse(dbman.DataReader["OBOROTKOLK"].ToString());
                    c.Folder = dbman.DataReader["FOLDER"].ToString();
                    c.Nd = int.Parse(dbman.DataReader["PORNOM"].ToString());
                    //c.IsDdsPurchases = int.Parse(dbman.DataReader["ISDDSPURCHASES"].ToString());
                    //c.IsDdsSales = int.Parse(dbman.DataReader["ISDDSSALES"].ToString());
                    //c.IsDdsPurchasesIncluded = int.Parse(dbman.DataReader["ISDDSPURCHASESINCLUDED"].ToString());
                    //c.IsDdsSalesIncluded = int.Parse(dbman.DataReader["ISDDSSALESINCLUDED"].ToString());
                    //c.VopPurchases = dbman.DataReader["VOPPURCHASES"].ToString();
                    //c.VopSales = dbman.DataReader["VOPSALES"].ToString();
                    //c.IsSales = int.Parse(dbman.DataReader["ISSALES"].ToString());
                    //c.IsPurchases = int.Parse(dbman.DataReader["ISPURCHASES"].ToString());
                    c.DDetails = dbman.DataReader["DDETAILS"].ToString();
                    c.CDetails = dbman.DataReader["CDETAILS"].ToString();
                    c.UserId = int.Parse(dbman.DataReader["USERID"].ToString());
                    c.Pr1 = dbman.DataReader["PR1"].ToString();
                    //c.Pr2 = dbman.DataReader["PR2"].ToString();
                    //c.KD = dbman.DataReader["KD"].ToString();
                    //c.DataInvoiseDnev = dbman.DataReader["DATAF"].ToString();
                    //c.NomInvoise = dbman.DataReader["DOCN"].ToString();
                    //c.Contragent = dbman.DataReader["NAMEKONTR"].ToString();
                    //c.KindDoc = dbman.DataReader["CODEDOC"].ToString();
                    //c.Vat = dbman.DataReader["NZDDS"].ToString();
                    //c.SumDds = Decimal.Parse(dbman.DataReader["DDS"].ToString());
                    //c.Sum = Decimal.Parse(dbman.DataReader["DDSSUMA"].ToString());
                    allConto.Add(c);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static IEnumerable<Conto> GetAllContoWithoutDds(int firmaId, Interface.CSearchAcc cSearchAcc, int tipdnev)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allConto;
        }

        internal static void Reorder()
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(3);
                dbman.AddParameters(0, "@WM", ConfigTempoSinglenton.GetInstance().WorkDate.Month);
                dbman.AddParameters(1, "@WY", ConfigTempoSinglenton.GetInstance().WorkDate.Year);
                dbman.AddParameters(2, "@FI",  ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "SETPORNOM");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static void Reorder()");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }



        internal static IEnumerable<ValutaControl> GetAllContoValuta(int FirmaId, int ValId, DateTime fromDate, DateTime toDate,string vidval,int mode=1,string codeclient="")
        {
             Dictionary<int, Dictionary<int, string>> nomen = new Dictionary<int, Dictionary<int, string>>();
            List<ValutaControl> result = new List<ValutaControl>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                string s =
                    string.Format(
                        "SELECT c.\"Id\",c.PORNOM,c.DOCNUM,c.\"Oborot\",c.\"Date\" as DD,c.FOLDER,c.USERID,c.\"DebitAccount\",c.\"CreditAccount\",m.\"VALUE\",m.VALVAL,m.KURS,m.KURSM,m.KURSD,m.LOOKUPVAL,c.\"Reason\",c.\"Note\",lf.\"Name\",m.VALUEDATE FROM \"conto\" c" +
                        " inner join CONTOMOVEMENT m on m.CONTOID=c.\"Id\"" +
                        " inner join \"lookupsfield\" lf on m.ACCFIELDKEY=lf.\"Id\" where (c.\"FirmId\"={0} and m.ACCID={1} and \"Date\">='{2}.{3}.{4}' and \"Date\"<='{5}.{6}.{7}' and m.\"TYPE\"={8}) order by c.\"Id\" ",
                        ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        ValId,
                        fromDate.Day,
                        fromDate.Month,
                        fromDate.Year,
                        toDate.Day,
                        toDate.Month,
                        toDate.Year,
                        mode);
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                int newid = 0;
                int oldid = 0;
                int pornor = 0;
                var c = new ValutaControl();
                while (dbman.DataReader.Read())
                {
                    newid = int.Parse(dbman.DataReader["Id"].ToString());
                    string nam = dbman.DataReader["Name"].ToString();
                    if (newid!=oldid)
                    {
                        oldid = newid;
                       
                        c = new ValutaControl();
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
                    if (nam == "Сума валута")
                    {
                        c.ValSum = decimal.Parse(dbman.DataReader["VALVAL"].ToString());
                        c.Kurs = decimal.Parse(dbman.DataReader["KURS"].ToString());
                        c.MainKurs = decimal.Parse(dbman.DataReader["KURSM"].ToString());
                        c.KursDif = decimal.Parse(dbman.DataReader["KURSD"].ToString());
                    }
                    if (nam == "Вид валута")
                    {
                        c.KindVal = dbman.DataReader["VALUE"].ToString();
                    }
                    if (nam == "Контрагент")
                    {
                        c.ClienCode = dbman.DataReader["VALUE"].ToString();
                        c.NameClient= dbman.DataReader["LOOKUPVAL"].ToString();
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
            if (!string.IsNullOrWhiteSpace(codeclient))
            {
                return result.Where(e => e.KindVal == vidval && e.ClienCode == codeclient);
            }
            return result.Where(e=>e.KindVal==vidval);
        }

        internal static List<List<string>> GetUnusableClients(bool delitem)
        {
            List<List<string>> alList = new List<List<string>>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s = "SELECT a.\"Id\",a.\"Name\",a.KONTRAGENT,a.BULSTAT, a.VAT FROM \"nom_17\" a " +
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
                s = "SELECT a.\"Id\", a.KONTRAGENT,a.BULSTAT, a.VAT, a.FIRMAID,cm.NZDDS FROM \"nom_17\" a " +
                    "inner join DDSDNEV cm on a.VAT=cm.NZDDS " +
                    "where a.FIRMAID=" + Entrence.CurrentFirma.Id;
                dbman.CloseReader();                
                dbman.ExecuteReader(CommandType.Text,s);
                while (dbman.DataReader.Read())
                {
                    var cont = dbman.DataReader["KONTRAGENT"].ToString();
                    if (delitem) { alList.RemoveAll(e => e[1] == cont); } else { alList.RemoveAll(e => e[0] == cont); }
                }
                s = "SELECT a.\"Id\", a.KONTRAGENT,a.BULSTAT, a.VAT, a.FIRMAID, a.\"Name\",cm.VALUENUM FROM \"nom_17\" a " +
                    "inner join CONTOMOVEMENT cm on cm.VALUENUM=a.KONTRAGENT " +
                    "where cm.LOOKUPID=17 and a.FIRMAID=" + Entrence.CurrentFirma.Id;
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
                
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetUnusableClients(bool delitem)");
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
                    var comm = string.Format("Delete from \"nom_17\" a where a.\"Id\"={0};", item[0]);
                    sb.AppendLine(comm);
                }
                var Rez = FbBatchExecution(sb.ToString());
                if (Rez!="")
                {

                }
            }
            return alList;
        }

        internal static Dictionary<string, string> GetOborotnaVedTemplate(DateTime ToDate, DateTime FromDate)
        {
            var rez = new Dictionary<string, string>();
            var Allacc = new List<AccountsModel>(GetAllAccounts(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id));
            List<OboronaVed> gruper = new List<OboronaVed>();
            foreach (var item in Allacc)
            {
                var obor = new OboronaVed();
                obor.Id = item.Id;
                obor.Num = item.Num.ToString();
                obor.SubNum = item.SubNum.ToString();
                obor.Name = item.NameMain;
                gruper.Add(obor);
            }
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                // "SELECT p.NUM,p.NAME,p.NSD,p.NSK,p.OBD,p.OBK,p.KSD,p.KSK FROM GETALLOBOROTKA('{0}.{1}.{2}','{3}.{4}.{5}',{6}) p"
                string s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",sum(a.\"Oborot\") as debit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"DebitAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}') " +
                   "group by b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\" " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year
                   );
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {

                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    var ite = gruper.FirstOrDefault(e => e.Id == id);
                    if (ite == null)
                    {
                        var obor = new OboronaVed();
                        obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                        obor.Num = dbman.DataReader["NUM"].ToString();
                        obor.SubNum = dbman.DataReader["SubNum"].ToString();
                        obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                        obor.OD = decimal.Parse(dbman.DataReader["debit"].ToString());
                        gruper.Add(obor);
                    }
                    else
                    {
                        ite.OD = decimal.Parse(dbman.DataReader["debit"].ToString());
                    }

                }
                
                s = string.Format(
                   "SELECT b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\",sum(a.\"Oborot\") as credit FROM \"conto\" a " +
                   "inner join \"accounts\" b on b.\"Id\"=a.\"CreditAccount\" " +
                   "where (a.\"FirmId\"='{0}' and a.\"Date\">='{1}.{2}.{3}' and a.\"Date\"<='{4}.{5}.{6}')" +
                   "group by b.\"Id\",b.NAMEMAIN,b.NUM,b.\"SubNum\" " +
                   "order by b.NUM,b.\"SubNum\"",
                    ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    ToDate.Day,
                    ToDate.Month,
                    ToDate.Year,
                    FromDate.Day,
                    FromDate.Month,
                    FromDate.Year
                   );
                dbman.CloseReader();
                dbman.ExecuteReader(CommandType.Text, s);
                while (dbman.DataReader.Read())
                {
                    long id = long.Parse(dbman.DataReader["Id"].ToString());
                    var ite = gruper.FirstOrDefault(e => e.Id == id);
                    if (ite == null)
                    {
                        var obor = new OboronaVed();
                        obor.Id = int.Parse(dbman.DataReader["Id"].ToString());
                        obor.Num = dbman.DataReader["NUM"].ToString();
                        obor.SubNum = dbman.DataReader["SubNum"].ToString();
                        obor.Name = dbman.DataReader["NAMEMAIN"].ToString();
                        obor.OK = decimal.Parse(dbman.DataReader["credit"].ToString());
                        gruper.Add(obor);
                    }
                    else
                    {
                        ite.OK = decimal.Parse(dbman.DataReader["credit"].ToString());
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static List<List<string>> GetOborotnaVed(DateTime ToDate, DateTime FromDate)");
            }

            finally
            {
                dbman.Dispose();
            }
            foreach (var item in gruper)
            {
                var sm = Allacc.FirstOrDefault(e => e.Id == item.Id);
                if (sm != null)
                {
                    if (sm.TypeAccount == 1)
                    {
                        item.NSD += sm.BeginSaldoL;
                        item.KSD = item.NSD + item.OD - item.NSK - item.OK;
                    }
                    else
                    {
                        item.NSK += sm.BeginSaldoL;
                        item.KSD = item.NSK + item.OK - item.NSD - item.OD;
                    }
                }
                item.GetTemplateKeys(rez);
            }
            return rez;
        }
    }
}

    
   
