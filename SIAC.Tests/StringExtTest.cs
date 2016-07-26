using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Helpers;

namespace SIAC.Tests
{
    [TestClass]
    public class StringExtTest
    {
        private string[] a = new string[] { "foo", "bar", "" };
        private string[] b = new string[] { "foo", "bar", "baz" };
        private string[] c = new string[] { "foo", "bar", " " };

        [TestMethod]
        public void TestIsNullOrEmptyComportamento()
        {
            Assert.IsTrue(StringExt.IsNullOrEmpty(a));
            Assert.IsFalse(StringExt.IsNullOrEmpty(b));
            Assert.IsFalse(StringExt.IsNullOrEmpty(c));
        }

        [TestMethod]
        public void IsNullOrWhiteSpaceComportamento()
        {
            Assert.IsTrue(StringExt.IsNullOrWhiteSpace(a));
            Assert.IsFalse(StringExt.IsNullOrWhiteSpace(b));
            Assert.IsTrue(StringExt.IsNullOrWhiteSpace(c));
        }
    }
}