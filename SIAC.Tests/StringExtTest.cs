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
    public class StringExtTest
    {
        private string[] a = new string[] { "foo", "bar", "" };
        private string[] b = new string[] { "foo", "bar", "baz" };
        private string[] c = new string[] { "foo", "bar", " " };

        [TestMethod]
        public void TestIsNullOrEmptyComportamento()
        {
            Assert.IsTrue(StringExt.IsNullOrEmpty(a));
            Assert.IsFalse(StringExt.IsNullOrEmpty(b));
            Assert.IsFalse(StringExt.IsNullOrEmpty(c));
        }

        [TestMethod]
        public void IsNullOrWhiteSpaceComportamento()
        {
            Assert.IsTrue(StringExt.IsNullOrWhiteSpace(a));
            Assert.IsFalse(StringExt.IsNullOrWhiteSpace(b));
            Assert.IsTrue(StringExt.IsNullOrWhiteSpace(c));
        }
    }
}