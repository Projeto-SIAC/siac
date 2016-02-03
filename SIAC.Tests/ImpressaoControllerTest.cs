using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class ImpressaoControllerTest
    {
        ImpressaoController controller = new ImpressaoController();

        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            RedirectToRouteResult resultado = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestInstitucionalComCodigoNulo()
        {
            RedirectToRouteResult resultado = controller.Institucional(null) as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }


        [TestMethod]
        public void TestAvaliacaoComCodigoNulo()
        {
            RedirectToRouteResult resultado = controller.Avaliacao(null) as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestInstitucionalComCodigoVazio()
        {
            RedirectToRouteResult resultado = controller.Institucional("") as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestAvaliacaoComCodigoVazio()
        {
            RedirectToRouteResult resultado = controller.Avaliacao("") as RedirectToRouteResult;
            Assert.AreEqual("Principal", resultado.RouteValues["controller"]);
        }
    }
}
