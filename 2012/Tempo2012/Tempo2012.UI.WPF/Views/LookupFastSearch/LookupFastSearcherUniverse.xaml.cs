using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tempo2012.EntityFramework;
using Tempo2012.EntityFramework.Models;
using Tempo2012.UI.WPF.CustomControls;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.FocusHelper;
using Tempo2012.UI.WPF.Models;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;
using Tempo2012.UI.WPF.ViewModels.ContragenManager;
using Tempo2012.UI.WPF.Views.ContragentManagment;


namespace Tempo2012.UI.WPF.Views.LookupFastSearch
{
    public delegate void ChangeElement(object sender, FastLookupEventArgs e);
    /// <summary>
    /// Interaction logic for LookupFastSearcher.xaml
    /// </summary>
    public partial class LookupFastSearcherUniverse : UserControl
    {


        public event ChangeElement ChangeElement;

        public static readonly DependencyProperty IsNotOpenNewProperty =
           DependencyProperty.RegisterAttached(
               "IsNotOpenNew",
               typeof(bool),
               typeof(LookupFastSearcherUniverse),
               new UIPropertyMetadata(false, OnIsNotOpenNewChanged));
        public bool IsNotOpenNew
        {
            get { return (bool)GetValue(IsNotOpenNewProperty); }
            set {
                SetValue(IsNotOpenNewProperty, value);
            }
        }

        private static void OnIsNotOpenNewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dc = (d as LookupFastSearcherUniverse).DataContext;//throw new NotImplementedException();
            (dc as SaldoItem).IsShowNew = Visibility.Hidden;
        }

        

        protected virtual void OnRefreshExecuted(FastLookupEventArgs e)
        {
            if (ChangeElement != null)
            {
                ChangeElement(this, e);
            }
        }
        public LookupFastSearcherUniverse()
        {
            InitializeComponent();
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (Equals(e.OriginalSource, this))
            {
                visior.Focus();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            visior.Focus();
        }

        private void dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            popi.IsOpen = false;
            SetCurrentItem();
        }

        private void dg_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetCurrentItem();
            }

        }

        private void SetCurrentItemNew()
        {
            visior.Visibility = Visibility.Visible;
            searcher.Visibility = Visibility.Hidden;
            var data1 = (DataContext as SaldoItem);
            var textBox = searcher;
            if (textBox != null)
                if (data1 != null)
                {
                    int count = 0;
                    int skip = 0;
                    List<Filter> list = new List<Filter>();
                    foreach (Filter filter in data1.GetFilters())
                        list.Add(filter);
                    if (list.Count == 0) return;
                    var tag = list.FirstOrDefault(w => w.FilterField == data1.SerachedField) ?? list[0];
                    ObservableCollection<ObservableCollection<string>> mainrez =
                        new ObservableCollection<ObservableCollection<string>>();
                    ObservableCollection<ObservableCollection<string>> rez =
                        new ObservableCollection<ObservableCollection<string>>();
                    if (!string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        mainrez = data1.GetDictionary(
                            string.Format("AND \"{0}\"='{1}'", tag.FilterField, textBox.Text),
                            string.Format(" order by \"{0}\"", tag.FilterField));

                        if (mainrez != null && mainrez.Count > 1)
                        {
                            data1.Value = mainrez[1][0];
                            data1.Lookupval = mainrez[1][1];
                            int h = 0;
                            if (int.TryParse(mainrez[1][0], out h))
                            {
                                data1.LiD = data1.GetLookUpId(0);
                            }
                            if (mainrez[1].Count>4)
                            {
                               
                                    data1.Bulstad =  mainrez[1][3];
                                    data1.Vat =  mainrez[1][4];
                                    OnRefreshExecuted(new FastLookupEventArgs(data1));
                            }
                        }
                        else
                        {
                            if (!IsNotOpenNew)
                            {
                                Calculate();
                            }
                            else
                            {
                                MessageBoxWrapper.Show("Няма номенклатура с търсения номер");
                            }
                        }
                    }
                   
                    //else
                    //{
                    //    rez = data1.GetDictionary("", "");
                    //}

                    //count = 21 - mainrez.Count;

                    //if (count > 0)
                    //{
                    //    foreach (var item in rez.Skip(skip).Take(count))
                    //    {
                    //        mainrez.Add(item);
                    //    }
                    //}
                    //dg.ItemsSource2D = mainrez;
                    //if (mainrez.Count > 1) dg.SelectedIndex = 0;
                    //popi.IsOpen = true;
                    //dg.Items.Refresh();
                    //e.Handled = true;
                }
        }

        private void SetCurrentItem()
        {
            popi.IsOpen = false;
            visior.Visibility = Visibility.Visible;
            searcher.Visibility = Visibility.Hidden;
            var data = dg.ItemsSource2D as ObservableCollection<ObservableCollection<string>>;
            var dat = DataContext as SaldoItem;
            if (data != null)
            {
                if (dg.SelectedIndex != -1)
                {
                    var dat1 = data[dg.SelectedIndex + 1];
                    if (dat != null)
                    {

                        dat.Value = dat1[0];
                        dat.Lookupval = dat1[1];
                        if (dat1.Count > 4)
                        {
                            dat.Bulstad = dat1[3];
                            dat.Vat = dat1[4];
                        }
                        dat.LiD = dat.GetLookUpId(0);
                    }
                    OnRefreshExecuted(new FastLookupEventArgs(dat));
                }
                else
                {
                    if (!IsNotOpenNew)
                    {
                        Calculate();
                    }
                    else
                    {
                        MessageBoxWrapper.Show("Няма номенклатура с търсения номер");
                    }
                }
            }
            visior.Focus();
        }


        private void searcher_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetCurrentItemNew();
                Skiplostfokus = true;
                visior.Focus();
                return;
            }
            if (e.Key == Key.F6)
            {
                popi.IsOpen = false;
                LookUpSelector contragenSelector = new LookUpSelector(DataContext as SaldoItem);
                contragenSelector.ShowDialog();
                var dat = DataContext as SaldoItem;
                if (contragenSelector.DialogResult.HasValue && contragenSelector.DialogResult.Value)
                {
                    var data = contragenSelector.dg.ItemsSource2D as ObservableCollection<ObservableCollection<string>>;
                    if (data != null)
                    {
                        if (contragenSelector.dg.SelectedIndex != -1)
                        {
                            var dat1 = data[contragenSelector.dg.SelectedIndex + 1];
                            if (dat != null)
                            {
                                dat.LiD = 0;
                                dat.Value = dat1[0];
                                dat.Lookupval = dat1[1];
                                if (dat1.Count > 4)
                                {
                                    dat.Bulstad = dat1[3];
                                    dat.Vat = dat1[4];
                                }
                                dat.LiD = dat.GetLookUpId(0);
                                OnRefreshExecuted(new FastLookupEventArgs(dat));
                            }
                        }
                    }

                }
                e.Handled = true;
                visior.Visibility = Visibility.Visible;
                searcher.Visibility = Visibility.Hidden;
            }
        }

        private void searcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Entrence.IsShowPopUp)
            {
               // e.Handled = true;
                return;
            }
            var data1 = (DataContext as SaldoItem);
            var textBox = sender as TextBox;
            if (textBox != null)
                if (data1 != null)
                {
                    int count = 0;
                    int skip = 0;
                    List<Filter> list = new List<Filter>();
                    foreach (Filter filter in data1.GetFilters())
                        list.Add(filter);
                    if (list.Count == 0) return;
                    var tag = list.FirstOrDefault(w => w.FilterField == data1.SerachedField) ?? list[0];
                    ObservableCollection<ObservableCollection<string>> mainrez =
                        new ObservableCollection<ObservableCollection<string>>();
                    ObservableCollection<ObservableCollection<string>> rez =
                        new ObservableCollection<ObservableCollection<string>>();
                    if (!string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        mainrez = data1.GetDictionary(
                            string.Format("AND \"{0}\"='{1}'", tag.FilterField, textBox.Text),
                            string.Format(" order by \"{0}\"", tag.FilterField));
                        skip = 1;
                        rez =
                            data1.GetDictionary(
                                string.Format("AND \"{0}\"<>'{1}' AND (UPPER (\"{0}\") like '%{2}%')", tag.FilterField,
                                    textBox.Text, textBox.Text.ToUpper()),
                                string.Format(" order by \"{0}\"", tag.FilterField));
                    }
                    else
                    {
                        rez = data1.GetDictionary("", "");
                    }

                    count = 21 - mainrez.Count;

                    if (count > 0)
                    {
                        foreach (var item in rez.Skip(skip).Take(count))
                        {
                            mainrez.Add(item);
                        }
                    }
                    dg.ItemsSource2D = mainrez;
                    if (mainrez.Count > 1) dg.SelectedIndex = 0;
                    popi.IsOpen = true;
                    dg.Items.Refresh();
                    e.Handled = true;
                }
        }

        private void visior_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F6)
            {
                popi.IsOpen = false;
                LookUpSelector contragenSelector = new LookUpSelector(DataContext as SaldoItem);
                contragenSelector.ShowDialog();
                if (contragenSelector.DialogResult.HasValue && contragenSelector.DialogResult.Value)
                {
                    var data = contragenSelector.dg.ItemsSource2D as ObservableCollection<ObservableCollection<string>>;
                    var dat = DataContext as SaldoItem;
                    if (data != null)
                    {
                        if (contragenSelector.dg.SelectedIndex != -1)
                        {
                            var dat1 = data[contragenSelector.dg.SelectedIndex + 1];
                            if (dat != null)
                            {

                                dat.Value = dat1[0];
                                dat.Lookupval = dat1[1];
                                if (dat1.Count > 4)
                                {
                                    dat.Bulstad = dat1[3];
                                    dat.Vat = dat1[4];
                                }
                                dat.LiD = dat.GetLookUpId(0);
                                OnRefreshExecuted(new FastLookupEventArgs(dat));
                            }
                        }
                    }
                }
                e.Handled = true;
                return;
            }
            
            
            if (e.Key == Key.Enter)
            {
                var sendy = sender as TextBoxEx;
                if (sendy != null && string.IsNullOrWhiteSpace(sendy.Text))
                {
                    var dat = DataContext as SaldoItem;
                    if (dat != null)
                    {
                        dat.Value = "";
                        dat.Lookupval = "";
                    }
                }
            }
            var _keyConv = new KeyConverter();
            string keyPressed = _keyConv.ConvertToString(e.Key);

            if (keyPressed != null && keyPressed.Length == 1 || keyPressed.Contains("NumPad"))
            {
                if (char.IsLetterOrDigit(keyPressed[0]))
                {
                    {
                        searcher.Visibility = Visibility.Visible;
                        visior.Visibility = Visibility.Hidden;
                        searcher.Text = "";
                        searcher.Focus();
                    }
                }
            }
        }

        private void Calculate()
        {
            var Context = new TempoDataBaseContext();
            var cp = this.DataContext as SaldoItem;
            if (cp != null && (cp.Relookup != 0 || cp.RCODELOOKUP != 0))
            {
                string search=Entrence.ConfigFirmaToLookup.GetField(ConfigTempoSinglenton.GetInstance().CurrentFirma.Id,
                    cp.Relookup);
                List<FieldValuePair> current = new List<FieldValuePair>();
                LookupModel lookup = null;
                if (cp.Relookup > 0)
                {
                    lookup = cp.SysLookup ? Context.GetSysLookup(cp.Relookup):Context.GetLookup(cp.Relookup);
                }
                else
                {
                    lookup = cp.SysLookup ? Context.GetSysLookup(cp.Relookup) : Context.GetLookup(cp.Relookup);
                }
                int i = 0;
                foreach (var item in lookup.Fields)
                {
                    if (cp.SysLookup && i == 0)
                    {
                        i++;
                        continue;
                    }
                    var nf = new FieldValuePair
                    {
                        Name = item.Name,
                        Length = item.Length,
                        Value = i==1&&string.IsNullOrWhiteSpace(search)?searcher.Text:"",
                        ReadOnly = item.NameEng != "Id",
                        IsRequared = item.IsRequared,
                        IsUnique = item.IsUnique,
                        RTABLENAME = item.RTABLENAME,
                        FieldName = item.NameEng
                    };
                    
                    if (item.NameEng == search)
                    {
                       nf.Value = searcher.Text;
                    }
                    current.Add(nf);
                }

                LookupsEdidViewModels vmm = new LookupsEdidViewModels(current,lookup.LookUpMetaData.Tablename,true);
                EditInsertLookups ds = new EditInsertLookups(vmm);
                ds.ShowDialog();
                if (ds.DialogResult.HasValue && ds.DialogResult.Value)
                {
                    //nov red
                    if (!cp.SysLookup)
                    {
                        lookup.Fields.Add(new TableField
                        {
                            DbField = "integer",
                            GROUP = 4,
                            Id = 4,
                            Length = 4,
                            IsRequared = false,
                            NameEng = "FIRMAID",
                            Name = "Фирма Номер"
                        });
                    }
                    bool sys=cp.SysLookup?!Context.SaveRow(ds.GetNewFields(), lookup):!Context.SaveRow(ds.GetNewFields(), lookup,ConfigTempoSinglenton.GetInstance().CurrentFirma.Id);
                    if (sys)
                    {
                        MessageBoxWrapper.Show(
                            "Получвава се дублиране на елемент от номенклатура! Номенклатурата не е записана!");
                    }
                    else
                    {
                        var dc = DataContext as SaldoItem;
                        var dat = ds.GetNewFields();
                        if (dat != null)
                        {
                            dc.Value = dat[1];
                            dc.Lookupval = dat[2];
                        }
                        if (dat != null && dat.Count > 5)
                        {
                            dc.Bulstad = dat[3];
                            dc.Vat = dat[4];
                        }
                        dc.LiD = 0;
                        OnRefreshExecuted(new FastLookupEventArgs(dc));
                    }
                }
                //cp.LookUp.Add(new SaldoItem { Value = ds.GetNewFields()[2], Key = ds.GetNewFields()[1] });
            }
        }


        private void searcher_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Entrence.IsShowPopUp)
            {
                // e.Handled = true;
                return;
            }
            
            if (e.Key==Key.Down)
            {
                Dg.SelectRowByIndex(dg,0);
            }
            if (e.Key == Key.Up)
            {
                Dg.SelectRowByIndex(dg, dg.Items.Count-1);
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var Context = new TempoDataBaseContext();
            var sen = (sender as Button);
            if (sen != null)
            {
                var cp = sen.CommandParameter as SaldoItem;
                if (cp != null && (cp.Relookup != 0 || cp.RCODELOOKUP != 0))
                {

                    List<FieldValuePair> current = new List<FieldValuePair>();
                    LookupModel lookup = null;
                    if (cp.Relookup > 0)
                    {
                        lookup = cp.SysLookup ? Context.GetSysLookup(cp.Relookup) : Context.GetLookup(cp.Relookup);

                    }
                    else
                    {
                        lookup = cp.SysLookup ? Context.GetSysLookup(cp.Relookup) : Context.GetLookup(cp.Relookup);
                    }
                    int i = 0;
                    foreach (var item in lookup.Fields)
                    {
                        if (cp.SysLookup && i == 0)
                        {
                            i++;
                            continue;
                        }
                        current.Add(new FieldValuePair
                        {
                            Name = item.Name,
                            Value = "",
                            Length = item.Length,
                            ReadOnly = (item.NameEng == "Id") ? false : true,
                            IsRequared = item.IsRequared,
                            IsUnique = item.IsUnique,
                            RTABLENAME = item.RTABLENAME,
                            FieldName = item.NameEng
                        });
                        
                    }

                    LookupsEdidViewModels vmm = new LookupsEdidViewModels(current, lookup.LookUpMetaData.Tablename,true);
                    EditInsertLookups ds = new EditInsertLookups(vmm);
                    ds.ShowDialog();
                    if (ds.DialogResult.HasValue && ds.DialogResult.Value)
                    {
                        //nov red
                        lookup.Fields.Add(new TableField { DbField = "integer", GROUP = 4, Id = 4, Length = 4, IsRequared = false, NameEng = "FIRMAID", Name = "Фирма Номер" });
                        if (!Context.SaveRow(ds.GetNewFields(), lookup, ConfigTempoSinglenton.GetInstance().CurrentFirma.Id))
                        {
                             MessageBoxWrapper.Show("Получвава се дублиране на елемент от номенклатура! Номенклатурата не е записана!");
                        }
                        else
                        {
                            var dc = DataContext as SaldoItem;
                            var dat = ds.GetNewFields();
                            if (dat != null)
                            {
                                if (dc != null)
                                {
                                    dc.Value = dat[1];
                                    dc.Lookupval = dat[2];
                                }
                            }
                            if (dat != null && dat.Count > 5)
                            {
                                if (dc != null)
                                {
                                    dc.Bulstad = dat[3];
                                    dc.Vat = dat[4];
                                }
                            }
                            OnRefreshExecuted(new FastLookupEventArgs(dc));
                        }
                    }
                        //cp.LookUp.Add(new SaldoItem { Value = ds.GetNewFields()[2], Key = ds.GetNewFields()[1] });
                    }


                }
            }

        private void Visior_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            return;
            //var data1 = (DataContext as SaldoItem);
            //var textBox = sender as TextBox;
            //if (textBox != null)
            //    if (data1 != null)
            //    {
            //        int count = 0;
            //        int skip = 0;
            //        List<Filter> list = new List<Filter>();
            //        foreach (Filter filter in data1.GetFilters())
            //            list.Add(filter);
            //        if (list.Count == 0) return;
            //        var tag =  list[0];
            //        ObservableCollection<ObservableCollection<string>> mainrez =
            //            new ObservableCollection<ObservableCollection<string>>();
            //        ObservableCollection<ObservableCollection<string>> rez =
            //            new ObservableCollection<ObservableCollection<string>>();
            //        if (!string.IsNullOrWhiteSpace(textBox.Text))
            //        {
            //            mainrez = data1.GetDictionary(
            //                string.Format("AND \"{0}\"='{1}'", tag.FilterField, textBox.Text),
            //                string.Format(" order by \"{0}\"", tag.FilterField));
                       
            //        }

                    
            //        if (mainrez != null && mainrez.Count>1)
            //        {
            //            data1.Value = mainrez[1][0];
            //            data1.Lookupval = mainrez[1][1];
            //            int h = 0;
            //            if (int.TryParse(mainrez[1][0],out h))
            //            {
            //                data1.LiD = h;
            //            }
            //        }


            //        e.Handled = true;
            //    }
        }

        private void Visior_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //SetCurrentItemNew();
        }

        private void Searcher_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!Skiplostfokus)
            {
                SetCurrentItemNew();
            }
            Skiplostfokus = false;
        }

        private bool Skiplostfokus;

        private void visior_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var _keyConv = new KeyConverter();
            string keyPressed = _keyConv.ConvertToString(e.Key);
            if (keyPressed.Contains("Oem"))
            {
                e.Handled=true;
            }
        }
    }
    }

