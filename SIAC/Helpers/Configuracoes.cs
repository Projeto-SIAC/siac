using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SIAC.Helpers
{
    public class Configuracoes
    {
        public static object Recuperar(string chave) =>
            Environment.GetEnvironmentVariable(chave) ?? ConfigurationManager.AppSettings[chave];
    }
}