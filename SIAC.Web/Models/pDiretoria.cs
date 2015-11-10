using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Diretoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodCampus}.{CodDiretoria}";

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

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