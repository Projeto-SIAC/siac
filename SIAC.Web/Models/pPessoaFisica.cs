using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class PessoaFisica
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static int Inserir(PessoaFisica pessoaFisica)
        {
            contexto.PessoaFisica.Add(pessoaFisica);
            contexto.SaveChanges();
            return pessoaFisica.CodPessoa;
        }
    }
}