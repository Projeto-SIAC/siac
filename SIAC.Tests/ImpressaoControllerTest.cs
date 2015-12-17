using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class ImpressaoControllerTest
    {
        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            var controller = new ImpressaoController();

            var result = controller.Index() as RedirectToRouteResult;

            Assert.AreEqual("Principal", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestInstitucionalComCodigoNulo()
        {
            var controller = new ImpressaoController();

            var result = controller.Institucional(null) as RedirectToRouteResult;

            Assert.AreEqual("Principal", result.RouteValues["controller"]);
        }


        [TestMethod]
        public void TestAvaliacaoComCodigoNulo()
        {
            var controller = new ImpressaoController();

            var result = controller.Avaliacao(null) as RedirectToRouteResult;

            Assert.AreEqual("Principal", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestInstitucionalComCodigoVazio()
        {
            var controller = new ImpressaoController();

            var result = controller.Institucional("") as RedirectToRouteResult;

            Assert.AreEqual("Principal", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestAvaliacaoComCodigoVazio()
        {
            var controller = new ImpressaoController();

            var result = controller.Avaliacao("") as RedirectToRouteResult;

            Assert.AreEqual("Principal", result.RouteValues["controller"]);
        }
    }
}
