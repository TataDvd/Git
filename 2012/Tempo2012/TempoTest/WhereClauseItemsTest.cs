using ReportBuilder;
using Tempo2012.UI.WPF.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TempoTest
{
    
    
    /// <summary>
    ///This is a test class for WhereClauseItemsTest and is intended
    ///to contain all WhereClauseItemsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WhereClauseItemsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for WhereClauseItems Constructor
        ///</summary>
        [TestMethod()]
        public void WhereClauseItemsConstructorTest()
        {
            WhereClauseItems target = new WhereClauseItems();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for OnPropertyChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Tempo2012.UI.WPF.exe")]
        public void OnPropertyChangedTest()
        {
            WhereClauseItems_Accessor target = new WhereClauseItems_Accessor(); // TODO: Initialize to an appropriate value
            string info = string.Empty; // TODO: Initialize to an appropriate value
            target.OnPropertyChanged(info);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            WhereClauseItems target = new WhereClauseItems(); // TODO: Initialize to an appropriate value
            target.View = "Accounts";
            target.Items=new List<WhereClauseItem>
                             {
                                 new WhereClauseItem{DbField = "Id",GlobalOperator = "and", Value = "20",SqlOperator = ">"},
                                 new WhereClauseItem{DbField = "Name",GlobalOperator = "and", Value = "20",SqlOperator = "="},
                                 new WhereClauseItem{DbField = "Description",GlobalOperator = "or", Value = "20",SqlOperator = "<>"}
                             };
            target.ShowItems = new List<ReportItem> { 
                                                        new ReportItem{DbField = "Id", IsVisible = true, Name = "Id", Size = 20},
                                                        new ReportItem { DbField = "Name", IsVisible = true, Name = "Name", Size = 20 },
                                                          new ReportItem { DbField = "First", IsVisible = true, Name = "First", Size = 20 },
                                                          new ReportItem { DbField = "Family", IsVisible = true, Name = "Family", Size = 20 }};
            string expected = "Select  Id , Name , First , Family  from  Accounts  where   Id>20 and Name=20 or Description<>20"; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Items
        ///</summary>
        [TestMethod()]
        public void ItemsTest()
        {
            WhereClauseItems target = new WhereClauseItems(); // TODO: Initialize to an appropriate value
            IEnumerable<WhereClauseItem> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<WhereClauseItem> actual;
            target.Items = expected;
            actual = target.Items;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
