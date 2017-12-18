using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tempo2012.UI.WPF.ViewModels;
using Tempo2012.UI.WPF.ViewModels.ContoManagment;

namespace TempoTest.Test_References.TestForViewModels
{
    [TestFixture] 
    public class ContoViewModelTests
    {
        private ContoViewModel _testClass;
        #region Setup/Teardown
        [SetUp]
        public void SetUp()
        {
            _testClass = new ContoViewModel();
            _testClass.Context = new FakeDataContect();
        }

        [TearDown]
        public void TearDown()
        {
            _testClass.DoFacturaInfoCredit();
        }

        #endregion

    }
}
