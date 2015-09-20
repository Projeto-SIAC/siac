using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Avaliacao
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static int ObterNumIdentificador(int codTipoAvaliacao)
        {
            int ano = DateTime.Now.Year;
            int semestre = (DateTime.Now.Month > 6) ? 2 : 1;

            Avaliacao avalTemp = contexto.Avaliacao.Where(a => a.Ano == ano && a.Semestre == semestre && a.CodTipoAvaliacao == codTipoAvaliacao).OrderByDescending(a => a.NumIdentificador).FirstOrDefault();

            return (avalTemp != null) ? avalTemp.NumIdentificador + 1 : 1;
        }
    }
}