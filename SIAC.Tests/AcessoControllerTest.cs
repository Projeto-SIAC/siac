using System;
using System.Web.Mvc;
using SIAC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.ViewModels;

namespace SIAC.Tests
{
    [TestClass]
    public class AcessoControllerTest
    {
        AcessoController controller = new AcessoController();

        [TestMethod]
        public void TestIndexComValoresVazios()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = String.Empty;
            form["txtSenha"] = String.Empty;
            AcessoIndexViewModel resultado = (controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public void TestIndexComValoresNulos()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = null;
            form["txtSenha"] = null;
            AcessoIndexViewModel resultado = (controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }


        [TestMethod]
        public void TestIndexComValoresEspaco()
        {
            FormCollection form = new FormCollection();
            form["txtMatricula"] = " ";
            form["txtSenha"] = " ";
            AcessoIndexViewModel resultado = (controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
            Assert.AreEqual(true, resultado.Erro);
        }

        [TestMethod]
        public void TestIndexComFormularioSemChaves()
        {
            FormCollection form = new FormCollection();
            AcessoIndexViewModel resultado = (controller.Index(form) as ViewResult).Model as AcessoIndexViewModel;
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
