using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Aluno
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static void Inserir(Aluno aluno)
        {
            contexto.Aluno.Add(aluno);
            contexto.SaveChanges();
        }

        public static List<Aluno> ListarOrdenadamente()
        {
            return contexto.Aluno.OrderBy(a => a.Usuario.PessoaFisica.Nome).ToList();
        }
    }
}