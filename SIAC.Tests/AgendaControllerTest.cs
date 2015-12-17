using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIAC.Controllers;
using System.Web.Mvc;

namespace SIAC.Tests
{
    [TestClass]
    public class AgendaControllerTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new AgendaController();           

            var result = controller.Index() as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
