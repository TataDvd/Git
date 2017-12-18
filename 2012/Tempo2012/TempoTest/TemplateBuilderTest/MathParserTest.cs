using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemplateGenerator;


namespace TempoTest.TemplateBuilderTest
{
    [TestClass]
    public class MathParserTest
    {
        [TestMethod]
        public void MathEvaluator_Evaluate()
        {
            var test = MathEvaluator.Evaluate("10+10*(45+145)-20");
            Assert.AreEqual(1890.0, test, "Грешно изчисление");
        }
        [TestMethod]
        public void MathEvaluator_Evaluate2()
        {
            var test = MathEvaluator.Evaluate("10*(9-4)/50");
            Assert.AreEqual(1, test, "Грешно изчисление");
        }
        [TestMethod]
        public void TestBuilderTest_Evaluate()
        {
            var testbuilder = new TemplateBuilder();
            testbuilder.LoadTemplate("E:\\TempoData\\Templates\\testblanka.txt");
            var t = testbuilder.ResultTemplate;
        }
    }
}
