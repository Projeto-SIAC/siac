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
                    qTemp.QuestaoAnexo.ElementAt(i).Legenda = questao.QuestaoAnexo.ElementAt(i).Legenda;
                    qTemp.QuestaoAnexo.ElementAt(i).Fonte = questao.QuestaoAnexo.ElementAt(i).Fonte;
                }
            }
            contexto.SaveChanges();
        }

        public static void AtualizarDtUltimoUso(List<Questao> questoes)
        {
            foreach (Questao questao in questoes)
                questao.DtUltimoUso = DateTime.Now;
        }

        public static List<Questao> ListarPorProfessor(string matricula) => contexto.Questao.Where(q => q.Professor.MatrProfessor.ToLower() == matricula.ToLower()).ToList();

        public static Questao ListarPorCodigo(int codigo) => contexto.Questao.Find(codigo);

        public static List<Questao> ListarPorTema(int codTema) => contexto.QuestaoTema.Where(qt => qt.CodTema == codTema).Select(qt => qt.Questao).ToList();

        public static List<QuestaoTema> ListarPorDisciplina(int codDisciplina, string[] Temas, int dificulDisc, int tipo, int qteQuestoes)
        {
            List<QuestaoTema> QuestoesTemas = new List<QuestaoTema>(); //LISTA DE QUESTÕES PARA AVALIAÇÃO
            List<QuestaoTema> QuestoesTotal = new List<QuestaoTema>(); //LISTA DE TODAS AS QUESTÕES DO BANCO
            List<QuestaoTema> QuestoesAtual = new List<QuestaoTema>(); //LISTA DE QUESTÕES FILTRADAS DE TODAS [QUESTÕES] PARA O RETORNO - MEIO TERMO

            Random r = new Random();
            int temaContador = 0;
            int temaAtual = int.Parse(Temas[temaContador]);

            if (qteQuestoes > 0)
            {
                foreach (string tema in Temas)
                {
                    int codTema = int.Parse(tema);
                    List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
                                              where qt.Questao.CodTipoQuestao == tipo
                                              && qt.Questao.CodDificuldade <= dificulDisc
                                              && qt.CodDisciplina == codDisciplina
                                              && qt.CodTema == codTema
                                              && !qt.Questao.FlagArquivo
                                              select qt).ToList();

                    temp = Models.QuestaoTema.LimparRepeticao(temp, QuestoesTemas, QuestoesTotal);

                    if (temp.Count != 0 && QuestoesTemas.Count < qteQuestoes)
                    {
                        for (int i = dificulDisc; i >= 1; i--)
                        {
                            List<QuestaoTema> lstideal = (from qt in temp where qt.Questao.CodDificuldade == i select qt).ToList();

                            if (lstideal.Count != 0)
                            {
                                int random = r.Next(0, lstideal.Count);

                                QuestoesTemas.Add(lstideal.ElementAtOrDefault(random));
                                temp.Remove(lstideal.ElementAtOrDefault(random));
                                lstideal.Clear();
                                break;
                            }
                        }
                        QuestoesTotal.AddRange(temp);
                    }
                }

                for (int i = dificulDisc; i >= 1 && QuestoesTemas.Count < qteQuestoes; i--)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.Questao.CodDificuldade == i
                                     select qt).ToList();
                    if (QuestoesAtual.Count != 0)
                    {
                        int random = 0;
                        if (QuestoesAtual.Count > 1)
                        {
                            random = r.Next(0, QuestoesAtual.Count);
                            QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));

                            temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(Temas[temaContador]);
                        }
                        else
                        {
                            QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                        }
                        QuestoesTotal.Remove(QuestoesAtual.ElementAtOrDefault(random));
                    }
                }


                if (qteQuestoes > QuestoesTemas.Count)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.Questao.CodDificuldade == dificulDisc
                                     select qt).ToList();

                    if (QuestoesAtual.Count + QuestoesTemas.Count >= qteQuestoes)
                    {
                        for (int i = QuestoesTemas.Count; i < qteQuestoes; i++)
                        {
                            int random = 0;
                            if (QuestoesAtual.Count > 1)
                            {
                                random = r.Next(0, QuestoesAtual.Count);
                                QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));
                            }
                            else
                            {
                                QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                            }

                            QuestoesAtual.RemoveAt(random);
                            QuestoesTotal.Remove(QuestoesAtual.ElementAtOrDefault(random));
                            temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(Temas[temaContador]);
                        }
                    }
                    else
                    {
                        QuestoesTemas.AddRange(QuestoesAtual);
                        foreach (QuestaoTema qt in QuestoesAtual)
                        {
                            QuestoesTotal.Remove(qt);
                        }

                        for (int i = dificulDisc - 1; i >= 1; i--)
                        {
                            QuestoesAtual = (from qt in QuestoesTotal
                                             where qt.Questao.CodDificuldade == i
                                             select qt).ToList();

                            if (QuestoesAtual.Count != 0)
                            {
                                if (QuestoesAtual.Count + QuestoesTemas.Count >= qteQuestoes)
                                {
                                    do
                                    {
                                        if (QuestoesAtual.Count != 0)
                                        {
                                            int random = 0;
                                            if (QuestoesAtual.Count > 1)
                                            {
                                                random = r.Next(0, QuestoesAtual.Count);
                                                QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));

                                                temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                                                temaAtual = int.Parse(Temas[temaContador]);
                                            }
                                            else
                                            {
                                                QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                                            }
                                            QuestoesAtual.RemoveAt(random);
                                            QuestoesTotal.Remove(QuestoesAtual.ElementAtOrDefault(random));

                                        }

                                    } while (QuestoesTemas.Count != qteQuestoes);
                                }
                                else
                                {
                                    QuestoesTemas.AddRange(QuestoesAtual);
                                    foreach (QuestaoTema qt in QuestoesAtual)
                                    {
                                        QuestoesTotal.Remove(qt);
                                    }
                                }
                            }

                            if (QuestoesTemas.Count == qteQuestoes || i == 1)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            /*
            temaContador = 0;
            QuestoesTotal = new List<QuestaoTema>();

            if (qteDiscu > 0)
            {
                int qteDiscuResultado = 0;

                foreach (string tema in Temas)
                {
                    int codTema = int.Parse(tema);
                    List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
                                              where qt.Questao.CodTipoQuestao == 2
                                              && qt.Questao.CodDificuldade <= dificulDisc
                                              && qt.CodDisciplina == codDisciplina
                                              && qt.CodTema == codTema
                                              && !qt.Questao.FlagArquivo
                                              select qt).ToList();

                    temp = Models.QuestaoTema.LimparRepeticao(temp, QuestoesTemas, QuestoesTotal);

                    if (temp.Count != 0 && qteDiscuResultado < qteDiscu)
                    {
                        for (int i = dificulDisc; i >= 1; i--)
                        {
                            List<QuestaoTema> lstideal = (from qt in temp where qt.Questao.CodDificuldade == i select qt).ToList();

                            if (lstideal.Count != 0)
                            {
                                int random = r.Next(0, lstideal.Count);

                                QuestoesTemas.Add(lstideal.ElementAtOrDefault(random));
                                qteDiscuResultado++;
                                temp.Remove(lstideal.ElementAtOrDefault(random));
                                break;
                            }
                        }
                        QuestoesTotal.AddRange(temp);
                    }
                }

                for (int i = dificulDisc; i >= 1 && qteDiscuResultado < qteDiscu; i--)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.Questao.CodDificuldade == i
                                     select qt).ToList();
                    if (QuestoesAtual.Count != 0)
                    {
                        int random = 0;
                        if (QuestoesAtual.Count > 1)
                        {
                            random = r.Next(0, QuestoesAtual.Count);
                            QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));
                            temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(Temas[temaContador]);
                        }
                        else
                        {
                            QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                        }
                        qteDiscuResultado++;
                        QuestoesTotal.Remove(QuestoesAtual.ElementAtOrDefault(random));
                    }
                }


                if (qteDiscu > qteDiscuResultado)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.Questao.CodDificuldade == dificulDisc
                                     select qt).ToList();

                    if (QuestoesAtual.Count + qteDiscuResultado >= qteDiscu)
                    {
                        for (int i = qteDiscuResultado; i < qteDiscu; i++)
                        {
                            int random = 0;
                            if (QuestoesAtual.Count > 1)
                            {
                                random = r.Next(0, QuestoesAtual.Count);
                                QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));
                            }
                            else
                            {
                                QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                            }
                            qteDiscuResultado++;
                            QuestoesAtual.RemoveAt(random);
                            QuestoesTotal.Remove(QuestoesAtual.ElementAtOrDefault(random));
                            temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(Temas[temaContador]);
                        }
                    }
                    else
                    {
                        QuestoesTemas.AddRange(QuestoesAtual);
                        qteDiscuResultado += QuestoesAtual.Count;
                        foreach (QuestaoTema qt in QuestoesAtual)
                        {
                            QuestoesTotal.Remove(qt);
                        }
                        for (int i = dificulDisc - 1; i >= 1; i--)
                        {
                            QuestoesAtual = (from qt in QuestoesTotal
                                             where qt.Questao.CodDificuldade == i
                                             select qt).ToList();

                            if (QuestoesAtual.Count != 0)
                            {
                                if (QuestoesAtual.Count + qteDiscuResultado >= qteDiscu)
                                {
                                    do
                                    {
                                        if (QuestoesAtual.Count != 0)
                                        {
                                            int random = 0;
                                            if (QuestoesAtual.Count > 1)
                                            {
                                                random = r.Next(0, QuestoesAtual.Count);
                                                QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));

                                                temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                                                temaAtual = int.Parse(Temas[temaContador]);
                                            }
                                            else
                                            {
                                                QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                                            }
                                            qteDiscuResultado++;
                                            QuestoesAtual.RemoveAt(random);
                                            QuestoesTotal.Remove(QuestoesAtual.ElementAtOrDefault(random));

                                        }

                                    } while (qteDiscuResultado != qteDiscu);
                                }
                                else
                                {
                                    QuestoesTemas.AddRange(QuestoesAtual);
                                    qteDiscuResultado += QuestoesAtual.Count;
                                    foreach (QuestaoTema qt in QuestoesAtual)
                                    {
                                        QuestoesTotal.Remove(qt);
                                    }
                                }
                            }

                            if (qteDiscuResultado == qteDiscu || i == 1)
                            {
                                break;
                            }
                        }
                    }
                }
            }*/
            return QuestoesTemas;
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
                foreach (Questao questao in questoes)
                {
                    string enunciado = questao.Enunciado.ToLower();
                    foreach (string palavra in tags)
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
                questoes.Add(Questao.ListarPorCodigo(codQuestao));

            return questoes;
        }
    }
}