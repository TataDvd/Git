using System.Collections.ObjectModel;
using System.Linq;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.Deklar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Interpretator;

namespace TempoTest
{


    /// <summary>
    ///This is a test class for DeklarviewModelTest and is intended
    ///to contain all DeklarviewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DeklarviewModelTest
    {


        private TestContext testContextInstance;
        private ContragentInfo LookupElementInfo;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion
        [TestMethod()]
        public void TestInterpretator()
        {
            var imp = new ZBaseInterpretator();
            imp.LoadProgramLine("E:\\test\\a.txt");
            imp.Apline_Sors();
        }

        /// <summary>
        ///A test for ReturnDeklar
        ///</summary>
        [TestMethod()]
        public void ReturnDeklarTest()
        {
            Dictionary<string, string> declar = new Dictionary<string, string>();
            declar.Add("data", DateTime.Now.ToShortDateString());
            declar.Add("firma", "Жоресте и ко");
            declar.Add("adress", "ЖК ОВЧА КУПЕЛ, БЛ.406");
            declar.Add("tel", "0898 70 64 35");
            declar.Add("dds", "123456789010123");
            declar.Add("iddds", "222111111111");
            declar.Add("sumatotal", "600.00");
            declar.Add("ddstotal", "109.00");
            declar.Add("sumatotal20", "500.00");
            declar.Add("ddstotal20", "100.00");
            declar.Add("sumatotalVOP", Vf.LevFormatUI);
            declar.Add("ddstotalVOP", Vf.LevFormatUI);
            declar.Add("sumatotalDrugi", Vf.LevFormatUI);
            declar.Add("ddstotalDrugi", Vf.LevFormatUI);
            declar.Add("sumatotal9", "100.00");
            declar.Add("ddstotal9", "9.00");

            declar.Add("sumatotal3glava", "0.11");
            declar.Add("sumatotal140146", "0.11");
            declar.Add("sumatotalVOD", "0.11");
            declar.Add("sumatotal21", "0.01");
            declar.Add("sumatotal69", "0.11");
            declar.Add("sumatotal3OVOP", "0.01");
            declar.Add("sumatotalVOP82", "0.11");

            declar.Add("dkpalen69", "0.69");
            declar.Add("dkpalen", "0.11");
            declar.Add("dkchast69", "0.69");
            declar.Add("dkchast", "0.12");
            declar.Add("godkor", "0.10");
            declar.Add("k73", "1.20");
            declar.Add("dktotal", "42.00");
            declar.Add("dds2040", "4.44");
            declar.Add("ddsk50", "3.00");
            declar.Add("ddsk50a", "3.00");
            declar.Add("ddsback1", "100.00");
            declar.Add("ddsback2", "9.00");
            declar.Add("ddsback3", Vf.LevFormatUI);
            declar.Add("DateTimeNow", DateTime.Now.ToShortDateString());
            declar.Add("mol", "Mol");
            declar.Add("dl", "Boss");
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = DeklarviewModel.ReturnDeklar(declar);

        }

        /// <summary>
        ///A test for Population
        ///</summary>
        [TestMethod()]
        public void PopulateConto()
        {
            Entrence.ConnectionString =
                "User ID=sysdba;Password=masterkey;Database=localhost:E:\\Samples\\Tempo2012MVVM\\Tempo2012\\Tempo2012.UI.WPF\\bin\\Debug\\data\\TEMPO2012.FDB;DataSource=localhost;";
            ObservableCollection<SaldoItem> ItemsCredit = new ObservableCollection<SaldoItem>();
            ObservableCollection<SaldoItem> ItemsDebit = new ObservableCollection<SaldoItem>();
            Tempo2012.EntityFramework.TempoDataBaseContext context =
                new Tempo2012.EntityFramework.TempoDataBaseContext();
            ObservableCollection<WraperConto> AllWrapedConto = new ObservableCollection<WraperConto>();
            var AllAccountsK =
                new ObservableCollection<AccountsModel>(
                    context.GetAllAccounts(1));
            var Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups(" where NAMEENG='k'"));

            var allconto = context.GetAllConto(1);
            foreach (Conto conto in allconto)
            {
                var cont = conto.Clone();
                cont.DataInvoise.AddDays(1);
                AllWrapedConto.Add(new WraperConto(cont));
            }
            foreach (WraperConto wraperConto in AllWrapedConto)
            {
                ItemsCredit = new ObservableCollection<SaldoItem>(LoadDetails(wraperConto.CurrentConto.Id, 2, context));
                ItemsDebit = new ObservableCollection<SaldoItem>(LoadDetails(wraperConto.CurrentConto.Id, 1, context));
                var oldid = wraperConto.Id;
                List<SaldoAnaliticModel> debit = new List<SaldoAnaliticModel>();
                List<SaldoAnaliticModel> credit = new List<SaldoAnaliticModel>();
                if (ItemsCredit != null)
                    foreach (SaldoItem currentsaldos in ItemsCredit)
                    {
                        SaldoAnaliticModel sa = new SaldoAnaliticModel();
                        sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                        sa.ACCID = wraperConto.CurrentConto.CreditAccount;
                        sa.DATA = wraperConto.CurrentConto.DataInvoise;
                        sa.TYPEACCKEY = 0;
                        sa.VALUEDATE = currentsaldos.ValueDate;
                        sa.VAL = currentsaldos.Value;
                        sa.VALUEMONEY = currentsaldos.Valuedecimal;
                        sa.VALUENUM = currentsaldos.ValueInt;
                        sa.VALUED = currentsaldos.Valuedecimald;
                        sa.KURS =currentsaldos.IsKol?currentsaldos.OnePrice:currentsaldos.ValueKurs;
                        sa.VALVAL = currentsaldos.IsKol?currentsaldos.ValueKol:currentsaldos.ValueVal;
                        sa.KURSM = currentsaldos.MainKurs;
                        sa.KURSD = currentsaldos.KursDif;
                        sa.TYPE = 2;
                        sa.LOOKUPID = currentsaldos.Relookup;
                        sa.CONTOID = wraperConto.CurrentConto.Id;
                        //context.SaveContoMovement(sa);
                        debit.Add(sa);
                    }
                if (ItemsDebit != null)
                    foreach (SaldoItem currentsaldos in ItemsDebit)
                    {
                        SaldoAnaliticModel sa = new SaldoAnaliticModel();

                        sa.ACCFIELDKEY = currentsaldos.Fieldkey;
                        sa.ACCID = wraperConto.CurrentConto.DebitAccount;
                        sa.DATA = DateTime.Now;
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
                        sa.TYPE = 1;
                        sa.LOOKUPID = currentsaldos.Relookup;

                        sa.CONTOID = wraperConto.CurrentConto.Id;
                        credit.Add(sa);
                        // context.SaveContoMovement(sa);
                    }
                context.SaveConto(wraperConto.CurrentConto,debit,credit);
                #region sells

                if (wraperConto.IsSales)
                {
                    DdsDnevnikModel ddsDnevnikModel = context.LoadDenevnicItem(oldid, 2);
                    //ddsDnevnikModel.DocId = CurrentWraperConto.CurrentConto.DocNum;
                    ddsDnevnikModel.Date = wraperConto.CurrentConto.Data;
                    ddsDnevnikModel.KindActivity = 2;

                    if (ddsDnevnikModel.LookupID == 0)
                    {
                        ddsDnevnikModel.CodeDoc = "01";
                        if (wraperConto.IsPurchases)
                        {
                            foreach (SaldoItem saldoItem in ItemsCredit)
                            {
                                if (saldoItem.IsLookUp)
                                {
                                    ddsDnevnikModel.LookupID = saldoItem.Relookup;
                                    if (saldoItem.SelectedLookupItem != null)
                                    {
                                        int lokalkey;
                                        if (int.TryParse(saldoItem.SelectedLookupItem.Key, out lokalkey))
                                        {
                                            ddsDnevnikModel.LookupElementID = lokalkey;
                                        }
                                    }
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
                        else
                        {
                            foreach (SaldoItem saldoItem in ItemsDebit)
                            {
                                if (saldoItem.IsLookUp)
                                {
                                    ddsDnevnikModel.LookupID = saldoItem.Relookup;
                                    if (saldoItem.SelectedLookupItem != null)
                                    {
                                        int lokalkey;
                                        if (int.TryParse(saldoItem.SelectedLookupItem.Key, out lokalkey))
                                        {
                                            ddsDnevnikModel.LookupElementID = lokalkey;
                                        }
                                    }
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

                    if (ddsDnevnikModel.LookupID > 0)
                    {
                        var SelectedLookup = new ObservableCollection<ContragentInfo>();
                        var Lookup = Lookups.FirstOrDefault(e => e.Id == ddsDnevnikModel.LookupID);
                        var list = context.GetLookup(Lookup.Tablename,
                                                     1);
                        foreach (List<string> li in list)
                        {
                            if (li != null && li.Count > 5)
                            {
                                SelectedLookup.Add(new ContragentInfo
                                                       {
                                                           Id = int.Parse(li[1]),
                                                           Name = li[2],
                                                           Bulstad = li[4],
                                                           Nzdds = li[5]
                                                       });

                            }
                        }
                        if (ddsDnevnikModel.LookupElementID > 0)
                        {
                            LookupElementInfo =
                                SelectedLookup.FirstOrDefault(e => e.Id == ddsDnevnikModel.LookupElementID);
                            ddsDnevnikModel.Num = wraperConto.CurrentConto.Id;
                            if (LookupElementInfo != null)
                            {
                                ddsDnevnikModel.LookupElementID = LookupElementInfo.Id;
                                ddsDnevnikModel.Bulstat = LookupElementInfo.Bulstad;
                                ddsDnevnikModel.Nzdds = LookupElementInfo.Nzdds;
                                ddsDnevnikModel.NameKontr = LookupElementInfo.Name;
                            }
                        }

                    }
                    ddsDnevnikModel.Num = wraperConto.CurrentConto.Id;
                    context.SaveDdsDnevnicModel(ddsDnevnikModel,ddsDnevnikModel.IsLinked);
                }


                #endregion

                #region purchases

                if (wraperConto.IsPurchases)
                {
                    var ddsDnevnikModel = context.LoadDenevnicItem(oldid, 1);
                    //ddsDnevnikModel.DocId = CurrentWraperConto.CurrentConto.DocNum;
                    ddsDnevnikModel.Date = wraperConto.CurrentConto.Data;
                    ddsDnevnikModel.KindActivity = 1;

                    if (ddsDnevnikModel.LookupID == 0)
                    {

                        foreach (SaldoItem saldoItem in ItemsCredit)
                        {
                            if (saldoItem.IsLookUp)
                            {
                                ddsDnevnikModel.LookupID = saldoItem.Relookup;

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

                    if (ddsDnevnikModel.LookupID > 0)
                    {
                        var SelectedLookup = new ObservableCollection<ContragentInfo>();
                        var Lookup = Lookups.FirstOrDefault(e => e.Id == ddsDnevnikModel.LookupID);
                        var list = context.GetLookup(Lookup.Tablename,
                                                     1);
                        foreach (List<string> li in list)
                        {
                            if (li != null && li.Count > 5)
                            {
                                SelectedLookup.Add(new ContragentInfo
                                                       {
                                                           Id = int.Parse(li[1]),
                                                           Name = li[2],
                                                           Bulstad = li[4],
                                                           Nzdds = li[5]
                                                       });

                            }
                        }
                        if (ddsDnevnikModel.LookupElementID > 0)
                        {
                            LookupElementInfo =
                                SelectedLookup.FirstOrDefault(e => e.Id == ddsDnevnikModel.LookupElementID);
                            ddsDnevnikModel.Num = wraperConto.CurrentConto.Id;
                            if (LookupElementInfo != null)
                            {
                                ddsDnevnikModel.LookupElementID = LookupElementInfo.Id;
                                ddsDnevnikModel.Bulstat = LookupElementInfo.Bulstad;
                                ddsDnevnikModel.Nzdds = LookupElementInfo.Nzdds;
                                ddsDnevnikModel.NameKontr = LookupElementInfo.Name;
                            }

                        }

                    }
                    ddsDnevnikModel.Num = wraperConto.CurrentConto.Id;
                    context.SaveDdsDnevnicModel(ddsDnevnikModel,ddsDnevnikModel.IsLinked);
                }
                #endregion

            }

        }

        /// <summary>
        ///A test for Population
        ///</summary>
        [TestMethod()]
        public void PopulateContoFromFile()
        {
            Entrence.ConnectionString =
                "User ID=sysdba;Password=masterkey;Database=localhost:E:\\Samples\\Tempo2012MVVM\\Tempo2012\\Tempo2012.UI.WPF\\bin\\Debug\\data\\TEMPO2012.FDB;DataSource=localhost;";
            ObservableCollection<SaldoItem> ItemsCredit = new ObservableCollection<SaldoItem>();
            ObservableCollection<SaldoItem> ItemsDebit = new ObservableCollection<SaldoItem>();
            TempoDataBaseContext context =new TempoDataBaseContext();
            ObservableCollection<WraperConto> AllWrapedConto = new ObservableCollection<WraperConto>();
            var AllAccountsK =
                new ObservableCollection<AccountsModel>(
                    context.GetAllAccounts(1));
            var Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups(" where NAMEENG='k'"));

            
            
               

            

        }

        private IEnumerable<SaldoItem> LoadDetails(int contoid, int typeconto, TempoDataBaseContext context)
        {
            if (contoid == 0) return new List<SaldoItem>();
            var items = ContoRepository.Instance.ContoItems(string.Format("{0}-{1}", contoid, typeconto));
            if (items!=null)
            return items;
            var itemsforcache = LoadCreditAnaliticAtributes(context.LoadContoDetails(contoid, typeconto), typeconto,
                                                            context);
            ContoRepository.Instance.Add(string.Format("{0}-{1}", contoid, typeconto), itemsforcache.ToList());
            return itemsforcache;
        }

        public IEnumerable<SaldoItem> LoadCreditAnaliticAtributes(IEnumerable<SaldoAnaliticModel> fields, int typecpnto,
                                                                  TempoDataBaseContext context)
        {
            List<SaldoItem> saldoItems = new List<SaldoItem>();
            foreach (SaldoAnaliticModel analiticalFields in fields)
            {
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
                SaldoItem saldoItem = new SaldoItem
                                          {
                                              Name = analiticalFields.Name,
                                              Type = saldotype,
                                              Value = analiticalFields.VAL,
                                              Fieldkey = analiticalFields.ACCFIELDKEY,
                                              IsK = typecpnto == 0,
                                              IsD = typecpnto == 1,
                                              Id = analiticalFields.ID,
                                              KursDif = analiticalFields.KURSD,
                                              ValueKurs = analiticalFields.KURS,
                                              MainKurs = analiticalFields.KURSM,
                                              ValueVal = analiticalFields.VALVAL,
                                              ValueCredit = analiticalFields.VALUEMONEY,
                                              Lookupval = analiticalFields.LOOKUPVAL
                                          };
                if (analiticalFields.ACCFIELDKEY == 29 || analiticalFields.ACCFIELDKEY == 30 ||
                    analiticalFields.ACCFIELDKEY == 31)
                {
                    saldoItem.IsDK = true;
                    if (analiticalFields.ACCFIELDKEY == 30)
                    {
                        saldoItem.InfoTitle = "Валутен курс";
                        saldoItem.IsVal = true;

                    }
                    if (analiticalFields.ACCFIELDKEY == 31)
                    {
                        saldoItem.InfoTitle = "Единичнa цена";
                        saldoItem.IsKol = true;

                    }
                }


                if (analiticalFields.LOOKUPID != 0)
                {
                    saldoItem.Key = analiticalFields.LOOKUPFIELDKEY.ToString();
                    saldoItem.IsLookUp = true;
                    saldoItem.Relookup = analiticalFields.LOOKUPID;
                    LookupModel lm = context.GetLookup(analiticalFields.LOOKUPID);
                    var list = context.GetLookup(lm.LookUpMetaData.Tablename,
                                                 1);
                    int k = 0;
                    foreach (IEnumerable<string> enumerable in list)
                    {
                        int i = 0;
                        SaldoItem saldoitem = new SaldoItem();
                        saldoitem.Name = saldoItem.Name;
                        foreach (string s in enumerable)
                        {

                            if (i == 2) saldoitem.Value = s;
                            if (i == 1) saldoitem.Key = s;
                            if (k == 0 && i == 1)
                            {

                                k++;
                            }
                            if (k == 1 && i == 2)
                            {

                                k++;
                            }
                            i++;
                        }
                        saldoItem.LookUp.Add(saldoitem);
                        saldoItem.SelectedLookupItem =
                            saldoItem.LookUp.FirstOrDefault(e => e.Value == saldoItem.Value);
                    }

                }

                saldoItems.Add(saldoItem);
            }
            return saldoItems;
        }

        public LookUpMetaData Lookup { get; set; }

    }
}