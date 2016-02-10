using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using SIAC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class LembreteControllerTest
    {
        LembreteController controller = new LembreteController();

        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            RedirectToRouteResult resultado = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Acesso", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestLembreteTodosJson()
        {
            JsonResult esperado = new JsonResult();
            List<object> resultados = new List<object>();

            MockHelper.FakeLoginUsuario("20150002", "/principal/lembrete");

            resultados.Add(controller.Principal());
            resultados.Add(controller.Institucional());
            resultados.Add(controller.Menu());
            resultados.Add(controller.Lembretes());
            resultados.Add(controller.Notificacoes());

            foreach (var resultado in resultados)
            {
                Assert.AreEqual(esperado.GetType(), resultado.GetType());
            }
        }
    }
}
