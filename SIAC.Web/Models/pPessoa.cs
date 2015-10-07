using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Pessoa
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static int Inserir(Pessoa pessoa)
        {
            contexto.Pessoa.Add(pessoa);
            contexto.SaveChanges();
            return pessoa.CodPessoa;
        }
    }
}