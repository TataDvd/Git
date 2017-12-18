using System.Collections.ObjectModel;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.ViewModels;
using System.Linq;

namespace Tempo2012.UI.WPF.Views.TetkaView
{
    public class FacturaControlInfoViewModel:BaseViewModel
    {
        public FacturaControlInfoViewModel(AccountsModel DAccountsModel, string nomerf, string contr)
        { 
            Title = string.Format("Справка движение по фактура {0} и контрагент {1} по сметка {2}", nomerf, contr,
                   DAccountsModel.ShortName);
            AllMovement = new ObservableCollection<AccItemSaldo>(Context.GetInfoFactura(DAccountsModel.Id,DAccountsModel.TypeAccountEx, nomerf, contr));
            AccItemSaldo accItemSaldo=new AccItemSaldo();
            var rezi = Context.GetAllAnaliticSaldos(DAccountsModel.Id, DAccountsModel.FirmaId);
            var saldo = rezi.FirstOrDefault(e => e.NameContragent.Trim() == contr.Trim() && e.NumInvoise.Trim() == nomerf.Trim());
            if (saldo!=null)
            {
                accItemSaldo.Nsd = saldo.BeginSaldoDebit;
                accItemSaldo.Nsc = saldo.BeginSaldoCredit;
            }
            
            accItemSaldo.Type = DAccountsModel.TypeAccountEx;
            accItemSaldo.Od = AllMovement.Sum(e => e.Od);
            accItemSaldo.Oc = AllMovement.Sum(e => e.Oc);
            Ns = string.Format("Начално дебитно салдо {0} Начално кредитно салдо {1}", accItemSaldo.Nsd,accItemSaldo.Nsc);
            Ks = string.Format("Краино дебитно салдо {0} Крайно кредитно салдо {1}", accItemSaldo.Ksd,accItemSaldo.Ksc);
            

        }
        public FacturaControlInfoViewModel(AccountsModel DAccountsModel,  string contr)
        {
            Title = string.Format("Справка движение фактури контрагент {0} по сметка {1}",  contr,
                   DAccountsModel.ShortName);
            AllMovement = new ObservableCollection<AccItemSaldo>(Context.GetInfoFactura(DAccountsModel.Id, DAccountsModel.TypeAccountEx,contr));
            AccItemSaldo accItemSaldo = new AccItemSaldo();
            var rezi = Context.GetAllAnaliticSaldos(DAccountsModel.Id, DAccountsModel.FirmaId);
            var saldo = rezi.FirstOrDefault(e => e.NameContragent.Trim() == contr.Trim() );
            if (saldo != null)
            {
                accItemSaldo.Nsd = saldo.BeginSaldoDebit;
                accItemSaldo.Nsc = saldo.BeginSaldoCredit;
            }

            accItemSaldo.Type = DAccountsModel.TypeAccountEx;
            accItemSaldo.Od = AllMovement.Sum(e => e.Od);
            accItemSaldo.Oc = AllMovement.Sum(e => e.Oc);
            Ns = string.Format("Начално дебитно салдо {0} Начално кредитно салдо {1}", accItemSaldo.Nsd, accItemSaldo.Nsc);
            Ks = string.Format("Краино дебитно салдо {0} Крайно дебитно салдо   {1}", accItemSaldo.Ksd, accItemSaldo.Ksc);


        }
        public ObservableCollection<AccItemSaldo> AllMovement { get; set; }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title");}
        }

        private string _ns;
        public string Ns
        {
            get { return _ns; }
            set { _ns = value;OnPropertyChanged("Ns"); }
        }

        private string _кs;
        public string Ks
        {
            get { return _кs; }
            set { _кs = value;OnPropertyChanged("Ks"); }
        }
    }
}