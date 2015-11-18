using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalAcadReposicao
    {
        public Professor Professor => this.Justificacao.FirstOrDefault()?.Professor;

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static AvalAcadReposicao ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = 0;
            int semestre = 0;
            int ano = 0;

            if (codigo.Length == 13)
            {

                int.TryParse(codigo.Substring(codigo.Length - 4), out numIdentificador);
                codigo = codigo.Remove(codigo.Length - 4);
                int.TryParse(codigo.Substring(codigo.Length - 1), out semestre);
                codigo = codigo.Remove(codigo.Length - 1);
                int.TryParse(codigo.Substring(codigo.Length - 4), out ano);
                codigo = codigo.Remove(codigo.Length - 4);

                int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

                AvalAcadReposicao avaliacao = contexto.AvalAcadReposicao.FirstOrDefault(aval => aval.Ano == ano && aval.Semestre == semestre && aval.NumIdentificador == numIdentificador && aval.CodTipoAvaliacao == codTipoAvaliacao);

                return avaliacao;
            }
            return null;
        }


    }
}