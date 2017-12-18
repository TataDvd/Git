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



namespace Tempo2012.UI.WPF.ViewModels
{
    public class NomenclatureViewModel:BaseViewModel
    {

        public NomenclatureViewModel()
        {
            this._Nomenclatures =new ObservableCollection<NomenclatureHedar>(FakeDataContext.GetAllNomenclatures());
            this._NomenclatureFields = new ObservableCollection<NomeclatureMetaData>(FakeDataContext.GetNomenlatureFields(1));
            this._Fields=new ObservableCollection<ObservableCollection<string>>();
            var list = FakeDataContext.GetNomenclatureContent(1);
            foreach (var li in list)
            {
                var ader = new ObservableCollection<string>(li);
                _Fields.Add(ader);
            }
        }

        private ObservableCollection<ObservableCollection<string>> _Fields;
        public ObservableCollection<ObservableCollection<string>> Fields
        {
            get { return _Fields;}
            set { _Fields = value; OnPropertyChanged("Fields");}
        }

        private NomenclatureHedar _CurrentNomenclatures;
        public NomenclatureHedar CurrentNomenclatures
        {
            get { return _CurrentNomenclatures; }
            set
            {
                _CurrentNomenclatures = value;
                this._NomenclatureFields = new ObservableCollection<NomeclatureMetaData>(FakeDataContext.GetNomenlatureFields(value.Id));
                var list = FakeDataContext.GetNomenclatureContent(value.Id);
                _Fields=new ObservableCollection<ObservableCollection<string>>();
                foreach (var li in list)
                {
                  var ader = new ObservableCollection<string>(li);
                 _Fields.Add(ader);
                }
                OnPropertyChanged("CurrentNomenclatures");
                OnPropertyChanged("NomenclatureFields");
                OnPropertyChanged("Fields");
            }
        }
        private ObservableCollection<NomenclatureHedar> _Nomenclatures;
        public ObservableCollection<NomenclatureHedar> Nomenclatures
        {
            get
            {
                return _Nomenclatures;
            }
            set
            {
                _Nomenclatures = value;
                OnPropertyChanged("Nomenclatures");
            }
        }
        private ObservableCollection<NomeclatureMetaData> _NomenclatureFields;
        public ObservableCollection<NomeclatureMetaData> NomenclatureFields
        {
            get
            {
                return _NomenclatureFields;
            }
            set
            {
                _NomenclatureFields = value;
                OnPropertyChanged("NomenclatureFields");
            }
        }

        protected override void Add()
        {
            //context.CreateTable(new List<TableField>
            //                        {
            //                            new TableField {DbField = "integer", Name = "test", IsNull = true},
            //                            new TableField {DbField = "integer", Name = "test2",IsNull = false}
            //                        },"nomen15");
            //NomenWizardDialog dlg = new NomenWizardDialog();
            //if (dlg.ShowDialog() == true)
            //{
            //    var test = dlg.Result;
            //    var rez = Nomenclatures.Max(e => e.Id);
            //    List<Conector> newconections=new List<Conector>();
            //    foreach (var item in test.SelectedItems)
            //    {
            //        Conector nc=new Conector{ChildId = item.Id,ParentId = rez+1};
            //        newconections.Add(nc);
            //    }
            //    FakeDataContext.SaveConectors(newconections);
            //    FakeDataContext.SaveNomenclatureHeader(new NomenclatureHedar { Description = test.Description, Id = rez + 1, Name = test.Name });
            //    Nomenclatures = new ObservableCollection<NomenclatureHedar>(FakeDataContext.GetAllNomenclatures());
            //    NomenclatureFields = new ObservableCollection<NomeclatureMetaData>(FakeDataContext.GetNomenlatureFields(rez + 1));
            //    CurrentNomenclatures = Nomenclatures.Where(e => e.Id == rez + 1).FirstOrDefault();
            //}
            //else
            //{
            //    MessageBox.Show("Отказано добавяне на номенклатури");
            //}
        }
    }
}
