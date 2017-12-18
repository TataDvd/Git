using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tempo2012.EntityFramework.Models;

namespace TempoTest.Users_Test
{
    [TestFixture]
    public class UsersTest
    {
        [Test]
        public void CanUpdateContoTest()
        {
            User user=new User();
            user.CanUpdateConto = true;

            Assert.AreEqual(user.CanUpdateConto,true);
            Assert.AreEqual(user.Rights, ConstantsBinary.UpdateConto);
        }

        [Test]
        public void CanUpdateContoSetByRights()
        {
            User user = new User();
            user.Rights = 8;

            Assert.AreEqual(user.CanUpdateConto, true);
            
        }
        [Test]
        public void CanUpdateContoSetAll()
        {
            User user = new User();
            user.Rights = ConstantsBinary.All;
            Assert.AreEqual(user.CanUpdateConto, true);
            Assert.AreEqual(user.CanUpdateSaldo, true);
            Assert.AreEqual(user.CanUpdateAcc, true);
            Assert.AreEqual(user.CanStoreReports, true);
            Assert.AreEqual(user.CanReportPeriodi, true);
            Assert.AreEqual(user.CanOborotReport, true);
            Assert.AreEqual(user.CanNewCurrency, true);


        }
    }
}
