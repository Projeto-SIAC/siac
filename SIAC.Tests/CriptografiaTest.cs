/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SIAC.Tests
{
    [TestClass]
    public class CriptografiaTest
    {
        private string valor = "foo";
        private string esperadoBase64 = "Zm9v";

        [TestMethod]
        public void TestBase64Encode()
        {
            var resultado = Helpers.Criptografia.Base64Encode(valor);
            Assert.AreEqual(esperadoBase64, resultado);
        }

        [TestMethod]
        public void TestBase64Decode()
        {
            var resultado = Helpers.Criptografia.Base64Decode(esperadoBase64);
            Assert.AreEqual(valor, resultado);
        }
    }
}