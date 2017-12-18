using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework.Models;
using System.Collections.ObjectModel;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class ContoViewModel:BaseViewModel
    {
        public ContoViewModel():base()
        {
            TypeDocuments = new ObservableCollection<LookUpSpecific>(context.GetAllDocTypes());
            CartotecaDebitAll = new ObservableCollection<CartotecaDebit>(context.GetAllCartotecaDebit());
            CartotecaCreditAll = new ObservableCollection<CartotecaCredit>(context.GetAllCartotecaCredit());
            //AllNationalAccounts = new ObservableCollection<LookUpSpecific>(context.GetAllNationalAccounts());
            AllConto = new ObservableCollection<Conto>(context.GetAllConto());
            CurrentConto = AllConto.Last();

        }
        private Conto _CurrentConto;
        public Conto CurrentConto
        {
            get {return _CurrentConto;}
            set
            {
                _CurrentConto = value;
                CurrentCartotecaCredit =new ObservableCollection<CartotecaCredit>(CartotecaCreditAll.Where(e => e.Id == value.CartotecaCredit));
                CurrentCartotecaDebit = new ObservableCollection<CartotecaDebit>(CartotecaDebitAll.Where(e => e.Id == value.CartotekaDebit));
                OnPropertyChanged("CurrentCartotecaCredit");
                OnPropertyChanged("CurrentCartotecaDebit");
                OnPropertyChanged("CurrentConto");
            }
        }
        public ObservableCollection<Conto> AllConto { get; set;}
        public ObservableCollection<CartotecaCredit> CartotecaCreditAll { get; set;}
        public ObservableCollection<CartotecaDebit> CartotecaDebitAll { get; set; }
        public ObservableCollection<LookUpSpecific> TypeDocuments { get; set; }
        public ObservableCollection<CartotecaDebit> CurrentCartotecaDebit{get;set;}
        public ObservableCollection<CartotecaCredit> CurrentCartotecaCredit{get;set;}
        public ObservableCollection<LookUpSpecific> AllNationalAccounts { get; set; }
        protected override void Add()
        {
            Conto temp = CurrentConto.Clone();
            AllConto.Add(CurrentConto);
            CurrentConto = temp;
        }
    }
}
