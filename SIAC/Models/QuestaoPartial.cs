using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Questao
    {
        [NotMapped]
        public Disciplina Disciplina => this.QuestaoTema.FirstOrDefault()?.Tema.Disciplina;

        [NotMapped]
        public List<AvalQuesPessoaResposta> Respostas => contexto.AvalQuesPessoaResposta.Where(r => r.CodQuestao == this.CodQuestao).ToList();

        public bool TemUmaCorreta()
        {
            bool unica = false;

            foreach (var alt in this.Alternativa)
            {
                if (alt.FlagGabarito)
                {
                    if (unica == true)
                    {
                        unica = false;
                        break;
                    }
                    unica = true;
                }
            }

            return unica;
        }

        public List<Alternativa> EmbaralharAlternativa()
        {
            List<Alternativa> lstAlternativa = this.Alternativa.ToList();
            List<Alternativa> lstAlternativaEmbaralhada = new List<Alternativa>();

            while (lstAlternativaEmbaralhada.Count != lstAlternativa.Count)
            {
                int i = Sistema.Random.Next(lstAlternativa.Count);
                Alternativa alt = lstAlternativa.ElementAt(i);
                lstAlternativaEmbaralhada.Add(alt);
                lstAlternativa.Remove(alt);
            }

            return lstAlternativaEmbaralhada;
        }

        public string ToJsonChart(List<AvalQuesPessoaResposta> lstResposta)
        {
            string json = string.Empty;

            json += "[";

            for (int i = 0, length = this.Alternativa.Count; i < length; i++)
            {
                string rgba = Helpers.CorDinamica.Rgba();
                json += "{";
                json += $"\"value\":\"{lstResposta.Where(r => r.RespAlternativa == i).Count()}\"";
                json += ",";
                json += $"\"label\":\"Alternativa {i.GetIndiceAlternativa()}\"";
                json += ",";
                json += $"\"color\":\"{rgba}\"";
                json += ",";
                json += $"\"highlight\":\"{rgba.Replace("1)", "0.8)")}\"";
                json += "}";
                if (i != length - 1)
                {
                    json += ",";
                }
            }

            json += "]";

            return json;
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Questao questao)
        {
            questao.DtCadastro = DateTime.Now;
            contexto.Questao.Add(questao);
            contexto.SaveChanges();
        }

        public static bool AlternarFlagArquivo(int codQuestao)
        {
            Questao questao = ListarPorCodigo(codQuestao);
            questao.FlagArquivo = !questao.FlagArquivo;
            contexto.SaveChanges();
            return questao.FlagArquivo;
        }

        public static bool PodeAtualizar(Questao questao) => contexto.AvalTemaQuestao.FirstOrDefault(atq => atq.CodQuestao == questao.CodQuestao) == null;

        public static void Atualizar(Questao questao)
        {
            Questao qTemp = contexto.Questao.FirstOrDefault(qst => qst.CodQuestao == questao.CodQuestao);

            qTemp.Enunciado = questao.Enunciado;
            qTemp.Objetivo = questao.Objetivo;
            if (qTemp.CodTipoQuestao == 1)
            {
                for (int i = 0; i < qTemp.Alternativa.Count; i++)
                {
                    qTemp.Alternativa.ElementAt(i).Enunciado = questao.Alternativa.ElementAt(i).Enunciado;
                    qTemp.Alternativa.ElementAt(i).Comentario = questao.Alternativa.ElementAt(i).Comentario;
                }
            }
            else
            {
                qTemp.Comentario = questao.Comentario;
                qTemp.ChaveDeResposta = questao.ChaveDeResposta;
            }

            if (qTemp.QuestaoAnexo.Count > 0)
            {
                for (int i = 0; i < qTemp.QuestaoAnexo.Count; i++)
                {
                    if (questao.QuestaoAnexo.ElementAt(i).CodTipoAnexo == TipoAnexo.TEXTO)
                    {
                        qTemp.QuestaoAnexo.ElementAt(i).Anexo = questao.QuestaoAnexo.ElementAt(i).Anexo;
                    }
                    qTemp.QuestaoAnexo.ElementAt(i).Legenda = questao.QuestaoAnexo.ElementAt(i).Legenda;
                    qTemp.QuestaoAnexo.ElementAt(i).Fonte = questao.QuestaoAnexo.ElementAt(i).Fonte;
                }
            }
            contexto.SaveChanges();
        }

        public static void AtualizarDtUltimoUso(List<Questao> questoes)
        {
            foreach (var questao in questoes)
                questao.DtUltimoUso = DateTime.Now;
        }

        public static List<Questao> ListarPorProfessor(string matricula) => contexto.Questao.Where(q => q.Professor.MatrProfessor.ToLower() == matricula.ToLower()).ToList();

        public static Questao ListarPorCodigo(int codigo) => contexto.Questao.Find(codigo);

        public static List<Questao> ListarPorTema(int codTema) => contexto.QuestaoTema.Where(qt => qt.CodTema == codTema).Select(qt => qt.Questao).ToList();

        public static List<QuestaoTema> ListarPorDisciplina(int codDisciplina, string[] temas, int disciplinaDificuldade, int tipoQuestao, int quantidadeQuestoes)
        {
            List<QuestaoTema> questoesTemas = new List<QuestaoTema>(); //LISTA DE QUESTÕES PARA AVALIAÇÃO
            List<QuestaoTema> questoesTotal = new List<QuestaoTema>(); //LISTA DE TODAS AS QUESTÕES DO BANCO
            List<QuestaoTema> questoesAtual = new List<QuestaoTema>(); //LISTA DE QUESTÕES FILTRADAS DE TODAS [QUESTÕES] PARA O RETORNO - MEIO TERMO

            int temaContador = 0;
            int temaAtual = int.Parse(temas[temaContador]);

            if (quantidadeQuestoes > 0)
            {
                foreach (var tema in temas)
                {
                    int codTema = int.Parse(tema);
                    List<QuestaoTema> temp =
                        (from qt in contexto.QuestaoTema
                         where qt.Questao.CodTipoQuestao == tipoQuestao
                         && qt.Questao.CodDificuldade <= disciplinaDificuldade
                         && qt.CodDisciplina == codDisciplina
                         && qt.CodTema == codTema
                         && !qt.Questao.FlagArquivo
                         select qt).ToList();

                    temp = Models.QuestaoTema.LimparRepeticao(temp, questoesTemas, questoesTotal);

                    if (temp.Count != 0 && questoesTemas.Count < quantidadeQuestoes)
                    {
                        for (int i = disciplinaDificuldade; i >= 1; i--)
                        {
                            List<QuestaoTema> lstIdeal = (from qt in temp where qt.Questao.CodDificuldade == i select qt).ToList();

                            if (lstIdeal.Count != 0)
                            {
                                int random = Sistema.Random.Next(0, lstIdeal.Count);

                                questoesTemas.Add(lstIdeal.ElementAtOrDefault(random));
                                temp.Remove(lstIdeal.ElementAtOrDefault(random));
                                lstIdeal.Clear();
                                break;
                            }
                        }
                        questoesTotal.AddRange(temp);
                    }
                }

                for (int i = disciplinaDificuldade; i >= 1 && questoesTemas.Count < quantidadeQuestoes; i--)
                {
                    questoesAtual =
                        (from qt in questoesTotal
                         where qt.Questao.CodDificuldade == i
                         select qt).ToList();
                    if (questoesAtual.Count != 0)
                    {
                        int random = 0;
                        if (questoesAtual.Count > 1)
                        {
                            random = Sistema.Random.Next(0, questoesAtual.Count);
                            questoesTemas.Add(questoesAtual.ElementAtOrDefault(random));

                            temaContador = (temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(temas[temaContador]);
                        }
                        else
                        {
                            questoesTemas.Add(questoesAtual.FirstOrDefault());
                        }
                        questoesTotal.Remove(questoesAtual.ElementAtOrDefault(random));
                    }
                }

                if (quantidadeQuestoes > questoesTemas.Count)
                {
                    questoesAtual =
                        (from qt in questoesTotal
                         where qt.Questao.CodDificuldade == disciplinaDificuldade
                         select qt).ToList();

                    if (questoesAtual.Count + questoesTemas.Count >= quantidadeQuestoes)
                    {
                        for (int i = questoesTemas.Count; i < quantidadeQuestoes; i++)
                        {
                            int random = 0;
                            if (questoesAtual.Count > 1)
                            {
                                random = Sistema.Random.Next(0, questoesAtual.Count);
                                questoesTemas.Add(questoesAtual.ElementAtOrDefault(random));
                            }
                            else
                            {
                                questoesTemas.Add(questoesAtual.FirstOrDefault());
                            }

                            questoesAtual.RemoveAt(random);
                            questoesTotal.Remove(questoesAtual.ElementAtOrDefault(random));
                            temaContador = (temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(temas[temaContador]);
                        }
                    }
                    else
                    {
                        questoesTemas.AddRange(questoesAtual);
                        foreach (var qt in questoesAtual)
                        {
                            questoesTotal.Remove(qt);
                        }

                        for (int i = disciplinaDificuldade - 1; i >= 1; i--)
                        {
                            questoesAtual =
                                (from qt in questoesTotal
                                 where qt.Questao.CodDificuldade == i
                                 select qt).ToList();

                            if (questoesAtual.Count != 0)
                            {
                                if (questoesAtual.Count + questoesTemas.Count >= quantidadeQuestoes)
                                {
                                    do
                                    {
                                        if (questoesAtual.Count != 0)
                                        {
                                            int random = 0;
                                            if (questoesAtual.Count > 1)
                                            {
                                                random = Sistema.Random.Next(0, questoesAtual.Count);
                                                questoesTemas.Add(questoesAtual.ElementAtOrDefault(random));

                                                temaContador = (temas.Length >= temaContador) ? 0 : temaContador++;
                                                temaAtual = int.Parse(temas[temaContador]);
                                            }
                                            else
                                            {
                                                questoesTemas.Add(questoesAtual.FirstOrDefault());
                                            }
                                            questoesAtual.RemoveAt(random);
                                            questoesTotal.Remove(questoesAtual.ElementAtOrDefault(random));
                                        }
                                    } while (questoesTemas.Count != quantidadeQuestoes);
                                }
                                else
                                {
                                    questoesTemas.AddRange(questoesAtual);
                                    foreach (var qt in questoesAtual)
                                    {
                                        questoesTotal.Remove(qt);
                                    }
                                }
                            }

                            if (questoesTemas.Count == quantidadeQuestoes || i == 1)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return questoesTemas;
        }

        public static List<Questao> Listar() => contexto.Questao.ToList();

        public static List<Questao> ListarPorPalavraChave(string[] palavraChave)
        {
            List<Questao> questoes = Questao.Listar();
            List<Questao> retorno = new List<Questao>();
            List<string> tags = new List<string>();
            string tagsReservadas = "ão das de dos das do da porque que como isso quais porquê quê por abaixo porém mas a e o as os para cujo quais";

            for (int i = 0; i < palavraChave.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(palavraChave[i]) && !tagsReservadas.Contains(palavraChave[i]))
                {
                    tags.Add(palavraChave[i].Trim());
                }
            }
            if (tags.Count != 0)
            {
                int contador = 0;
                foreach (var questao in questoes)
                {
                    string enunciado = questao.Enunciado.ToLower();
                    foreach (var palavra in tags)
                    {
                        if (enunciado.Contains(palavra))
                            contador++;
                    }
                    if (contador == tags.Count)
                        retorno.Insert(0, questao);
                    else if (contador != 0)
                        retorno.Add(questao);
                    contador = 0;
                }
            }
            return retorno;
        }

        public static List<QuestaoTema> ObterNovasQuestoes(List<QuestaoTema> QuestoesOriginais, int codTipoQuestao)
        {
            if (QuestoesOriginais.Count > 0)
            {
                int codDisciplina = QuestoesOriginais.FirstOrDefault().CodDisciplina;
                List<int> temas = (from qt in QuestoesOriginais select qt.CodTema).Distinct().ToList();
                List<int> codQuestoes = (from qt in QuestoesOriginais select qt.CodQuestao).Distinct().ToList();
                int codDificuldade = (from qt in QuestoesOriginais select qt.Questao.CodDificuldade).Max();

                List<QuestaoTema> questoes = new List<QuestaoTema>();

                foreach (var codTema in temas)
                {
                    List<QuestaoTema> qstTemp =
                        (from qt in contexto.QuestaoTema
                         where qt.CodDisciplina == codDisciplina
                         && qt.CodTema == codTema
                         && qt.Questao.CodDificuldade <= codDificuldade
                         && qt.Questao.CodTipoQuestao == codTipoQuestao
                         && !qt.Questao.FlagArquivo
                         select qt).ToList();

                    qstTemp = Models.QuestaoTema.LimparRepeticao(qstTemp, QuestoesOriginais, questoes);

                    if (qstTemp.Count > 0)
                    {
                        questoes.AddRange(qstTemp);
                    }
                }

                if (questoes.Count > 0)
                {
                    return questoes;
                }
            }
            return null;
        }

        public static List<Questao> ListarQuestoesFiltradas(int codDisciplina, int[] Temas, int dificulDisc, int tipo)
        {
            List<QuestaoTema> QuestoesTotal = new List<QuestaoTema>();

            foreach (var tema in Temas)
            {
                List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
                                          where qt.Questao.CodTipoQuestao == tipo
                                          && qt.Questao.CodDificuldade == dificulDisc
                                          && qt.CodDisciplina == codDisciplina
                                          && qt.CodTema == tema
                                          && !qt.Questao.FlagArquivo
                                          select qt).ToList();

                temp = Models.QuestaoTema.LimparRepeticao(temp, QuestoesTotal);

                if (temp.Count != 0)
                {
                    QuestoesTotal.AddRange(temp);
                }
            }
            if (QuestoesTotal.Count > 0)
            {
                List<Questao> questoes = (from qt in QuestoesTotal
                                          select qt.Questao).ToList();
                return questoes;
            }
            return null;
        }

        public static List<Questao> ListarPorCodigos(int[] codQuestoes)
        {
            List<Questao> questoes = new List<Questao>();

            foreach (var codQuestao in codQuestoes)
                questoes.Add(Questao.ListarPorCodigo(codQuestao));

            return questoes;
        }

        public static int? RetornarCodigoAleatorio(int codDisciplina, int codDificuldade = 0, int codTipoQuestao = 0, int[] evite = null)
        {
            IQueryable<Questao> questoes = contexto.Questao.Where(q => q.QuestaoTema.FirstOrDefault(t => t.CodDisciplina == codDisciplina) != null);

            if (codDificuldade > 0)
            {
                questoes = questoes.Where(q => q.CodDificuldade <= codDificuldade);
            }

            if (codTipoQuestao > 0)
            {
                questoes = questoes.Where(q => q.CodTipoQuestao == codTipoQuestao);
            }

            int[] finalQuestoes = questoes.Select(q => q.CodQuestao).ToArray();

            if (evite != null)
            {
                finalQuestoes = finalQuestoes.Except(evite).ToArray();
            }

            if (finalQuestoes.Length > 0)
            {
                int i = Sistema.Random.Next(finalQuestoes.Length);
                return finalQuestoes[i];
            }

            return null;
        }
    }
}