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
using System.Text.RegularExpressions;

namespace SIAC.Tests
{
    [TestClass]
    public class FormateTest
    {
        [TestMethod]
        public void TestParaCPF()
        {
            var regex = new Regex(@"[0-9]{3}\.[0-9]{3}\.[0-9]{3}\-[0-9]{2}");
            var resultado = Helpers.Formate.ParaCPF("12345678900");
            Assert.IsTrue(regex.IsMatch(resultado));
        }

        [TestMethod]
        public void TestDeCPF()
        {
            var cpf = "123.456.789-00";
            var esperado = "12345678900";
            var resultado = Helpers.Formate.DeCPF(cpf);
            Assert.AreEqual(esperado, resultado);
        }
    }
}