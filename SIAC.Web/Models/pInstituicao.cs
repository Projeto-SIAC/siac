using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Instituicao
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static List<Instituicao> ListarOrdenadamente()
        {
            return contexto.Instituicao.OrderBy(ins => ins.Sigla).ToList();
                
        }
    }
}