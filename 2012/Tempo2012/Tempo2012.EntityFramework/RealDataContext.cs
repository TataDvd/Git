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
       
             
        
        public static IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorAnaliticField()
        {
            List<MapAnanaliticAccToAnaliticField> conn = new List<MapAnanaliticAccToAnaliticField>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
           
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"conectoranaliticfield\"");
                while (dbman.DataReader.Read())
                {
                    conn.Add(new MapAnanaliticAccToAnaliticField
                                 {
                                     Id = int.Parse(dbman.DataReader["Id"].ToString()),
                                     AnaliticalFieldId = int.Parse(dbman.DataReader["AnaliticalFieldId"].ToString()),
                                     AnaliticalNameID = int.Parse(dbman.DataReader["AnaliticalNameID"].ToString()),
                                     Required = dbman.DataReader["REQUIRED"].ToString() == "1",
                                     SortOrder = int.Parse(dbman.DataReader["SORTORDER"].ToString())
                                 });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllConnectorAnaliticField");
            }

            finally
            {
                dbman.Dispose();
            }
            return conn;
        }
        public static IEnumerable<AccountsModel> GetAllAccounts(int firmaID,int year=-1)
        {
            List<AccountsModel> allacc = new List<AccountsModel>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string s = string.Format("select * from \"accounts\" a left outer join \"lookups\" b on a.\"PartidNum\"=b.\"Id\" "
                    + " left outer join \"analiticalaccount\" aa on aa.\"Id\"=a.\"AnaliticalNum\""
                    + " left outer join \"analiticalaccounttype\" aaa on aaa.\"Id\"=aa.\"TypeID\""
                    + " where a.\"FirmaId\"={0} and a.YY={1} order by a.NUM,a.\"SubNum\",a.\"AnaliticalNum\",a.\"PartidNum\"",
                       firmaID, year == -1 ? ConfigTempoSinglenton.GetInstance().WorkDate.Year : year);
                       dbman.ExecuteReader(CommandType.Text,
                                        s);
                while (dbman.DataReader.Read())
                {
                    int temp = 1;
                    int temp1 = 1;
                    int temp2 = 1;
                    int kol = 0;
                    int val = 0;
                    if (int.TryParse(dbman.DataReader["TypeAccount"].ToString(), out temp))
                    {
                        temp1 = temp;
                    }
                    if (int.TryParse(dbman.DataReader["ISBUDJET"].ToString(), out temp))
                    {
                        temp2 = temp;
                    }
                    if (int.TryParse(dbman.DataReader["KOL"].ToString(), out temp))
                    {
                        kol = temp;
                    }
                    if (int.TryParse(dbman.DataReader["SV"].ToString(), out temp))
                    {
                        val = temp;
                    }
                    allacc.Add(new AccountsModel
                                   {
                                       Id = int.Parse(dbman.DataReader["Id"].ToString()),
                                       NameMain = dbman.DataReader["NameMain"].ToString(),
                                       NameMainEng = dbman.DataReader["NameMainEng"].ToString(),
                                       NameSub = dbman.DataReader["NameSub"].ToString(),
                                       SubNum = int.Parse(dbman.DataReader["SubNum"].ToString()),
                                       AnaliticalNum = int.Parse(dbman.DataReader["AnaliticalNum"].ToString()),
                                       FirmaId = int.Parse(dbman.DataReader["FirmaId"].ToString()),
                                       LevelAccount = int.Parse(dbman.DataReader["LevelAccount"].ToString()),
                                       NameSubEng = dbman.DataReader["NameSubEng"].ToString(),
                                       Num = int.Parse(dbman.DataReader["Num"].ToString()),
                                       PartidNum = int.Parse(dbman.DataReader["PartidNum"].ToString()),
                                       LookUpName = dbman.DataReader["Name"].ToString(),
                                       SaldoKL = decimal.Parse(dbman.DataReader["SALDO"].ToString()),
                                       SaldoKV = decimal.Parse(dbman.DataReader["SALDOVALUTA"].ToString()),
                                       TypeAnaliticalKey =
                                           int.Parse(dbman.DataReader["TYPEANALITICALKEY"].ToString()),
                                       SaldoDL = decimal.Parse(dbman.DataReader["SALDODEBIT"].ToString()),
                                       SaldoDV =
                                           decimal.Parse(dbman.DataReader["SALDODEBITVALUTA"].ToString()),
                                       TypeAccountEx = temp2,
                                       TypeAccount = temp1,
                                       TypeSaldo = int.Parse(dbman.DataReader["TypeSaldo"].ToString()),
                                       SaldoDK = decimal.Parse(dbman.DataReader["SALDODK"].ToString()),
                                       SaldoKK = decimal.Parse(dbman.DataReader["SALDOKK"].ToString()),
                                       OborotDK = decimal.Parse(dbman.DataReader["OBOROTDK"].ToString()),
                                       OborotDL = decimal.Parse(dbman.DataReader["OBOROTDL"].ToString()),
                                       OborotDV = decimal.Parse(dbman.DataReader["OBOROTDV"].ToString()),
                                       OborotKK = decimal.Parse(dbman.DataReader["OBOROTKK"].ToString()),
                                       OborotKL = decimal.Parse(dbman.DataReader["OBOROTL"].ToString()),
                                       OborotKV = decimal.Parse(dbman.DataReader["OBOROTKV"].ToString()),
                                       Kol=kol,
                                       Val=val
                                   });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllAccounts");
            }

            finally
            {
                dbman.Dispose();
            }
            return allacc;
        }
        public static IEnumerable<LookUpSpecific> GetKindDocuments()
        {
            List<LookUpSpecific> result = new List<LookUpSpecific>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"nomenclatures\"");
                while (dbman.DataReader.Read())
                {
                    result.Add(new LookUpSpecific
                                   {
                                       Id = int.Parse(dbman.DataReader["Id"].ToString()),
                                       Name = string.Format("{0} {1}",dbman.DataReader["CodeId"],dbman.DataReader["Name"]),
                                       CodetId = dbman.DataReader["CodeId"].ToString()
                                   });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetKindDocuments");
            }

            finally
            {
                dbman.Dispose();
            }
           
            return result;
        }
        public static IEnumerable<Conto> GetAllConto(int firmaId,long page=-1)
        {
            List<Conto> allConto = new List<Conto>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text,
                    String.Format(
                        "select * from \"conto\" a where \"FirmId\"={0} and \"Date\">='1.{2}.{1}' and \"Date\"<='{3}.{2}.{1}' order by a.\"Id\"",
                                             firmaId,
                                            ConfigTempoSinglenton.GetInstance().WorkDate.Year, ConfigTempoSinglenton.GetInstance().WorkDate.Month, GetEndDate(ConfigTempoSinglenton.GetInstance().WorkDate.Month, ConfigTempoSinglenton.GetInstance().WorkDate.Year)));
                LoadConto(allConto,dbman);
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllConto(int firmaId,long page=-1)");
            }

            finally
            {
                dbman.Dispose();
            }
            
            return allConto;
        }

        public static IEnumerable<Conto> GetAllConto(int firmaId, ISearchAcc pSearcAcc,int startingIndex, int numberOfRecord)
        {
            List<Conto> allConto = new List<Conto>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                
                dbman.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("select First {0} SKIP {1} * from \"conto\" c",numberOfRecord,startingIndex));
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
                {
                    string expresetion = "{0}";
                    if (pSearcAcc.NumDoc.StartsWith("*"))
                    {
                        expresetion = string.Format("%{0}", expresetion);
                    }
                    if (pSearcAcc.NumDoc.EndsWith("*"))
                    {
                        expresetion = string.Format("{0}%", expresetion);
                    }
                    sb.AppendFormat(" AND DOCNUM LIKE "+ expresetion, pSearcAcc.NumDoc);
                }
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
                sb.AppendFormat(" order by c.\"Id\" desc");
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
        public static IEnumerable<CartotecaCredit> GetAllCartotecaCredit()
        {
            List<CartotecaCredit> Cc = new List<CartotecaCredit>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"cities\"");
                while (dbman.DataReader.Read())
                {
                    Cc.Add(new CartotecaCredit
                               {
                                   Id = int.Parse(dbman.DataReader["Id"].ToString()),
                                   ContoId = int.Parse(dbman.DataReader["ContoId"].ToString()),
                                   TitleValue = dbman.DataReader["TitleValue"].ToString(),
                                   TypeValue = dbman.DataReader["TypeValue"].ToString(),
                                   Value = dbman.DataReader["Value"].ToString()
                               });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllCartotecaCredit()");
            }

            finally
            {
                dbman.Dispose();
            }
            if (Cc.Count == 0)
            {
                Cc.Add(new CartotecaCredit
                           {Id = 1, ContoId = 1, TitleValue = "Сума лева", TypeValue = "money", Value = "100.50"});
                Cc.Add(new CartotecaCredit
                           {Id = 2, ContoId = 2, TitleValue = "Сума лева", TypeValue = "money", Value = "200.50"});

            }
            return Cc;
        }
        public static IEnumerable<CartotecaDebit> GetAllCartotecaDebit()
        {
            List<CartotecaDebit> Cc = new List<CartotecaDebit>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"cities\"");
                while (dbman.DataReader.Read())
                {
                    Cc.Add(new CartotecaDebit
                               {
                                   Id = int.Parse(dbman.DataReader["Id"].ToString()),
                                   ContoId = int.Parse(dbman.DataReader["ContoId"].ToString()),
                                   TitleValue = dbman.DataReader["TitleValue"].ToString(),
                                   TypeValue = dbman.DataReader["TypeValue"].ToString(),
                                   Value = dbman.DataReader["Value"].ToString()
                               });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllCartotecaDebit");
            }

            finally
            {
                dbman.Dispose();
            }
            if (Cc.Count == 0)
            {
                Cc.Add(new CartotecaDebit
                           {Id = 1, ContoId = 1, TitleValue = "Сума лева", TypeValue = "money", Value = "50.50"});
                Cc.Add(new CartotecaDebit
                           {Id = 2, ContoId = 2, TitleValue = "Сума лева", TypeValue = "money", Value = "150.50"});
            }
            return Cc;
        }
        public static IEnumerable<LookUpSpecific> GetNationalAccounts()
        {
            List<LookUpSpecific> result = new List<LookUpSpecific>();
            string na = "na";
            if (ConfigTempoSinglenton.GetInstance().CurrentFirma.AccType == 1) na = "na1";
            if (ConfigTempoSinglenton.GetInstance().CurrentFirma.AccType == 2) na = "na2";
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, string.Format("select * from \"{0}\" order by \"CodetId\"", na));
                while (dbman.DataReader.Read())
                {
                    result.Add(new LookUpSpecific
                                   {
                                       Id = int.Parse(dbman.DataReader["Id"].ToString()),
                                       Name = dbman.DataReader["Name"].ToString(),
                                       CodetId = dbman.DataReader["CodetId"].ToString(),
                                       TypeAcc = int.Parse(dbman.DataReader["AP"].ToString())
                                   });

                }


            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<LookUpSpecific> GetNationalAccounts()");
            }

            finally
            {
                dbman.Dispose();
            }

            return result;
        }
        public static bool DeleteAccount(int id)
        {

            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(2);
                dbman.AddParameters(0, "@Id", id);
                dbman.AddOutputParameters(1, "@YES", 0);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "DELETEACCOUNT");
                var test = dbman.Parameters[1].Value;
                if (test != null)
                {
                    int a = (int)test;
                    result=a==0;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static bool DeleteAccount(int id)");
                result = false;
            }
            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorTypeField()
        {
            List<MapAnanaliticAccToAnaliticField> conn = new List<MapAnanaliticAccToAnaliticField>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, "select * from \"MPATYPETOAFIELD\"");
                while (dbman.DataReader.Read())
                {
                    conn.Add(new MapAnanaliticAccToAnaliticField
                                 {
                                     AnaliticalFieldId = int.Parse(dbman.DataReader["ATYPEID"].ToString()),
                                     AnaliticalNameID = int.Parse(dbman.DataReader["AFIELDID"].ToString())
                                 });

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<MapAnanaliticAccToAnaliticField> GetAllConnectorTypeField()");
            }

            finally
            {
                dbman.Dispose();
            }
            //if (conn.Count == 0)
            //{
            //    conn.AddRange(new List<MapAnanaliticAccToAnaliticField>
            //                      {
            //                          new MapAnanaliticAccToAnaliticField {Id = 1, AnaliticalFieldId = 1, AnaliticalNameID = 1},
            //                          new MapAnanaliticAccToAnaliticField {Id = 2, AnaliticalFieldId = 2, AnaliticalNameID = 1},
            //                          new MapAnanaliticAccToAnaliticField {Id = 3, AnaliticalFieldId = 2, AnaliticalNameID = 2},
            //                          new MapAnanaliticAccToAnaliticField {Id = 4, AnaliticalFieldId = 3, AnaliticalNameID = 1},
            //                          new MapAnanaliticAccToAnaliticField {Id = 5, AnaliticalFieldId = 3, AnaliticalNameID = 3},
            //                          new MapAnanaliticAccToAnaliticField {Id = 6, AnaliticalFieldId = 4, AnaliticalNameID = 1},
            //                          new MapAnanaliticAccToAnaliticField {Id = 7, AnaliticalFieldId = 4, AnaliticalNameID = 2},
            //                          new MapAnanaliticAccToAnaliticField {Id = 8, AnaliticalFieldId = 4, AnaliticalNameID = 3},
            //                          new MapAnanaliticAccToAnaliticField {Id = 9, AnaliticalFieldId = 5, AnaliticalNameID = 1},
            //                          new MapAnanaliticAccToAnaliticField {Id = 10, AnaliticalFieldId = 5, AnaliticalNameID = 4},
            //                          new MapAnanaliticAccToAnaliticField {Id = 11, AnaliticalFieldId = 5, AnaliticalNameID = 5},
            //                          new MapAnanaliticAccToAnaliticField {Id = 12, AnaliticalFieldId = 6, AnaliticalNameID = 1},
            //                          new MapAnanaliticAccToAnaliticField {Id = 13, AnaliticalFieldId = 6, AnaliticalNameID = 2},
            //                          new MapAnanaliticAccToAnaliticField {Id = 14, AnaliticalFieldId = 6, AnaliticalNameID = 4},
            //                          new MapAnanaliticAccToAnaliticField {Id = 15, AnaliticalFieldId = 6, AnaliticalNameID = 5},
            //                          new MapAnanaliticAccToAnaliticField {Id = 16, AnaliticalFieldId = 7, AnaliticalNameID = 1}
            //                      });

            //}
            return conn;
        }

        internal static IEnumerable<Conto> GetPrevConto(int firmaId, ISearchAcc pSearcAcc)
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
                    sb.AppendFormat(" AND c.\"Id\"<'{0}' AND c.\"Id\">'{1}'", pSearcAcc.Id,int.Parse(pSearcAcc.Id)-1000);

                }
                if (!String.IsNullOrWhiteSpace(pSearcAcc.UserId))
                {
                    sb.AppendFormat(" AND c.USERID='{0}'", pSearcAcc.UserId);

                }
                sb.AppendFormat(" order by c.\"Id\" desc");
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
    

        public static IEnumerable<TableField> GetSysLookUpFields(int id)
        {
            string command =
                string.Format(
                    "SELECT f.\"Id\", f.\"Name\", f.\"DBField\", f.NAMEENG,f.\"Length\",f.RKEYNAME,f.RFIELDNAME,f.RTABLENAME,f.RCODELOOKUP,d.ISREQUARED,d.ISUNIQUE,d.TN FROM \"syslookups\" a Inner join \"syslookupsdetails\" d on a.\"Id\"=d.\"IdLookUp\" Inner join \"sysfield\" f on d.\"IdLookField\"=f.\"Id\" Where a.\"Id\"={0} order by d.SORTORDER",
                    id);
            List<TableField> list = new List<TableField>();
            if (id == 10 || id == 12)
            {
                list.Add(new TableField
                         {DbField = "integer", Name = "Ключ", Id = 0, IsRequared = true, Length = 10, NameEng = "ID"});
            }
            else
            {
                list.Add(new TableField
                         {DbField = "integer", Name = "Ключ", Id = 0, IsRequared = true, Length = 10, NameEng = "Id"});
            }


            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.ExecuteReader(CommandType.Text, command);
                while (dbman.DataReader.Read())
                {
                    int test;
                    TableField newtable = new TableField();

                    newtable.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    newtable.Name = dbman.DataReader["Name"].ToString();
                    newtable.NameEng = dbman.DataReader["NameEng"].ToString();
                    newtable.DbField = dbman.DataReader["DbField"].ToString();
                    //newtable.IsRequared = int.Parse(dbman.DataReader["ISREQUARED"].ToString()) == 1 ? true : false;
                    //newtable.IsUnique = int.Parse(dbman.DataReader["ISUNIQUE"].ToString()) == 1 ? true : false;
                    newtable.Length = int.Parse(dbman.DataReader["Length"].ToString());
                    newtable.RFIELDKEY = dbman.DataReader["RKEYNAME"].ToString();
                    newtable.RFIELDNAME = dbman.DataReader["RFIELDNAME"].ToString();
                    newtable.RTABLENAME = dbman.DataReader["RTABLENAME"].ToString();
                    newtable.RCODELOOKUP = int.TryParse(dbman.DataReader["RCODELOOKUP"].ToString(), out test)
                                               ? test
                                               : 0;
                    newtable.Tn = dbman.DataReader["TN"].ToString();
                    newtable.GROUP = 0;
                                                     
                    list.Add(newtable);
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static IEnumerable<TableField> GetSysLookUpFields(int id)");
            }

            finally
            {
                dbman.Dispose();
            }

            return list;
        }
        public static LookupModel GetSysLookup(int numer)
        {
            return new LookupModel(GetSysLookUpFields(numer).ToList(),
                                   GetSystemLookups().FirstOrDefault(e => e.Id == numer));
        }
       

        public static bool DeleteAt(long p)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.ExecuteNonQuery(CommandType.Text, string.Format(Commands.DeleteATConector, p));
                dbman.ExecuteNonQuery(CommandType.Text, string.Format(Commands.DeleteAT, p));
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static bool DeleteAt(long p)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static bool SaveAt(AnaliticalAccountType currentAllTypeAccount,
                                  System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> fieldsSelected)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(4);
                dbman.AddParameters(0, "@Name", currentAllTypeAccount.Name);
                dbman.AddParameters(1, "@SL", currentAllTypeAccount.Sl ? 1 : 0);
                dbman.AddParameters(2, "@SV", currentAllTypeAccount.Sv ? 1 : 0);
                dbman.AddParameters(3, "@KOL", currentAllTypeAccount.Kol ? 1 : 0);
                dbman.ExecuteNonQuery(CommandType.Text, Commands.insertAT);
                var newid = dbman.ExecuteScalar(CommandType.Text,
                                                    "select gen_id(genanaliticalaccounttype, 0) from rdb$database");
                currentAllTypeAccount.Id = ((long?) newid).GetValueOrDefault();
                foreach (var field in fieldsSelected)
                {
                    string commands =
                        string.Format(
                            "INSERT INTO \"MPATYPETOAFIELD\" (ATYPEID,AFIELDID) VALUES ({0},{1})",
                            currentAllTypeAccount.Id,
                            field.Id);
                    dbman.ExecuteNonQuery(CommandType.Text, commands);

                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"public static bool SaveAt(AnaliticalAccountType currentAllTypeAccount,System.Collections.ObjectModel.ObservableCollection<AnaliticalFields> fieldsSelected)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static bool UpdateAt(AnaliticalAccountType currentAllTypeAccount,
                                    ObservableCollection<AnaliticalFields> fieldsSelected)
        {
            bool result = true;

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(5);
                dbman.AddParameters(0, "@Name", currentAllTypeAccount.Name);
                dbman.AddParameters(1, "@Id", currentAllTypeAccount.Id);
                dbman.AddParameters(2, "@SL", currentAllTypeAccount.Sl ? 1 : 0);
                dbman.AddParameters(3, "@SV", currentAllTypeAccount.Sv ? 1 : 0);
                dbman.AddParameters(4, "@KOL", currentAllTypeAccount.Kol ? 1 : 0);
                dbman.ExecuteNonQuery(CommandType.Text, Commands.UpdateAT);
                dbman.ExecuteNonQuery(CommandType.Text,
                                          string.Format(Commands.DeleteATConector, currentAllTypeAccount.Id));
                foreach (var field in fieldsSelected)
                {
                    string commands =
                        string.Format(
                            "INSERT INTO \"MPATYPETOAFIELD\" (ATYPEID,AFIELDID) VALUES ({0},{1})",
                            currentAllTypeAccount.Id,
                            field.Id);
                    dbman.ExecuteNonQuery(CommandType.Text, commands);

                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message,"public static bool UpdateAt(AnaliticalAccountType currentAllTypeAccount,ObservableCollection<AnaliticalFields> fieldsSelected)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static bool UpdateAccount(AccountsModel acc, bool p,ObservableCollection<AnaliticalFields> SelectedAnaliticalFields,out string errormesage,int year=-1)
        {
            bool result = true;
            errormesage = "OK";
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.CreateParameters(24);
                dbman.AddParameters(0, "@NameMain", acc.NameMain);
                dbman.AddParameters(1, "@NameMainEng", acc.NameMainEng);
                dbman.AddParameters(2, "@NameSub", acc.NameSub);
                dbman.AddParameters(3, "@NameSubEng", acc.NameSubEng);
                dbman.AddParameters(4, "@SubNum", acc.SubNum);
                dbman.AddParameters(5, "@AnaliticalNum", acc.AnaliticalNum);
                dbman.AddParameters(6, "@PartidNum", acc.PartidNum);
                dbman.AddParameters(7, "@TypeAccount", acc.TypeAccount);
                dbman.AddParameters(8, "@LevelAccount", acc.LevelAccount);
                dbman.AddParameters(9, "@TypeSaldo", acc.TypeSaldo);
                dbman.AddParameters(10, "@FirmaId", ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                dbman.AddParameters(13, "@Id", acc.Id);
                dbman.AddParameters(14, "@Num", acc.Num);
                dbman.AddParameters(15, "@SALDO", acc.SaldoKL);
                dbman.AddParameters(16, "@SALDOVALUTA", acc.SaldoKV);
                dbman.AddParameters(17, "@TYPEANALITICALKEY", acc.TypeAnaliticalKey);
                dbman.AddParameters(18, "@SALDODEBIT", acc.SaldoDL);
                dbman.AddParameters(19, "@SALDODEBITVALUTA", acc.SaldoDV);
                dbman.AddParameters(20, "@ISBUDJET", acc.TypeAccountEx);
                dbman.AddParameters(21, "@SALDODK", acc.SaldoDK);
                dbman.AddParameters(22, "@SALDOKK", acc.SaldoKK);
                dbman.AddParameters(23, "@YY", year==-1?ConfigTempoSinglenton.GetInstance().WorkDate.Year:year);
                if (p)
                {
                    dbman.AddParameters(11, "@IsNew", 1);
                    dbman.AddOutputParameters(12, "@NewId", acc.Id);

                    dbman.ExecuteNonQuery(CommandType.StoredProcedure, Commands.InsertAccount);
                    var test = dbman.Parameters[12].Value;
                    if (test != null)
                    {
                        acc.Id = (int) test;
                    }
                    else
                    {
                        acc.Id = -1;
                    }
                    if (acc.Id == -1)
                    {
                        errormesage = "Има вече въведена същата сметка в сметкоплана!";
                        result = false;
                    }
                }
                else
                {
                    dbman.AddParameters(11, "@IsNew", 0);
                    dbman.AddOutputParameters(12, "@NewId", acc.Id);
                    dbman.ExecuteNonQuery(CommandType.StoredProcedure, Commands.InsertAccount);
                }
                if (SelectedAnaliticalFields != null)
                {
                    dbman.ExecuteNonQuery(CommandType.Text,
                        string.Format("Delete from MAPACCTOLOOKUP where ACCOUNTS_ID={0}", acc.Id));
                    foreach (var item in SelectedAnaliticalFields)
                    {
                        if (item.IdLookUp > 0)
                        {
                            string cmd =
                                string.Format(
                                    "INSERT INTO MAPACCTOLOOKUP (ACCOUNTS_ID, LOOKUP_ID, FIELDLOOKUP_ID, ANALITIC_ID, ANALITIC_FIELD_ID) VALUES ({0},{1},{2},{3},{4})",
                                    acc.Id, item.IdLookUp, item.IdField, acc.AnaliticalNum, item.Id);
                            dbman.ExecuteNonQuery(CommandType.Text, cmd);

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                errormesage = ex.Message;
                Logger.Instance().WriteLogError(ex.Message, "public static bool UpdateAccount(AccountsModel acc, bool p,ObservableCollection<AnaliticalFields> SelectedAnaliticalFields,out string errormesage)");
                result = false;
            }

            finally
            {
                dbman.Dispose();
                LookUpRepository.Instance.RemoveAf(acc.Id);
            }

            return result;
        }

        public static void LoadMapToLookUps(ObservableCollection<AnaliticalFields> selectedAnaliticalFields, int accId,long analiticId)
        {

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                foreach (AnaliticalFields selectedAnaliticalField in selectedAnaliticalFields)
                {
                    string cmd =
                        string.Format(
                            "select * from SELECTMAPAFIELDTOLOOKUP a where a.ACCOUNTS_ID={0} AND a.ANALITIC_ID={1} AND a.ANALITIC_FIELD_ID={2}",
                            accId, analiticId, selectedAnaliticalField.Id);
                    dbman.ExecuteReader(CommandType.Text, cmd);
                    while (dbman.DataReader.Read())
                    {
                        selectedAnaliticalField.IdField = int.Parse(dbman.DataReader["FIELDLOOKUP_ID"].ToString());
                        selectedAnaliticalField.IdLookUp = int.Parse(dbman.DataReader["LOOKUP_ID"].ToString());
                        selectedAnaliticalField.NameFieldLookUp = dbman.DataReader["NAMEFIELDLOOKUP"].ToString();
                        selectedAnaliticalField.NameLookUp = dbman.DataReader["NAMELOOKUP"].ToString();
                    }
                    dbman.DataReader.Close();
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void LoadMapToLookUps(ObservableCollection<AnaliticalFields> selectedAnaliticalFields, int accId,long analiticId)");
            }

            finally
            {
                dbman.Dispose();
            }


        }

        public static Dictionary<string, List<string>> LoadMapToLookUps(int accId, long analiticId)
        {
            List<AnaliticalFields> sAnaliticalFieldses = new List<AnaliticalFields>();
            Dictionary<string, List<string>> elements = new Dictionary<string, List<string>>();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();

                string cmd =
                    string.Format(
                        "select * from SELECTMAPAFIELDTOLOOKUPTNFN a where a.ACCOUNTS_ID={0} AND a.ANALITIC_ID={1}",
                        accId, analiticId);
                dbman.ExecuteReader(CommandType.Text, cmd);

                while (dbman.DataReader.Read())
                {
                    AnaliticalFields selectedAnaliticalField = new AnaliticalFields()
                                                                   {
                                                                       IdField =
                                                                           int.Parse(
                                                                               dbman.DataReader["FIELDLOOKUP_ID"].
                                                                                   ToString()),
                                                                       IdLookUp =
                                                                           int.Parse(
                                                                               dbman.DataReader["LOOKUP_ID"].
                                                                                   ToString()),
                                                                       NameFieldLookUp =
                                                                           dbman.DataReader["FN"].ToString(),
                                                                       NameLookUp =
                                                                           dbman.DataReader["TN"].ToString(),
                                                                       Name =
                                                                           dbman.DataReader["NAMELOOKUP"].ToString()
                                                                   };
                    sAnaliticalFieldses.Add(selectedAnaliticalField);
                }

                foreach (var item in sAnaliticalFieldses)
                {
                    //cmd =
                    //    string.Format(
                    //        "select * from \"{0}\"",
                    //        item.NameLookUp);
                    //dbman.ExecuteReader(CommandType.Text, cmd);
                    //
                    //while (dbman.DataReader.Read())
                    //{
                    //    StringBuilder s = new StringBuilder();
                    //    for (int i = 1; i < 3; i++)
                    //    {
                    //        s.AppendFormat(" {0}", dbman.DataReader[i]);
                    //    }
                    //    element.Add(s.ToString());
                    //}
                    List<string> element = new List<string>();
                    element.Add(string.Format("Връзка към номенклатура {0} ",item.Name));
                    elements.Add(item.Name, element);
                }

            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static Dictionary<string, List<string>> LoadMapToLookUps(int accId, long analiticId)");
            }

            finally
            {
                dbman.Dispose();
            }
            return elements;
        }

        public static void SaveSaldos(ObservableCollection<SaldosModel> Fields, int accID, int lookUpId)
        {

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.ExecuteNonQuery(CommandType.Text, string.Format(Commands.DeleteSaldos, accID, lookUpId));
                foreach (SaldosModel observableCollection in Fields)
                {

                    dbman.ExecuteNonQuery(CommandType.Text,
                                              string.Format(Commands.InsertSaldos, observableCollection.SaldoDebit,
                                                            observableCollection.SaldoCredit, 0, 0, accID, lookUpId,
                                                            observableCollection.Nom));

                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void SaveSaldos(ObservableCollection<SaldosModel> Fields, int accID, int lookUpId)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }

        }

        public static SaldosModel GetSaldo(int _currentAccId, int lookpId, string rowId)
        {
            SaldosModel result = new SaldosModel();

            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            dbman.Open();

            string cmd =
                string.Format(
                    "select * from SALDOS a where a.ACCID={0} AND a.PARTIDID={1} AND a.ANALITICID={2}",
                    _currentAccId, lookpId, rowId);
            dbman.ExecuteReader(CommandType.Text, cmd);

            if (dbman.DataReader.Read())
            {
                result.SaldoDebit = decimal.Parse(dbman.DataReader["SALDODEBIT"].ToString());
                result.SaldoCredit = decimal.Parse(dbman.DataReader["SALDOKREDIT"].ToString());
                result.SaldoValutaDebit = decimal.Parse(dbman.DataReader["SALDOVALUTADEBIT"].ToString());
                result.SaldoValutaCredit = decimal.Parse(dbman.DataReader["SALDOVALUTAKREDIT"].ToString());

            }
            return result;
        }

        public static void GetAllMovementsSaldos(int accid, int accnum, int firmid, out decimal sumalvksub, out decimal sumalvdsub)
        {
            sumalvksub = 0;
            sumalvdsub = 0;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                
                string command = string.Format("SELECT sum(a.SALDO) sumk,sum(a.SALDODEBIT) sumd FROM \"accounts\" a where a.NUM={0} and a.\"SubNum\">0 and a.\"FirmaId\"={1} and a.YY={2}",
                                        accnum, firmid, ConfigTempoSinglenton.GetInstance().WorkDate.Year);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    decimal test = 0;
                    sumalvdsub =decimal.TryParse(dbman.DataReader["sumd"].ToString(),out test)?test:0;
                    sumalvksub =decimal.TryParse(dbman.DataReader["sumk"].ToString(),out test)?test:0;
                    if (Math.Abs(sumalvdsub) > Math.Abs(sumalvksub))
                    {
                        sumalvdsub = sumalvdsub - sumalvksub;
                        sumalvksub = 0;
                    }
                    else
                    {
                        sumalvksub = sumalvksub - sumalvdsub;
                        sumalvdsub = 0;
                    }

                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void GetAllMovementsSaldos(int accid, int accnum, int firmid, out decimal sumalvksub, out decimal sumalvdsub)");
            }

            finally
            {
                dbman.Dispose();
            }

            
        }

        public static void GetAllMovementsSalosVK(int accid, out string sumalvd, out string sumalvk, out string sumavd, out string sumavk, out string sumakd, out string sumakk)
        {
            sumalvk = Vf.LevFormatUI;
            sumalvd = Vf.LevFormatUI;
            sumavk = Vf.ValFormatUI;
            sumavd = Vf.ValFormatUI;
            sumakk = Vf.KolFormatUI;
            sumakd = Vf.KolFormatUI;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string command =
                    string.Format(
                        "SELECT sum(VALUEMONEY) sumk,sum(valued) sumd,sum(VALVALK) sumvk,sum(VALVALD) sumvd,sum(VALVALK) sumvk,sum(VALKOLD) sumkd,sum(VALKOLK) sumkk FROM MOVEMENT a where accid={0}",
                        accid);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    sumalvd =string.Format(Vf.LevFormat,decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumd"].ToString())?"0.00":dbman.DataReader["sumd"].ToString()));
                    sumalvk =string.Format(Vf.LevFormat,decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumk"].ToString())?"0.00":dbman.DataReader["sumk"].ToString()));
                    sumavd = string.Format(Vf.ValFormat,decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumvd"].ToString())?"0.00":dbman.DataReader["sumvd"].ToString()));
                    sumavk = string.Format(Vf.ValFormat,decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumvk"].ToString())?"0.00":dbman.DataReader["sumvk"].ToString()));
                    sumakd = string.Format(Vf.KolFormat,decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumkd"].ToString())?"0.00":dbman.DataReader["sumkd"].ToString()));
                    sumakk = string.Format(Vf.KolFormat,decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumkk"].ToString())?"0.00":dbman.DataReader["sumkk"].ToString()));
                    decimal test = 0;
                    decimal sumalvdsub = decimal.TryParse(sumavd, out test) ? test : 0;
                    decimal sumalvksub = decimal.TryParse(sumavk, out test) ? test : 0;
                    if (Math.Abs(sumalvdsub) > Math.Abs(sumalvksub))
                    {
                        sumalvdsub = sumalvdsub - sumalvksub;
                        sumalvksub = 0;
                        sumavd = sumalvdsub.ToString();
                        sumavk = "0.00";
                    }
                    else
                    {
                        sumalvksub = sumalvksub - sumalvdsub;
                        sumalvdsub = 0;
                        sumavk = sumalvksub.ToString();
                        sumavd = "0.00";

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void GetAllMovementsSalosVK(int accid, out string sumalvd, out string sumalvk, out string sumavd, out string sumavk, out string sumakd, out string sumakk)");
            }

            finally
            {
                dbman.Dispose();
            }
        }

        public static List<List<string>> GetAllMovements(int accid,int accnum,int firmid, out string sumalvk, out string sumalvd, out string sumalvksub, out string sumalvdsub)
        {
            List<List<string>> allmovement = new List<List<string>>();
            sumalvk = Vf.LevFormatUI;
            sumalvd = Vf.LevFormatUI;
            sumalvksub =Vf.LevFormatUI;
            sumalvdsub = Vf.LevFormatUI;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string command =
                    string.Format(
                        "SELECT * FROM MOVEMENT a inner join \"lookupsfield\" b on a.ACCFIELDKEY=b.\"Id\" where accid={0} order by a.\"group\",b.\"GROUP\",b.\"Id\"",
                        accid);
                dbman.ExecuteReader(CommandType.Text, command);
                List<string> row = new List<string>();
                List<string> title = new List<string>();
                int group = -1;
                int curqroup = -1;
                bool firstgroup = false;
                bool havesaldo = false;
                while (dbman.DataReader.Read())
                {
                    havesaldo = true;
                    curqroup = int.Parse(dbman.DataReader["group"].ToString());
                    if (curqroup != group)
                    {
                        if (group == -1)
                        {
                            group = curqroup;
                        }
                        else
                        {
                            if (!firstgroup)
                            {
                                title.Add("Група");
                                allmovement.Add(title);
                                firstgroup = true;
                            }
                            row.Add(group.ToString());
                            allmovement.Add(row);
                            row = new List<string>();
                            group = curqroup;
                        }
                    }

                    if (dbman.DataReader["NAMEENG"].ToString() == "SUMALV" || dbman.DataReader["NAMEENG"].ToString() == "COLICHESTVO" || dbman.DataReader["NAMEENG"].ToString() == "SUMAVALUTA")
                    {
                        if (!firstgroup) title.Add(dbman.DataReader["Name"].ToString().Replace('.', ' ') + " дебит");
                        if (!firstgroup) title.Add(dbman.DataReader["Name"].ToString().Replace('.',' ')+" кредит");
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("SUMALV"))
                        {
                            decimal sumd = 0;
                            row.Add(string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["VALUED"].ToString(), out sumd) ? sumd : 0));
                            row.Add(string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["VALUEMONEY"].ToString(), out sumd) ? sumd : 0));
                        }
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("COLICHESTVO"))
                        {
                            decimal sumd = 0;
                            row.Add(string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["VALKOLD"].ToString(), out sumd) ? sumd : 0));
                            row.Add(string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["VALKOLK"].ToString(), out sumd) ? sumd : 0));
                        }
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("SUMAVALUTA"))
                        {
                            decimal sumd = 0;
                            row.Add(string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["VALVALD"].ToString(), out sumd) ? sumd : 0));
                            row.Add(string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["VALVALK"].ToString(), out sumd) ? sumd : 0));
                        }
                        
                    }
                    else
                    {
                        if (!firstgroup) title.Add(dbman.DataReader["Name"].ToString());
                        row.Add(dbman.DataReader["VALUE"].ToString());
                    }

                }
                if (!firstgroup&&havesaldo)
                {
                    title.Add("Група");
                    allmovement.Add(title);
                    firstgroup = true;
                }
                if (havesaldo)
                {
                    row.Add(curqroup.ToString());
                    allmovement.Add(row);
                }
                dbman.DataReader.Close();
                command = string.Format("SELECT sum(VALUEMONEY) sumk,sum(valued) sumd FROM MOVEMENT a where accid={0}",
                                        accid);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    decimal sumd=0;
                    sumalvd = string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["sumd"].ToString(),out sumd) ? sumd : 0);
                    sumalvd = string.Format(Vf.LevFormat, decimal.TryParse(dbman.DataReader["sumk"].ToString(),out sumd) ? sumd : 0);
                }
                dbman.DataReader.Close();
                command = string.Format("SELECT sum(a.SALDO) sumk,sum(a.SALDODEBIT) sumd FROM \"accounts\" a where a.NUM={0} and a.\"SubNum\">0 and a.\"FirmaId\"={1} and a.YY={2}",
                                        accnum, firmid, ConfigTempoSinglenton.GetInstance().WorkDate.Year);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    var debit = dbman.DataReader["sumd"].ToString();
                    if (string.IsNullOrWhiteSpace(debit))
                    {
                        debit = Vf.LevFormatUI;
                    }
                    var credit = dbman.DataReader["sumd"].ToString();
                    if (string.IsNullOrWhiteSpace(credit))
                    {
                        credit = Vf.LevFormatUI;
                    }
                    sumalvdsub = string.Format(Vf.LevFormat,decimal.Parse(debit));
                    sumalvksub = string.Format(Vf.LevFormat,decimal.Parse(credit));
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static List<List<string>> GetAllMovements(int accid,int accnum,int firmid, out string sumalvk, out string sumalvd, out string sumalvksub, out string sumalvdsub)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allmovement;
        }
        public static List<List<string>> GetAllMovementsDetail(int accid, int accnum, int firmid, out string sumalvk, out string sumalvd, out string sumalvksub, out string sumalvdsub)
        {
            List<List<string>> allmovement = new List<List<string>>();
            sumalvk = Vf.LevFormatUI;
            sumalvd = Vf.LevFormatUI;
            sumalvksub = Vf.LevFormatUI;
            sumalvdsub = Vf.LevFormatUI;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string command =
                    string.Format(
                        "SELECT * FROM MOVEMENT a inner join \"lookupsfield\" b on a.ACCFIELDKEY=b.\"Id\" where accid={0} order by a.\"group\",a.SORTORDER",
                        accid);
                dbman.ExecuteReader(CommandType.Text, command);
                List<string> row = new List<string>();
                List<string> title = new List<string>();
                int group = -1;
                int curqroup = -1;
                bool firstgroup = false;
                bool havesaldo = false;
                while (dbman.DataReader.Read())
                {
                    havesaldo = true;
                    curqroup = int.Parse(dbman.DataReader["group"].ToString());
                    var lk=int.Parse(dbman.DataReader["LOOKUPID"].ToString());
                    if (curqroup != group)
                    {
                        if (group == -1)
                        {
                            group = curqroup;
                        }
                        else
                        {
                            if (!firstgroup)
                            {
                                title.Add("Група");
                                allmovement.Add(title);
                                firstgroup = true;
                            }
                            row.Add(group.ToString());
                            allmovement.Add(row);
                            row = new List<string>();
                            group = curqroup;
                        }
                    }

                    if (dbman.DataReader["NAMEENG"].ToString() == "SUMALV" || dbman.DataReader["NAMEENG"].ToString() == "COLICHESTVO" || dbman.DataReader["NAMEENG"].ToString() == "SUMAVALUTA")
                    {
                        if (!firstgroup) title.Add(dbman.DataReader["Name"].ToString().Replace('.', ' ') + " дебит");
                        if (!firstgroup) title.Add(dbman.DataReader["Name"].ToString().Replace('.', ' ') + " кредит");
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("SUMALV"))
                        {
                            row.Add(string.Format(Vf.LevFormat, decimal.Parse(dbman.DataReader["VALUED"].ToString())));
                            row.Add(string.Format(Vf.LevFormat, decimal.Parse(dbman.DataReader["VALUEMONEY"].ToString())));
                        }
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("COLICHESTVO"))
                        {
                            row.Add(string.Format(Vf.KolFormat, decimal.Parse(dbman.DataReader["VALKOLD"].ToString())));
                            row.Add(string.Format(Vf.KolFormat, decimal.Parse(dbman.DataReader["VALKOLK"].ToString())));
                        }
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("SUMAVALUTA"))
                        {
                            row.Add(string.Format(Vf.ValFormat, decimal.Parse(dbman.DataReader["VALVALD"].ToString())));
                            row.Add(string.Format(Vf.ValFormat, decimal.Parse(dbman.DataReader["VALVALK"].ToString())));
                        }

                    }
                    else
                    {
                        var field = dbman.DataReader["Name"].ToString();
                        if (!firstgroup)
                        {
                            if (field == "Контрагент")
                            {
                                title.Add("Номер");
                                title.Add(field);
                            }
                            else
                            {
                               title.Add(field);
                            }
                        }
                        if (field == "Контрагент")
                        {
                            row.Add(dbman.DataReader["VALUE"].ToString());
                            row.Add(dbman.DataReader["VALS"].ToString());
                        }
                        else
                        {
                            row.Add(dbman.DataReader["VALUE"].ToString());
                        }
                    }

                }
                if (!firstgroup && havesaldo)
                {
                    title.Add("Група");
                    allmovement.Add(title);
                    firstgroup = true;
                }
                if (havesaldo)
                {
                    row.Add(curqroup.ToString());
                    allmovement.Add(row);
                }
                dbman.DataReader.Close();
                command = string.Format("SELECT sum(VALUEMONEY) sumk,sum(valued) sumd FROM MOVEMENT a where accid={0}",
                                        accid);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    sumalvd = string.Format(Vf.LevFormat, decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumd"].ToString())?"0":dbman.DataReader["sumd"].ToString()));
                    sumalvk = string.Format(Vf.LevFormat, decimal.Parse(string.IsNullOrWhiteSpace(dbman.DataReader["sumk"].ToString())?"0":dbman.DataReader["sumk"].ToString()));
                    if (Math.Abs(decimal.Parse(sumalvd)) > Math.Abs(decimal.Parse(sumalvk)))
                    {
                        sumalvd = (decimal.Parse(sumalvd) - decimal.Parse(sumalvk)).ToString();
                        sumalvk = Vf.LevFormatUI;
                    }
                    else
                    {
                        sumalvk = (decimal.Parse(sumalvk) - decimal.Parse(sumalvd)).ToString();
                        sumalvd = Vf.LevFormatUI;
                    }
                }
                dbman.DataReader.Close();
                command = string.Format("SELECT sum(a.SALDO) sumk,sum(a.SALDODEBIT) sumd FROM \"accounts\" a where a.NUM={0} and a.\"SubNum\">0 and a.\"FirmaId\"={1} and a.YY={2}",
                                        accnum, firmid, ConfigTempoSinglenton.GetInstance().WorkDate.Year);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    var debit = dbman.DataReader["sumd"].ToString();
                    if (string.IsNullOrWhiteSpace(debit))
                    {
                        debit = Vf.LevFormatUI;
                    }
                    var credit = dbman.DataReader["sumk"].ToString();
                    if (string.IsNullOrWhiteSpace(credit))
                    {
                        credit = Vf.LevFormatUI;
                    }
                    if (Math.Abs(decimal.Parse(debit)) > Math.Abs(decimal.Parse(credit)))
                    {
                        debit = (decimal.Parse(debit) - decimal.Parse(credit)).ToString();
                        credit = Vf.LevFormatUI;
                    }
                    else
                    {
                        credit = (decimal.Parse(credit) - decimal.Parse(debit)).ToString();
                        debit = Vf.LevFormatUI;
                    }
                    sumalvdsub = string.Format(Vf.LevFormat, decimal.Parse(debit));
                    sumalvksub = string.Format(Vf.LevFormat, decimal.Parse(credit));
                    
                }
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static List<List<string>> GetAllMovementsDetail(int accid, int accnum, int firmid, out string sumalvk, out string sumalvd, out string sumalvksub, out string sumalvdsub)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allmovement;
        }

        public static List<SaldoFactura> GetAllAnaliticSaldos(int accid,int firmid, string kindValuta = null)
        {
            List<SaldoFactura> allmovement = new List<SaldoFactura>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string command =
                    string.Format(
                        "SELECT * FROM MOVEMENT a " +
                        "inner join \"lookupsfield\" b on a.ACCFIELDKEY=b.\"Id\"" +
                        "left outer join MAPACCTOLOOKUP m on a.ACCID=m.ACCOUNTS_ID and a.ACCFIELDKEY=m.ANALITIC_FIELD_ID"+
                        " where accid={0} order by a.\"group\",a.SORTORDER",
                        accid);
                dbman.ExecuteReader(CommandType.Text, command);
                //List<string> row = new List<string>();
                List<string> title = new List<string>();
                int group = -1;
                int curqroup = -1;
                bool firstgroup = false;
                bool havesaldo = false;
                SaldoFactura workSaldos=new SaldoFactura();
                while (dbman.DataReader.Read())
                {
                    havesaldo = true;
                    curqroup = int.Parse(dbman.DataReader["group"].ToString());
                    if (curqroup != group)
                    {
                        if (group == -1)
                        {
                            group = curqroup;
                        }
                        else
                        {
                            if (!firstgroup)
                            {
                                title.Add("Група");
                                firstgroup = true;
                            }
                            
                            allmovement.Add(workSaldos.Clone());
                            workSaldos=new SaldoFactura();
                            group = curqroup;
                        }
                    }

                    if (dbman.DataReader["NAMEENG"].ToString() == "SUMALV" || dbman.DataReader["NAMEENG"].ToString() == "COLICHESTVO" || dbman.DataReader["NAMEENG"].ToString() == "SUMAVALUTA")
                    {
                       
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("SUMALV"))
                        {
                            workSaldos.BeginSaldoDebit=decimal.Parse(dbman.DataReader["VALUED"].ToString());
                            workSaldos.BeginSaldoCredit=decimal.Parse(dbman.DataReader["VALUEMONEY"].ToString());
                            //workSaldos.Fields = string.Format("{0}|{1} ", workSaldos.Fields, workSaldos.BeginSaldoDebit - workSaldos.BeginSaldoCredit);
                        }
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("COLICHESTVO"))
                        {
                            workSaldos.BeginSaldoDebitKol= decimal.Parse(dbman.DataReader["VALKOLD"].ToString());
                            workSaldos.BeginSaldoCreditKol=decimal.Parse(dbman.DataReader["VALKOLK"].ToString());
                            //workSaldos.Fields = string.Format("{0}|{1} ", workSaldos.Fields, workSaldos.BeginSaldoDebitKol - workSaldos.BeginSaldoCreditKol);
                        }
                        if (dbman.DataReader["NAMEENG"].ToString().Equals("SUMAVALUTA"))
                        {
                            workSaldos.BeginSaldoDebitValuta=decimal.Parse(dbman.DataReader["VALVALD"].ToString());
                            workSaldos.BeginSaldoCreditValuta= decimal.Parse(dbman.DataReader["VALVALK"].ToString());
                            //workSaldos.Fields = string.Format("{0}|{1} ", workSaldos.Fields, workSaldos.BeginSaldoDebitValuta - workSaldos.BeginSaldoCreditValuta);
                        }
                        
                       
                    }
                    else
                    {
                        string name = dbman.DataReader["Name"].ToString();
                        string lookup = dbman.DataReader["VALS"].ToString();
                        if (!name.Contains("Дата ")) workSaldos.Details=string.Format("{0}|{1} ", workSaldos.Details,dbman.DataReader["VALUE"]);
                        workSaldos.Fields = string.Format("{0}|{1} ", workSaldos.Fields, string.IsNullOrWhiteSpace(lookup)?dbman.DataReader["VALUE"]:dbman.DataReader["VALUE"]+"---"+lookup);
                        if (name=="Номер фактура")
                        {
                            workSaldos.NumInvoise = dbman.DataReader["VALUE"].ToString();
                        }
                        if (name=="Контрагент")
                        {
                            workSaldos.NameContragent = dbman.DataReader["VALS"].ToString();
                            workSaldos.Code= dbman.DataReader["VALUE"].ToString();
                            var a = dbman.DataReader["LOOKUP_ID"].ToString();
                            int b = 0;
                            if (int.TryParse(a, out b))
                            {
                                workSaldos.LookupId = b;
                            }
                           
                        }
                        if (name == "Номенклатурен номер")
                        {
                            workSaldos.NameMaterial = dbman.DataReader["VALS"].ToString();
                            workSaldos.CodeMaterial = dbman.DataReader["VALUE"].ToString();
                        }
                        if (name=="Дата на фактура")
                        {
                            workSaldos.Date = DateTime.Parse(dbman.DataReader["VALUEDATE"].ToString());
                        }
                        if (name == "Вид валута")
                        {
                            workSaldos.CodeValuta = dbman.DataReader["VALUE"].ToString();
                            workSaldos.KindValuta = dbman.DataReader["VALS"].ToString();
                        }
                    }

                }
                if (havesaldo) allmovement.Add(workSaldos);
               
                
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static List<SaldoFactura> GetAllAnaliticSaldos(int accid,int firmid)");
            }

            finally
            {
                dbman.Dispose();
            }
            if (kindValuta != null)
                return allmovement.Where(e => e.CodeValuta == kindValuta).ToList();
            return allmovement;
        }
        public static IEnumerable<SaldoAnaliticModel> GetCurrentMovements(int accid, int groupid)
        {
            List<SaldoAnaliticModel> allmovement = new List<SaldoAnaliticModel>();
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string command =
                    string.Format(
                        "SELECT * FROM MOVEMENT a " +
                        "inner join \"lookupsfield\" b on a.ACCFIELDKEY=b.\"Id\"" +
                       // "left outer join MAPACCTOLOOKUP m on a.ACCID=m.ACCOUNTS_ID and a.ACCFIELDKEY=m.ANALITIC_FIELD_ID"+
                        "where accid={0} and a.\"group\"={1} order by a.\"group\",a.SORTORDER",
                        accid, groupid);
                dbman.ExecuteReader(CommandType.Text, command);

                while (dbman.DataReader.Read())
                {
                    var saldo = new SaldoAnaliticModel();
                    saldo.ID = int.Parse(dbman.DataReader["Id"].ToString());
                    saldo.ACCFIELDKEY = int.Parse(dbman.DataReader["ACCFIELDKEY"].ToString());
                    saldo.ACCID = int.Parse(dbman.DataReader["ACCID"].ToString());
                    saldo.DATA = DateTime.Parse(dbman.DataReader["DATA"].ToString());
                    saldo.VAL = dbman.DataReader["VALUE"].ToString();
                    saldo.VALUED = decimal.Parse(dbman.DataReader["VALUED"].ToString());
                    saldo.DBField = dbman.DataReader["DBField"].ToString();
                    saldo.GROUP = int.Parse(dbman.DataReader["GROUP"].ToString());
                    saldo.ISNULL = int.Parse(dbman.DataReader["ISNULL"].ToString());
                    saldo.LOOKUPFIELDKEY =
                        int.Parse(dbman.DataReader["LOOKUPFIELDKEY"].ToString());
                    saldo.LOOKUPID = int.Parse(dbman.DataReader["LOOKUPID"].ToString());
                    //int b = 0;
                    //var a = dbman.DataReader["LOOKUP_ID"].ToString();
                    //if (int.TryParse(a, out b))
                    //{
                    //    saldo.LOOKUPID = b;
                    //}
                    saldo.VALUEDATE = DateTime.Parse(dbman.DataReader["VALUEDATE"].ToString());
                    saldo.VALUEMONEY = decimal.Parse(dbman.DataReader["VALUEMONEY"].ToString());
                    saldo.VALUENUM = int.Parse(dbman.DataReader["VALUENUM"].ToString());
                    decimal test;
                    saldo.VALKOLD = decimal.TryParse(dbman.DataReader["VALKOLD"].ToString(),out test)?decimal.Parse(dbman.DataReader["VALKOLD"].ToString()):0;
                    saldo.VALKOLK = decimal.TryParse(dbman.DataReader["VALKOLK"].ToString(),out test)?decimal.Parse(dbman.DataReader["VALKOLK"].ToString()):0;
                    saldo.VALVALD = decimal.TryParse(dbman.DataReader["VALVALD"].ToString(),out test)?decimal.Parse(dbman.DataReader["VALVALD"].ToString()):0;
                    saldo.VALVALK = decimal.TryParse(dbman.DataReader["VALVALK"].ToString(),out test)?decimal.Parse(dbman.DataReader["VALVALK"].ToString()):0;
                    saldo.VALS = dbman.DataReader["VALS"].ToString();
                    saldo.Name = dbman.DataReader["Name"].ToString();
                    allmovement.Add(saldo);
                }

            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, " public static IEnumerable<SaldoAnaliticModel> GetCurrentMovements(int accid, int groupid)");
            }

            finally
            {
                dbman.Dispose();
            }

            return allmovement;
        }

        public static void UpdateMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(14);
                dbman.AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
                dbman.AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
                dbman.AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
                dbman.AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
                dbman.AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
                dbman.AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
                dbman.AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
                dbman.AddParameters(7, "@VALUED", saldoAnaliticModel.VALUED);
                dbman.AddParameters(8, "@GROUPID", saldoAnaliticModel.GROUP);
                dbman.AddParameters(9, "@VALKOLK", saldoAnaliticModel.VALKOLK);
                dbman.AddParameters(10, "@VALKOLD", saldoAnaliticModel.VALKOLD);
                dbman.AddParameters(11, "@VALVALD", saldoAnaliticModel.VALVALD);
                dbman.AddParameters(12, "@VALVALK", saldoAnaliticModel.VALVALK);
                dbman.AddParameters(13, "@VALS", saldoAnaliticModel.VALS);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "UPDATEMOVENT");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void UpdateMovement(SaldoAnaliticModel saldoAnaliticModel)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }

        internal static void DeleteMovement(int accID, int selectedgroup)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                if (selectedgroup == 0)
                {
                    dbman.ExecuteNonQuery(CommandType.Text,
                                              string.Format("DELETE FROM MOVEMENT WHERE ACCID={0}", accID));
                }
                else
                {
                    dbman.ExecuteNonQuery(CommandType.Text,
                                              string.Format("DELETE FROM MOVEMENT WHERE ACCID={0} AND \"group\"={1}", accID,
                                                            selectedgroup));
                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "internal static void DeleteMovement(int accID, int selectedgroup)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }

        internal static IEnumerable<SaldoAnaliticModel> LoadAllAnaliticfields(int accID)
        {
            List<SaldoAnaliticModel> allfields =null;
            allfields = LookUpRepository.Instance.AnaliticalFields(accID);
            if (allfields != null)
            {
                return allfields;
            }
            allfields=new List<SaldoAnaliticModel>();
            bool sv=false, sl=false, kol=false;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                string command =
                string.Format("SELECT a.\"Id\",a.\"AnaliticalNum\",a.TYPEANALITICALKEY,af.\"Name\",af.\"DBField\",aa.\"TypeID\",l.LOOKUP_ID,l.FIELDLOOKUP_ID,ca.\"AnaliticalFieldId\",SL,SV,KOL,af.RTABLENAME,af.RFIELDKEY,af.RFIELDNAME,af.RCODELOOKUP,ca.REQUIRED" +
                              " FROM \"accounts\" a " +
                              "inner join \"analiticalaccount\" aa on a.\"AnaliticalNum\"=aa.\"Id\"" +
                              "inner join \"analiticalaccounttype\" aat on aa.\"TypeID\"=aat.\"Id\""+
                              "inner join \"conectoranaliticfield\" ca on aa.\"Id\"=ca.\"AnaliticalNameID\"" +
                              "inner join \"lookupsfield\" af on af.\"Id\"=ca.\"AnaliticalFieldId\" " +
                              "left outer join MAPACCTOLOOKUP l on l.ACCOUNTS_ID=a.\"Id\" and l.ANALITIC_ID=aa.\"Id\" and l.ANALITIC_FIELD_ID=ca.\"AnaliticalFieldId\""+
                              " where a.\"Id\"={0} order by ca.SORTORDER", accID);
                dbman.ExecuteReader(CommandType.Text, command);

                while (dbman.DataReader.Read())
                {
                    int lookupkey, lookupfieldkey,svv,sll,koll;
                    if (int.TryParse(dbman.DataReader["SL"].ToString(), out sll))
                    {
                        if (sll==1) sl = true;
                    }
                    if (int.TryParse(dbman.DataReader["SV"].ToString(), out svv))
                    {
                        if (svv == 1) sv = true;
                    }
                    if (int.TryParse(dbman.DataReader["KOL"].ToString(), out koll))
                    {
                        if (koll == 1) kol = true;
                    }
                    if (int.TryParse(dbman.DataReader["FIELDLOOKUP_ID"].ToString(),out lookupfieldkey))
                    {

                    }
                   if (int.TryParse(dbman.DataReader["LOOKUP_ID"].ToString(),out lookupkey))
                    {

                    }
                    allfields.Add(new SaldoAnaliticModel
                    {
                        ID = int.Parse(dbman.DataReader["Id"].ToString()),
                        ACCFIELDKEY = int.Parse(dbman.DataReader["AnaliticalFieldId"].ToString()),
                        ACCID = int.Parse(dbman.DataReader["Id"].ToString()),
                        DATA = DateTime.Now,
                        VAL = "",
                        VALUED = 0,
                        DBField = dbman.DataReader["DBField"].ToString(),
                        GROUP = 0,
                        ISNULL = 0,
                        LENGTH = 20,
                        LOOKUPFIELDKEY = lookupfieldkey,
                        LOOKUPID = lookupkey,
                        VALUEDATE = DateTime.Now,
                        VALUEMONEY = 0,
                        VALUENUM = 0,
                        Name = dbman.DataReader["Name"].ToString(),
                        RFIELDKEY = dbman.DataReader["RFIELDKEY"].ToString(),
                        RFIELDNAME = dbman.DataReader["RFIELDNAME"].ToString(),
                        RTABLENAME = dbman.DataReader["RTABLENAME"].ToString(),
                        Required = int.Parse(dbman.DataReader["REQUIRED"].ToString()) == 1,
                                            IsKol=kol,
                                            IsValutna=sv
                                        });

                }
                //if (sl)
                //{
                //    allfields.Add(new SaldoAnaliticModel
                //    {
                //        ID = accID,
                //        ACCFIELDKEY = 0,
                //        ACCID = accID,
                //        DATA = DateTime.Now,
                //        VAL = "0",
                //        VALUED = 0,
                //        DBField = "DECIMAL",
                //        GROUP = 0,
                //        ISNULL = 0,
                //        LENGTH = 20,
                //        LOOKUPFIELDKEY = 0,
                //        LOOKUPID = 0,
                //        VALUEDATE = DateTime.Now,
                //        VALUEMONEY = 0,
                //        VALUENUM = 0,
                //        Name = "Сума лева"
                //    });
                //}
                if (sv)
                {
                    allfields.Add(new SaldoAnaliticModel
                    {
                        ID = accID,
                        ACCFIELDKEY = 30,
                        ACCID = accID,
                        DATA = DateTime.Now,
                        VAL = "0",
                        VALUED = 0,
                        DBField = "DECIMAL",
                        GROUP = 0,
                        ISNULL = 0,
                        LENGTH = 20,
                        LOOKUPFIELDKEY = 0,
                        LOOKUPID = 0,
                        VALUEDATE = DateTime.Now,
                        VALUEMONEY = 0,
                        VALUENUM = 0,
                        Name = "Сума валута"
                    });
                }
                if (kol)
                {
                    allfields.Add(new SaldoAnaliticModel
                    {
                        ID = accID,
                        ACCFIELDKEY = 31,
                        ACCID = accID,
                        DATA = DateTime.Now,
                        VAL = "0",
                        VALUED = 0,
                        DBField = "DECIMAL",
                        GROUP = 0,
                        ISNULL = 0,
                        LENGTH = 20,
                        LOOKUPFIELDKEY = 0,
                        LOOKUPID = 0,
                        VALUEDATE = DateTime.Now,
                        VALUEMONEY = 0,
                        VALUENUM = 0,
                        Name = "Количествo"
                    });
                }
                
                command =
                string.Format("SELECT a.\"Id\" as ACCID,aat.\"Id\" as IDD,af.\"Name\" as NA,af.\"DBField\" as DBF,af.\"Id\" AF FROM \"accounts\" a inner join \"analiticalaccount\" aa on a.\"AnaliticalNum\"=aa.\"Id\" inner join \"analiticalaccounttype\" aat on aat.\"Id\"=aa.\"TypeID\" inner join MPATYPETOAFIELD ca on aat.\"Id\"=ca.ATYPEID inner join \"lookupsfield\" af on af.\"Id\"=ca.AFIELDID where a.\"Id\"={0}",accID);
                dbman.DataReader.Close();
                dbman.ExecuteReader(CommandType.Text, command);

                while (dbman.DataReader.Read())
                {
                   
                    allfields.Add(new SaldoAnaliticModel
                                        {
                                            ID = int.Parse(dbman.DataReader["IDD"].ToString()),
                                            ACCFIELDKEY =int.Parse(dbman.DataReader["AF"].ToString()),
                                            ACCID = int.Parse(dbman.DataReader["ACCID"].ToString()),
                                            DATA = DateTime.Now,
                                            VAL = "0",
                                            VALUED = 0,
                                            DBField = dbman.DataReader["DBF"].ToString(),
                                            GROUP = 0,
                                            ISNULL = 0,
                                            LENGTH = 20,
                                            LOOKUPFIELDKEY =0,
                                            LOOKUPID = 0,
                                            VALUEDATE = DateTime.Now,
                                            VALUEMONEY = 0,
                                            VALUENUM = 0,
                                            Name = dbman.DataReader["NA"].ToString()
                                        });

                }
            }
            catch (Exception e)
            {
                Logger.Instance().WriteLogError(e.Message, "internal static IEnumerable<SaldoAnaliticModel> LoadAllAnaliticfields(int accID)");
            }
            LookUpRepository.Instance.Add(accID,allfields);
            return allfields;
        }

       
    }
}