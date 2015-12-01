using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Instituicao
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Instituicao> ListarOrdenadamente()
        {
            return contexto.Instituicao.OrderBy(ins => ins.Sigla).ToList();
        }

        public static Instituicao ListarPorCodigo(int codInstituicao)
        {
            return contexto.Instituicao.FirstOrDefault(ins => ins.CodInstituicao == codInstituicao);
        }
    }
}