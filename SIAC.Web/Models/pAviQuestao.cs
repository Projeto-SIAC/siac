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

        public static void Remover(AviQuestao questao)
        {
            AviQuestao questaoTemp = contexto.AviQuestao.FirstOrDefault(q => q.Ano == questao.Ano
                                                                         && q.Semestre == questao.Semestre
                                                                         && q.CodTipoAvaliacao == questao.CodTipoAvaliacao
                                                                         && q.NumIdentificador == questao.NumIdentificador
                                                                         && q.CodAviModulo == questao.CodAviModulo
                                                                         && q.CodAviCategoria == questao.CodAviCategoria
                                                                         && q.CodAviIndicador == questao.CodAviIndicador
                                                                         && q.CodOrdem == questao.CodOrdem);
            if (questaoTemp != null)
            {
                contexto.AviQuestao.Remove(questaoTemp);
                contexto.SaveChanges();
            }
        }

        public static void Atualizar(AviQuestao questao)
        {
            AviQuestao temp = contexto.AviQuestao.FirstOrDefault(q => q.Ano == questao.Ano
                                                                   && q.Semestre == questao.Semestre
                                                                   && q.CodTipoAvaliacao == questao.CodTipoAvaliacao
                                                                   && q.NumIdentificador == questao.NumIdentificador
                                                                   && q.CodAviModulo == questao.CodAviModulo
                                                                   && q.CodAviCategoria == questao.CodAviCategoria
                                                                   && q.CodAviIndicador == questao.CodAviIndicador
                                                                   && q.CodOrdem == questao.CodOrdem);
            temp.Enunciado = questao.Enunciado;
            temp.Observacao = questao.Observacao;
            temp.AviQuestaoAlternativa = questao.AviQuestaoAlternativa;

            contexto.SaveChanges();
        }
    }
}