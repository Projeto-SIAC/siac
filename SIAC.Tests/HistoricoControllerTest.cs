using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class HistoricoControllerTest
    {
        HistoricoController controller = new HistoricoController();

        [TestMethod]
        public void TestIndexModel()
        {
            string esperado = "20150002";
            MockHelper.FakeLoginUsuario(esperado, "/historico");
            Usuario resultado = (controller.Index() as ViewResult).Model as Usuario;
            Assert.AreEqual(esperado, resultado.Matricula);
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
