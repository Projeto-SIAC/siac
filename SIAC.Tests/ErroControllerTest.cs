using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class ErroControllerTest
    {
        ErroController controller = new ErroController();

        [TestMethod]
        public void TestIndexComCodigoZero()
        {
            ViewResult resultado = controller.Index(0) as ViewResult;
            Assert.AreNotEqual(String.Empty, resultado.ViewBag.Codigo);
        }
    }
}
