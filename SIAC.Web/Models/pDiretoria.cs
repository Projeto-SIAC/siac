using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Diretoria
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static void Inserir(Diretoria diretoria)
        {
            contexto.Diretoria.Add(diretoria);
            contexto.SaveChanges();
        }

        public static List<Diretoria> ListarOrdenadamente()
        {
            return contexto.Diretoria.OrderBy(d => d.Sigla).ToList();
        }

        public static Diretoria ListarPorCodigo(int codDiretoria)
        {
            return contexto.Diretoria.FirstOrDefault(d => d.CodDiretoria == codDiretoria);
        }
    }
}