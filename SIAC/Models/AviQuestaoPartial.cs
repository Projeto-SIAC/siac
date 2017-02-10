/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviQuestao
    {
        [NotMapped]
        public List<AviQuestaoPessoaResposta> Respostas
        {
            get
            {
                List<PessoaFisica> pessoas = this.AviQuestaoPessoaResposta.Select(pr => pr.PessoaFisica).Distinct().ToList();

                List<AviQuestaoPessoaResposta> retorno = new List<AviQuestaoPessoaResposta>();
                if (pessoas.Count > 0)
                {
                    foreach (var pessoa in pessoas)
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

        public string ToJsonChart(List<AviQuestaoPessoaResposta> respostas = null)
        {
            respostas = this.Respostas;
            string json = string.Empty;
            json += "[";

            for (int i = 1, length = this.AviQuestaoAlternativa.Count; i <= length; i++)
            {
                string rgba = Helpers.CorDinamica.Rgba();

                json += "{";
                json += $"\"value\":\"{respostas.Where(r => r.RespAlternativa == i).Count()}\"";
                json += ",";
                json += $"\"label\":\"Alternativa {(i - 1).GetIndiceAlternativa()}\"";
                json += ",";
                json += $"\"color\":\"{rgba}\"";
                json += ",";
                json += $"\"highlight\":\"{rgba.Replace("1)", "0.8)")}\"";
                json += "}";

                if (i != length)
                {
                    json += ",";
                }
            }

            json += "]";

            return json;
        }


        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(AviQuestao questao)
        {
            contexto.AviQuestao.Add(questao);
            contexto.SaveChanges();
        }

        public static int ObterNovaOrdem(AvalAvi avi)
        {
            int questaoIndice = avi.AviQuestao.Count > 0 ? avi.AviQuestao.Max(q => q.CodOrdem) : 0;
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