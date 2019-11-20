using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.Extentions;
using System.Linq;
using Tempo2012.UI.WPF.Views.Dialogs;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using System.Diagnostics;

namespace Tempo2012.UI.WPF.Views
{
    public class ImportInvoiseIndicatorViewModel : BaseViewModel
    {

        public int CurrentInvoise { get { return currentInvoise; } set { currentInvoise = value; OnPropertyChanged("CurrentInvoise"); } }
        public int TotalInvoise { get { return totalInvoise; } set { totalInvoise = value; OnPropertyChanged("TotalInvoise"); } }

        public List<string> ChangedNames = new List<string>();
        public List<string> NewNames = new List<string>();
        public int CurrentProgress
        {
            get { return currentProgress; }
            private set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        internal void Start()
        {

        }

        public ImportInvoiseIndicatorViewModel(string filename, int docnum)
            : base()
        {
            StartImportCommand = new DelegateCommand((o) => this.StartImport(), (o) => this.CanStartImport());
            Visible = Visibility.Hidden;
            AllAccounts = new List<AccountsModel>(Context.GetAllAccounts(ConfigTempoSinglenton.GetInstance().ActiveFirma));
            sm410 = AllAccounts.FirstOrDefault(e => e.Num == 410 && e.SubNum == 0);
            sm500 = AllAccounts.FirstOrDefault(e => e.Num == 500 && e.SubNum == 0);
            sm410_1 = AllAccounts.FirstOrDefault(e => e.Num == 410 && e.SubNum == 1);
            sm701 = AllAccounts.FirstOrDefault(e => e.Num == 701 && e.SubNum == 0);
            sm703 = AllAccounts.FirstOrDefault(e => e.Num == 702 && e.SubNum == 0);
            sm709 = AllAccounts.FirstOrDefault(e => e.Num == 702 && e.SubNum == 0);
            sm411 = AllAccounts.FirstOrDefault(e => e.Num == 411 && e.SubNum == 0);
            ddssmetka = AllAccounts.FirstOrDefault(e => e.Short == Entrence.DdsSmetkaK);
            KindDocLookup = new List<LookUpSpecific>(Context.GetAllDocTypes());
            TypeDocuments = new List<LookUpSpecific>(Context.GetAllDocTypes());
            ItemsDdsDnevPurchases = new List<DdsItemModel>(Context.GetAllDnevItems(1));
            ItemsDdsDnevSales = new List<DdsItemModel>(Context.GetAllDnevItems(0));
            DefaultDocNom = docnum;
            Lines = File.ReadAllLines(filename);
            TotalInvoise = Lines.Length;
        }

        private bool CanStartImport()
        {
            return true;
        }

        private void StartImport()
        {
            Visible = Visibility.Visible;
            ChangedNames.Clear();
            NewNames.Clear();
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(DoCopy);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ChangedNames.Count > 0)
            {
                string path = Path.Combine(Entrence.CurrentFirmaPath,string.Format("ChangedNames{0}{1}{2}{3}{4}",DateTime.Now.Day,DateTime.Now.Month,DateTime.Now.Year,DateTime.Now.Hour,DateTime.Now.Minute) + ".txt");
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(String.Join("\n", ChangedNames.ToArray()));
                }
                Process.Start(path);
            }
            if (NewNames.Count > 0)
            {
                string path = Path.Combine(Entrence.CurrentFirmaPath,string.Format("NewNames{0}{1}{2}{3}{4}",DateTime.Now.Day,DateTime.Now.Month,DateTime.Now.Year,DateTime.Now.Hour,DateTime.Now.Minute) + ".txt");
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(String.Join("\n", NewNames.ToArray()));
                }
                Process.Start(path);
            }

            Visible = Visibility.Hidden;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
            CurrentInvoise = e.ProgressPercentage;
        }

        private void DoCopy(object sender, DoWorkEventArgs e)
        {
            var i = 0;
            foreach (var line in Lines)
            {

                var item = line.Split('|');
                string nomFak = item[0];
                var itemdat = item[1].Split('.');
                DateTime dataF = new DateTime(int.Parse(itemdat[2]), int.Parse(itemdat[1]), int.Parse(itemdat[0]));
                string viddoc = item[2];
                string klient = item[3];
                string bulstat = item[4];
                string ddsnom = item[5];
                decimal sumastoki = mydecimal.Parse(item[6]);
                decimal sumausl = mydecimal.Parse(item[7]);
                decimal suma709 = mydecimal.Parse(item[8]);
                decimal dds = mydecimal.Parse(item[9]);
                int nachin = int.Parse(item[10]);
                decimal avans = mydecimal.Parse(item[11]);
                string nomfakavans= item[12];
                if (sumastoki == 0 && sumausl == 0 && suma709 == 0 && avans==0)
                {
                    continue;
                }
                
                ImportFact(nomFak, dataF, viddoc, klient, bulstat, ddsnom, sumastoki, sumausl, suma709, dds, nachin,avans, nomfakavans);
                DefaultDocNom++;
                bw.ReportProgress(i++);
            }
            
        }

        private void ImportFact(string nomFak, DateTime dataF, string viddoc, string klient, string bulstat, string ddsnom, decimal sumastoki, decimal sumausl, decimal suma709, decimal dds, int nachin,decimal avans,string nomfakavans)
        {
            if (string.IsNullOrWhiteSpace(bulstat))
            {
                bulstat = "999999999999999";
            }
            ContoAll c = new ContoAll();
            c.Conto = new Conto();
            c.Conto.DocNum = DefaultDocNom.ToString();
            c.Conto.FirmId = Entrence.CurrentFirma.Id;
            c.Conto.UserId = Entrence.UserId;
            c.Conto.Reason = "ПРОДАЖБА НА СТОКИ";
            c.KindDeal = viddoc;
            c.Conto.KD = viddoc;
            c.Conto.KindDoc = viddoc;
            var isavans = false;
            if (avans != 0)
            {
                if (suma709 == 0 && sumastoki == 0 && sumausl == 0)
                {//avans
                    c.Conto.Reason = "АВАНСОВО ПЛАЩАНЕ";
                    isavans = true;
                }
                else //doplastane
                {
                    c.Conto.Reason = "ПРОДАЖБА СТОКИ";
                }
            }
            if (sumastoki == 0)
            {
                c.Conto.Reason = "УСЛУГИ";
                if (suma709 != 0) c.Conto.Reason = "TECDOC";
            }
            if (string.IsNullOrWhiteSpace(ddsnom))
            {
                ddsnom = bulstat;
            }
            if (viddoc == "3")
            {
                suma709 *= -1;
                sumausl *= -1;
                sumastoki *= -1;
                dds *= -1;
            }
            if (nomFak.Length > 3)
            {
                c.Conto.NumberObject = int.Parse(nomFak.Substring(0, 3));
                if (nomFak.Substring(0, 3) == "001")
                {
                    c.Conto.NumberObject = 0;
                }
            }
            if (viddoc == "81")
            {
                nomFak = "8";
                bulstat = "999999999999999";
                ddsnom = "999999999999999";
            }
            c.Conto.Note = string.Format("{0},{1}", nomFak, klient);

            switch (nachin)
            {
                case 3:
                    c.Conto.Folder = "10";
                    c.Conto.DebitAccount = sm410.Id;
                    break;
                case 1: case 2: c.Conto.Folder = "1"; c.Conto.DebitAccount = sm500.Id; break;
                case 4: c.Conto.Folder = "10"; c.Conto.DebitAccount = sm410_1.Id; break;
                case 5: c.Conto.Folder = "10"; c.Conto.DebitAccount = sm410_1.Id; break;
            }
            c.Conto.KindDoc = viddoc;
            c.Conto.Data = dataF;
            c.Conto.DataInvoise = dataF;
            if (string.IsNullOrWhiteSpace(ddsnom))
            {
                if (nachin == 4 || nachin == 5 )
                {
                    klient = "ПОСТЕРМИНАЛИ";
                }
                else
                {
                    klient = "ФИЗИЧЕСКО ЛИЦЕ";
                }
            }
            var sbor = true;
            if (c.Conto.Reason.Length>50)
            {
                c.Conto.Reason = c.Conto.Reason.Substring(0, 50);
            }
            if ((suma709 != 0 && sumastoki != 0) || (suma709 != 0 && sumausl != 0) || (sumastoki != 0 && sumausl != 0))
            {
                sbor = false;
            }
            if (sbor)
            {
                if (sumastoki != 0)
                {
                    c.Conto.CreditAccount = sm701.Id;
                    c.Conto.Oborot = sumastoki;
                }
                if (sumausl != 0)
                {
                    c.Conto.CreditAccount = sm703.Id;
                    c.Conto.Oborot = sumausl;
                }
                if (suma709 != 0)
                {
                    c.Conto.CreditAccount = sm709.Id;
                    c.Conto.Oborot = suma709;
                }
                c.Conto.VopSales = "ДК";
                c.KindDds = "ДК";
                c.Conto.IsDdsSales = 1;
                c.Conto.IsSales = 1;
                LoadAnaliticDetailsD(c);
                LoadAnaliticDetailsK(c);
                c.NameClient = klient;
                SetAnaliticVal(ddsnom, c,0);
                SetAnaliticVal(nomFak, c,1);
                SetAnaliticVal(string.Format("{0}.{1}.{2}",dataF.Day, dataF.Month, dataF.Year),c,2);
                DataFactura = dataF;
                c.Conto.CDetails = "";
                c.Conto.DDetails = "";
                if (c.ItemsCredit != null)
                {
                    foreach (SaldoItem currentsaldos in c.ItemsCredit)
                    {
                        if (currentsaldos.Fieldkey == 30)
                        {
                            c.Conto.OborotValutaK = currentsaldos.ValueVal;
                            c.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.ValueVal, currentsaldos.Lookupval);
                            currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                        }
                        else
                        {
                            if (currentsaldos.Fieldkey == 31)
                            {
                                c.Conto.OborotKolK = currentsaldos.ValueKol;
                                c.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                    currentsaldos.ValueKol, currentsaldos.Lookupval);
                                //currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                            }
                            else
                            {
                                c.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                    currentsaldos.Value, currentsaldos.Lookupval);
                            }

                        }
                    }
                }
                if (c.ItemsDebit != null)
                {
                    foreach (SaldoItem currentsaldos in c.ItemsDebit)
                    {
                        if (currentsaldos.Fieldkey == 30)
                        {
                            c.Conto.OborotValutaD = currentsaldos.ValueVal;
                            c.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.ValueVal, currentsaldos.Lookupval);
                            currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                        }
                        else
                        {
                            if (currentsaldos.Fieldkey == 31)
                            {
                                c.Conto.OborotKolD = currentsaldos.ValueKol;
                                c.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                    currentsaldos.ValueKol, currentsaldos.Lookupval);
                            }
                            else
                            {
                                c.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                    currentsaldos.Value, currentsaldos.Lookupval);
                            }

                        }

                    }
                }
                if (dds == 0) 
                {
                    if (ddsnom.StartsWith("BG") || ddsnom.StartsWith("1") || ddsnom.StartsWith("2")
                        || ddsnom.StartsWith("3") || ddsnom.StartsWith("4") || ddsnom.StartsWith("5")
                        || ddsnom.StartsWith("6") || ddsnom.StartsWith("7") || ddsnom.StartsWith("8")
                        || ddsnom.StartsWith("0") || ddsnom.StartsWith("9"))

                    {
                        c.Conto.VopSales = "ОСВ";
                        c.KindDds = "ОСВ";
                    }
                    else
                    {
                        c.Conto.VopSales = "ВОД";
                        c.KindDds = "ВОД";
                    }
                }
                SaveMainConto(c);
                if (dds != 0)
                {
                    c.Conto.IsDdsSalesIncluded=0;
                    c.Conto.IsDdsSales = 1;
                    c.Conto.IsSales = 1;
                    SaveDDS(c);
                    c.Conto.IsDdsSales = 0;
                    c.Conto.IsSales = 0;
                    c.Conto.VopSales = "";
                    c.Conto.IsDdsPurchases = 0;
                    c.Conto.IsPurchases = 0;
                    c.Conto.VopPurchases = "";
                    c.Conto.Oborot = dds;
                    c.Conto.CreditAccount = ddssmetka.Id;
                    LoadAnaliticDetailsK(c);
                    SaveMainConto(c);
                }
                else
                {
                    c.Conto.IsDdsSalesIncluded = 0;
                    c.Conto.IsDdsSales = 1;
                    c.Conto.IsSales = 1;
                    SaveDDS(c);
                }
               
            }
            else
            {
                c.Conto.VopSales = "ДК";
                c.KindDds = "ДК";
                if (dds == 0)
                {
                    if (ddsnom.StartsWith("BG") || ddsnom.StartsWith("1") || ddsnom.StartsWith("2")
                        || ddsnom.StartsWith("3") || ddsnom.StartsWith("4") || ddsnom.StartsWith("5")
                        || ddsnom.StartsWith("6") || ddsnom.StartsWith("7") || ddsnom.StartsWith("8")
                        || ddsnom.StartsWith("0") || ddsnom.StartsWith("9"))

                    {
                        c.Conto.VopSales = "ОСВ";
                        c.KindDds = "ОСВ";
                    }
                    else
                    {
                        c.Conto.VopSales = "ВОД";
                        c.KindDds = "ВОД";
                    }
                }
                var sdelka = c.Conto.VopSales;
                var vid = c.KindDds;
                if (avans != 0 && isavans)
                {

                    c.Conto.CreditAccount = sm411.Id;
                    c.Conto.Oborot = avans;
                    c.Conto.IsDdsSales =1;
                    c.Conto.IsSales = 1;
                    c.Conto.VopSales = "";
                    c.Conto.IsDdsPurchases = 0;
                    c.Conto.IsPurchases = 0;
                    c.Conto.VopPurchases = "";
                    NewMethod(nomFak, dataF, klient, ddsnom, c);
                    if (dds != 0)
                    {
                        c.Conto.IsDdsSalesIncluded = 0;
                        c.Conto.IsDdsSales = 1;
                        c.Conto.IsSales = 1;
                        c.Sborno = true;
                        c.Conto.Oborot = suma709 + sumastoki + sumausl;
                        c.Conto.Oborot = SaveDDS(c);
                        c.Conto.IsDdsSales = 0;
                        c.Conto.IsSales = 0;
                        c.Conto.VopSales = "";
                        c.Conto.IsDdsPurchases = 0;
                        c.Conto.IsPurchases = 0;
                        c.Conto.VopPurchases = "";
                        //c.Conto.Oborot = dds;
                        c.Conto.CreditAccount = ddssmetka.Id;
                        LoadAnaliticDetailsK(c);
                        SaveMainConto(c);
                    }
                    else
                    {
                        c.Conto.IsDdsSalesIncluded = 0;
                        c.Conto.IsDdsSales = 1;
                        c.Conto.IsSales = 1;
                        c.Sborno = true;
                        c.Conto.Oborot = suma709 + sumastoki + sumausl;
                        SaveDDS(c);
                        c.Conto.IsDdsSales = 0;
                        c.Conto.IsSales = 0;
                        c.Conto.VopSales = "";
                        c.Conto.IsDdsPurchases = 0;
                        c.Conto.IsPurchases = 0;
                        c.Conto.VopPurchases = "";
                    }
                    return;
                }
                if (suma709 != 0)
                {
                    c.Conto.Reason = "TECDOC";
                    c.Conto.CreditAccount = sm709.Id;
                    c.Conto.Oborot = suma709;
                    c.Conto.IsDdsSales = 0;
                    c.Conto.IsSales = 0;
                    c.Conto.VopSales = "";
                    c.Conto.IsDdsPurchases = 0;
                    c.Conto.IsPurchases = 0;
                    c.Conto.VopPurchases = "";
                    NewMethod(nomFak, dataF, klient, ddsnom, c);
                }
                if (sumausl != 0)
                {
                    c.Conto.Reason = "УСЛУГИ";
                    c.Conto.CreditAccount = sm703.Id;
                    if (sumastoki != 0)
                    {
                        c.Conto.Oborot = sumausl;
                        c.Conto.IsDdsSales = 0;
                        c.Conto.IsSales = 0;
                        c.Conto.VopSales = "";
                        c.Conto.IsDdsPurchases = 0;
                        c.Conto.IsPurchases = 0;
                        c.Conto.VopPurchases = "";
                    }
                    else
                    {
                        c.Conto.Oborot = sumausl;
                        c.Conto.IsDdsSalesIncluded = 0;
                        c.Conto.IsDdsSales = 1;
                        c.Conto.IsSales = 1;
                        c.Conto.VopSales = sdelka;
                        c.KindDds = vid;
                        c.Conto.IsDdsPurchases = 0;
                        c.Conto.IsPurchases = 0;
                        c.Conto.VopPurchases = "";
                    }
                    NewMethod(nomFak, dataF, klient, ddsnom, c);
                    if (sumastoki == 0)
                    {
                        if (dds != 0)
                        {
                            c.Conto.IsDdsSalesIncluded = 0;
                            c.Conto.IsDdsSales = 1;
                            c.Conto.IsSales = 1;
                            c.Sborno = true;
                            c.Conto.Oborot = suma709 + sumastoki + sumausl;
                            c.Conto.Oborot = SaveDDS(c);
                            c.Conto.IsDdsSales = 0;
                            c.Conto.IsSales = 0;
                            c.Conto.VopSales = "";
                            c.Conto.IsDdsPurchases = 0;
                            c.Conto.IsPurchases = 0;
                            c.Conto.VopPurchases = "";
                            //c.Conto.Oborot = dds;
                            c.Conto.CreditAccount = ddssmetka.Id;
                            LoadAnaliticDetailsK(c);
                            SaveMainConto(c);

                        }
                        else
                        {
                            c.Conto.IsDdsSalesIncluded = 0;
                            c.Conto.IsDdsSales = 1;
                            c.Conto.IsSales = 1;
                            c.Sborno = true;
                            c.Conto.Oborot = suma709 + sumastoki + sumausl;
                            SaveDDS(c);
                            c.Conto.IsDdsSales = 0;
                            c.Conto.IsSales = 0;
                            c.Conto.VopSales = "";
                            c.Conto.IsDdsPurchases = 0;
                            c.Conto.IsPurchases = 0;
                            c.Conto.VopPurchases = "";

                        }
                    }
                }
                if (sumastoki != 0)
                {
                    c.Conto.Reason = "ПРОДАЖБА НА СТОКИ";
                    if (avans != 0)
                    {
                        c.Conto.Pr1 = nomfakavans;
                    }
                    c.Conto.CreditAccount = sm701.Id;
                    c.Conto.Oborot = sumastoki;
                    c.Conto.IsDdsSales = 1;
                    c.Conto.IsSales = 1;
                    c.Conto.VopSales = sdelka;
                    c.KindDds = vid;
                    NewMethod(nomFak, dataF, klient, ddsnom, c);
                   
                        if (dds != 0)
                        {
                            c.Conto.IsDdsSalesIncluded = 0;
                            c.Conto.IsDdsSales = 1;
                            c.Conto.IsSales = 1;
                            c.Sborno = true;
                            c.Conto.Oborot = suma709 + sumastoki + sumausl;
                            c.Conto.Oborot=SaveDDS(c);
                            c.Conto.IsDdsSales = 0;
                            c.Conto.IsSales = 0;
                            c.Conto.VopSales = "";
                            c.Conto.IsDdsPurchases = 0;
                            c.Conto.IsPurchases = 0;
                            c.Conto.VopPurchases = "";
                            //c.Conto.Oborot = dds;
                            c.Conto.CreditAccount = ddssmetka.Id;
                            LoadAnaliticDetailsK(c);
                            SaveMainConto(c);
                        }
                        else
                        {
                            c.Conto.IsDdsSalesIncluded = 0;
                            c.Conto.IsDdsSales = 1;
                            c.Conto.IsSales = 1;
                            c.Sborno = true;
                            c.Conto.Oborot = suma709 + sumastoki + sumausl;
                            SaveDDS(c);
                            c.Conto.IsDdsSales = 0;
                            c.Conto.IsSales = 0;
                            c.Conto.VopSales = "";
                            c.Conto.IsDdsPurchases = 0;
                            c.Conto.IsPurchases = 0;
                            c.Conto.VopPurchases = "";
                        }
                   if (avans!=0)
                    {
                        c.Conto.Reason = "ЗAКРИВАНЕ НА АВАНС";
                        c.Conto.Pr1 = nomFak;
                        c.Conto.CreditAccount = sm701.Id;
                        c.Conto.DebitAccount = sm411.Id;
                        c.Conto.Oborot = avans;
                        c.Conto.IsDdsSales = 0;
                        c.Conto.IsSales = 0;
                        c.Conto.VopSales = sdelka;
                        c.KindDds = vid;
                        NewMethod(nomfakavans, dataF, klient, ddsnom, c);
                    }
                }
                

                
            }
        }

        private void NewMethod(string nomFak, DateTime dataF, string klient, string ddsnom, ContoAll c)
        {
            
            LoadAnaliticDetailsD(c);
            LoadAnaliticDetailsK(c);
            c.NameClient = klient;
            SetAnaliticVal(ddsnom, c, 0);
            SetAnaliticVal(nomFak, c, 1);
            SetAnaliticVal(string.Format("{0}.{1}.{2}", dataF.Day, dataF.Month, dataF.Year), c, 2);
            DataFactura = dataF;
            c.Conto.CDetails = "";
            c.Conto.DDetails = "";
            if (c.ItemsCredit != null)
            {
                foreach (SaldoItem currentsaldos in c.ItemsCredit)
                {
                    if (currentsaldos.Fieldkey == 30)
                    {
                        c.Conto.OborotValutaK = currentsaldos.ValueVal;
                        c.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                            currentsaldos.ValueVal, currentsaldos.Lookupval);
                        currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                    }
                    else
                    {
                        if (currentsaldos.Fieldkey == 31)
                        {
                            c.Conto.OborotKolK = currentsaldos.ValueKol;
                            c.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.ValueKol, currentsaldos.Lookupval);
                            //currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                        }
                        else
                        {
                            c.Conto.CDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.Value, currentsaldos.Lookupval);
                        }

                    }
                }
            }
            if (c.ItemsDebit != null)
            {
                foreach (SaldoItem currentsaldos in c.ItemsDebit)
                {
                    if (currentsaldos.Fieldkey == 30)
                    {
                        c.Conto.OborotValutaD = currentsaldos.ValueVal;
                        c.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                            currentsaldos.ValueVal, currentsaldos.Lookupval);
                        currentsaldos.Value = currentsaldos.ValueVal.ToString(Vf.LevFormatUI);
                    }
                    else
                    {
                        if (currentsaldos.Fieldkey == 31)
                        {
                            c.Conto.OborotKolD = currentsaldos.ValueKol;
                            c.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.ValueKol, currentsaldos.Lookupval);
                        }
                        else
                        {
                            c.Conto.DDetails += string.Format("{0} - {1} {2}\n", currentsaldos.Name,
                                currentsaldos.Value, currentsaldos.Lookupval);
                        }

                    }

                }

            }
            SaveMainConto(c);
        }

        private decimal SaveDDS(ContoAll allconto)
        {
            var currItemDdsDnevPurchases = ItemsDdsDnevPurchases.FirstOrDefault(e => e.Code == allconto.KindDds);
            var currItemDdsDnevSales = ItemsDdsDnevSales.FirstOrDefault(e => e.Code == allconto.KindDds);
            var kindddds = KindDocLookup.FirstOrDefault(e => e.CodetId == allconto.KindDeal);
            int codeDnev = 2;
            if (allconto.KindDeal == "1") codeDnev = 1;
            DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(allconto.Conto.Id, codeDnev);

            //ddsDnevnikModel.DocId = allconto.Conto.DocNum;
            ddsDnevnikModel.IsSuma = allconto.Sborno ? 1:0;
            ddsDnevnikModel.Date = allconto.Conto.Data;
            ddsDnevnikModel.DataF = allconto.Conto.Data;
            ddsDnevnikModel.KindActivity = codeDnev;
            ddsDnevnikModel.KindDoc = kindddds.Id;
            ddsDnevnikModel.Title = codeDnev == 1 ? "Дневник покупки" : "Дневник продажби";
            ddsDnevnikModel.CodeDoc = kindddds.CodetId;
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

            if (allconto.ItemsCredit == null && allconto.ItemsDebit == null)
            {
                ddsDnevnikModel.DataF = DataFactura;
                ddsDnevnikModel.Bulstat = Bulstad;
                ddsDnevnikModel.Nzdds = Vat ?? Bulstad;
                ddsDnevnikModel.DocId = Factura;
                ddsDnevnikModel.LookupID = 17;
                ddsDnevnikModel.ClNum = CodeClient;
            }
            decimal ddspercent = 0;
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
            vm.ddsDnevnikModel.NameKontr = allconto.NameClient;
            vm.SaveCommand.Execute(null);
            return Decimal.Round(allconto.Conto.Oborot * ddspercent / 100, 2);
        }
        private void SetAnaliticVal(string s,  ContoAll allconto, int i)
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
                             string.Format("AND \"{0}\"='{1}' OR \"{0}\"='BG{1}'", "BULSTAT", s.Replace("BG", "")),
                            string.Format(" order by \"{0}\"", "BULSTAT"));
                        if (mainrez.Count <= 1)
                        {
                            if (!NewNames.Contains(allconto.NameClient+ " " + Bulstad)) { NewNames.Add(allconto.NameClient+ " " + Bulstad); }
                            var lookupModel = Context.GetLookup(17);
                            CodeClient = Context.SelectMax(lookupModel.LookUpMetaData.Tablename, lookupModel.Fields[1].NameEng);
                            var lookupval = allconto.NameClient;
                            lookupModel.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                            Context.SaveRow(new List<string> { CodeClient, CodeClient, lookupval, lookupval, s, s }, lookupModel, Entrence.CurrentFirma.Id);
                        }
                        else
                        {
                            //
                            CodeClient = mainrez[1][0];
                            if (allconto.NameClient != mainrez[1][2])
                            {
                                if (!ChangedNames.Contains(allconto.NameClient + " " + Bulstad)) { ChangedNames.Add(allconto.NameClient+ " " + Bulstad); }
                            }
                            int h = 0;
                            if (int.TryParse(mainrez[1][0], out h))
                            {
                               var LiD = item1.GetLookUpId(0);
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
                             string.Format("AND \"{0}\"='{1}' OR \"{0}\"='BG{1}'", "BULSTAT", s.Replace("BG", "")),
                            string.Format(" order by \"{0}\"", "BULSTAT"));
                        if (mainrez.Count <= 1)
                        {
                            if (!NewNames.Contains(allconto.NameClient + " " + Bulstad)) { NewNames.Add(allconto.NameClient + " " + Bulstad); }
                            var lookupModel = Context.GetLookup(17);
                            CodeClient = Context.SelectMax(lookupModel.LookUpMetaData.Tablename, lookupModel.Fields[1].NameEng);
                            var lookupval = allconto.NameClient;
                            lookupModel.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                            Context.SaveRow(new List<string> { CodeClient, CodeClient, lookupval, lookupval, s, s }, lookupModel, Entrence.CurrentFirma.Id);
                        }
                        else
                        {
                            CodeClient = mainrez[1][1];
                            if (allconto.NameClient != mainrez[1][2])
                            {
                                if (!ChangedNames.Contains(allconto.NameClient + " " + Bulstad)) { ChangedNames.Add(allconto.NameClient+" "+Bulstad); }
                            }
                            //allconto.NameClient = ;
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
                            string.Format("AND \"{0}\"='{1}' OR \"{0}\"='BG{1}'", "BULSTAT", s.Replace("BG","")),
                            string.Format(" order by \"{0}\"", "BULSTAT"));
                        if (mainrez != null && mainrez.Count > 1)
                        {
                            item.Value = mainrez[1][0];
                            if (allconto.NameClient!=mainrez[1][1])
                            {
                                if (!ChangedNames.Contains(allconto.NameClient + " " + Bulstad)) { ChangedNames.Add(allconto.NameClient+ " " + Bulstad); }
                            }
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
                            if (!NewNames.Contains(allconto.NameClient + " " + Bulstad)) { NewNames.Add(allconto.NameClient + " " + Bulstad); }
                            var lookupModel = Context.GetLookup(17);
                            item.Value = Context.SelectMax(lookupModel.LookUpMetaData.Tablename, lookupModel.Fields[1].NameEng);
                            item.Lookupval = allconto.NameClient;
                            item.Bulstad = s;
                            item.Vat = s;
                            lookupModel.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                            Context.SaveRow(new List<string> { item.Value, item.Value, item.Lookupval, item.Lookupval, s, s }, lookupModel, Entrence.CurrentFirma.Id);
                        }
                    }
                }
            }

        }
        public ICommand StartImportCommand { get; private set; }

        public void LoadAnaliticDetailsD(ContoAll conto)
        {

            var list = Context.LoadAllAnaliticfields(conto.Conto.DebitAccount);
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

        private  void SaveMainConto(ContoAll allconto)
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
            Context.SaveConto(allconto.Conto, debit, credit);
        }


        public void LoadAnaliticDetailsK(ContoAll conto)
        {

            var list = Context.LoadAllAnaliticfields(conto.Conto.CreditAccount);
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
    

        public  IEnumerable<SaldoItem> LoadCreditAnaliticAtributes(IEnumerable<SaldoAnaliticModel> fields, int typecpnto)
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

        private Visibility _visible;
       
        private BackgroundWorker bw;
        private int currentProgress;
        private int currentInvoise;
        private int totalInvoise;
        private AccountsModel sm410;
        private AccountsModel sm411;
        private AccountsModel sm500;
        private AccountsModel sm410_1;
        private AccountsModel sm701;
        private AccountsModel sm703;
        private AccountsModel sm709;
        private AccountsModel ddssmetka;

        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged("Visible"); }
        }

        public int DefaultDocNom { get; internal set; }
        public string[] Lines { get; private set; }
        public List<AccountsModel> AllAccounts { get; private set; }
        public List<LookUpSpecific> KindDocLookup { get; private set; }
        public List<LookUpSpecific> TypeDocuments { get; private set; }
        public List<DdsItemModel> ItemsDdsDnevPurchases { get; private set; }
        public List<DdsItemModel> ItemsDdsDnevSales { get; private set; }
        public string Bulstad { get; private set; }
        public string CodeClient { get; private set; }
        public string Vat { get; private set; }
        public string Factura { get; private set; }
        public DateTime DataFactura { get; private set; }
        public int LiD { get; private set; }
    }
}