using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SIAC.Tests
{
    [TestClass]
    public class CaptchaTest : BaseControllerTest
    {
        [TestMethod]
        public void TestNovoChamaSessao()
        {
            var resultado = Helpers.Captcha.Novo();

            this.moqHttpContext.Verify(x => x.Session, Times.AtLeastOnce());
        }
    }
}