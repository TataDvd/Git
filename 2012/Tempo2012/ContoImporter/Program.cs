using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Interface;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Extentions;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;

namespace ContoImporter
{
    class Program
    {
        private static string CodeClient;

        public static void LoadConfig()
        {
            var loadConfigManager = new LoadConfigManager();

            Entrence.Mask = new CSearchAcc();

            Entrence.DdsSmetkaD = loadConfigManager.GetPrameter("DDSSMETKAD") ?? Entrence.DdsSmetkaD ?? "453/1";
            Entrence.DdsSmetkaK = loadConfigManager.GetPrameter("DDSSMETKAK") ?? Entrence.DdsSmetkaK ?? "453/2";
            int id;
            Entrence.UserId = int.TryParse(loadConfigManager.GetPrameter("USERID"), out id) ? id : Entrence.UserId > 0 ? Entrence.UserId : 1;
        }
        static void Main(string[] args)
        {
            try
            {
                ConfigTempoSinglenton currentconfig = ConfigTempoSinglenton.GetInstance();
                Entrence.ConnectionString = string.Format(currentconfig.ConectionString, AppDomain.CurrentDomain.BaseDirectory);
                LoadConfig();
                var context = new TempoDataBaseContext();
                FirmaId = ConfigTempoSinglenton.GetInstance().ActiveFirma;
                KindDocLookup = new ObservableCollection<LookUpSpecific>(context.GetAllDocTypes());
                TypeDocuments = new ObservableCollection<LookUpSpecific>(context.GetAllDocTypes());
                ItemsDdsDnevPurchases = new ObservableCollection<DdsItemModel>(context.GetAllDnevItems(1));
                ItemsDdsDnevSales = new ObservableCollection<DdsItemModel>(context.GetAllDnevItems(0));
                

                // Test if input arguments were supplied:
                if (args.Length < 1)
                {
                    System.Console.WriteLine("Невалидни Параметри");
                    return;
                }
                string path = args[0];
                string command = "2";
                if (args.Length > 1)
                {
                    command = args[1];
                }
                int num;
                switch (command)
                {
                    case "1":
                        {
                            serialiseConto(context);
                            break;
                        }
                    case "2":
                        {
                            InsertConto(context, path);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();

            }
            //serialiseConto(context);
        }

        public static ObservableCollection<LookUpSpecific> KindDocLookup { get; set; }

        public static int FirmaId { get; set; }

        private static void InsertConto(TempoDataBaseContext context, string path)
        {
            ContoAll allconto = new ContoAll();
            allconto.Conto = new Conto();
            allconto.Conto.UserId = Entrence.UserId;
            allconto.Conto.FirmId = FirmaId;
            var lines = File.ReadAllLines(path, Encoding.GetEncoding("Windows-1251"));
            foreach (string line in lines)
            {
                var command = line.Split('|');
                if (command.Length > 1)
                {
                    var oper = command[0];
                    switch (oper)
                    {
                        case "A":
                            DateTime date = DateTime.Now;
                            if (DateTime.TryParse(command[1], out date))
                            {
                                allconto.Conto.Data = date;
                            }
                            else
                            {
                                var c = command[1].Split('.');
                                if (c.Length > 2)
                                {
                                    date = new DateTime(int.Parse(c[2]), int.Parse(c[1]), int.Parse(c[0]));
                                    allconto.Conto.Data = date;
                                }
                                else
                                {
                                    allconto.Conto.Data = DateTime.Now;
                                }

                            }
                            AllAccounts = new ObservableCollection<AccountsModel>(context.GetAllAccounts(FirmaId,allconto.Conto.Data.Year));
                            break;
                        case "Z":
                            DateTime date1 = DateTime.Now;
                            if (DateTime.TryParse(command[1], out date1))
                            {
                                allconto.Conto.DataInvoise = date1;
                            }
                            else
                            {
                                var c = command[1].Split('.');
                                if (c.Length > 2)
                                {
                                    date = new DateTime(int.Parse(c[2]), int.Parse(c[1]), int.Parse(c[0]));
                                    allconto.Conto.DataInvoise = date1;
                                }
                                else
                                {
                                    allconto.Conto.DataInvoise = DateTime.Now;
                                }

                            }

                            break;
                        case "C":
                        {
                            allconto.Conto.Oborot = mydecimal.Parse(command[1]); 
                            break;
                        }
                        case "N":
                            allconto.Conto.DocNum = command[1];
                            break;
                        case "P":
                            allconto.Conto.Folder = command[1];
                            break;
                        case "O":
                            if (command.Length > 50)
                            {
                                command[1] = command[1].Substring(0, 50);
                            }
                            allconto.Conto.Reason = command[1];
                            break;
                        case "T":
                            if (command.Length > 50)
                            {
                                command[1] = command[1].Substring(0, 50);
                            }
                            allconto.Conto.Note = command[1];
                            break;
                        case "K":
                            allconto.Conto.KD = command[1];
                            break;
                        case "H":
                            allconto.KindDeal = command[1];
                            break;
                        case "W":
                            allconto.KindDds = command[1];
                            break;
                        case "Q":
                            allconto.CountActions = command[1];
                            break;
                        case "U":
                            int t = 0;
                            if (int.TryParse(command[1], out t))
                            {
                                 allconto.Conto.UserId = t;
                            }
                            break;
                        case "S":
                            //дебит
                            LoadAnaliticDetailsD(command[1], context, allconto);
                            break;
                        case "D":
                            //кредит
                            LoadAnaliticDetailsK(command[1], context, allconto);
                            break;
                        case "I":
                            //кредит
                            allconto.NameClient = command[1];
                            break;
                        case "9":
                            allconto.Conto.NumberObject = int.Parse(command[1]);
                            break;
                        case "1":
                            SetAnaliticVal(command[1], context, allconto, 0);
                            break;
                        case "3":
                            SetAnaliticVal(command[1], context, allconto, 1);
                            break;
                        case "5":
                            SetAnaliticVal(command[1], context, allconto, 2);
                            break;
                        case "7":
                            SetAnaliticVal(command[1], context, allconto, 3);
                            break;
                        case "2":
                            SetAnaliticVal(command[1], context, allconto, 4);
                            break;
                        case "4":
                            SetAnaliticVal(command[1], context, allconto, 5);
                            break;
                        case "6":
                            SetAnaliticVal(command[1], context, allconto, 6);
                            break;
                        case "8":
                            SetAnaliticVal(command[1], context, allconto, 7);
                            break;
                    }
                }
            }
            allconto.Conto.CDetails = "";
            allconto.Conto.DDetails = "";
            if (allconto.ItemsCredit != null)
            {
                foreach (SaldoItem currentsaldos in allconto.ItemsCredit)
                {
                    if (currentsaldos.Fieldkey == 30)
                    {
                        allconto.Conto.OborotValutaK = currentsaldos.ValueVal;
                        allconto.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                            currentsaldos.ValueVal, currentsaldos.Lookupval);
                        currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                    }
                    else
                    {
                        if (currentsaldos.Fieldkey == 31)
                        {
                            allconto.Conto.OborotKolK = currentsaldos.ValueKol;
                            allconto.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.ValueKol, currentsaldos.Lookupval);
                            //currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                        }
                        else
                        {
                            allconto.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.Value, currentsaldos.Lookupval);
                        }

                    }
                }
            }
            if (allconto.ItemsDebit != null)
            {
                foreach (SaldoItem currentsaldos in allconto.ItemsDebit)
                {
                    if (currentsaldos.Fieldkey == 30)
                    {
                        allconto.Conto.OborotValutaD = currentsaldos.ValueVal;
                        allconto.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                            currentsaldos.ValueVal, currentsaldos.Lookupval);
                        currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                    }
                    else
                    {
                        if (currentsaldos.Fieldkey == 31)
                        {
                            allconto.Conto.OborotKolD = currentsaldos.ValueKol;
                            allconto.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.ValueKol, currentsaldos.Lookupval);
                        }
                        else
                        {
                            allconto.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.Value, currentsaldos.Lookupval);
                        }

                    }

                }
            }
            if (allconto.KindDeal == "2")
            {
                allconto.Conto.IsDdsSales = 1;
                allconto.Conto.VopSales = allconto.KindDds;
                allconto.Conto.IsSales = 1;
            }
            else
            {
                if (allconto.KindDeal == "1")
                {
                    allconto.Conto.IsDdsPurchases = 1;
                    allconto.Conto.IsPurchases = 1;
                    allconto.Conto.VopPurchases = allconto.KindDds;
                }
                else
                {
                    if (allconto.KindDeal == "3")
                    {
                        allconto.Conto.IsDdsSales = 1;
                        allconto.Conto.IsSales = 1;
                        allconto.Conto.IsPurchases = 1;
                        allconto.Conto.IsDdsPurchases = 1;
                    }
                    else
                    {
                        allconto.Conto.IsDdsSales = 0;
                        allconto.Conto.IsSales = 0;
                        allconto.Conto.IsPurchases = 0;
                        allconto.Conto.IsDdsPurchases = 0;
                    }
                }
            }
        
            SaveMainConto(context, allconto);
            if (allconto.KindDeal == "1" || allconto.KindDeal == "2" && allconto.CountActions!="3")
            {
                SaveDDS(context, allconto);
            }
          
            if (allconto.CountActions == "3")
            {
                var dds=SaveDDS(context, allconto);
                allconto.Conto.IsDdsSales = 0;
                allconto.Conto.IsSales = 0;
                allconto.Conto.VopSales = "";
                allconto.Conto.IsDdsPurchases = 0;
                allconto.Conto.IsPurchases = 0;
                allconto.Conto.VopPurchases = "";
                allconto.Conto.Oborot = dds;
                if (allconto.KindDeal=="2")
                {
                    LoadAnaliticDetailsK(Entrence.DdsSmetkaK, context, allconto);
                    SaveMainConto(context, allconto);
                }
                else
                {
                    if (allconto.KindDeal == "1")
                    {
                        LoadAnaliticDetailsD(Entrence.DdsSmetkaD, context, allconto);
                        SaveMainConto(context, allconto);
                    }
                    else
                    {
                         
                    }
                }
               
            }
        }

        private static decimal SaveDDS(TempoDataBaseContext context, ContoAll allconto)
        {
            var itemsDdsDnevPurchases = new List<DdsItemModel>(context.GetAllDnevItems(1));
            var itemsDdsDnevSales = new List<DdsItemModel>(context.GetAllDnevItems(0));
            var currItemDdsDnevPurchases = itemsDdsDnevPurchases.FirstOrDefault(e => e.Code == allconto.KindDds);
            var currItemDdsDnevSales = itemsDdsDnevSales.FirstOrDefault(e => e.Code == allconto.KindDds);
            int codeDnev = 2;
            if (allconto.KindDeal == "1") codeDnev = 1;
            DdsDnevnikModel ddsDnevnikModel = context.LoadDenevnicItem(allconto.Conto.Id, codeDnev);

            //ddsDnevnikModel.DocId = allconto.Conto.DocNum;
            ddsDnevnikModel.Date = allconto.Conto.Data;
            ddsDnevnikModel.DataF = allconto.Conto.Data;
            ddsDnevnikModel.KindActivity = codeDnev;
            ddsDnevnikModel.KindDoc = 1;
            ddsDnevnikModel.Title = codeDnev == 1 ? "Дневник покупки" : "Дневник продажби";
            ddsDnevnikModel.CodeDoc = allconto.Conto.KD;
            ddsDnevnikModel.Stoke = allconto.Conto.Reason;
            ddsDnevnikModel.DdsIncluded = allconto.IsDdsInclude ? "ВКЛЮЧЕН ДДС" : "НЕВКЛЮЧЕН ДДС";
            if (codeDnev == 2)
            {

                if (allconto.ItemsCredit != null)
                {
                    foreach (SaldoItem saldoItem in allconto.ItemsCredit)
                    {
                        if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                        {
                            ddsDnevnikModel.LookupID = saldoItem.Relookup;
                            ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                            ddsDnevnikModel.ClNum = saldoItem.Value;

                        }
                        if (saldoItem.Name.Contains("Дата на фактура"))
                        {
                            ddsDnevnikModel.DataF = saldoItem.ValueDate;

                        }
                        if (saldoItem.Name.Contains("Номер фактура"))
                        {
                            ddsDnevnikModel.DocId = saldoItem.Value;
                        }
                    }
                }
                if (allconto.ItemsDebit != null)
                {
                    foreach (SaldoItem saldoItem in allconto.ItemsDebit)
                    {
                        if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                        {
                            ddsDnevnikModel.LookupID = saldoItem.Relookup;
                            ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                            ddsDnevnikModel.ClNum = saldoItem.Value;
                        }
                        if (saldoItem.Name.Contains("Дата на фактура"))
                        {
                            ddsDnevnikModel.DataF = saldoItem.ValueDate;

                        }
                        if (saldoItem.Name.Contains("Номер фактура"))
                        {
                            ddsDnevnikModel.DocId = saldoItem.Value;
                        }
                    }
                }
            }
            else
            {

                if (allconto.ItemsDebit != null)
                {
                    foreach (SaldoItem saldoItem in allconto.ItemsDebit)
                    {
                        if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                        {
                            ddsDnevnikModel.LookupID = saldoItem.Relookup;
                            ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                            ddsDnevnikModel.ClNum = saldoItem.Value;

                        }
                        if (saldoItem.Name.Contains("Дата на фактура"))
                        {
                            ddsDnevnikModel.DataF = saldoItem.ValueDate;

                        }
                        if (saldoItem.Name.Contains("Номер фактура"))
                        {
                            ddsDnevnikModel.DocId = saldoItem.Value;
                        }
                    }
                }
                if (allconto.ItemsDebit != null)
                {
                    foreach (SaldoItem saldoItem in allconto.ItemsCredit)
                    {
                        if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                        {
                            ddsDnevnikModel.LookupID = saldoItem.Relookup;
                            ddsDnevnikModel.LookupElementID = saldoItem.LiD;
                            ddsDnevnikModel.ClNum = saldoItem.Value;
                        }
                        if (saldoItem.Name.Contains("Дата на фактура"))
                        {
                            ddsDnevnikModel.DataF = saldoItem.ValueDate;

                        }
                        if (saldoItem.Name.Contains("Номер фактура"))
                        {
                            ddsDnevnikModel.DocId = saldoItem.Value;
                        }
                    }
                }
            }

            if (allconto.ItemsCredit == null && allconto.ItemsDebit==null)
            {
                ddsDnevnikModel.DataF = DataFactura;
                ddsDnevnikModel.Bulstat = Bulstad;
                ddsDnevnikModel.Nzdds = Vat??Bulstad;
                ddsDnevnikModel.DocId = Factura;
                ddsDnevnikModel.LookupID = 17;
                ddsDnevnikModel.ClNum = CodeClient;
            }
            decimal ddspercent=0;
            ddspercent = codeDnev == 1
                    ? currItemDdsDnevPurchases != null ? currItemDdsDnevPurchases.DdsPercent : 0
                    : currItemDdsDnevSales != null ? currItemDdsDnevSales.DdsPercent : 0;
                string name = codeDnev == 1
                    ? currItemDdsDnevPurchases != null ? currItemDdsDnevPurchases.Name : ""
                    : currItemDdsDnevSales != null ? currItemDdsDnevSales.Name : "";
            var vm = new DdsViewModel(ddsDnevnikModel, new DdsDnevnicItem
            {
                DdsPercent = ddspercent,
                DdsSuma = allconto.Conto.Oborot,
                Name = name,
                In = true
                });
                vm.SaveCommand.Execute(null);
            return Decimal.Round(allconto.Conto.Oborot*ddspercent/100,2);
        }

        private static void SaveMainConto(TempoDataBaseContext context, ContoAll allconto)
        {
            
            int ii = 0;
            List<SaldoAnaliticModel> debit = new List<SaldoAnaliticModel>();
            List<SaldoAnaliticModel> credit = new List<SaldoAnaliticModel>();
            if (allconto.ItemsCredit != null)
                foreach (SaldoItem currentsaldos in allconto.ItemsCredit)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();
                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = allconto.Conto.CreditAccount;
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
                    sa.TYPEACCKEY = 0;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.VALUEMONEY = currentsaldos.Valuedecimal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.Valuedecimald;
                    sa.KURS = currentsaldos.IsKol ? currentsaldos.OnePrice : currentsaldos.ValueKurs;
                    sa.VALVAL = currentsaldos.IsKol ? currentsaldos.ValueKol : currentsaldos.ValueVal;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 2;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.CONTOID = allconto.Conto.Id;
                    sa.SORTORDER = ii;
                    debit.Add(sa);
                    ii++;
                }
            ii = 0;
            if (allconto.ItemsDebit != null)
                foreach (SaldoItem currentsaldos in allconto.ItemsDebit)
                {
                    SaldoAnaliticModel sa = new SaldoAnaliticModel();

                    sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                    sa.ACCID = allconto.Conto.DebitAccount;
                    sa.DATA = DateTime.Now;
                    sa.LOOKUPFIELDKEY = currentsaldos.LiD;
                    sa.TYPEACCKEY = 0;
                    sa.VALUEDATE = currentsaldos.ValueDate;
                    sa.VAL = currentsaldos.Value;
                    sa.VALUEMONEY = currentsaldos.Valuedecimal;
                    sa.VALUENUM = currentsaldos.ValueInt;
                    sa.VALUED = currentsaldos.Valuedecimald;
                    sa.KURS = currentsaldos.IsKol ? currentsaldos.OnePrice : currentsaldos.ValueKurs;
                    sa.VALVAL = currentsaldos.IsKol ? currentsaldos.ValueKol : currentsaldos.ValueVal;
                    sa.KURSM = currentsaldos.MainKurs;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.KURSD = currentsaldos.KursDif;
                    sa.TYPE = 1;
                    sa.LOOKUPID = currentsaldos.Relookup;
                    sa.LOOKUPVAL = currentsaldos.Lookupval;
                    sa.CONTOID = allconto.Conto.Id;
                    sa.SORTORDER = ii;
                    credit.Add(sa);
                    ii++;
                }
            context.SaveConto(allconto.Conto,debit,credit);
        }

        private static void SetAnaliticVal(string s, TempoDataBaseContext context, ContoAll allconto, int i)
        {
            SaldoItem item = null;
            switch (i)
            {
                case 0:
                    if (allconto.ItemsDebit != null && allconto.ItemsDebit.Count > 0) item = allconto.ItemsDebit[0];
                    Bulstad = s;
                    if (item == null)
                    {
                        var item1 = new SaldoItem { SysLookup = false, Relookup = 17 };
                        var mainrez = item1.GetDictionary(
                            string.Format("AND \"{0}\"='{1}'", "BULSTAT", s),
                            string.Format(" order by \"{0}\"", "BULSTAT"));
                        if (mainrez.Count <= 1)
                        {
                            var lookupModel = context.GetLookup(17);
                            CodeClient = context.SelectMax(lookupModel.LookUpMetaData.Tablename, lookupModel.Fields[1].NameEng);
                            var lookupval = allconto.NameClient;
                            lookupModel.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                            context.SaveRow(new List<string> { CodeClient, CodeClient, lookupval, lookupval, s, s }, lookupModel, FirmaId);
                        }
                        else
                        {
                            CodeClient = mainrez[1][0];
                            allconto.NameClient = mainrez[1][1];
                            int h = 0;
                            if (int.TryParse(mainrez[1][0], out h))
                            {
                                LiD = item1.GetLookUpId(0);
                            }
                            if (mainrez[1].Count > 4)
                            {

                                Bulstad = mainrez[1][3];
                                Vat = mainrez[1][4];
                            }
                        }
                    }
                    break;
                case 1:
                    if (allconto.ItemsDebit != null && allconto.ItemsDebit.Count > 1) item = allconto.ItemsDebit[1];
                    
                    Factura = s;
                    break;
                case 2:
                    if (allconto.ItemsDebit != null && allconto.ItemsDebit.Count > 2) item = allconto.ItemsDebit[2];
                    DateTime date1;
                    if (DateTime.TryParse(s, out date1))
                    {
                        DataFactura = date1;
                    }
                    else
                    {
                        var c = s.Split('.');
                        if (c.Length > 2)
                        {
                            DataFactura = new DateTime(int.Parse(c[2]), int.Parse(c[1]), int.Parse(c[0]));
                            
                        }
                        else
                        {
                            DataFactura = DateTime.Now;
                        }

                    }
                    break;
                case 3:
                    if (allconto.ItemsDebit != null && allconto.ItemsDebit.Count > 3) item = allconto.ItemsDebit[3];
                    break;
                case 4:
                    if (allconto.ItemsCredit != null && allconto.ItemsCredit.Count > 0) item = allconto.ItemsCredit[0];
                    Bulstad = s;
                    if (item == null)
                    {
                        var item1 = new SaldoItem { SysLookup = false, Relookup = 17 };
                        var mainrez = item1.GetDictionary(
                            string.Format("AND \"{0}\"='{1}'", "BULSTAT", s),
                            string.Format(" order by \"{0}\"", "BULSTAT"));
                        if (mainrez.Count <= 1)
                        {
                            var lookupModel = context.GetLookup(17);
                            CodeClient = context.SelectMax(lookupModel.LookUpMetaData.Tablename, lookupModel.Fields[1].NameEng);
                            var lookupval = allconto.NameClient;
                            lookupModel.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                            context.SaveRow(new List<string> { CodeClient, CodeClient, lookupval, lookupval, s, s }, lookupModel, FirmaId);
                        }
                        else
                        {
                            CodeClient = mainrez[1][1];
                            allconto.NameClient = mainrez[1][2];
                            int h = 0;
                            if (int.TryParse(mainrez[1][0], out h))
                            {
                                LiD = item1.GetLookUpId(0);
                            }
                            if (mainrez[1].Count > 4)
                            {

                                Bulstad = mainrez[1][3];
                                Vat = mainrez[1][4];
                            }
                        }
                    }
                    break;
                case 5:
                    if (allconto.ItemsCredit != null && allconto.ItemsCredit.Count > 1) item = allconto.ItemsCredit[1];
                    Factura = s;
                    break;
                case 6:
                    if (allconto.ItemsCredit != null && allconto.ItemsCredit.Count > 2) item = allconto.ItemsCredit[2];
                    DateTime date2;
                    if (DateTime.TryParse(s, out date2))
                    {
                        DataFactura = date2;
                    }
                    else
                    {
                        var c = s.Split('.');
                        if (c.Length > 2)
                        {
                            DataFactura = new DateTime(int.Parse(c[2]), int.Parse(c[1]), int.Parse(c[0]));

                        }
                        else
                        {
                            DataFactura = DateTime.Now;
                        }

                    }
                    break;
                case 7:
                    if (allconto.ItemsCredit != null && allconto.ItemsCredit.Count > 3) item = allconto.ItemsCredit[3];
                    break;
            }
            if (item != null)
            {
                if (!item.IsLookUp)
                {
                    item.Value = s;
                }
                else
                {
                    if (item.Name == "Контрагент")
                    {
                        var mainrez = item.GetDictionary(
                            string.Format("AND \"{0}\"='{1}'", "BULSTAT",s ),
                            string.Format(" order by \"{0}\"", "BULSTAT"));
                        if (mainrez != null && mainrez.Count > 1)
                        {
                            item.Value = mainrez[1][0];
                            item.Lookupval = mainrez[1][1];
                            int h = 0;
                            if (int.TryParse(mainrez[1][0], out h))
                            {
                                item.LiD = item.GetLookUpId(0);
                            }
                            if (mainrez[1].Count > 4)
                            {

                                item.Bulstad = mainrez[1][3];
                                item.Vat = mainrez[1][4];
                            }
                        }
                        else
                        {
                            var lookupModel = context.GetLookup(17);
                            item.Value= context.SelectMax(lookupModel.LookUpMetaData.Tablename, lookupModel.Fields[1].NameEng);
                            item.Lookupval = allconto.NameClient;
                            item.Bulstad = s;
                            item.Vat = s;
                            lookupModel.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                            context.SaveRow(new List<string> { item.Value,item.Value, item.Lookupval, item.Lookupval, s, s }, lookupModel, FirmaId);
                        }
                    }
                }
            }
            
        }

        private static void serialiseConto(TempoDataBaseContext context)
        {
            var Allconto = context.GetAllConto(1, 2000, 20);
            ContoAll contoAll = new ContoAll();
            contoAll.Conto = Allconto.First();
            contoAll.ItemsDebit =
                new List<SaldoItem>(DbHelper.LoadCreditAnaliticAtributes(context.LoadContoDetails(contoAll.Conto.Id, 1), 1));

            contoAll.ItemsCredit =
                new List<SaldoItem>(DbHelper.LoadCreditAnaliticAtributes(context.LoadContoDetails(contoAll.Conto.Id, 2), 2));
            SerializeUtil.SerializeToXML<ContoAll>("test.xml", contoAll);
        }

        public static ObservableCollection<LookUpSpecific> TypeDocuments { get; set; }

        public static ObservableCollection<DdsItemModel> ItemsDdsDnevPurchases { get; set; }

        public static ObservableCollection<DdsItemModel> ItemsDdsDnevSales { get; set; }

        public static ObservableCollection<AccountsModel> AllAccounts { get; set; }

        public static void  LoadAnaliticDetailsD(string accname, TempoDataBaseContext context, ContoAll conto)
        {
            if (!accname.Contains("/"))
            {
                int num;
                if (int.TryParse(accname, out num))
                {
                    var model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    if (model != null)
                    {
                        conto.Conto.DebitAccount = model.Id;
                        var list = context.LoadAllAnaliticfields(model.Id);
                        conto.ItemsDebit = new List<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList(), 0));
                        foreach (SaldoItem saldoItem in conto.ItemsDebit)
                        {
                            if (saldoItem.Type == SaldoItemTypes.Date)
                            {
                                saldoItem.Value = conto.Conto.Data.ToShortDateString();
                            }
                        }
                        if (conto.ItemsDebit.Count == 0)
                        {
                            conto.ItemsDebit = null;
                        }
                    }



                }
            }
            else
            {
                int num, subnum;
                var ac = accname.Split('/');

                if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
                {
                    var model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                    if (model != null)
                    {
                        conto.Conto.DebitAccount = model.Id;
                        var list = context.LoadAllAnaliticfields(model.Id);
                        conto.ItemsDebit =new List<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList(), 0));
                        foreach (SaldoItem saldoItem in conto.ItemsDebit)
                        {
                            if (saldoItem.Type == SaldoItemTypes.Date)
                            {
                                saldoItem.Value = conto.Conto.Data.ToShortDateString();
                            }
                        }
                        if (conto.ItemsDebit.Count == 0)
                        {
                            conto.ItemsDebit = null;
                        }
                    }
                }


            }
        }

        public static void LoadAnaliticDetailsK(string accname, TempoDataBaseContext context, ContoAll conto)
        {
            if (!accname.Contains("/"))
            {
                int num;
                if (int.TryParse(accname, out num))
                {
                    var model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    if (model != null)
                    {
                        conto.Conto.CreditAccount = model.Id;

                    }
                    var list = context.LoadAllAnaliticfields(model.Id);
                    conto.ItemsCredit = new List<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList(), 1));
                    foreach (SaldoItem saldoItem in conto.ItemsCredit)
                    {
                        if (saldoItem.Type == SaldoItemTypes.Date)
                        {
                            saldoItem.Value = conto.Conto.Data.ToShortDateString();
                        }
                    }
                    if (conto.ItemsCredit.Count == 0)
                    {
                        conto.ItemsCredit = null;
                    }


                }
            }
            else
            {
                int num, subnum;
                var ac = accname.Split('/');

                if (int.TryParse(ac[0], out num) && int.TryParse(ac[1], out subnum))
                {
                    var model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                    if (model != null)
                    {
                        conto.Conto.CreditAccount = model.Id;
                        var list = context.LoadAllAnaliticfields(model.Id);
                        conto.ItemsCredit = new List<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList(), 1));
                        foreach (SaldoItem saldoItem in conto.ItemsCredit)
                        {
                            if (saldoItem.Type == SaldoItemTypes.Date)
                            {
                                saldoItem.Value = conto.Conto.Data.ToShortDateString();
                            }
                        }
                        if (conto.ItemsCredit.Count == 0)
                        {
                            conto.ItemsCredit = null;
                        }
                    }
                }


            }
        }
        public static IEnumerable<SaldoItem> LoadCreditAnaliticAtributes(IEnumerable<SaldoAnaliticModel> fields, int typecpnto)
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
                       
                        saldoItem.IsVal = true;
                       
                    }
                    if (analiticalFields.ACCFIELDKEY == 31)
                    {
                        
                        saldoItem.IsKol = true;
                        saldoItem.ValueKol = analiticalFields.VALVAL;
                        saldoItem.OnePrice = analiticalFields.KURS;
                    }
                   
                }
                if (analiticalFields.LOOKUPID != 0)
                {
                  
                    saldoItem.Relookup = analiticalFields.LOOKUPID;
                    saldoItem.IsLookUp = true;
                   
                }
                saldoItems.Add(saldoItem);
            }
            return saldoItems;
        }

        public AccountsModel DAccountsModel { get; set; }
        public static string Bulstad { get; private set; }
        public static string Factura { get; private set; }
        public static DateTime DataFactura { get; private set; }
        public static int LiD { get; private set; }
        public static string Vat { get; private set; }
    }
}
