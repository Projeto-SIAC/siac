using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace SIAC.Tests
{
    [TestClass]
    public class CorDinamicaTest
    {
        [TestMethod]
        public void TestRgbaRetorno()
        {
            var regex = new Regex(@"rgba\(\d{1,3},\d{1,3},\d{1,3},\d+\)");
            var resultado = Helpers.CorDinamica.Rgba();
            Assert.IsInstanceOfType(resultado, typeof(string));
            Assert.IsTrue(regex.IsMatch(resultado));
        }
    }
}