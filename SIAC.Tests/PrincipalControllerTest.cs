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
        [TestMethod]
        public void TestPendenteModel()
        {
            HttpContext.Current = MockHelper.FakeHttpContext("http://siac.apphb.com/principal/pendente");

            Helpers.Sessao.Inserir("UsuarioMatricula", "20150002");

            var controller = new PrincipalController();           

            var result = controller.Pendente() as ViewResult;

            var modelEsperado = new List<Avaliacao>();

            Assert.AreEqual(modelEsperado.GetType(), result.Model.GetType());
        }        
    }
}
