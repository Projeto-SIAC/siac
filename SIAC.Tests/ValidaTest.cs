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
using SIAC.Helpers;

namespace SIAC.Tests
{
    [TestClass]
    public class ValidaTest
    {
        private string cpf = "38756554516";
        private string email = "example@foobar.com";

        [TestMethod]
        public void TestCPFComportamento()
        {
            Assert.IsTrue(Valida.CPF(cpf));
            Assert.IsFalse(Valida.CPF("11122233344"));
        }

        [TestMethod]
        public void TestEmailComportamento()
        {
            Assert.IsTrue(Valida.Email(email));
            Assert.IsFalse(Valida.Email("foobar.com"));
            Assert.IsFalse(Valida.Email("@foobar.com"));
            Assert.IsFalse(Valida.Email("example@.com"));
            Assert.IsFalse(Valida.Email("example@com"));
        }
    }
}