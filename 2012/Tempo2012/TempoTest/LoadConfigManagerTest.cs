using ReportBuilder;
using Tempo2012.EntityFramework;
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
    public class LoadConfigManagerTest
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            var loadConfigManager =
                new LoadConfigManager(@"E:\Samples\TempoVS2012\Tempo2012\Tempo2012.UI.WPF\bin\Debug\tempo.ini");
            Assert.AreEqual("ON",loadConfigManager.GetPrameter("OBEKT"));
            Assert.AreEqual(@"D:\TEMPO\WORDPAD.EXE",loadConfigManager.GetPrameter("WORDPADNAME"));
        }
    }
}
