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

            string matricula = "20150002";
            HttpContext.Current = MockHelper.FakeHttpContext("http://siac.apphb.com/principal/agenda/academicas");
            Helpers.Sessao.Inserir("UsuarioMatricula", matricula);
            Helpers.Sessao.Inserir("UsuarioCategoriaCodigo", 2);
            var usuario = Repositorio.GetInstance().Usuario.Find(matricula);
            Sistema.UsuarioAtivo[matricula] = usuario.UsuarioAcesso.Last();

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
