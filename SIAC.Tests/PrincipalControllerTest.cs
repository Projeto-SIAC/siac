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
        public void TestPendenteModel()
        {
            HttpContext.Current = MockHelper.FakeHttpContext("http://siac.apphb.com/principal/pendente");
            Helpers.Sessao.Inserir("UsuarioMatricula", "20150002");

            List<Avaliacao> esperado = new List<Avaliacao>();
            ViewResult resultado = controller.Pendente() as ViewResult;
            Assert.AreEqual(esperado.GetType(), resultado.Model.GetType());
        }        
    }
}
