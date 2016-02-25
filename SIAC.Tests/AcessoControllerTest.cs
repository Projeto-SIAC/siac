using System;
using System.Web.Mvc;
using SIAC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.ViewModels;
using System.Threading.Tasks;

namespace SIAC.Tests
{
    [TestClass]
    public class AcessoControllerTest
    {
        AcessoController controller = new AcessoController();

        [TestMethod]
        public async Task TestIndexComValoresVazios()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = String.Empty;
            form["txtSenha"] = String.Empty;
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public async Task TestIndexComValoresNulos()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = null;
            form["txtSenha"] = null;
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }


        [TestMethod]
        public async Task TestIndexComValoresEspaco()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = " ";
            form["txtSenha"] = " ";
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public async Task TestIndexComFormularioSemChaves()
        {
            FormCollection form = new FormCollection();
            AcessoIndexViewModel resultado = (await controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public void TestSairRedirecionamento()
        {
            RedirectResult resultado = controller.Sair() as RedirectResult;
            Assert.AreEqual("~/", resultado.Url);
        }
    }
}
