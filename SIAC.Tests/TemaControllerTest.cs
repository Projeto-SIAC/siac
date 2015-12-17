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
        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            var controller = new TemaController();

            var result = controller.Index() as RedirectToRouteResult;

            Assert.AreEqual("Acesso", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaZero()
        {
            var controller = new TemaController();

            var result = controller.RecuperarTemasPorCodDisciplina("0") as JsonResult;

            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaVazio()
        {
            var controller = new TemaController();

            var result = controller.RecuperarTemasPorCodDisciplina("") as JsonResult;

            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaNulo()
        {
            var controller = new TemaController();

            var result = controller.RecuperarTemasPorCodDisciplina(null) as JsonResult;

            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoZero()
        {
            var controller = new TemaController();

            var result = controller.RecuperarTemasPorCodDisciplina("0") as JsonResult;

            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoVazio()
        {
            var controller = new TemaController();

            var result = controller.RecuperarTemasPorCodDisciplina("") as JsonResult;

            Assert.AreEqual(null, result.Data);
        }

        [TestMethod]
        public void TestRecuperarTemasPorCodDisciplinaTemQuestaoNulo()
        {
            var controller = new TemaController();

            var result = controller.RecuperarTemasPorCodDisciplina(null) as JsonResult;

            Assert.AreEqual(null, result.Data);
        }
    }
}
