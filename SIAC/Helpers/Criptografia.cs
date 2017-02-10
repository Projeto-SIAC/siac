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
using System;
using System.Configuration;
using System.Text;
using DevOne.Security.Cryptography.BCrypt;

namespace SIAC.Helpers
{
    public class Criptografia
    {
        public static bool ChecarSenha(string senha, string hash)
        {
            return BCryptHelper.CheckPassword(senha, hash);
        }

        public static string RetornarHash(string senha)
        {
            return BCryptHelper.HashPassword(senha, BCryptHelper.GenerateSalt((int)Configuracoes.Recuperar("SIAC_LOG_ROUNDS", 10)));
        }

        public static string RetornarHashSHA256(string valor)
        {
            string strSenha = Helpers.Configuracoes.Recuperar("SIAC_SECRET") + valor;
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            System.Text.StringBuilder sb = new StringBuilder();
            sha.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(strSenha));
            byte[] result = sha.Hash;
            for (int i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("x2"));
            return sb.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            // Thanks, Kevin Driedger
            // http://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            // Thanks, Kevin Driedger
            // http://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}