using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace SIAC.Tests
{
    [TestClass]
    public class FormateTest
    {
        [TestMethod]
        public void TestParaCPF()
        {
            var regex = new Regex(@"[0-9]{3}\.[0-9]{3}\.[0-9]{3}\-[0-9]{2}");
            var resultado = Helpers.Formate.ParaCPF("12345678900");
            Assert.IsTrue(regex.IsMatch(resultado));
        }

        [TestMethod]
        public void TestDeCPF()
        {
            var cpf = "123.456.789-00";
            var esperado = "12345678900";
            var resultado = Helpers.Formate.DeCPF(cpf);
            Assert.AreEqual(esperado, resultado);
        }
    }
}