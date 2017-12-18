using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ReportBuilder;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;

namespace TempoTest.ExportBuilderTest
{
    public class WrapConto
    {
        public Conto Conto { get; set;}
        public List<SaldoItem> ItemsDebits;
        public List<SaldoItem> ItemsCredits;
        public string Fn { get; set;}
        public string Cn { get; set;}
        public string Bulstad { get; set;}
        public decimal Suma { get; set;}
        public string CodeDds { get; set;}
        public int TypeDds { get; set; }
    } 
    public class ContoReader : IExportable
    {
        public const int All = 48889;
        private IDbManager _DBManager; 
        private IDbManager DBManager 
        {
            get
            {
                if (_DBManager == null)
                {
                    _DBManager = new DBManager(DataProvider.Firebird);
                    _DBManager.ConnectionString = Entrence.ConnectionString;
                }
                return _DBManager; 
            }
        }
        private Tempo2012.EntityFramework.TempoDataBaseContext context=new Tempo2012.EntityFramework.TempoDataBaseContext();
        private IEnumerable<AccountsModel> acc;

        public List<List<string>> GetAllItems()
        {

            Entrence.ConnectionString = @"User ID=sysdba;Password=masterkey;Database=localhost:E:\Samples\TempoSvn\Tempo2012\Tempo2012.UI.WPF\bin\Debug\data\TEMPO2012.FDB;DataSource=localhost;";
            
            List<List<string>> rezi=new List<List<string>>();
            
            acc = context.GetAllAccounts(2);

            List<WrapConto> rez = ReadCont();
            int i = 0;
            DBManager.Open();
            DBManager.BeginTransaction();
            try
            {
                foreach (WrapConto CurrentConto in rez)
                {
                    if (i==2621)
                    {
                        i++;
                        i--;
                    }
                    i++;
                    SaveConto(CurrentConto);
                    SaveDds(CurrentConto);
                }
                DBManager. CommitTransaction();
            }
            catch (Exception)
            {

                DBManager. RollBackTransaction();
            }

            finally
            {
                DBManager. Dispose();
            }
            return rezi;
        }

        private void SaveDds(WrapConto CurrentConto)
        {
            if (CurrentConto.TypeDds == 0) return;
            DdsDnevnikModel ddsDnevnikModel = new DdsDnevnikModel();
            ddsDnevnikModel.DetailItems = new List<DdsItemModel>(GetAllDnevItems(CurrentConto.TypeDds));
            var item = ddsDnevnikModel.DetailItems.FirstOrDefault(e => e.Code == "ДК");
            if (item != null)
            {
                item.Dds = CurrentConto.Conto.Oborot;
                if (CurrentConto.Suma>0)
                {
                    item.DdsSuma = CurrentConto.Suma;
                }
                else
                {
                    item.DdsSuma = item.Dds*(item.DdsPercent/100);
                }
            }
            ddsDnevnikModel.Date = CurrentConto.Conto.Data;
            ddsDnevnikModel.DataF = CurrentConto.Conto.Data;
            ddsDnevnikModel.DocId = CurrentConto.Fn;
            ddsDnevnikModel.KindActivity = CurrentConto.TypeDds;
            ddsDnevnikModel.Nzdds = CurrentConto.Bulstad;
            ddsDnevnikModel.Bulstat = CurrentConto.Bulstad;
            ddsDnevnikModel.NameKontr = CurrentConto.Cn;
            ddsDnevnikModel.KindDoc = 1;
            ddsDnevnikModel.CodeDoc = "01";
            ddsDnevnikModel.A8 = 0;
            ddsDnevnikModel.IsSuma = 0;
            ddsDnevnikModel.Num = CurrentConto.Conto.Id;
            SaveDdsDnevnicModel(ddsDnevnikModel);
        }

        private IEnumerable<DdsItemModel> GetAllDnevItems(int type)
        {
            if (type==1 && ListP!=null)
            {
                return ListP;
            }
            else
            {
                if (ListS != null) return ListS;
            }
            List<DdsItemModel> result = new List<DdsItemModel>();
            string typednev = "DDSDNEVFIELDS";
            if (type == 1) typednev = "DDSDNEVSELLSFIELDS";
            string command = string.Format("SELECT * FROM {0}", typednev);
            DBManager. ExecuteReader(CommandType.Text, command);
            while (DBManager. DataReader.Read())
            {
                result.Add(new DdsItemModel
                {
                    DdsPercent = int.Parse(DBManager. DataReader["DDSPERCENT"].ToString()),
                    Name = DBManager. DataReader["NAME"].ToString(),
                    Id = int.Parse(DBManager. DataReader["ID"].ToString()),
                    Code = DBManager. DataReader["CODE"].ToString(),
                    IsNotComputed = false//true
                });
            }
            DBManager. DataReader.Close();
            if (type == 1)
            {
                ListP = result;
            }
            else
            {
                ListS = result;
            }
            return result;
        }

        private void SaveDdsDnevnicModel(DdsDnevnikModel ddsDnevnikModel)
        {
           
                DBManager. CreateParameters(20);
                DBManager. AddParameters(0, "@NOM", ddsDnevnikModel.Num);
                DBManager. AddParameters(1, "@BRANCH", ddsDnevnikModel.Branch);
                DBManager. AddParameters(2, "@DOCN", ddsDnevnikModel.DocId);
                DBManager. AddParameters(3, "@DATADOC", ddsDnevnikModel.Date);
                DBManager. AddParameters(4, "@KINDACTIVITY", ddsDnevnikModel.KindActivity);
                DBManager. AddParameters(5, "@KINDDOC", ddsDnevnikModel.KindDoc);
                DBManager. AddParameters(6, "@STOKE", ddsDnevnikModel.Stoke);
                DBManager. AddParameters(7, "@BULSTAD", ddsDnevnikModel.Bulstat);
                DBManager. AddParameters(8, "@NZDDS", ddsDnevnikModel.Nzdds);
                DBManager. AddParameters(9, "@LOOKUPID", ddsDnevnikModel.LookupID);
                DBManager. AddParameters(10, "@LOOKUPELEMENTID", ddsDnevnikModel.LookupElementID);
                DBManager. AddOutputParameters(11, "@NEWID", ddsDnevnikModel.Id);
                DBManager. AddParameters(12, "@NAMEKONTR", ddsDnevnikModel.NameKontr);
                DBManager. AddParameters(13, "@SUMA", ddsDnevnikModel.Suma);
                DBManager. AddParameters(14, "@DDSSUMA", ddsDnevnikModel.SumaDDS);
                DBManager. AddParameters(15, "@CODEDOC", ddsDnevnikModel.CodeDoc);
                DBManager. AddParameters(16, "@DATAF", ddsDnevnikModel.DataF);
                DBManager. AddParameters(17, "@A8", ddsDnevnikModel.A8);
                DBManager. AddParameters(18, "@CLNUM", ddsDnevnikModel.ClNum);
                DBManager. AddParameters(19, "@ISSUMA", ddsDnevnikModel.IsSuma);
                DBManager. ExecuteNonQuery(CommandType.StoredProcedure, "ADDDDSDNEV");
                ddsDnevnikModel.Id = (int)DBManager. Parameters[11].Value;
                DBManager. ExecuteNonQuery(CommandType.Text, string.Format("Delete from DDSDNEVTOFIELDS d where d.IDDDSDNEV={0}", ddsDnevnikModel.Id));
                foreach (var item in ddsDnevnikModel.DetailItems)
                {
                    string command =
                        string.Format(
                            "INSERT INTO DDSDNEVTOFIELDS (IDDDSDNEV, IDDDSFIELD, SUMADDS, SUMAWITHDDS,DDS,DDSP) VALUES ({0},{1},{2},{3},{4},{5})", ddsDnevnikModel.Id, item.Id, item.DdsSuma.ToString(Vf.LevFormatUI), item.DdsTotal.ToString(Vf.LevFormatUI), item.Dds.ToString(Vf.LevFormatUI), item.DdsPercent.ToString(Vf.LevFormatUI));
                    DBManager. ExecuteNonQuery(CommandType.Text, command);
                }
               
            


        }

        private void SaveConto(WrapConto CurrentConto)
        {
            CurrentConto.Conto.CDetails = "";
            CurrentConto.Conto.DDetails = "";
            foreach (SaldoItem currentsaldos in CurrentConto.ItemsCredits)
            {
                if (currentsaldos.Fieldkey==30)
                {
                    CurrentConto.Conto.OborotValutaK = currentsaldos.Valuedecimal;
                }
                if (currentsaldos.Fieldkey == 31)
                {
                    CurrentConto.Conto.OborotKolK = currentsaldos.Valuedecimal;
                }
                CurrentConto.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,currentsaldos.Value,currentsaldos.Lookupval);
            }
            foreach (SaldoItem currentsaldos in CurrentConto.ItemsDebits)
            {
                if (currentsaldos.Fieldkey == 30)
                {
                    CurrentConto.Conto.OborotValutaD = currentsaldos.Valuedecimal;
                }
                if (currentsaldos.Fieldkey == 31)
                {
                    CurrentConto.Conto.OborotKolD = currentsaldos.Valuedecimal;
                }
                CurrentConto.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name, currentsaldos.Value, currentsaldos.Lookupval);
            }
            SaveContoLocal(CurrentConto.Conto);
            if (CurrentConto.Conto.Id == 0) return;
            if (CurrentConto.ItemsCredits != null)
                foreach (SaldoItem currentsaldos in CurrentConto.ItemsCredits)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();
                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = CurrentConto.Conto.CreditAccount;
                    sa.DATA = DateTime.Now;
                    //if (currentsaldos.SelectedLookupItem != null && !string.IsNullOrWhiteSpace(currentsaldos.SelectedLookupItem.Key))
                    //{
                    //    if (currentsaldos.SelectedLookupItem.Key != null)
                    //    {
                    //        int rez;
                    //        sa.LOOKUPFIELDKEY = int.TryParse(currentsaldos.SelectedLookupItem.Key,out rez)?rez:0;
                    //        sa.LOOKUPVAL = currentsaldos.SelectedLookupItem.Value;
                    //    }
                        
                    //}
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;
                    sa.TYPEACCKEY = 2;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.VALUEMONEY = currentsaldos.Valuedecimal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.Valuedecimald;
                    sa.KURS = currentsaldos.ValueKurs;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.VALVAL = currentsaldos.ValueVal;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 2;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.CONTOID = CurrentConto.Conto.Id;
                    SaveContoMovement(sa);
                }
            if (CurrentConto.ItemsDebits != null)
                foreach (SaldoItem currentsaldos in CurrentConto.ItemsDebits)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();
                                               
                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID =  CurrentConto.Conto.DebitAccount;
                    sa.DATA = DateTime.Now;
                    //if (currentsaldos.SelectedLookupItem != null && !string.IsNullOrWhiteSpace(currentsaldos.SelectedLookupItem.Key))
                    //{
                    //    if (currentsaldos.SelectedLookupItem.Key != null)
                    //    {
                    //        int rez;
                    //        sa.LOOKUPFIELDKEY = int.TryParse(currentsaldos.SelectedLookupItem.Key,out rez)?rez:0;
                    //        sa.LOOKUPVAL = currentsaldos.SelectedLookupItem.Value;
                    //    }
                        
                    //}
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.TYPEACCKEY = 1;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.VALUEMONEY = currentsaldos.Valuedecimal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.Valuedecimald;
                    sa.KURS = currentsaldos.ValueKurs;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.VALVAL = currentsaldos.ValueVal;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 1;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;                    
                    sa.CONTOID = CurrentConto.Conto.Id;

                    SaveContoMovement(sa);
                }
        
        }

        private void SaveContoMovement(SaldoAnaliticModel saldoAnaliticModel)
        {
            
               
                DBManager. CreateParameters(17);
                DBManager. AddParameters(0, "@ACCID", saldoAnaliticModel.ACCID);
                DBManager. AddParameters(1, "@ACCFIELDKEY", saldoAnaliticModel.ACCFIELDKEY);
                DBManager. AddParameters(2, "@LOOKUPFIELDKEY", saldoAnaliticModel.LOOKUPFIELDKEY);
                DBManager. AddParameters(3, "@VAL", saldoAnaliticModel.VAL);
                DBManager. AddParameters(4, "@VALUEDATE", saldoAnaliticModel.VALUEDATE);
                DBManager. AddParameters(5, "@VALUEMONEY", saldoAnaliticModel.VALUEMONEY);
                DBManager. AddParameters(6, "@VALUENUM", saldoAnaliticModel.VALUENUM);
                DBManager. AddParameters(7, "@TYPEACCKEY", saldoAnaliticModel.TYPEACCKEY);
                DBManager. AddParameters(8, "@VALUED", saldoAnaliticModel.VALUED);
                DBManager. AddParameters(9, "@LOOKUPID", saldoAnaliticModel.LOOKUPID);
                DBManager. AddParameters(10, "@CONTOID", saldoAnaliticModel.CONTOID);
                DBManager. AddParameters(11, "@TYPE", saldoAnaliticModel.TYPE);
                DBManager. AddParameters(12, "@KURS", saldoAnaliticModel.KURS);
                DBManager. AddParameters(13, "@KURSD", saldoAnaliticModel.KURSD);
                DBManager. AddParameters(14, "@KURSM", saldoAnaliticModel.KURSM);
                DBManager. AddParameters(15, "@VALVAL", saldoAnaliticModel.VALVAL);
                DBManager. AddParameters(16, "@LOOKUPVAL", saldoAnaliticModel.LOOKUPVAL);
                DBManager. ExecuteNonQuery(CommandType.StoredProcedure, "ADDMOVENTCONTO");
       
           

            

            
        }

        private void SaveContoLocal(Conto conto)
        {
           
                CreateParameters(conto, true);
                DBManager. ExecuteNonQuery(CommandType.StoredProcedure, "ADDCONTO");
                conto.Id = (int)DBManager. Parameters[29].Value;
               
            

            
        }

        private void CreateParameters(Conto CurrentConto, bool b)
        {
            DBManager. CreateParameters(30);
            DBManager. AddParameters(0, "@Date", CurrentConto.Data);
            DBManager. AddParameters(1, "@Oborot", CurrentConto.Oborot);
            DBManager. AddParameters(2, "@Reason", CurrentConto.Reason);
            DBManager. AddParameters(3, "@Note", CurrentConto.Note);
            DBManager. AddParameters(4, "@DataInvoise", CurrentConto.DataInvoise);
            DBManager. AddParameters(5, "@NumberObject", CurrentConto.NumberObject);
            DBManager. AddParameters(6, "@DebitAccount", CurrentConto.DebitAccount);
            DBManager. AddParameters(7, "@CreditAccount", CurrentConto.CreditAccount);
            DBManager. AddParameters(8, "@FirmId", CurrentConto.FirmId);
            DBManager. AddParameters(9, "@DocumentId", CurrentConto.DocumentId);
            DBManager. AddParameters(10, "@CartotekaDebit", CurrentConto.CartotekaDebit);
            DBManager. AddParameters(11, "@CartotecaCredit", CurrentConto.CartotecaCredit);
            DBManager. AddParameters(12, "@DOCNUM", CurrentConto.DocNum);
            DBManager. AddParameters(13, "@OBOROTVALUTA", CurrentConto.OborotValutaD);
            DBManager. AddParameters(14, "@OBOROTKOL", CurrentConto.OborotKolD);
            DBManager. AddParameters(15, "@OBOROTVALUTAK", CurrentConto.OborotValutaK);
            DBManager. AddParameters(16, "@OBOROTKOLK", CurrentConto.OborotKolK);
            DBManager. AddParameters(17, "@FOLDER", CurrentConto.Folder);
            DBManager. AddParameters(18, "@ISDDSSALES", CurrentConto.IsDdsSales);
            DBManager. AddParameters(19, "@ISDDSPURCHASES", CurrentConto.IsDdsPurchases);
            DBManager. AddParameters(20, "@VOPPURCHASES", CurrentConto.VopPurchases);
            DBManager. AddParameters(21, "@VOPSALES", CurrentConto.VopSales);
            DBManager. AddParameters(22, "@ISDDSPURCHASESINCLUDED", CurrentConto.IsDdsPurchasesIncluded);
            DBManager. AddParameters(23, "@ISDDSSALESINCLUDED", CurrentConto.IsDdsSalesIncluded);
            DBManager. AddParameters(24, "@ISSALES", CurrentConto.IsSales);
            DBManager. AddParameters(25, "@ISPURCHASES", CurrentConto.IsPurchases);
            DBManager. AddParameters(26, "@DDETAILS", CurrentConto.DDetails);
            DBManager. AddParameters(27, "@CDETAILS", CurrentConto.CDetails);
            DBManager. AddParameters(28, "@USERID", CurrentConto.UserId);
            if (b) DBManager. AddOutputParameters(29, "@NEWID", CurrentConto.Id); else DBManager. AddParameters(29, "@ContoID", CurrentConto.Id);
        }
        private Regex Rgx { get; set; }
        private List<WrapConto> ReadCont()
        {
            Rgx = new Regex(@"^\d*\.{0,1}\d+$", RegexOptions.IgnoreCase);
            List<WrapConto> rez = new List<WrapConto>();
            var enc = Encoding.GetEncoding("Windows-1251");
            string[] lines = System.IO.File.ReadAllLines(@"E:\Samples\TempoSvn\Tempo2012\Documents\hronologia_s_kartoni.txt", enc);
            WrapConto item = new WrapConto
                                 {
                                     Conto = new Conto{FirmId = 2}
                                 };
            
            int numrow = 0;
            int total = 0;
            
            
            foreach (string line in lines.Skip(1))
            {
                if (line == "----------------------------------------------------------------------------------------")
                {
                    rez.Add(item);
                    item = new WrapConto
                    {
                        Conto = new Conto { FirmId = 2 }
                    };
                    numrow = 0;
                    DAccountsModel = null;
                    CAccountsModel = null;
                    total++;
                    continue;
                }
                if (numrow==0)
                {
                    firstrowparse(line, ref item);
                }
                if (numrow == 1)
                {
                    secondrowparse(line, ref item);
                }
                if (numrow == 2)
                {
                    thirthrowparse(line, ref item);
                }
                if (numrow == 3)
                {
                    fourrowparse(line, ref item);
                }
                if (numrow == 4)
                {
                    fivehrowparse(line, ref item);
                }
                if (numrow >= 5)
                {
                    sixandmorerowparse(line, ref item,numrow);
                }

                numrow++;
               
            }
            return rez;
        }

        private void sixandmorerowparse(string line, ref WrapConto item,int numrow)
        {
            line = line.Replace("|", "");
            var splitline = line.Split(':');
            bool ebane=false;
            string[] splitline1={""};
            if (splitline.Length>2)
            {
                ebane = true;
                var half = line.Substring(1, line.Length/2-1);
                var secondhalf = line.Substring(line.Length/2, line.Length/2-1);
                splitline = half.Split(':');
                splitline1 = secondhalf.Split(':');
            }
            if (numrow==5 && CAccountsModel != null && CAccountsModel.Short=="410")
            {
                var saldo = item.ItemsCredits.FirstOrDefault(e => e.Name == "Контрагент");
                if (saldo != null)
                {
                    saldo.Value = !ebane ? splitline[1].Trim() : splitline1[1].Trim();
                    var tag = saldo.GetFilters().ToList()[0];
                    var mainrez = saldo.GetDictionary(string.Format("AND \"{0}\"='{1}'", tag.FilterField, saldo.Value), string.Format(" order by \"{0}\"", tag.FilterField));
                    if (mainrez != null && mainrez.Count>1 && mainrez[1].Count>1) saldo.Lookupval = mainrez[1][1];
                }
            }
            if (numrow == 5 && DAccountsModel != null && DAccountsModel.Short == "410")
            {
                var saldo = item.ItemsDebits.FirstOrDefault(e => e.Name == "Контрагент");
                if (saldo != null)
                {
                    saldo.Value = splitline[1].Trim();
                    var tag = saldo.GetFilters().ToList()[0];
                    var mainrez = saldo.GetDictionary(string.Format("AND \"{0}\"='{1}'", tag.FilterField, saldo.Value), string.Format(" order by \"{0}\"", tag.FilterField));
                    if (mainrez != null && mainrez.Count > 1 && mainrez[1].Count > 1) saldo.Lookupval = mainrez[1][1];
                }
            }
            if (CAccountsModel != null && CAccountsModel.Short == "453/2")
            {
                item.Conto.IsDdsSales = 1;
                item.Conto.IsSales = 1;
                item.Conto.VopSales = "ДК";
                item.TypeDds = 2;
                if (numrow==5)
                {
                    item.Fn =!ebane?splitline[1].Trim():splitline1[1].Trim();
                    if (!Rgx.IsMatch(item.Fn))
                    {

                        string[] numbers;
                        numbers = Regex.Split(item.Fn, @"\D+");
                        item.Fn = numbers[0];
                    }
                    if (item != null)
                    {
                        var saldo = item.ItemsDebits.FirstOrDefault(e => e.Name == "Номер фактура");
                        if (saldo != null)
                        {
                            saldo.Value = item.Fn;
                            
                        }
                    }
                }
                if (numrow == 6)
                {
                    item.Cn = !ebane ? splitline[1].Trim() : splitline1[1].Trim();
                }
                if (numrow == 7)
                {
                    item.Bulstad = !ebane ? splitline[1].Trim() : splitline1[1].Trim();
                    
                }
                if (numrow == 8)
                {
                    decimal sum;
                    if (decimal.TryParse(!ebane ? splitline[1].Trim() : splitline1[1].Trim(), out sum))
                    {
                        item.Suma = sum;
                    }
                }

            }
            if (DAccountsModel != null && DAccountsModel.Short == "453/1")
            {
                item.Conto.IsDdsPurchases = 1;
                item.Conto.IsPurchases = 1;
                item.Conto.VopPurchases = "ДК";
                if (numrow == 5)
                {
                    item.Fn = splitline[1].Trim();
                    if (!Rgx.IsMatch(item.Fn))
                    {

                        string[] numbers;
                        numbers = Regex.Split(item.Fn, @"\D+");
                        item.Fn = numbers[0];
                    }
                    if (item != null)
                    {
                        var saldo = item.ItemsCredits.FirstOrDefault(e => e.Name == "Номер фактура");
                        if (saldo != null)
                        {
                            saldo.Value = item.Fn;

                        }
                    }
                }
                if (numrow == 6)
                {
                     item.Cn = splitline[1].Trim();
                }
                if (numrow == 7)
                {
                    item.Bulstad = splitline[1].Trim();
                }
                if (numrow == 8)
                {
                    decimal sum;
                    if (decimal.TryParse(splitline[1].Trim(), out sum))
                    {
                        item.Suma = sum;
                    }
                }
            }
        }


        private void firstrowparse(string line, ref WrapConto item)
        {
            line = line.Replace("|", "").Replace("НОМЕР", ":НОМЕР").Replace("ДАТА", ":ДАТА").Replace("ПАПКА", ":ПАПКА").Replace("СЧ.", ":СЧ.");
            var splitline = line.Split(':');
            item.Conto.CartotekaDebit = int.Parse(splitline[1].Trim());
            item.Conto.CartotecaCredit = All;
            item.Conto.DocNum = splitline[3].Trim();
            item.Conto.Data = new DateTime(2014, 10, int.Parse(splitline[5].Trim()));
            item.Conto.Folder = splitline[7].Trim();
            item.Conto.UserId = int.Parse(splitline[9].Trim());
        }

        private void secondrowparse(string line, ref WrapConto item)
        {
            line = line.Replace("|", "").Replace("Поделение", ":Поделение").Replace("Дата на док", ":Дата на док");
            var splitline = line.Split(':');
            item.Conto.DataInvoise = ParseData(splitline[5].Trim());
            item.CodeDds = splitline[1].Trim();
        }

        private void thirthrowparse(string line, ref WrapConto item)
        {
            line = line.Replace("|", "").Replace("ЗАБ.", ":ЗАБ.");
            var splitline = line.Split(':');
            item.Conto.Reason  = splitline[1].Trim();
            item.Conto.Note= splitline.Length > 3 ? splitline[3].Trim() : "";
        }

        private void fourrowparse(string line, ref WrapConto item)
        {
            line = line.Replace("|", "").Replace("обекта", "обекта:");
            var splitline = line.Split(':');
            item.Conto.NumberObject = int.Parse(splitline[1].Trim());
        }

        private void fivehrowparse(string line, ref WrapConto item)
        {
            line = line.Replace("|", "").Replace("ОБОРОТ", ":ОБОРОТ").Replace("К.СМ.", ":К.СМ.");
            var splitline = line.Split(':');
            item.Conto.Oborot = decimal.Parse(splitline[3].Trim());
            item.Conto.DebitAccount = readsmetka(splitline[1].Trim(),true);
            item.Conto.CreditAccount = readsmetka(splitline[5].Trim(),false);
            item.ItemsDebits =new List<SaldoItem>(LoadCreditAnaliticAtributes(context.LoadAllAnaliticfields(item.Conto.DebitAccount),1));
            item.ItemsCredits = new List<SaldoItem>(LoadCreditAnaliticAtributes(context.LoadAllAnaliticfields(item.Conto.CreditAccount),2));
        }

        private int readsmetka(string trim,bool isdebit)
        {
            var ac = acc.FirstOrDefault(e => e.Short == trim.Replace("//","/"));
            if (ac!=null)
            {
                if(isdebit)
                {
                    DAccountsModel = ac;
                }
                else
                {
                    CAccountsModel = ac;
                }
                return ac.Id;
            }
            else
            { 
                if (trim=="000")
                {
                    return -2;
                }
                return -1;
            }
            
        }

        private DateTime ParseData(string p)
        {
            var splitline = p.Split('.');
            return new DateTime(int.Parse(splitline[2]), int.Parse(splitline[1]), int.Parse(splitline[0]));
        }

        public List<IDbFields> GetAllFields()
        {

            return new List<IDbFields> { new DbField { DbType = "int", Title = "Id" },
                                         new DbField { DbType = "varchar", Title = "Oborot" },
                                         new DbField { DbType = "varchar", Title = "Reason" },
                                         new DbField { DbType = "varchar", Title = "Note" },
                                         new DbField { DbType = "varchar", Title = "DataInvoise" },
                                         new DbField { DbType = "int", Title = "NumberObject"}, 
                                         new DbField { DbType = "int", Title = "DebitAccount" },
                                         new DbField { DbType = "varchar", Title = "DocumentId" },
                                         new DbField { DbType = "varchar", Title = "FirmId" },
                                         new DbField { DbType = "varchar", Title = "DOCNUM" },
                                         new DbField { DbType = "varchar", Title = "OBOROTVALUTA" },
                                         new DbField { DbType = "int", Title = "OBOROTKOL"}
            };
           
        }

        public string FileName { get; set; }

        public string TableName { get; set; }

        public IEnumerable<SaldoItem> LoadCreditAnaliticAtributes(IEnumerable<SaldoAnaliticModel> fields, int typecpnto)
        {
            List<SaldoItem> saldoItems = new List<SaldoItem>();
            int offset = 16;
            if (typecpnto == 2) offset = 60;
            int current = 0;
            foreach (SaldoAnaliticModel analiticalFields in fields)
            {
                current++;
                //Titles.Add(analiticalFields.Name);
                SaldoItemTypes saldotype = SaldoItemTypes.String;
                if (analiticalFields.DBField == "integer")
                {
                    saldotype = SaldoItemTypes.Integer;

                }
                if (analiticalFields.DBField.Contains("DECIMAL"))
                {
                    saldotype = SaldoItemTypes.Currency;

                }
                if (analiticalFields.DBField == "Date")
                {
                    saldotype = SaldoItemTypes.Date;

                }

                SaldoItem saldoItem = new SaldoItem();
                saldoItem.Type = saldotype;
                saldoItem.Name = analiticalFields.Name;
                saldoItem.Value = analiticalFields.VAL;
                saldoItem.Fieldkey = analiticalFields.ACCFIELDKEY;
                saldoItem.IsK = typecpnto == 0;
                saldoItem.IsD = typecpnto == 1;
                saldoItem.Id = analiticalFields.ID;
                saldoItem.KursDif = analiticalFields.KURSD;
                saldoItem.ValueKurs = analiticalFields.KURS;
                saldoItem.MainKurs = analiticalFields.KURSM;
                saldoItem.ValueVal = analiticalFields.VALVAL;
                saldoItem.ValueCredit = analiticalFields.VALUEMONEY;
                saldoItem.Lookupval = analiticalFields.LOOKUPVAL;
                saldoItem.TabIndex = offset + current;
                if (analiticalFields.ACCFIELDKEY == 29 || analiticalFields.ACCFIELDKEY == 30 ||
                    analiticalFields.ACCFIELDKEY == 31)
                {
                    saldoItem.IsDK = true;
                    if (analiticalFields.ACCFIELDKEY == 30)
                    {
                        //saldoItem.InfoTitle = "Валутен курс";
                        saldoItem.IsVal = true;
                        //if (typecpnto == 0)
                        //{
                        //    try
                        //    {
                        //        saldoItem.InfoValue = DAccountsModel.EndSaldoL / DAccountsModel.EndSaldoV;
                        //    }
                        //    catch (Exception)
                        //    {
                        //        saldoItem.InfoValue = 0;
                        //    }

                        //}
                        //if (typecpnto == 1)
                        //{
                        //    try
                        //    {
                        //        saldoItem.InfoValue = CAccountsModel.EndSaldoL / CAccountsModel.EndSaldoV;
                        //    }
                        //    catch (Exception)
                        //    {
                        //        saldoItem.InfoValue = 0;
                        //    }

                        //}
                    }
                    if (analiticalFields.ACCFIELDKEY == 31)
                    {
                        //saldoItem.InfoTitle = "Единичнa цена";
                        saldoItem.IsKol = true;
                        saldoItem.ValueKol = analiticalFields.VALVAL;
                        saldoItem.OnePrice = analiticalFields.KURS;
                        //if (typecpnto == 1)
                        //{
                        //    try
                        //    {
                        //        saldoItem.InfoValue = (DAccountsModel.EndSaldoL / DAccountsModel.EndSaldoK);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        saldoItem.InfoValue = 0;
                        //    }

                        //}
                        //if (typecpnto == 0)
                        //{
                        //    try
                        //    {
                        //        saldoItem.InfoValue = CAccountsModel.EndSaldoL / CAccountsModel.EndSaldoK;
                        //    }
                        //    catch (Exception)
                        //    {
                        //        saldoItem.InfoValue = 0; ;
                        //    }

                        //}
                    }
                }
                if (analiticalFields.LOOKUPID != 0)
                {
                    saldoItem.LiD = analiticalFields.LOOKUPFIELDKEY;

                    saldoItem.Relookup = analiticalFields.LOOKUPID;
                    saldoItem.IsLookUp = true;
                    
                }
                saldoItems.Add(saldoItem);
            }
            return saldoItems;
        }

        public AccountsModel CAccountsModel { get; set; }

        public AccountsModel DAccountsModel { get; set; }



        public List<DdsItemModel> ListP { get; set; }
        public List<DdsItemModel> ListS { get; set; }


        public string SourceFile { get; set;}
    }
}
