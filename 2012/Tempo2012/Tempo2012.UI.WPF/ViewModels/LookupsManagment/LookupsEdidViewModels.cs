using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.Models;

namespace Tempo2012.UI.WPF.ViewModels
{
    public class LookupsEdidViewModels : BaseViewModel
    {
        public LookupsEdidViewModels(IEnumerable<FieldValuePair> fields,string tableName,bool insert)
        {
            _Fields = new ObservableCollection<FieldValuePair>(fields);
            string bulsi="";
            if (!insert) return;
            foreach (FieldValuePair fieldValuePair in _Fields)
            {
                if (fieldValuePair.Name == "БУЛСТАТ" || fieldValuePair.Name == "ЗДДС номер")
                {
                    if (!string.IsNullOrWhiteSpace(fieldValuePair.Value))
                    {
                        bulsi = fieldValuePair.Value;
                    }
                }
                if (!string.IsNullOrWhiteSpace(fieldValuePair.RTABLENAME))
                {
                    if (fieldValuePair.RTABLENAME == "AUTO")
                    {
                        fieldValuePair.Value = Context.SelectMax(tableName, fieldValuePair.FieldName);
                        fieldValuePair.ReadOnly = false;
                    }
                    else
                    {
                        fieldValuePair.IsLookUp = true;
                        fieldValuePair.LookUp = new ObservableCollection<SaldoItem>();
                        var list = Context.GetSysLookup(fieldValuePair.RTABLENAME);
                        int k = 0;
                        foreach (List<string> enumerable in list)
                        {
                            int i = 0;
                            SaldoItem saldoitem = new SaldoItem();
                            saldoitem.Value = enumerable[2];
                            saldoitem.Key = enumerable[1];



                            fieldValuePair.LookUp.Add(saldoitem);
                        }
                        fieldValuePair.SelectedLookupItem =
                            fieldValuePair.LookUp.FirstOrDefault(e => e.Key == fieldValuePair.Value);
                    }

                }
                if (!string.IsNullOrWhiteSpace(fieldValuePair.Tn))
                {
                    fieldValuePair.IsLookUp = true;
                    fieldValuePair.LookUp = new ObservableCollection<SaldoItem>();
                    var list = Context.GetSysLookup(fieldValuePair.Tn);
                    int k = 0;
                    foreach (List<string> enumerable in list)
                    {
                        int i = 0;
                        SaldoItem saldoitem = new SaldoItem();
                        saldoitem.Value = enumerable[2];
                        saldoitem.Key = enumerable[1];



                        fieldValuePair.LookUp.Add(saldoitem);
                    }
                    fieldValuePair.SelectedLookupItem =
                        fieldValuePair.LookUp.FirstOrDefault(e => e.Key == fieldValuePair.Value);
                }
            }
            foreach (FieldValuePair fieldValuePair in _Fields)
            {
                if (fieldValuePair.Name == "БУЛСТАТ" || fieldValuePair.Name == "ЗДДС номер")
                {
                    fieldValuePair.Value = bulsi;
                }
            }
        }

        public
            LookupsEdidViewModels()
            {
                _Fields = new ObservableCollection<FieldValuePair>();
                _Fields.Add(new FieldValuePair {Name = "Test", Type = "DB", Value = "1"});
                _Fields.Add(new FieldValuePair {Name = "Test1", Type = "DB1", Value = "Ибре"});
                _Fields.Add(new FieldValuePair {Name = "Test2", Type = "DB2", Value = "Оро"});
            }
        private
            ObservableCollection < FieldValuePair > _Fields
            {
                get;
                set;
            }
        public
            ObservableCollection < FieldValuePair > Fields
            {
                get
                {
                    return _Fields;
                }
                set
                {
                    _Fields = value;
                    OnPropertyChanged("Fields");
                }
            }


        
    }
    }
