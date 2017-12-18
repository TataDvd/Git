using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;

namespace Tempo2012.UI.WPF.CustomControls
{
    public class AccSelectorViewModel:BaseViewModel
    {
        public AccSelectorViewModel() 
        {
            _ShowContragentSum = false;
            _WithContragentSum = true;
            FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
            _allAccounts = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
        }
        public AccSelectorViewModel(bool withsum)
        {
            _ShowContragentSum = withsum;
            FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
            _allAccounts = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
        }
        private AccountsModel _dAccountsModel;

        public AccountsModel DAccountsModel
        {
            get { return _dAccountsModel; }
            set
            {
                if (_dAccountsModel != null && (value != null && _dAccountsModel.Id == value.Id)) return;
                _dAccountsModel = value;
                if (_dAccountsModel != null) if (value != null) _dAccountsModel.Search = value.Short;
                OnPropertyChanged("DAccountsModel");
                if (value != null)
                {
                    var list = Context.LoadAllAnaliticfields(value.Id);
                    ItemsDebit = new ObservableCollection<SaldoItem>(LoadCreditAnaliticAtributes(list.ToList().Where(e=>e.Required), 0));
                    OnPropertyChanged("ItemsDebit");
                }
            }
        }

        public ObservableCollection<SaldoItem> ItemsDebit { get; set; }

        internal string LoadAnaliticDetailsDebit(string accname)
        {
            if (!accname.Contains("/"))
            {
                int num;
                if (int.TryParse(accname, out num))
                {
                    var model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                    if (model != null)
                    {
                        DAccountsModel = model;
                        return model.Short;
                    }
                    else
                    {
                        //MainAcc mainAcc =
                        //    new MainAcc(
                        //        new AccountsModel
                        //        {
                        //            FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                        //            TypeAccountEx = 1,
                        //            Num = num
                        //        }, EditMode.Add, true, "", true);
                        //if (mainAcc.NoAcc)
                        //{
                            MessageBoxWrapper.Show("Нама такава сметка в сметкопланa");

                        //    ItemsDebit = new ObservableCollection<SaldoItem>();
                        //    DAccountsModel = null;
                        //    return "";
                        //}
                        //mainAcc.ShowDialog();
                        //if (mainAcc.DialogResult.HasValue && mainAcc.DialogResult.Value)
                        //{
                        //    AllAccounts = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                        //    model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == 0);
                        //    if (model != null)
                        //    {
                        //        DAccountsModel = model;
                        //        return model.Short;
                        //    }
                        //}
                        //ItemsDebit = new ObservableCollection<SaldoItem>();
                        //DAccountsModel = null;
                        //return "";


                    }
                }
                ItemsDebit = new ObservableCollection<SaldoItem>();
                DAccountsModel = null;
                return "";
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
                        DAccountsModel = model;
                        return model.Short;
                    }
                    //MainAcc mainAcc =
                    //    new MainAcc(
                    //        new AccountsModel
                    //        {
                    //            FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    //            TypeAccountEx = 1,
                    //            Num = num,
                    //            SubNum = subnum
                    //        }, EditMode.Add, false, "", true);
                    //mainAcc.ShowDialog();
                    //if (mainAcc.DialogResult.HasValue && mainAcc.DialogResult.Value)
                    //{
                    //    AllAccounts = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                    //    model = AllAccounts.FirstOrDefault(e => e.Num == num && e.SubNum == subnum);
                    //    if (model != null)
                    //    {
                    //        DAccountsModel = model;
                    //        return model.Short;
                    //    }
                    //}
                    //ItemsDebit = new ObservableCollection<SaldoItem>();
                    //DAccountsModel = null;
                    //return "";
                }
            }
            ItemsDebit = new ObservableCollection<SaldoItem>();
            DAccountsModel = null;
            return ""; 
        }
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
                        //        saldoItem.InfoValue = DAccountsModel.EndSaldoL/DAccountsModel.EndSaldoV;
                        //    }
                        //    catch (Exception)
                        //    {
                        //        saldoItem.InfoValue =0;
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
                        //    saldoItem.InfoTitle = "Единичнa цена";
                        saldoItem.IsKol = true;
                        saldoItem.ValueKol = analiticalFields.VALVAL;
                        saldoItem.OnePrice = analiticalFields.KURS;
                    }
                    //    if (typecpnto == 1)
                    //    {
                    //        try
                    //        {
                    //            saldoItem.InfoValue = (DAccountsModel.EndSaldoL / DAccountsModel.EndSaldoK);
                    //        }
                    //        catch (Exception)
                    //        {
                    //            saldoItem.InfoValue = 0;
                    //        }

                    //    }
                    //    if (typecpnto == 0)
                    //    {
                    //        try
                    //        {
                    //            saldoItem.InfoValue = CAccountsModel.EndSaldoL / CAccountsModel.EndSaldoK;
                    //        }
                    //        catch (Exception)
                    //        {
                    //            saldoItem.InfoValue = 0; ;
                    //        }

                    //    }
                    //}
                }
                if (analiticalFields.LOOKUPID != 0)
                {
                    //saldoItem.LiD = analiticalFields.LOOKUPFIELDKEY;

                    saldoItem.Relookup = analiticalFields.LOOKUPID;
                    saldoItem.IsLookUp = true;
                    //LookupModel lm = Context.GetLookup(analiticalFields.LOOKUPID);




                    //var list = Context.GetLookupDictionary(lm.LookUpMetaData.Tablename, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,""," FIRST 30 ");
                    //int k = 0;
                    //foreach (var enumerable in list)
                    //{
                    //    int i = 0;
                    //    SaldoItem saldoitem = new SaldoItem();
                    //    saldoitem.Name = saldoItem.Name;
                    //    saldoitem.Key = enumerable[lm.Fields[0].NameEng].ToString();
                    //    saldoitem.Value = enumerable[lm.Fields[1].NameEng].ToString();
                    //    saldoItem.LookUp.Add(saldoitem);
                    //    saldoItem.SelectedLookupItem =
                    //        saldoItem.LookUp.FirstOrDefault(e => e.Value == saldoItem.Value);
                    //}
                    //if (!string.IsNullOrWhiteSpace(analiticalFields.RTABLENAME))
                    //{
                    //    saldoItem.Key = analiticalFields.LOOKUPFIELDKEY.ToString();
                    //    saldoItem.IsLookUp = true;
                    //    var list1 = Context.GetLookup(analiticalFields.RTABLENAME, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                    //    k = 0;
                    //    foreach (IEnumerable<string> enumerable in list1)
                    //    {
                    //        int i = 0;
                    //        SaldoItem saldoitem = new SaldoItem();
                    //        saldoitem.Name = saldoItem.Name;
                    //        List<string> s = new List<string>(enumerable);
                    //        saldoitem.Key = s[1];
                    //        saldoitem.Value = s[2];

                    //        saldoItem.LookUp.Add(saldoitem);

                    //    }
                    //}
                }
                saldoItems.Add(saldoItem);
            }
            return saldoItems;
        }

        public int FirmaId { get; set; }

        private ObservableCollection<AccountsModel> _allAccounts;
        private bool _WithContragentSum;
        private bool _ShowContragentSum;

        public ObservableCollection<AccountsModel> AllAccounts
        {
            get
            {
                if (_allAccounts == null)
                {
                    FirmaId = ConfigTempoSinglenton.GetInstance().CurrentFirma.Id;
                    _allAccounts = new ObservableCollection<AccountsModel>(Context.GetAllAccounts(FirmaId));
                }
                return _allAccounts;
            }
            set
            {
                _allAccounts = value;
            }
        }

        public bool WithContragentSum { 
            get { return _WithContragentSum; }
            set
            {
                _WithContragentSum = value;
                OnPropertyChanged("WithContragentSum");
           }
        }

        public bool ShowContragentSum
        {
            get { return _ShowContragentSum; }
            set
            {
                _ShowContragentSum = value;
                OnPropertyChanged("ShowContragentSum");
            }
        }
    }
    
}
