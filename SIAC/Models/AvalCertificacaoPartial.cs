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
using SIAC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalCertificacao
    {
        [NotMapped]
        public List<PessoaFisica> PessoasRealizaram
        {
            get
            {
                List<PessoaFisica> retorno = new List<PessoaFisica>();
                foreach (var pf in this.PessoaFisica)
                {
                    var lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == pf.CodPessoa);
                    if (lstRespostas.Count() > 0)
                        retorno.Add(pf);
                }
                return retorno;
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(AvalCertificacao avalCertificacao)
        {
            contexto.AvalCertificacao.Add(avalCertificacao);
            contexto.SaveChanges();
        }

        public static List<AvalCertificacao> ListarCorrecaoPendentePorProfessor(int codProfessor) =>
            contexto.AvalQuesPessoaResposta
                .Where(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalCertificacao.CodProfessor == codProfessor && !a.RespNota.HasValue)
                .OrderBy(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.DtAplicacao)
                .Select(a => a.AvalTemaQuestao.AvaliacaoTema.Avaliacao.AvalCertificacao)
                .Distinct()
                .ToList();

        public static AvalCertificacao ListarPorCodigoAvaliacao(string codigo) => Avaliacao.ListarPorCodigoAvaliacao(codigo)?.AvalCertificacao;

        public static List<AvalCertificacao> ListarPorPessoa(int codPessoaFisica) =>
            contexto.AvalCertificacao.Where(ac => ac.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();

        public static List<AvalCertificacao> ListarPorProfessor(int codProfessor) =>
            contexto.AvalCertificacao.Where(ac => ac.CodProfessor == codProfessor)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();

        public static List<AvalCertificacao> ListarAgendadaPorUsuario(Usuario usuario)
        {
            int codPessoaFisica = usuario.CodPessoaFisica;
            switch (usuario.CodCategoria)
            {
                case Categoria.ALUNO:
                    return contexto.AvalCertificacao
                        .Where(a => a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                        .ToList();

                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.Last().CodProfessor;
                    return contexto.AvalCertificacao
                        .Where(a => a.CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();

                case Categoria.COLABORADOR:
                    int codColaborador = usuario.Colaborador.Last().CodColaborador;
                    return contexto.AvalCertificacao
                        .Where(a =>
                            a.Avaliacao.DtAplicacao.HasValue
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo &&
                            (
                                (a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                             || (a.Professor.TurmaDiscProfHorario.Where(t => t.Turma.Curso.CodColabCoordenador == codColaborador
                             || t.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                             || t.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                             || t.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0).Count() > 0)
                            )
                        )
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();

                default:
                    return new List<AvalCertificacao>();
            }
        }

        public static List<AvalCertificacao> ListarAgendadaPorUsuario(Usuario usuario, DateTime inicio, DateTime termino)
        {
            int codPessoaFisica = usuario.CodPessoaFisica;
            switch (usuario.CodCategoria)
            {
                case Categoria.ALUNO:
                    return contexto.AvalCertificacao
                        .Where(a => a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null
                            && a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                        .ToList();

                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.LastOrDefault()?.CodProfessor ?? 0;
                    return contexto.AvalCertificacao
                        .Where(a => a.CodProfessor == codProfessor
                            && a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo)
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();

                case Categoria.COLABORADOR:
                    int codColaborador = usuario.Colaborador.LastOrDefault()?.CodColaborador ?? 0;
                    return contexto.AvalCertificacao
                        .Where(a =>
                            a.Avaliacao.DtAplicacao >= inicio && a.Avaliacao.DtAplicacao <= termino
                            && a.Avaliacao.AvalPessoaResultado.Count == 0
                            && !a.Avaliacao.FlagArquivo &&
                            (
                                (a.PessoaFisica.FirstOrDefault(p => p.CodPessoa == codPessoaFisica) != null)
                                || (a.Professor.TurmaDiscProfHorario.Where(t => t.Turma.Curso.CodColabCoordenador == codColaborador
                                        || t.Turma.Curso.Diretoria.CodColaboradorDiretor == codColaborador
                                        || t.Turma.Curso.Diretoria.Campus.CodColaboradorDiretor == codColaborador
                                        || t.Turma.Curso.Diretoria.Campus.Instituicao.Reitoria.Where(r => r.CodColaboradorReitor == codColaborador).Count() > 0).Count() > 0
                                    )
                            )
                        )
                        .OrderBy(a => a.Avaliacao.DtAplicacao)
                        .ToList();

                default:
                    return new List<AvalCertificacao>();
            }
        }

        public static bool CorrigirQuestaoAluno(string codAvaliacao, string matrAluno, int codQuestao, double notaObtida, string profObservacao)
        {
            if (!StringExt.IsNullOrWhiteSpace(codAvaliacao, matrAluno) && codQuestao != 0)
            {
                AvalCertificacao cert = ListarPorCodigoAvaliacao(codAvaliacao);

                int codPessoaFisica = int.Parse(matrAluno);

                AvalQuesPessoaResposta resposta = cert.Avaliacao.PessoaResposta.FirstOrDefault(pr => pr.CodQuestao == codQuestao && pr.CodPessoaFisica == codPessoaFisica);

                resposta.RespNota = notaObtida;
                resposta.ProfObservacao = profObservacao;

                cert.Avaliacao.AvalPessoaResultado
                    .Single(r => r.CodPessoaFisica == codPessoaFisica)
                    .Nota = cert.Avaliacao.PessoaResposta
                    .Where(pr => pr.CodPessoaFisica == codPessoaFisica)
                    .Average(pr => pr.RespNota);

                contexto.SaveChanges();

                return true;
            }

            return false;
        }
    }
}