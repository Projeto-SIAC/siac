using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SIAC.Tests
{
    [TestClass]
    public class CriptografiaTest
    {
        private string valor = "foo";
        private string esperadoBase64 = "Zm9v";

        [TestMethod]
        public void TestBase64Encode()
        {
            var resultado = Helpers.Criptografia.Base64Encode(valor);
            Assert.AreEqual(esperadoBase64, resultado);
        }

        [TestMethod]
        public void TestBase64Decode()
        {
            var resultado = Helpers.Criptografia.Base64Decode(esperadoBase64);
            Assert.AreEqual(valor, resultado);
        }
    }
}