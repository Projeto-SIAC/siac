using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using SIAC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIAC.Tests
{
    [TestClass]
    public class AgendaControllerTest : BaseControllerTest
    {
        private AgendaController controller;

        [TestInitialize]
        public override void SetupTest()
        {
            base.SetupTest();
            this.controller = new AgendaController();
            var context = new ControllerContext(this.moqHttpContext.Object, new RouteData(), this.controller);
            this.controller.ControllerContext = context;
        }

        [TestMethod]
        public void TestIndexView()
        {
            ViewResult resultado = controller.Index() as ViewResult;
            Assert.AreEqual("Index", resultado.ViewName);
        }

        [TestMethod]
        public void TestAgendaTodosJson()
        {
            JsonResult esperado = new JsonResult();
            List<object> resultados = new List<object>();

            this.Login("superusuario");

            resultados.Add(controller.Academicas("2015-02-02T08:55", "2015-02-02T08:55"));
            resultados.Add(controller.Certificacoes("2015-02-02T08:55", "2015-02-02T08:55"));
            resultados.Add(controller.Reposicoes("2015-02-02T08:55", "2015-02-02T08:55"));
            resultados.Add(controller.Horarios("2015-02-02T08:55", "2015-02-02T08:55"));
            resultados.Add(controller.Conflitos("2015-02-02T08:55", "2015-02-02T08:55"));

            foreach (var resultado in resultados)
            {
                Assert.AreEqual(esperado.GetType(), resultado.GetType());
            }
        }
    }
}
