using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AviQuestao
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AviQuestao questao)
        {
            contexto.AviQuestao.Add(questao);
            contexto.SaveChanges();
        }

        public static int ObterNovaOrdem(AvalAvi avi, int modulo, int categoria, int indicador)
        {
            AviQuestao questao = contexto.AviQuestao.Where(aq => aq.Ano == avi.Ano
                                                              && aq.Semestre == avi.Semestre
                                                              && aq.CodTipoAvaliacao == avi.CodTipoAvaliacao
                                                              && aq.NumIdentificador == avi.NumIdentificador
                                                              && aq.CodAviModulo == modulo
                                                              && aq.CodAviCategoria == categoria
                                                              && aq.CodAviIndicador == indicador)
                                                              .OrderByDescending(aq => aq.CodOrdem)
                                                              .FirstOrDefault();

            return questao != null ? questao.CodOrdem + 1 : 1;
        }
    }
}