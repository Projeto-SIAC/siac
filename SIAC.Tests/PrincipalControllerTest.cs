using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using SIAC.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class PrincipalControllerTest
    {
        PrincipalController controller = new PrincipalController();
        
        [TestMethod]
        public void TestAvaliacaoRedirect()
        {
            string esperado = "Index";
            RedirectToRouteResult resultado = controller.Avaliacao() as RedirectToRouteResult;
            Assert.AreEqual(esperado, resultado.RouteValues["Action"]);
        }

        [TestMethod]
        public void TestPendenteModel()
        {
            MockHelper.FakeLoginUsuario("20150002", "/principal/pendente");

            List<Avaliacao> esperado = new List<Avaliacao>();
            ViewResult resultado = controller.Pendente() as ViewResult;
            Assert.AreEqual(esperado.GetType(), resultado.Model.GetType());
        }        
    }
}
