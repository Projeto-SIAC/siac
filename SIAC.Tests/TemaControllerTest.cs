using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class TemaControllerTest
    {
        TemaController controller = new TemaController();

        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            RedirectToRouteResult resultado = controller.Index() as RedirectToRouteResult;
            Assert.AreEqual("Acesso", resultado.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaZero()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("0") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaVazio()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaNulo()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina(null) as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoZero()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("0") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoVazio()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina("") as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoNulo()
        {
            JsonResult resultado = controller.RecuperarTemasPorCodDisciplina(null) as JsonResult;
            Assert.AreEqual(null, resultado.Data);
        }
    }
}
