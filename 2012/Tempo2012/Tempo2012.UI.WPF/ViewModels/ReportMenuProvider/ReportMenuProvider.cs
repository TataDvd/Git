using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tempo2012.EntityFramework;
using Tempo2012.UI.WPF.Dialogs;
using Tempo2012.UI.WPF.Views.Dialogs;

namespace Tempo2012.UI.WPF.ViewModels.ReportMenuProvider
{
    public class ReportMenuProviderViewModel:BaseViewModel
    {
        public int FromDay { get; set; }
        
        public int FromMonth { get; set; }

        public int CurrentYear { get; set; }

        public int ToMonth { get; set; }

        public ReportMenuProviderViewModel(int currentYear,int currentMonth)
        {
            FromMonth = currentMonth;
            ToMonth = currentMonth;
            CurrentYear = currentYear;
        }
       

        protected override void Add()
        {
            ToMonth = FromMonth;
            FromDay = 1;
            ToDay = GetEndDayMonth(ToMonth);
        }
        protected override void AddNew()
        {
            FromMonth = GetMonthFromForm(FromMonth,"Въведи произволен месец за справката и натисни Enter");
            ToMonth = FromMonth;
            FromDay = 1;
            ToDay = GetEndDayMonth(ToMonth);
        }

        
        protected override void Update()
        {
            FromMonth = GetMonthFromForm(FromMonth,"Въведи начален месец и натисни Enter");
            ToMonth = GetMonthFromForm(FromMonth+1,"Въведи краен месец и натисни Enter");
            FromDay = 1;
            ToDay = GetEndDayMonth(ToMonth);
        }
        protected override void Delete()
        {
           
            DateTime fromdate = GetDateFromForm("Избери начална дата");
            FromMonth = fromdate.Month;
            FromDay = fromdate.Day;
            DateTime todate = GetDateFromForm("Избери крайна дата");
            ToMonth = todate.Month;
            ToDay = todate.Day;
        }

        protected virtual DateTime GetDateFromForm(string title)
        {
            DateTime result=ConfigTempoSinglenton.GetInstance().WorkDate;
            DataSelector ds = new DataSelector(result,title,false);
            ds.ShowDialog();
            if (ds.DialogResult.HasValue && ds.DialogResult.Value)
            {

                result = ds.SelectedDate;
                //confi.DataContext = vm.ConfigParams;
            }
            return result;
        }

        protected virtual int GetMonthFromForm(int def,string title)
        {
            int result=1;
            MonthSelector ms=new MonthSelector(def.ToString(),title);
            ms.ShowDialog();
            if (ms.DialogResult.HasValue && ms.DialogResult.Value)
            {

                result = ms.Month;
                //confi.DataContext = vm.ConfigParams;
            }
            return result;
        }

        protected override void View()
        {
            FromDay = 1;
            FromMonth = 1;
            ToDay = 31;
            ToMonth = 12;
        }

        public DateTime FromDate()
        {
            if (FromDay == 0) FromDay = 1;
           return new DateTime(CurrentYear, FromMonth, FromDay);
        }

        public DateTime ToDate()
        {
           if (ToDay!=0)
           {
               return new DateTime(CurrentYear,ToMonth,ToDay);
           }
            return new DateTime(CurrentYear, ToMonth, GetEndDayMonth(ToMonth));
        }

        public int ToDay { get; set;}

        private int GetEndDayMonth(int toMonth)
        {
            int rez = 30;
            switch (toMonth)
            {
                case 1:case 3:case 5:case 7:case 8:case 10:case 12:
                    {
                        rez = 31;
                    }
                    break;
                case 2:
                    rez=IsYearBig(CurrentYear)? 29:28;
                    break;
            }
            return rez;
        }

        protected virtual bool IsYearBig(int currentYear)
        {
            return currentYear%4 == 0;
        }

        
    }
}
