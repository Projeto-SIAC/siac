using System;
using System.Configuration;

namespace SIAC.Helpers
{
    public class Criptografia
    {
        public static string RetornarHash(string senha)
        {
            string strSenha = (Environment.GetEnvironmentVariable("SALT") ?? ConfigurationManager.AppSettings["SALT"]) + senha;
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sha.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(strSenha));
            byte[] result = sha.Hash;
            for (int i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("x2"));
            return sb.ToString();
        }
    }
}