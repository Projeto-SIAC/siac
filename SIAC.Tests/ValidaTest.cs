using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Helpers;

namespace SIAC.Tests
{
    [TestClass]
    public class ValidaTest
    {
        private string cpf = "38756554516";
        private string email = "example@foobar.com";

        [TestMethod]
        public void TestCPFComportamento()
        {
            Assert.IsTrue(Valida.CPF(cpf));
            Assert.IsFalse(Valida.CPF("11122233344"));
        }

        [TestMethod]
        public void TestEmailComportamento()
        {
            Assert.IsTrue(Valida.Email(email));
            Assert.IsFalse(Valida.Email("foobar.com"));
            Assert.IsFalse(Valida.Email("@foobar.com"));
            Assert.IsFalse(Valida.Email("example@.com"));
            Assert.IsFalse(Valida.Email("example@com"));
        }
    }
}