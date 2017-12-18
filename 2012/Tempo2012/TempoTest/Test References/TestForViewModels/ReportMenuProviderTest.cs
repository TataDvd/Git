using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tempo2012.UI.WPF.ViewModels.ReportMenuProvider;

namespace TempoTest.Test_References.TestForViewModels
{
    [TestFixture]
    public class ReportMenuProviderTest
    {
        private ReportMenuProviderViewModel _testClass;
        #region Setup/Teardown
        [SetUp]
        public void SetUp()
        {
            _testClass=new ReportMenuProviderViewModel(2014,6);
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void ReturnCurrentMonthTest()
        {
            _testClass.AddCommand.Execute(null);

            DateTime date=_testClass.FromDate();

            Assert.AreEqual(date.Month, _testClass.FromMonth);
            Assert.AreEqual(date.Year, _testClass.CurrentYear);
        }

        [Test]
        public void ReturnVariableMonthTest()
        {
            _testClass.AddNewCommand.Execute(null);

            DateTime date = _testClass.FromDate();

            Assert.AreEqual(date.Month, _testClass.FromMonth);
            Assert.AreEqual(date.Year, _testClass.CurrentYear);
            
        }

        [Test]
        public void ReturnFromToMonthTest()
        {
            _testClass.UpdateCommand.Execute(null);

            DateTime date = _testClass.FromDate();

            Assert.AreEqual(date.Month, _testClass.FromMonth);
            Assert.AreEqual(date.Year, _testClass.CurrentYear);

            DateTime todate = _testClass.ToDate();

            Assert.AreEqual(todate.Month, _testClass.ToMonth);
            Assert.AreEqual(todate.Year, _testClass.CurrentYear);
        }

        [Test]
        public void ReturnAllYearTest()
        {
            _testClass.ViewCommand.Execute(null);

            DateTime date = _testClass.FromDate();

            Assert.AreEqual(date.Month, 1);
            Assert.AreEqual(date.Day, 1);

            DateTime todate = _testClass.ToDate();

            Assert.AreEqual(todate.Month, 12);
            Assert.AreEqual(todate.Day, 31);
        }
        #endregion
    }
   
}
