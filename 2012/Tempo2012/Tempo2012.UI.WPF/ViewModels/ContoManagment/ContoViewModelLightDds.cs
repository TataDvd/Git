using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting;
using WindowsInput;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.Properties;
using Tempo2012.UI.WPF.ViewModels.Dnevnici;
using Tempo2012.UI.WPF.Views.Dnevnici;
using System.Windows;

namespace Tempo2012.UI.WPF.ViewModels.ContoManagment
{
    public partial class ContoViewModelLight
    {
       
        public delegate void SetFocusElement(object sender, SetFocusEventArg e);

        public event SetFocusElement SetFocusExecuted;

        protected virtual void OnSetFocusExecuted(SetFocusEventArg e)
        {
            if (SetFocusExecuted != null)
            {
                SetFocusExecuted(this, e);
            }
        }
        public ObservableCollection<LookUpSpecific> KindDocLookup { get; set; }
        public LookUpSpecific KindDoc
        {
            get { return _kindDoc; }
            set
            {
                _kindDoc = value;
                OnPropertyChanged("KindDoc");
                if (value != null) Kd = value.CodetId;
            }
        }
        private void Sells()
        {
            DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, 0);
            ddsDnevnikModel.DocId = CurrentWraperConto.CurrentConto.DocNum;
            ddsDnevnikModel.Date = CurrentWraperConto.CurrentConto.Data;
            ddsDnevnikModel.KindActivity = 1;
            ddsDnevnikModel.CodeDoc = Kd;
            ddsDnevnikModel.Stoke = CurrentWraperConto.CurrentConto.Reason;
            ddsDnevnikModel.Title = "Дневник покупки";
            DdsSellsView dialog = new DdsSellsView(ddsDnevnikModel,DddDelete,DdsSaved,DdsCancel);
            dialog.ShowDialog();
        }

        private void DdsCancel(object sender, DdsCancelEventArgs e)
        {
            CanceledDds = true;
            if (e.Kind == 1)
            {
                IsPurchases = false;
                IsDdsPurchases = false;
                IsDdsIncludePurchases = false;
                VopPurchases = "";
                CurrItemDdsDnevPurchases = null;
                if (CurrentWraperConto != null)
                {
                    CurrentWraperConto.IsPurchases = false;
                }
            }
            else
            {
                IsSales = false;
                IsDdsSales = false;
                IsDdsIncludeSales = false;
                VopSales = "";
                CurrItemDdsDnevSales = null;
                if (CurrentWraperConto != null)
                {
                    CurrentWraperConto.IsSales = false;
                }
            }

            IsDdsInclude = IsDdsIncludePurchases || IsDdsIncludeSales;
            IsDds = IsPurchases || IsSales;
            if (!IsDds)
            {

                KindDoc = null;
                Kd = "";
            }
        }

        private void DddDelete(object sender, DdsEventArgs e)
        {
            if (e.Kind == 1)
            {
                IsPurchases = false;
                IsDdsPurchases = false;
                IsDdsIncludePurchases = false;
                VopPurchases = "";
                CurrItemDdsDnevPurchases = null;
                if (CurrentWraperConto != null)
                {
                    CurrentWraperConto.IsPurchases = false;
                }
            }
            else
            { 
                IsSales = false;
                IsDdsSales = false;
                IsDdsIncludeSales = false;
                VopSales = "";
                CurrItemDdsDnevSales = null;
                if (CurrentWraperConto != null)
                {
                    CurrentWraperConto.IsSales = false;
                }
            }
             
            IsDdsInclude = IsDdsIncludePurchases || IsDdsIncludeSales;
            IsDds = IsPurchases || IsSales;
            if (!IsDds)
            {

                KindDoc = null;
                Kd = "";
            }
        }
        private void Purchase()
        {
            DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, 1);
            ddsDnevnikModel.DocId = CurrentWraperConto.CurrentConto.DocNum;
            ddsDnevnikModel.Date = CurrentWraperConto.CurrentConto.Data;
            ddsDnevnikModel.KindActivity = 2;
            ddsDnevnikModel.CodeDoc = Kd;
            ddsDnevnikModel.Stoke = CurrentWraperConto.CurrentConto.Reason;
            ddsDnevnikModel.Title = "Дневник продажби";
            DdsSellsView dialog = new DdsSellsView(ddsDnevnikModel,DddDelete,DdsSaved,DdsCancel);
            dialog.ShowDialog();
        }

        private void DdsSaved(object sender, DdsEventArgs e)
        {
            if (!e.IsSaved)
            {
                DddDelete(sender,e);
            }
            else
            {
                if (e.Kind == 1)
                {
                    IsDds = true;
                    IsPurchases = true;
                    CurrItemDdsDnevPurchases = ItemsDdsDnevPurchases.FirstOrDefault(e1 => e1.Code == e.Selestitem);
                    if (CurrentWraperConto != null)
                    {
                        CurrentWraperConto.IsPurchases = true;
                    }
                }
                else
                {
                    IsDds = true;
                    IsSales = true;
                    CurrItemDdsDnevSales = ItemsDdsDnevSales.FirstOrDefault(e1 => e1.Code == e.Selestitem);
                    if (CurrentWraperConto != null)
                    {
                        CurrentWraperConto.IsSales = true;
                    }
                }
                KindDoc = KindDocLookup.FirstOrDefault(e1 => e1.CodetId == e.CodeDoc);
                UpdateConto();
            }
        }

        private void DdsDelayUpdate(object sender, DdsEventArgs e)
        {
            
                if (e.Kind == 1)
                {
                    IsDdsNew = true;
                    IsPurchasesNew = true;
                    CurrItemDdsDnevPurchasesNew = ItemsDdsDnevPurchases.FirstOrDefault(e1 => e1.Code == e.Selestitem);
                }
                else
                {
                    IsDdsNew = true;
                    IsSalesNew = true;
                    CurrItemDdsDnevSalesNew = ItemsDdsDnevSales.FirstOrDefault(e1 => e1.Code == e.Selestitem);
                }
                KindDocNew = KindDocLookup.FirstOrDefault(e1 => e1.CodetId == e.CodeDoc);
                
            
        }

        private void UpdateUiDds()
        {
            IsDds = IsDdsNew;
            IsDdsPurchases = IsPurchasesNew;
            CurrItemDdsDnevPurchases = CurrItemDdsDnevPurchasesNew;
            IsDdsSales = IsSalesNew;
            CurrItemDdsDnevSales = CurrItemDdsDnevSalesNew;
        }

        internal void CheckedDdsSales(int i, bool isDds, bool isDdsInclude, bool isSales)
        {
            if ((Mode == EditMode.Add || Mode == EditMode.Edit) && ConfigTempoSinglenton.GetInstance().CurrentFirma.RegisterDds)
            {
                IsDds = isDds;
                IsDdsInclude = isDdsInclude;
                IsSales = isSales;
                if (i == -1) { VopSales = null;
                    CurrItemDdsDnevSales = null;return; }
                VopSales = VopsSales[i];
                OnSetFocusExecuted(string.IsNullOrWhiteSpace(Kd)
                        ? new SetFocusEventArg("DDS")
                        : new SetFocusEventArg("Ob"));
            }
        }

        internal void CheckedDdsPurchases(int i, bool isDds, bool isDdsInclude, bool isPurchases)
        {
            if ((Mode == EditMode.Add || Mode == EditMode.Edit) && ConfigTempoSinglenton.GetInstance().CurrentFirma.RegisterDds)
            {
                IsDds = isDds;
                IsDdsInclude = isDdsInclude;
                IsPurchases = isPurchases;
                if (i == -1) { VopPurchases = null;CurrItemDdsDnevPurchases = null; return; }
                VopPurchases = VopsPurchases[i];
                OnSetFocusExecuted(string.IsNullOrWhiteSpace(Kd)
                         ? new SetFocusEventArg("DDS")
                         : new SetFocusEventArg("Ob"));
            }

        }


        public bool IsDdsNew { get; set; }

        public bool IsPurchasesNew { get; set; }

        public DdsItemModel CurrItemDdsDnevPurchasesNew { get; set; }

        public bool IsSalesNew { get; set; }

        public DdsItemModel CurrItemDdsDnevSalesNew { get; set; }

        public LookUpSpecific KindDocNew { get; set; }

        private void UpdateRelatedDds()
        {
            if (MessageBoxWrapper.Show("Запази стари стойности в дневник?", "Предупреждение", MessageBoxWrapperButton.YesNo) == MessageBoxWrapperResult.Yes)
            {
                return;
            }
            bool haschange = false;
            DdsDnevnikModel ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, 1);
            ddsDnevnikModel.CodeDoc = Kd;
            ddsDnevnikModel.Stoke = CurrentWraperConto.CurrentConto.Reason;
            ddsDnevnikModel.Date = CurrentWraperConto.CurrentConto.Data;
            if (ItemsDebit != null && (ItemsCredit != null && (ItemsCredit.Count==0 && ItemsDebit.Count==0))) ddsDnevnikModel.DataF=CurrentWraperConto.CurrentConto.Data;
            if (ddsDnevnikModel.IsLinked)
            {
                foreach (SaldoItem saldoItem in ItemsDebit)
                {
                    if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                    {
                        ddsDnevnikModel.LookupID = saldoItem.Relookup;
                        ddsDnevnikModel.LookupElementID = 0;
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
                foreach (SaldoItem saldoItem in ItemsCredit)
                {
                    if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                    {
                        ddsDnevnikModel.LookupID = saldoItem.Relookup;
                        ddsDnevnikModel.LookupElementID = 0;
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

                var vm = new DdsViewModel(ddsDnevnikModel,
                    new DdsDnevnicItem
                    {
                        DdsPercent = CurrItemDdsDnevPurchases.DdsPercent,
                        DdsSuma = CurrentWraperConto.Oborot,
                        Name = CurrItemDdsDnevPurchases.Name,
                        In = true
                        });
                    vm.SaveFromOutside();
                
                haschange = true;
            }
            ddsDnevnikModel = Context.LoadDenevnicItem(CurrentWraperConto.CurrentConto.Id, 2);
            ddsDnevnikModel.CodeDoc = Kd;
            ddsDnevnikModel.Stoke = CurrentWraperConto.CurrentConto.Reason;
            ddsDnevnikModel.Date = CurrentWraperConto.CurrentConto.Data;
            if (ItemsDebit != null && (ItemsCredit != null && (ItemsCredit.Count == 0 && ItemsDebit.Count == 0))) ddsDnevnikModel.DataF = CurrentWraperConto.CurrentConto.Data;
            if (ddsDnevnikModel.IsLinked )
            {
                

                    foreach (SaldoItem saldoItem in ItemsCredit)
                    {
                        if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                        {
                            ddsDnevnikModel.LookupID = saldoItem.Relookup;
                            ddsDnevnikModel.LookupElementID = 0;
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
                    foreach (SaldoItem saldoItem in ItemsDebit)
                    {
                        if (saldoItem.IsLookUp && saldoItem.Name.Contains("Контрагент"))
                        {
                            ddsDnevnikModel.LookupID = saldoItem.Relookup;
                            ddsDnevnikModel.LookupElementID = 0;
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

                var vm = new DdsViewModel(ddsDnevnikModel,
                    new DdsDnevnicItem
                    {
                        DdsPercent = CurrItemDdsDnevSales.DdsPercent,
                        DdsSuma = CurrentWraperConto.Oborot,
                        Name = CurrItemDdsDnevSales.Name,
                        In = true
                        });
                    vm.SaveFromOutside();
               
                haschange = true;
            }
            if (haschange)
            {
                MessageBoxWrapper.Show(Resources.ContoViewModel_UpdateRelatedDds_Message,Resources.ContoViewModel_UpdateRelatedDds_Title);
            }
        }

        public bool CanceledDds { get; set; }
    }
}
