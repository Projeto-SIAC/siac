using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using SIAC.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC.Tests
{
    [TestClass]
    public class PrincipalControllerTest : BaseControllerTest
    {
        private PrincipalController controller;

        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            this.controller = new PrincipalController();
            var context = new ControllerContext(this.moqHttpContext.Object, new RouteData(), this.controller);
            this.controller.ControllerContext = context;
        }

        [TestMethod]
        public void TestAvaliacaoRedirect()
        {
            string esperado = "Index";
            RedirectToRouteResult resultado = controller.Avaliacao() as RedirectToRouteResult;
            Assert.AreEqual(esperado, resultado.RouteValues["Action"]);
        }

    }
}
