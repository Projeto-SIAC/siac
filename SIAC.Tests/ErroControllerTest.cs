using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class ErroControllerTest
    {
        [TestMethod]
        public void TestIndexComCodigoZero()
        {
            var controller = new ErroController();           

            var result = controller.Index(0) as ViewResult;

            Assert.AreNotEqual("", result.ViewBag.Codigo);
        }
    }
}
