using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.Configuration;
using System.Data;
using System.IO;

namespace Tempo2012.EntityFramework
{
    public static partial class RealDataContext
    {
        public static bool SaveConto(Conto CurrentConto, List<SaldoAnaliticModel> debit, List<SaldoAnaliticModel> credit)
        {
            bool res = true;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                CreateParameters(CurrentConto,true,dbman);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "ADDCONTO");
                CurrentConto.Id = (int)dbman.Parameters[33].Value;
                foreach (var item in debit)
                {
                    item.CONTOID = CurrentConto.Id;
                    SaveContoMovementWithNo(item, dbman);
                }
                foreach (var item in credit)
                {
                    item.CONTOID = CurrentConto.Id;
                    SaveContoMovementWithNo(item, dbman);
                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                res = false;
                dbman.RollBackTransaction();
                Logger.Instance().WriteLogError(ex.Message, "SaveConto");
            }

            finally
            {
                dbman.Dispose();
            }
            return res;
        }

        
        private static void CreateParameters(Conto CurrentConto,bool add,DBManager dbman)
        {
            if (add)
            {
                dbman.CreateParameters(34);
            }
            else
            {
                dbman.CreateParameters(33);
            }
            dbman.AddParameters(0, "@Date", CurrentConto.Data);
            dbman.AddParameters(1, "@Oborot", Math.Round(CurrentConto.Oborot,2));
            dbman.AddParameters(2, "@Reason", CurrentConto.Reason);
            dbman.AddParameters(3, "@Note", CurrentConto.Note);
            dbman.AddParameters(4, "@DataInvoise", CurrentConto.DataInvoise);
            dbman.AddParameters(5, "@NumberObject", CurrentConto.NumberObject);
            dbman.AddParameters(6, "@DebitAccount", CurrentConto.DebitAccount);
            dbman.AddParameters(7, "@CreditAccount", CurrentConto.CreditAccount);
            dbman.AddParameters(8, "@FirmId", CurrentConto.FirmId);
            dbman.AddParameters(9, "@DocumentId", CurrentConto.DocumentId);
            dbman.AddParameters(10, "@CartotekaDebit", CurrentConto.CartotekaDebit);
            dbman.AddParameters(11, "@CartotecaCredit", CurrentConto.CartotecaCredit);
            dbman.AddParameters(12, "@DOCNUM", CurrentConto.DocNum);
            dbman.AddParameters(13, "@OBOROTVALUTA", CurrentConto.OborotValutaD);
            dbman.AddParameters(14, "@OBOROTKOL", CurrentConto.OborotKolD);
            dbman.AddParameters(15, "@OBOROTVALUTAK", CurrentConto.OborotValutaK);
            dbman.AddParameters(16, "@OBOROTKOLK", CurrentConto.OborotKolK);
            dbman.AddParameters(17, "@FOLDER", CurrentConto.Folder);
            dbman.AddParameters(18, "@ISDDSSALES", CurrentConto.IsDdsSales);
            dbman.AddParameters(19, "@ISDDSPURCHASES", CurrentConto.IsDdsPurchases);
            dbman.AddParameters(20, "@VOPPURCHASES", CurrentConto.VopPurchases);
            dbman.AddParameters(21, "@VOPSALES", CurrentConto.VopSales);
            dbman.AddParameters(22, "@ISDDSPURCHASESINCLUDED", CurrentConto.IsDdsPurchasesIncluded);
            dbman.AddParameters(23, "@ISDDSSALESINCLUDED", CurrentConto.IsDdsSalesIncluded);
            dbman.AddParameters(24, "@ISSALES", CurrentConto.IsSales);
            dbman.AddParameters(25, "@ISPURCHASES", CurrentConto.IsPurchases);
            dbman.AddParameters(26, "@DDETAILS", CurrentConto.DDetails);
            dbman.AddParameters(27, "@CDETAILS", CurrentConto.CDetails);
            dbman.AddParameters(28, "@USERID", CurrentConto.UserId);
            dbman.AddParameters(29, "@PR1", CurrentConto.Pr1);
            dbman.AddParameters(30, "@PR2", CurrentConto.Pr2);
            dbman.AddParameters(31, "@KD", CurrentConto.KD);
            if (add) {
                dbman.AddParameters(32, "@PORNOM", CurrentConto.CartotecaCredit);
                dbman.AddOutputParameters(33,"@NEWID", CurrentConto.Id);
            } else
                dbman.AddParameters(32, "@ContoID", CurrentConto.Id);
        }

        public static void UpdateConto(Conto CurrentConto)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                CreateParameters(CurrentConto,false,dbman);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "UPDATECONTO");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "UpdateConto");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
            
        }
        public static void SaveMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(16);
                dbman.AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
                dbman.AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
                dbman.AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
                dbman.AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
                dbman.AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
                dbman.AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
                dbman.AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
                dbman.AddParameters(7, "@TYPEACCKEY", saldoAnaliticModel.TYPEACCKEY);
                dbman.AddParameters(8, "@VALUED", saldoAnaliticModel.VALUED);
                dbman.AddParameters(9, "@LOOKUPID", saldoAnaliticModel.LOOKUPID);
                dbman.AddParameters(10, "@VALKOLK",saldoAnaliticModel.VALKOLK);
                dbman.AddParameters(11, "@VALKOLD", saldoAnaliticModel.VALKOLD);
                dbman.AddParameters(12, "@VALVALK", saldoAnaliticModel.VALVALK);
                dbman.AddParameters(13, "@VALVALD", saldoAnaliticModel.VALVALD); 
                dbman.AddParameters(14, "@VALS", saldoAnaliticModel.VALS);
                dbman.AddParameters(15, "@SORTORDER", saldoAnaliticModel.SORTORDER);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "ADDMOVENT");

                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "SaveMovement");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }
        public static bool CheckMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            bool result = true;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(11);
                dbman.AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
                dbman.AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
                dbman.AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
                dbman.AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
                dbman.AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
                dbman.AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
                dbman.AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
                dbman.AddParameters(7, "@TYPEACCKEY", saldoAnaliticModel.TYPEACCKEY);
                dbman.AddParameters(8, "@VALUED", saldoAnaliticModel.VALUED);
                dbman.AddParameters(9, "@LOOKUPID", saldoAnaliticModel.LOOKUPID);
                dbman.AddOutputParameters(10, "@ISIN", saldoAnaliticModel.LOOKUPID);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "CHECKMOVENT");
                dbman.CommitTransaction();
                result=(int)dbman.Parameters[10].Value==0;
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "GetAllLookupsFields");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static void SaveContoMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(18);
                dbman.AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
                dbman.AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
                dbman.AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
                dbman.AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
                dbman.AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
                dbman.AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
                dbman.AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
                dbman.AddParameters(7, "@TYPEACCKEY", saldoAnaliticModel.TYPEACCKEY);
                dbman.AddParameters(8, "@VALUED", saldoAnaliticModel.VALUED);
                dbman.AddParameters(9, "@LOOKUPID", saldoAnaliticModel.LOOKUPID);
                dbman.AddParameters(10, "@CONTOID", saldoAnaliticModel.CONTOID);
                dbman.AddParameters(11,"@TYPE",saldoAnaliticModel.TYPE);
                dbman.AddParameters(12, "@KURS",saldoAnaliticModel.KURS);
                dbman.AddParameters(13, "@KURSD",saldoAnaliticModel.KURSD);
                dbman.AddParameters(14, "@KURSM",saldoAnaliticModel.KURSM);
                dbman.AddParameters(15, "@VALVAL",saldoAnaliticModel.VALVAL);
                dbman.AddParameters(16, "@LOOKUPVAL", saldoAnaliticModel.LOOKUPVAL);
                dbman.AddParameters(17, "@SORTORDER", saldoAnaliticModel.SORTORDER);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "ADDMOVENTCONTO");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, " Movement(SaldoAnaliticModel saldoAnaliticModel)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }

        public static void SaveContoMovementWithNo(SaldoAnaliticModel saldoAnaliticModel,DBManager dbman)
        {
           
            dbman.CreateParameters(18);
            dbman.AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
            dbman.AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
            dbman.AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
            dbman.AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
            dbman.AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
            dbman.AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
            dbman.AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
            dbman.AddParameters(7, "@TYPEACCKEY", saldoAnaliticModel.TYPEACCKEY);
            dbman.AddParameters(8, "@VALUED", saldoAnaliticModel.VALUED);
            dbman.AddParameters(9, "@LOOKUPID", saldoAnaliticModel.LOOKUPID);
            dbman.AddParameters(10, "@CONTOID", saldoAnaliticModel.CONTOID);
            dbman.AddParameters(11, "@TYPE", saldoAnaliticModel.TYPE);
            dbman.AddParameters(12, "@KURS", saldoAnaliticModel.KURS);
            dbman.AddParameters(13, "@KURSD", saldoAnaliticModel.KURSD);
            dbman.AddParameters(14, "@KURSM", saldoAnaliticModel.KURSM);
            dbman.AddParameters(15, "@VALVAL", saldoAnaliticModel.VALVAL);
            dbman.AddParameters(16, "@LOOKUPVAL", saldoAnaliticModel.LOOKUPVAL);
            dbman.AddParameters(17, "@SORTORDER", saldoAnaliticModel.SORTORDER);
            dbman.ExecuteNonQuery(CommandType.StoredProcedure, "ADDMOVENTCONTO");
            
        }
        public static void UpdateContoMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(19);
                dbman.AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
                dbman.AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
                dbman.AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
                dbman.AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
                dbman.AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
                dbman.AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
                dbman.AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
                dbman.AddParameters(7, "@TYPEACCKEY", saldoAnaliticModel.TYPEACCKEY);
                dbman.AddParameters(8, "@VALUED", saldoAnaliticModel.VALUED);
                dbman.AddParameters(9, "@LOOKUPID", saldoAnaliticModel.LOOKUPID);
                dbman.AddParameters(10, "@CONTOID", saldoAnaliticModel.CONTOID);
                dbman.AddParameters(11, "@TYPE", saldoAnaliticModel.TYPE);
                dbman.AddParameters(12, "@ID", saldoAnaliticModel.ID);
                dbman.AddParameters(13, "@KURS", saldoAnaliticModel.KURS);
                dbman.AddParameters(14, "@KURSD", saldoAnaliticModel.KURSD);
                dbman.AddParameters(15, "@KURSM", saldoAnaliticModel.KURSM);
                dbman.AddParameters(16, "@VALVAL", saldoAnaliticModel.VALVAL);
                dbman.AddParameters(17, "@LOOKUPVAL", saldoAnaliticModel.LOOKUPVAL);
                dbman.AddParameters(18, "@SORTORDER", saldoAnaliticModel.SORTORDER);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "UPDATEMOVENTCONTO");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "UpdateContoMovement");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }
        }

        public static bool DeleteConto(int contoid)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            bool result=true;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(1);
                dbman.AddParameters(0, "@CONTOID", contoid);
                //dbman.AddParameters(1, "@ISPORNOM", 1);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "DELETECONTO");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "DeleteConto(int contoid)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
        public static IEnumerable<SaldoAnaliticModel> LoadContoDetails(int contoid, int typeconto)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<SaldoAnaliticModel> allfields = new List<SaldoAnaliticModel>();
            try
            {
                dbman.Open();
                string command =
                string.Format("SELECT * FROM CONTOMOVEMENT a inner join \"lookupsfield\" l on l.\"Id\"=a.ACCFIELDKEY where a.CONTOID={0} and a.\"TYPE\"={1} order by a.SORTORDER", contoid, typeconto);
                dbman.ExecuteReader(CommandType.Text, command);

                while (dbman.DataReader.Read())
                {
                    allfields.Add(new SaldoAnaliticModel
                    {
                        ID = int.Parse(dbman.DataReader["ID"].ToString()),
                        ACCFIELDKEY = int.Parse(dbman.DataReader["ACCFIELDKEY"].ToString()),
                        ACCID = int.Parse(dbman.DataReader["ACCID"].ToString()),
                        DATA = DateTime.Now,
                        VAL = dbman.DataReader["VALUE"].ToString(),
                        VALUED = decimal.Parse(dbman.DataReader["VALUED"].ToString()),
                        DBField = dbman.DataReader["DBField"].ToString(),
                        GROUP = 0,
                        ISNULL = 0,
                        LENGTH = 20,
                        LOOKUPFIELDKEY = int.Parse(dbman.DataReader["LOOKUPFIELDKEY"].ToString()),
                        LOOKUPID =  int.Parse(dbman.DataReader["LOOKUPID"].ToString()),
                        VALUEDATE =DateTime.Parse( dbman.DataReader["VALUEDATE"].ToString()),
                        VALUEMONEY = decimal.Parse(dbman.DataReader["VALUEMONEY"].ToString()),
                        VALUENUM = decimal.Parse(dbman.DataReader["VALUENUM"].ToString()),
                        Name = dbman.DataReader["Name"].ToString(),
                        CONTOID = contoid,
                        RFIELDKEY = dbman.DataReader["RFIELDKEY"].ToString(),
                        RFIELDNAME = dbman.DataReader["RFIELDNAME"].ToString(),
                        RTABLENAME = dbman.DataReader["RTABLENAME"].ToString(),
                        KURS =decimal.Parse(dbman.DataReader["KURS"].ToString()),
                        VALVAL=decimal.Parse(dbman.DataReader["VALVAL"].ToString()),
                        KURSM=decimal.Parse(dbman.DataReader["KURSM"].ToString()),
                        KURSD=decimal.Parse(dbman.DataReader["KURSD"].ToString()),
                        LOOKUPVAL = dbman.DataReader["LOOKUPVAL"].ToString(),
                        SORTORDER = int.Parse(dbman.DataReader["SORTORDER"].ToString())
                    });
                }
                dbman.DataReader.Close();
            }
            catch (Exception e)
            {
                Logger.Instance().WriteLogError(e.Message, "public static IEnumerable<SaldoAnaliticModel> LoadContoDetails(int contoid, int typeconto)");
            }
            finally
            {
                dbman.Dispose();
            }

            return allfields;

        }
        public static IEnumerable<DdsItemModel> LoadDnevItems(int type)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            List<DdsItemModel> result=new List<DdsItemModel>();
            dbman.Open();
            string typednev = "DDSDNEVFIELDS";
            if (type == 1) typednev = "DDSDNEVSELLSFIELDS";
            string command = string.Format("SELECT * FROM {0}", typednev);
            dbman.ExecuteReader(CommandType.Text, command);
            while (dbman.DataReader.Read())
            {
                result.Add(new DdsItemModel
                {
                    DdsPercent = int.Parse(dbman.DataReader["DDSPERCENT"].ToString()),
                    Name = dbman.DataReader["NAME"].ToString(),
                    Id = int.Parse(dbman.DataReader["ID"].ToString()),
                    Code =  dbman.DataReader["CODE"].ToString(),
                    IsNotComputed = false
                });
            }
            dbman.DataReader.Close();
            dbman.Dispose();
            return result;
        }

        public static DdsDnevnikModel LoadDenevnicItem(int contoId,int type)
        {
            DdsDnevnikModel result = new DdsDnevnikModel();
            result.Num = contoId;
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                bool master = false;
                dbman.Open();
                string command =
                string.Format("SELECT * FROM DDSDNEV where NOM={0} and KINDACTIVITY={1}", contoId,type);
                dbman.ExecuteReader(CommandType.Text, command);
                if (dbman.DataReader.Read())
                {
                    result.Id = int.Parse(dbman.DataReader["Id"].ToString());
                    result.Branch = dbman.DataReader["BRANCH"].ToString();
                    result.Date = DateTime.Parse(dbman.DataReader["DATADOC"].ToString());
                    result.DataF = DateTime.Parse(dbman.DataReader["DATAF"].ToString());
                    result.DocId = dbman.DataReader["DOCN"].ToString();
                    result.KindActivity = int.Parse(dbman.DataReader["KINDACTIVITY"].ToString());
                    result.KindDoc=int.Parse(dbman.DataReader["KINDDOC"].ToString());
                    result.Stoke=dbman.DataReader["STOKE"].ToString();
                    result.LookupElementID = int.Parse(dbman.DataReader["LOOKUPELEMENTID"].ToString());
                    result.LookupID = int.Parse(dbman.DataReader["LOOKUPID"].ToString());
                    result.Suma = Decimal.Parse(dbman.DataReader["SUMA"].ToString());
                    result.SumaDDS = Decimal.Parse(dbman.DataReader["DDSSUMA"].ToString());
                    result.NameKontr = dbman.DataReader["NAMEKONTR"].ToString();
                    result.CodeDoc = dbman.DataReader["CODEDOC"].ToString();
                    result.Nzdds = dbman.DataReader["NZDDS"].ToString();
                    result.Bulstat = dbman.DataReader["BULSTAD"].ToString();
                    result.ClNum = dbman.DataReader["CLNUM"].ToString();
                    int val;
                    result.A8 =dbman.DataReader["A8"].ToString();
                    result.IsSuma = int.TryParse(dbman.DataReader["ISSUMA"].ToString(), out val) ? val : 0;
                    master = true;
                    result.IsLinked = true;
                }
                dbman.DataReader.Close();
                if (master)
                {

                    string typednev = "DDSDNEVFIELDS";
                    if (type == 1) typednev = "DDSDNEVSELLSFIELDS";
                    command =
                    string.Format("SELECT * FROM {0} a inner join DDSDNEVTOFIELDS m on a.ID=m.IDDDSFIELD where m.IDDDSDNEV={1}",typednev, result.Id);
                    dbman.ExecuteReader(CommandType.Text, command);
                    while (dbman.DataReader.Read())
                    {
                        var n = new DdsItemModel
                        {
                            Name = dbman.DataReader["NAME"].ToString(),
                            Id = int.Parse(dbman.DataReader["IDDDSFIELD"].ToString()),
                            Code = dbman.DataReader["CODE"].ToString(),
                            IsNotComputed = true,
                            In = true
                        };
                        n.DdsPercent = decimal.Parse(dbman.DataReader["DDSP"].ToString());
                        n.DdsSuma = decimal.Parse(dbman.DataReader["SUMADDS"].ToString());
                        n.Dds = decimal.Parse(dbman.DataReader["DDS"].ToString());
                        n.DdsTotal=decimal.Parse(dbman.DataReader["SUMAWITHDDS"].ToString());
                        n.IsNotComputed = false;
                        result.DetailItems.Add(n);
                    }
                    dbman.DataReader.Close();
                }
                else
                {
                    string typednev = "DDSDNEVFIELDS";
                    if (type == 1) typednev = "DDSDNEVSELLSFIELDS";
                    command = string.Format("SELECT * FROM {0}", typednev);
                    dbman.ExecuteReader(CommandType.Text, command);
                    while (dbman.DataReader.Read())
                    {
                        result.DetailItems.Add(new DdsItemModel
                        {
                            DdsPercent = int.Parse(dbman.DataReader["DDSPERCENT"].ToString()),
                            Name = dbman.DataReader["NAME"].ToString(),
                            Id = int.Parse(dbman.DataReader["ID"].ToString()),
                            Code = dbman.DataReader["CODE"].ToString(),
                            IsNotComputed = false,
                         });
                    }
                    //command =
                    //string.Format("SELECT * FROM DDSDNEVFIELDS a where a.\"TYPE\"={0}", typednev);
                    //dbman.ExecuteReader(CommandType.Text, command);
                    //while (dbman.DataReader.Read())
                    //{
                    //    result.DetailItems.Add(new DdsItemModel
                    //    {
                    //        DdsPercent = int.Parse(dbman.DataReader["DDSPERCENT"].ToString()),
                    //        DdsSuma =0,
                    //        Name = dbman.DataReader["NAME"].ToString(),
                    //        Id = int.Parse(dbman.DataReader["ID"].ToString()) 
                    //    });
                    //}
                    dbman.DataReader.Close();
                    
                }

            }
            catch (Exception e)
            {
                Logger.Instance().WriteLogError(e.Message, "public static DdsDnevnikModel LoadDenevnicItem(int contoId,int type)");
            }
            finally
            {
                dbman.Dispose();
            }
            return result;
        }

        public static void SaveDdsDnevnicModel(DdsDnevnikModel ddsDnevnikModel,bool isedit)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(20);
                dbman.AddParameters(0, "@NOM", ddsDnevnikModel.Num);
                dbman.AddParameters(1, "@BRANCH", ddsDnevnikModel.Branch);
                dbman.AddParameters(2, "@DOCN", ddsDnevnikModel.DocId);
                dbman.AddParameters(3, "@DATADOC",ddsDnevnikModel.Date);
                dbman.AddParameters(4, "@KINDACTIVITY", ddsDnevnikModel.KindActivity);
                dbman.AddParameters(5, "@KINDDOC", ddsDnevnikModel.KindDoc);
                dbman.AddParameters(6, "@STOKE", ddsDnevnikModel.Stoke);
                dbman.AddParameters(7, "@BULSTAD", ddsDnevnikModel.Bulstat);
                dbman.AddParameters(8, "@NZDDS", ddsDnevnikModel.Nzdds);
                dbman.AddParameters(9, "@LOOKUPID", ddsDnevnikModel.LookupID);
                dbman.AddParameters(10, "@LOOKUPELEMENTID", ddsDnevnikModel.LookupElementID);
                dbman.AddOutputParameters(11,"@NEWID",ddsDnevnikModel.Id);
                dbman.AddParameters(12, "@NAMEKONTR", ddsDnevnikModel.NameKontr);
                dbman.AddParameters(13, "@SUMA", ddsDnevnikModel.Suma);
                dbman.AddParameters(14, "@DDSSUMA", ddsDnevnikModel.SumaDDS);
                dbman.AddParameters(15, "@CODEDOC", ddsDnevnikModel.CodeDoc);
                dbman.AddParameters(16, "@DATAF", ddsDnevnikModel.DataF);
                dbman.AddParameters(17, "@A8", ddsDnevnikModel.A8);
                dbman.AddParameters(18, "@CLNUM", ddsDnevnikModel.ClNum);
                dbman.AddParameters(19, "@ISSUMA", ddsDnevnikModel.IsSuma);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "ADDDDSDNEV");
                ddsDnevnikModel.Id=(int)dbman.Parameters[11].Value;
                if (isedit) dbman.ExecuteNonQuery(CommandType.Text, string.Format("Delete from DDSDNEVTOFIELDS d where d.IDDDSDNEV={0}", ddsDnevnikModel.Id));
                foreach (var item in ddsDnevnikModel.DetailItems)
                {
                    if (!item.In) continue;
                    string command =
                        string.Format(
                            "INSERT INTO DDSDNEVTOFIELDS (IDDDSDNEV, IDDDSFIELD, SUMADDS, SUMAWITHDDS,DDS,DDSP) VALUES ({0},{1},{2},{3},{4},{5})", ddsDnevnikModel.Id, item.Id, item.DdsSuma.ToString(Vf.LevFormatUI), item.DdsTotal.ToString(Vf.LevFormatUI), item.Dds.ToString(Vf.LevFormatUI),item.DdsPercent.ToString(Vf.LevFormatUI));
                    dbman.ExecuteNonQuery(CommandType.Text, command);
                }
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static void SaveDdsDnevnicModel(DdsDnevnikModel ddsDnevnikModel)");
                dbman.RollBackTransaction();

            }

            finally
            {
                dbman.Dispose();
            }

        }
        
        public static bool DeleteDdsDnevnicModel(int id)
        {
            var dbman = new DBManager(DataProvider.Firebird);
            dbman.ConnectionString = Entrence.ConnectionString;
            bool result = true;
            try
            {
                dbman.Open();
                dbman.BeginTransaction();
                dbman.CreateParameters(1);
                dbman.AddParameters(0, "@NOM", id);
                dbman.ExecuteNonQuery(CommandType.StoredProcedure, "DELETEDNEV");
                dbman.CommitTransaction();
            }

            catch (Exception ex)
            {
                Logger.Instance().WriteLogError(ex.Message, "public static bool DeleteDdsDnevnicModel(int id)");
                dbman.RollBackTransaction();
                result = false;
            }

            finally
            {
                dbman.Dispose();
            }
            return result;
        }
    }
}
