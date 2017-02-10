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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Avaliacao
    {
        [NotMapped]
        public string CodAvaliacao => $"{TipoAvaliacao.Sigla.ToUpper()}{Ano}{Semestre}{NumIdentificador.ToString("0000")}";

        [NotMapped]
        public DateTime? DtTermino => this.DtAplicacao.HasValue && this.Duracao.HasValue ? this.DtAplicacao.Value.AddMinutes(this.Duracao.Value) : new Nullable<DateTime>();

        [NotMapped]
        public Professor Professor
        {
            get
            {
                switch (this.CodTipoAvaliacao)
                {
                    case TipoAvaliacao.ACADEMICA:
                        return this.AvalAcademica.Professor;

                    case TipoAvaliacao.CERTIFICACAO:
                        return this.AvalCertificacao.Professor;

                    case TipoAvaliacao.REPOSICAO:
                        return this.AvalAcadReposicao.Professor;

                    default:
                        return null;
                }
            }
        }

        [NotMapped]
        public List<Tema> Temas => this.AvaliacaoTema.Select(at => at.Tema).Distinct().ToList();

        [NotMapped]
        public List<Questao> Questao
        {
            get
            {
                List<Questao> lstQuestao = new List<Questao>();
                foreach (var avaliacaoTema in AvaliacaoTema)
                    foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        lstQuestao.Add(avalTemaQuestao.QuestaoTema.Questao);
                return lstQuestao;
            }
        }

        [NotMapped]
        public List<QuestaoTema> QuestaoTema
        {
            get
            {
                List<QuestaoTema> QuestoesTema = new List<QuestaoTema>();
                foreach (var item in this.AvaliacaoTema)
                    foreach (var qt in item.AvalTemaQuestao)
                        QuestoesTema.Add(qt.QuestaoTema);
                return QuestoesTema;
            }
        }

        [NotMapped]
        public List<AvalQuesPessoaResposta> PessoaResposta
        {
            get
            {
                List<AvalQuesPessoaResposta> lstPessoaResposta = new List<AvalQuesPessoaResposta>();
                foreach (var avaliacaoTema in AvaliacaoTema)
                    foreach (var avalTemaQuestao in avaliacaoTema.AvalTemaQuestao)
                        lstPessoaResposta.AddRange(avalTemaQuestao.AvalQuesPessoaResposta);
                return lstPessoaResposta;
            }
        }

        [NotMapped]
        public int CodDificuldade => Questao.Max(q => q.CodDificuldade);

        [NotMapped]
        public Dificuldade Dificuldade => Questao.Count > 0 ? contexto.Dificuldade.Find(this.Questao.Max(q => q.CodDificuldade)) : null;

        [NotMapped]
        public int TipoQuestoes
        {
            get
            {
                int tipo = 0;

                foreach (var questao in this.Questao)
                    if (questao.CodTipoQuestao == TipoQuestao.OBJETIVA)
                    {
                        tipo += 1;
                        break;
                    }

                foreach (var questao in this.Questao)
                    if (questao.CodTipoQuestao == TipoQuestao.DISCURSIVA)
                    {
                        tipo += 2;
                        break;
                    }

                return tipo > 0 ? tipo : -1;
            }
        }

        [NotMapped]
        public bool FlagPendente => this.AvalPessoaResultado.Count > 0 || this.FlagArquivo ? false : true;

        [NotMapped]
        public bool FlagRealizada => this.AvalPessoaResultado.Count > 0;

        [NotMapped]
        public bool FlagAgendada => this.DtAplicacao.HasValue && this.FlagPendente;

        [NotMapped]
        public bool FlagAgora
        {
            get
            {
                var total = (DateTime.Now - DtAplicacao.Value).TotalMinutes;
                return total < (Duracao / 2) && total > 0;
            }
        }

        [NotMapped]
        public bool FlagVencida => (DateTime.Now - DtAplicacao.Value).TotalMinutes > (Duracao / 2);

        [NotMapped]
        public bool FlagCorrecaoPendente => this.FlagRealizada && this.PessoaResposta.Where(pr => !pr.RespNota.HasValue).Count() > 0;

        [NotMapped]
        public List<Questao> QuestaoEmbaralhada
        {
            get
            {
                List<Questao> lstQuestao = new List<Questao>();
                List<Questao> lstQuestaoEmbalharada = new List<Questao>();
                foreach (var avalTema in this.AvaliacaoTema)
                    lstQuestao.AddRange(avalTema.AvalTemaQuestao.Select(a => a.QuestaoTema.Questao).ToList());

                while (lstQuestao.Count > 0)
                {
                    int i = Sistema.Random.Next(lstQuestao.Count);
                    Questao qst = lstQuestao.ElementAt(i);
                    lstQuestaoEmbalharada.Add(qst);
                    lstQuestao.Remove(qst);
                }

                return lstQuestaoEmbalharada.OrderBy(q => q.CodTipoQuestao).ToList();
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static int ObterNumIdentificador(int codTipoAvaliacao)
        {
            if (Sistema.NumIdentificador.Count == 0)
            {
                for (int i = 1; i <= contexto.TipoAvaliacao.Max(t => t.CodTipoAvaliacao); i++)
                {
                    int ano = DateTime.Now.Year;
                    int semestre = DateTime.Now.SemestreAtual();
                    Avaliacao avalTemp = contexto.Avaliacao
                        .Where(a => a.Ano == ano
                            && a.Semestre == semestre
                            && a.CodTipoAvaliacao == i)
                        .OrderByDescending(a => a.NumIdentificador)
                        .FirstOrDefault();
                    if (avalTemp != null)
                        Sistema.NumIdentificador.Add(i, avalTemp.NumIdentificador + 1);
                    else
                        Sistema.NumIdentificador.Add(i, 1);
                }
            }
            return Sistema.NumIdentificador[codTipoAvaliacao]++;
        }

        public static Avaliacao ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);
            int semestre = int.Parse(codigo.Substring(codigo.Length - 1));
            codigo = codigo.Remove(codigo.Length - 1);
            int ano = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);

            int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

            Avaliacao aval = contexto.Avaliacao
                .FirstOrDefault(a => a.Ano == ano
                    && a.Semestre == semestre
                    && a.NumIdentificador == numIdentificador
                    && a.CodTipoAvaliacao == codTipoAvaliacao);

            return aval;
        }

        public static bool AlternarFlagArquivo(string codigo)
        {
            Avaliacao aval = ListarPorCodigoAvaliacao(codigo);
            if (aval.AvalPessoaResultado.Count == 0)
            {
                aval.FlagArquivo = !aval.FlagArquivo;
                contexto.SaveChanges();
            }
            return aval.FlagArquivo;
        }

        public static bool AlternarFlagLiberada(string codAvaliacao)
        {
            Avaliacao aval = ListarPorCodigoAvaliacao(codAvaliacao);
            aval.FlagLiberada = !aval.FlagLiberada;
            contexto.SaveChanges();
            return aval.FlagLiberada;
        }

        public static void AtualizarQuestoes(string codigo, int[] questoes)
        {
            Avaliacao aval = ListarPorCodigoAvaliacao(codigo);

            List<QuestaoTema> questoesTema = new List<QuestaoTema>();

            foreach (var questao in questoes)
            {
                List<QuestaoTema> qtTemp = contexto.QuestaoTema.Where(qt => qt.CodQuestao == questao).ToList();
                questoesTema.AddRange(qtTemp);
            }

            if (questoesTema.Count > 0)
            {
                //Deletar questões antigas
                List<AvalTemaQuestao> questoesAntigas = contexto.AvalTemaQuestao
                    .Where(atq => atq.Ano == aval.Ano
                        && atq.Semestre == aval.Semestre
                        && atq.CodTipoAvaliacao == aval.CodTipoAvaliacao
                        && atq.NumIdentificador == aval.NumIdentificador)
                    .ToList();
                contexto.AvalTemaQuestao.RemoveRange(questoesAntigas);

                List<AvalTemaQuestao> questoesAdicionadas = new List<AvalTemaQuestao>();

                foreach (var tema in aval.Temas)
                {
                    List<QuestaoTema> questaoTema = questoesTema.Where(qt => qt.Tema == tema).ToList();
                    AvaliacaoTema avalTema = aval.AvaliacaoTema.FirstOrDefault(at => at.Tema == tema);

                    if (questaoTema.Count > 0)
                    {
                        foreach (var qt in questaoTema)
                        {
                            AvalTemaQuestao proximaQuestao = new AvalTemaQuestao
                            {
                                AvaliacaoTema = avalTema,
                                QuestaoTema = qt
                            };

                            //Verificar se a questão já não foi adicionada
                            if (questoesAdicionadas
                                .Where(atq => atq.Ano == aval.Ano
                                    && atq.Semestre == aval.Semestre
                                    && atq.CodTipoAvaliacao == aval.CodTipoAvaliacao
                                    && atq.NumIdentificador == aval.NumIdentificador
                                    && atq.CodQuestao == qt.CodQuestao).ToList().Count == 0)
                            {
                                contexto.AvalTemaQuestao.Add(proximaQuestao);
                                questoesAdicionadas.Add(proximaQuestao);
                            }
                        }
                    }
                }

                contexto.SaveChanges();
            }
        }
    }
}