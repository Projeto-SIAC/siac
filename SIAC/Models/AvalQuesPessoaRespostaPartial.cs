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
using System;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalQuesPessoaResposta
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static void SalvarResposta(Avaliacao avaliacao, Questao questao, PessoaFisica pessoa, string resposta, string comentario)
        {
            if (avaliacao != null && questao != null && pessoa != null && !String.IsNullOrWhiteSpace(resposta))
            {
                bool sobreposicao = true;

                AvalQuesPessoaResposta corrente = contexto.AvalQuesPessoaResposta
                    .FirstOrDefault(r =>
                        r.Ano == avaliacao.Ano
                        && r.Semestre == avaliacao.Semestre
                        && r.CodTipoAvaliacao == avaliacao.CodTipoAvaliacao
                        && r.NumIdentificador == avaliacao.NumIdentificador
                        && r.CodQuestao == questao.CodQuestao
                        && r.CodPessoaFisica == pessoa.CodPessoa);

                if (corrente == null)
                {
                    sobreposicao = false;
                    corrente = new AvalQuesPessoaResposta()
                    {
                        AvalTemaQuestao = avaliacao.AvaliacaoTema
                            .Where(at => at.AvalTemaQuestao.FirstOrDefault(atq => atq.CodQuestao == questao.CodQuestao) != null)
                            .Select(at => at.AvalTemaQuestao.FirstOrDefault(atq => atq.CodQuestao == questao.CodQuestao))
                            .FirstOrDefault(),
                        PessoaFisica = pessoa
                    };
                }

                switch (questao.CodTipoQuestao)
                {
                    case TipoQuestao.OBJETIVA:
                        corrente.RespAlternativa = int.Parse(resposta);
                        break;

                    case TipoQuestao.DISCURSIVA:
                        corrente.RespDiscursiva = resposta;
                        break;

                    default:
                        break;
                }

                corrente.RespComentario = !String.IsNullOrWhiteSpace(comentario) ? comentario : null;

                if (sobreposicao == false)
                {
                    contexto.AvalQuesPessoaResposta.Add(corrente);
                }

                contexto.SaveChanges();
            }
        }
    }
}