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
    public class PerfilControllerTest
    {
        PerfilController controller = new PerfilController();

        [TestMethod]
        public void TestIndexModel()
        {
            string esperado = "20150002";
            MockHelper.FakeLoginUsuario(esperado, "/perfil");
            Usuario resultado = (controller.Index() as ViewResult).Model as Usuario;
            Assert.AreEqual(esperado, resultado.Matricula);
        }

        [TestMethod]
        public void TestEstatisticasModel()
        {
            string esperado = "20150002";
            MockHelper.FakeLoginUsuario(esperado, "/perfil");
            Usuario resultado = (controller.Estatisticas() as PartialViewResult).Model as Usuario;
            Assert.AreEqual(esperado, resultado.Matricula);
        }

        [TestMethod]
        public void TestEnviarOpiniaoEmpty()
        {
            string matricula = "20150002";
            MockHelper.FakeLoginUsuario(matricula, "/perfil/enviaropiniao");
            Contexto c = Repositorio.GetInstance();
            Usuario usuario = c.Usuario.Find(matricula);
            int quantidade = usuario.UsuarioOpiniao.Count;
            controller.EnviarOpiniao("");
            Usuario novoUsuario = c.Usuario.Find(matricula);
            int novaQuantidade = novoUsuario.UsuarioOpiniao.Count;
            Assert.AreEqual(quantidade, novaQuantidade);
        }

        [TestMethod]
        public void TestEnviarOpiniao()
        {
            string matricula = "20150002";
            MockHelper.FakeLoginUsuario(matricula, "/perfil/enviaropiniao");
            Contexto c = Repositorio.GetInstance();
            Usuario usuario = c.Usuario.Find(matricula);
            int quantidade = usuario.UsuarioOpiniao.Count;
            controller.EnviarOpiniao("Oi, minha opinião.");
            Usuario novoUsuario = c.Usuario.Find(matricula);
            int novaQuantidade = novoUsuario.UsuarioOpiniao.Count;
            bool resultado = novaQuantidade > quantidade;
            Assert.IsTrue(resultado);
            if (resultado)
            {
                UsuarioOpiniao usuarioOpiniao = novoUsuario.UsuarioOpiniao.Last();
                c.UsuarioOpiniao.Remove(usuarioOpiniao);
                c.SaveChanges();
            }
        }
    }
}
