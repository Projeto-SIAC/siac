using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class LembreteControllerTest
    {
        [TestMethod]
        public void TestIndexRedirecionamento()
        {
            var controller = new LembreteController();

            var result = controller.Index() as RedirectToRouteResult;

            Assert.AreEqual("Acesso", result.RouteValues["controller"]);
        }
    }
}
