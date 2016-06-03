using System;
using System.Configuration;
using System.Text;

namespace SIAC.Helpers
{
    public class Criptografia
    {
        public static string RetornarHash(string senha)
        {
            string strSenha = Helpers.Configuracoes.Recuperar("SIAC_SALT") + senha;
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