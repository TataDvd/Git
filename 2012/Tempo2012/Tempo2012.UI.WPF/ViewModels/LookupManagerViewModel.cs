using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using CoffeeLibrary;
using GlobalizedWizard;
using Tempo2012.EntityFramework.FakeData;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Views;
using System.Windows.Forms;
using System.Windows.Input;
using Tempo2012.UI.WPF.Dialogs;



namespace Tempo2012.UI.WPF.ViewModels
{
    public class LookupManagerViewModel:BaseViewModel
    {

        private int _CurrentRowIndex;
        public int CurrentRowIndex
        {
            get
            {
                return _CurrentRowIndex;
            }
            set 
            {
                if (_CurrentRowIndex == value) return;
                _CurrentRowIndex = value;
               
            }
        }
        
        public LookupManagerViewModel()
        {
            this._Lookups = new ObservableCollection<LookUpMetaData>(context.GetAllLookups());
            if (this._Lookups != null)
            {
                _Lookup = _Lookups[0];
                this.CalculateFields();
            }
            
        }

        private void CalculateFields()
        {
            this.Fields = new ObservableCollection<ObservableCollection<string>>();
            LookupModel lm = context.GetLookup(_Lookup.Id);
            var title = new ObservableCollection<string>();
            foreach (var field in lm.Fields)
            {
                if (field.Name == "Id") continue;
                title.Add(field.Name);
            }
            Fields.Add(title);
            var list = context.GetLookup(_Lookup.Tablename);
            foreach (var li in list)
            {
                var ader = new ObservableCollection<string>(li);
                Fields.Add(ader);
            }
            
        }



        public ObservableCollection<ObservableCollection<string>> Fields
        {
            get;
            set;
        }

        private ObservableCollection<LookUpMetaData> _Lookups;
        public ObservableCollection<LookUpMetaData> Lookups
        {
            get
            {
                return _Lookups;
            }
            set
            {
                _Lookups = value;
                OnPropertyChanged("Lookups");
            }
        }

        private LookUpMetaData _Lookup;
        public LookUpMetaData Lookup
        {
            get
            {
                return _Lookup;
            }
            set
            {
                _Lookup = value;
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
            }
        }
       
        protected override void Add()
        {
            editdialog(false);
           
        }
        protected override void Update()
        {
            editdialog(true);
            
        }

        private void editdialog(bool state)
        {
            var lookup=context.GetLookup(Lookup.Id);
            if ((this.CurrentRowIndex == -1) && state) return;
            List<FieldValuePair> current = new List<FieldValuePair>();
            for (var i = 0; i < this.Fields[this.CurrentRowIndex + 1].Count; i++)
            {
                if (state)
                {
                    current.Add(new FieldValuePair
                    {
                        Name = Fields[0][i],
                        Value = Fields[this.CurrentRowIndex + 1][i],
                        Length = lookup.Fields[i].Length,
                        ReadOnly = (lookup.Fields[i].NameEng == "Id") ? false : true
                    });
                }
                else
                {
                    current.Add(new FieldValuePair
                    {
                        Name = Fields[0][i],
                        Value = "",
                        Length = lookup.Fields[i].Length,
                        ReadOnly = (lookup.Fields[i].NameEng == "Id") ? false : true
                    });
                }
            }
            LookupsEdidViewModels vm = new LookupsEdidViewModels(current);
            EditInsertLookups ds = new EditInsertLookups(vm);
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {
                if (state)
                {
                    //update
                    context.UpdateRow(ds.GetNewFields(),lookup);
                }
                else
                { 
                    //nov red
                    context.SaveRow(ds.GetNewFields(),lookup);
                }
                CalculateFields();
                OnPropertyChanged("Fields");
                OnPropertyChanged("Lookup");
            }
        }
        protected override void Save()
        {
           base.Save();
        }
    }
}
