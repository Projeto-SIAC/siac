using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class AvalPessoaResultado
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalPessoaResultado avalPessoaResultado)
        {
            contexto.AvalPessoaResultado.Add(avalPessoaResultado);
            contexto.SaveChanges();
        }
    }
}