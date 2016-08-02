using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIAC.Tests
{
    [TestClass]
    public class ExtensoesTest
    {
        [TestMethod]
        public void TestStringIsNumber()
        {
            var numeroA = "1";
            var numeroB = "1.1";
            var numeroC = "-1245.5";
            var naoNumeroA = "333a";
            var naoNumeroB = "123:3";

            Assert.IsTrue(numeroA.IsNumber());
            Assert.IsTrue(numeroB.IsNumber());
            Assert.IsTrue(numeroC.IsNumber());

            Assert.IsFalse(naoNumeroA.IsNumber());
            Assert.IsFalse(naoNumeroB.IsNumber());
        }

        [TestMethod]
        public void TestStringBetween()
        {
            var texto = "Elementar, meu caro Watson.";
            var esperado = "meu caro";

            Assert.AreEqual(esperado, texto.Between(11, 19));
        }

        [TestMethod]
        public void TestStringSumarizada()
        {
            var texto = "Toque outra vez, Sam";
            var esperado1 = $"1. {texto}";
            var esperado2 = $"1.1. {texto}";
            var esperado3 = $"1.4.1. {texto}";

            Assert.AreEqual(esperado1, texto.Sumarizada(new [] { 1 }));
            Assert.AreEqual(esperado2, texto.Sumarizada(new [] { 1, 1 }));
            Assert.AreEqual(esperado3, texto.Sumarizada(new [] { 1, 4, 1 }));
        }

        [TestMethod]
        public void TestStringReplaceChars()
        {
            var texto = "Eu discordo do que você diz, mas defenderei até a morte o seu direito de dizê-lo";
            var esperado1 = "Eu discordo do que você dix, mas defenderei até a morte o seu direito de dixê-lo";
            var esperado2 = "Eu qiscorqo qo que você qix, mas qefenqerei até a morte o seu qireito qe qixê-lo";
            var esperado3 = "Eu qkscorqo qo que você qkx, mas qefenqerek até a morte o seu qkrekto qe qkxê-lo";

            Assert.AreEqual(esperado1, texto.ReplaceChars("z", "x"));
            Assert.AreEqual(esperado2, texto.ReplaceChars("zd", "xq"));
            Assert.AreEqual(esperado3, texto.ReplaceChars("zdi", "xqk"));
        }

        [TestMethod]
        public void TestStringToHtml()
        {
            var texto = "Luke, I am your father";
            var esperado1 = $"<span>{texto}</span>";
            var esperado2 = $"<div><span>{texto}</span></div>";

            Assert.AreEqual(esperado1, texto.ToHtml(new [] { "span" }));
            Assert.AreEqual(esperado2, texto.ToHtml(new[] { "div", "span" }));
        }

        [TestMethod]
        public void TestStringToHtmlParagrafo()
        {
            var texto = "Isto é para os loucos. Os desajustados. Os rebeldes. Os criadores de caso. Os que vêem as coisas de forma diferente.\nEnquanto alguns os vêem como loucos, nós vemos gênios. Porque as pessoas que são loucas o suficiente para pensarem que podem mudar o mundo, são as que de fato o fazem";
            var esperado = "<p>Isto é para os loucos. Os desajustados. Os rebeldes. Os criadores de caso. Os que vêem as coisas de forma diferente.</p><p>Enquanto alguns os vêem como loucos, nós vemos gênios. Porque as pessoas que são loucas o suficiente para pensarem que podem mudar o mundo, são as que de fato o fazem</p>";

            Assert.AreEqual(esperado, texto.ToHtml(new[] { "p" }));
        }

        [TestMethod]
        public void TestStringToShortString()
        {
            var texto = "Se alguma coisa está difícil de ser feita, é porque não é para ser feita!";
            var esperado = "Se alguma coisa está difícil de ser feita, é porque...";

            Assert.AreEqual(esperado, texto.ToShortString(50));
        }

        [TestMethod]
        public void TestStringRemoveSpaces()
        {
            var texto = "\t\tQue a Força \nesteja\r\n       com você.      ";
            var esperado = "Que a Força esteja com você.";

            Assert.AreEqual(esperado, texto.RemoveSpaces());
        }
    }
}
