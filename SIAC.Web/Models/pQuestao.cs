using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Questao
    {
        public Disciplina Disciplina => this.QuestaoTema.FirstOrDefault()?.Tema.Disciplina;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

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

        public static bool PodeAtualizar(Questao questao)
        {
            AvalTemaQuestao temp = contexto.AvalTemaQuestao.FirstOrDefault(atq => atq.CodQuestao == questao.CodQuestao);

            return (temp == null) ? true : false;
        }

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
                    qTemp.QuestaoAnexo.ElementAt(i).Legenda = questao.QuestaoAnexo.ElementAt(i).Legenda;
                    qTemp.QuestaoAnexo.ElementAt(i).Fonte = questao.QuestaoAnexo.ElementAt(i).Fonte;
                }
            }
            contexto.SaveChanges();
        }

        public static void AtualizarDtUltimoUso(List<Questao> questoes)
        {
            foreach (Questao questao in questoes)
            {
                questao.DtUltimoUso = DateTime.Now;
            }
        }

        public static List<Questao> ListarPorProfessor(string matricula) => contexto.Questao.Where(q => q.Professor.MatrProfessor.ToLower() == matricula.ToLower()).ToList();

        public static Questao ListarPorCodigo(int codigo) => contexto.Questao.Find(codigo);

        public static List<Questao> ListarPorTema(int codTema) =>
            (from qt in contexto.QuestaoTema
             where qt.CodTema == codTema
             select qt.Questao).ToList();

        public static List<QuestaoTema> ListarPorDisciplina(int codDisciplina, string[] temas, int disciplinaDificuldade, int tipoQuestao, int quantidadeQuestoes)
        {
            List<QuestaoTema> questoesTemas = new List<QuestaoTema>(); //LISTA DE QUESTÕES PARA AVALIAÇÃO
            List<QuestaoTema> questoesTotal = new List<QuestaoTema>(); //LISTA DE TODAS AS QUESTÕES DO BANCO
            List<QuestaoTema> questoesAtual = new List<QuestaoTema>(); //LISTA DE QUESTÕES FILTRADAS DE TODAS [QUESTÕES] PARA O RETORNO - MEIO TERMO

            Random r = new Random();
            int temaContador = 0;
            int temaAtual = int.Parse(temas[temaContador]);

            if (quantidadeQuestoes > 0)
            {
                foreach (string tema in temas)
                {
                    int codTema = int.Parse(tema);
                    List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
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
                                int random = r.Next(0, lstIdeal.Count);

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
                    questoesAtual = (from qt in questoesTotal
                                     where qt.Questao.CodDificuldade == i
                                     select qt).ToList();
                    if (questoesAtual.Count != 0)
                    {
                        int random = 0;
                        if (questoesAtual.Count > 1)
                        {
                            random = r.Next(0, questoesAtual.Count);
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
                    questoesAtual = (from qt in questoesTotal
                                     where qt.Questao.CodDificuldade == disciplinaDificuldade
                                     select qt).ToList();

                    if (questoesAtual.Count + questoesTemas.Count >= quantidadeQuestoes)
                    {
                        for (int i = questoesTemas.Count; i < quantidadeQuestoes; i++)
                        {
                            int random = 0;
                            if (questoesAtual.Count > 1)
                            {
                                random = r.Next(0, questoesAtual.Count);
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
                        foreach (QuestaoTema qt in questoesAtual)
                        {
                            questoesTotal.Remove(qt);
                        }

                        for (int i = disciplinaDificuldade - 1; i >= 1; i--)
                        {
                            questoesAtual = (from qt in questoesTotal
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
                                                random = r.Next(0, questoesAtual.Count);
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
                                    foreach (QuestaoTema qt in questoesAtual)
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

        //MÉTODO PARA USAR EM AJAX 
        public static List<Questao> ListarPorPalavraChave(string[] palavraChave)
        {
            List<Questao> todas = Questao.Listar();
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
                foreach (Questao questao in todas)
                {
                    foreach (string palavra in tags)
                    {
                        if (questao.Enunciado.ToLower().Contains(palavra)) contador++;
                    }
                    if (contador == tags.Count)
                        retorno.Insert(0, questao);
                    else if (contador == 0) { }
                    else retorno.Add(questao);
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

                Random r = new Random();

                foreach (int codTema in temas)
                {
                    List<QuestaoTema> qstTemp = (from qt in contexto.QuestaoTema
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

            foreach (int tema in Temas)
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

            foreach (int codQuestao in codQuestoes)
            {
                questoes.Add(Questao.ListarPorCodigo(codQuestao));
            }

            return questoes;
        }
    }
}