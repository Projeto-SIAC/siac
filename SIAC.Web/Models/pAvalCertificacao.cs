using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalCertificacao
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalCertificacao avalCertificacao)
        {
            contexto.AvalCertificacao.Add(avalCertificacao);
            contexto.SaveChanges();
        }
    }
}