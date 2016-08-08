using System;
using System.Configuration;

namespace SIAC.Helpers
{
    public class Configuracoes
    {
        public static object Recuperar(string chave, object padrao = null)
        {
            var valor = Environment.GetEnvironmentVariable(chave) ?? ConfigurationManager.AppSettings[chave];
            if (valor != null)
            {
                return valor;
            }
            return padrao;
        }
    }
}