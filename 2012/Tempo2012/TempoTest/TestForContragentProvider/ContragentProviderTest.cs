using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using dragonz.actb.provider;

namespace TempoTest.TestForContragentProvider
{
    [TestFixture]
    public class ContragentProviderTest
    {
        [Test]
        public void TestFoBulstad()
        {
            List<string> list = new List<string> { "1|#bg211122|Темпо", "2|#bg33333|Мемпо", "3|#bg454433|Гемпо", "4|#bg4433222|Клин" };
            ContragentProvider cp=new ContragentProvider(list);
            var res =new List<string>(cp.GetItems("1"));
            Assert.AreEqual(res[0],"1 bg211122 Темпо");
            res = new List<string>(cp.GetItems("#bg33"));
            Assert.AreEqual(res[0], "2 bg33333 Мемпо");
            res = new List<string>(cp.GetItems("Мем"));
            Assert.AreEqual(res[0], "2 bg33333 Мемпо");
            res = new List<string>(cp.GetItems("Гем"));
            Assert.AreEqual(res[0], "3 bg454433 Гемпо");
        }
    }
}
