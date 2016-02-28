﻿using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviQuestao
    {
        public List<AviQuestaoPessoaResposta> Respostas
        {
            get
            {
                List<PessoaFisica> pessoas = this.AviQuestaoPessoaResposta.Select(pr => pr.PessoaFisica).Distinct().ToList();
                
                List<AviQuestaoPessoaResposta> retorno = new List<AviQuestaoPessoaResposta>();
                if (pessoas.Count > 0)
                {
                    foreach (PessoaFisica pessoa in pessoas)
                    { 
                        AviQuestaoPessoaResposta resposta = this.AviQuestaoPessoaResposta
                            .Where(pr => pr.CodOrdem == this.CodOrdem && pr.CodPessoaFisica == pessoa.CodPessoa)
                            .OrderByDescending(pr => pr.CodRespostaOrdem)
                            .FirstOrDefault();
                        if (resposta != null)
                            retorno.Add(resposta);
                    }
                }
                return retorno;
            }
        }

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(AviQuestao questao)
        {
            contexto.AviQuestao.Add(questao);
            contexto.SaveChanges();
        }

        public static int ObterNovaOrdem(AvalAvi avi)
        {
            int questaoIndice = contexto.AviQuestao
                .Where(aq => aq.Ano == avi.Ano
                    && aq.Semestre == avi.Semestre
                    && aq.CodTipoAvaliacao == avi.CodTipoAvaliacao
                    && aq.NumIdentificador == avi.NumIdentificador)
                    .Max(q => q.CodOrdem);
            return questaoIndice + 1;
        }

        public static void Remover(AviQuestao questao)
        {
            AviQuestao questaoTemp = contexto.AviQuestao
                .FirstOrDefault(q => q.Ano == questao.Ano
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
            AviQuestao temp = contexto.AviQuestao
                .FirstOrDefault(q => q.Ano == questao.Ano
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