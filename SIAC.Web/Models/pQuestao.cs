using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Questao
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static void Inserir(Questao questao)
        {
            questao.DtCadastro = DateTime.Now;
            contexto.Questao.Add(questao);
            contexto.SaveChanges();
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
            else { 
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

        public static List<Questao> ListarPorProfessor(string matricula)
        {
            int codProfessor = contexto.Professor.SingleOrDefault(p => p.MatrProfessor == matricula).CodProfessor;

            return contexto.Questao.Where(q => q.CodProfessor == codProfessor).OrderByDescending(q=>q.DtCadastro).ToList();
        }

        public static Questao PesquisarPorCodigo(int codigo)
        {
            return contexto.Questao.SingleOrDefault(q => q.CodQuestao == codigo);
        }

        public static List<Questao> ListarPorTema(int codTema)
        {
            List<Questao> questoes = (from qt in contexto.QuestaoTema
                                      where qt.CodTema == codTema
                                      select qt.Questao).ToList();

            return questoes;
        }

        public static List<QuestaoTema> ListarPorDisciplina(int codDisciplina, string[] Temas, int dificulDisc, int qteObj = 0, int qteDiscu = 0)
        {
            List<QuestaoTema> QuestoesTemas = new List<QuestaoTema>(); //LISTA DE QUESTÕES PARA AVALIAÇÃO
            List<QuestaoTema> QuestoesTotal = new List<QuestaoTema>(); //LISTA DE TODAS AS QUESTÕES DO BANCO
            List<QuestaoTema> QuestoesAtual = new List<QuestaoTema>(); //LISTA DE QUESTÕES FILTRADAS DE TODAS [QUESTÕES] PARA O RETORNO - MEIO TERMO

            int temaContador = 0;
            int temaAtual = int.Parse(Temas[temaContador]);

            if (qteObj > 0)
            {
                foreach (string tema in Temas)
                {
                    int codTema = int.Parse(tema);
                    List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
                                              where qt.Questao.CodTipoQuestao == 1 && qt.Questao.CodDificuldade <= dificulDisc && qt.CodDisciplina == codDisciplina && qt.CodTema == codTema
                                              select qt).ToList();

                    QuestoesTotal.AddRange(temp);
                }

                for (int i = dificulDisc; i >= 1 && QuestoesTemas.Count < qteObj; i--)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.Questao.CodDificuldade == i
                                     select qt).ToList();
                    if (QuestoesAtual.Count != 0)
                    {
                        int random = 0;
                        if (QuestoesAtual.Count > 1)
                        {
                            random = new Random().Next(0, QuestoesAtual.Count);
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


                if (qteObj > QuestoesTemas.Count)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.Questao.CodDificuldade == dificulDisc
                                     select qt).ToList();

                    if (QuestoesAtual.Count + QuestoesTemas.Count >= qteObj)
                    {
                        for (int i = QuestoesTemas.Count; i < qteObj; i++)
                        {
                            int random = 0;
                            if (QuestoesAtual.Count > 1)
                            {
                                random = new Random().Next(0, QuestoesAtual.Count);
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
                        foreach(QuestaoTema qt in QuestoesAtual)
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
                                if (QuestoesAtual.Count + QuestoesTemas.Count >= qteObj)
                                {
                                    do
                                    {
                                        if (QuestoesAtual.Count != 0)
                                        {
                                            int random = 0;
                                            if (QuestoesAtual.Count > 1)
                                            {
                                                random = new Random().Next(0, QuestoesAtual.Count);
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

                                    } while (QuestoesTemas.Count != qteObj);
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

                            if (QuestoesTemas.Count == qteObj || i == 1)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            temaContador = 0;
            QuestoesTotal = new List<QuestaoTema>();

            if (qteDiscu > 0)
            {
                int qteDiscuResultado = 0;

                foreach (string tema in Temas)
                {
                    int codTema = int.Parse(tema);
                    List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
                                              where qt.Questao.CodTipoQuestao == 2 && qt.Questao.CodDificuldade <= dificulDisc && qt.CodDisciplina == codDisciplina && qt.CodTema == codTema
                                              select qt).ToList();

                    QuestoesTotal.AddRange(temp);
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
                            random = new Random().Next(0, QuestoesAtual.Count);
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
                                random = new Random().Next(0, QuestoesAtual.Count);
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
                                                random = new Random().Next(0, QuestoesAtual.Count);
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
            }
            return QuestoesTemas;
        }

        public static List<Questao> ListarPorPalavraChave(string matrProfessor, string[] palavraChave)
        {
            List<Questao> todas = ListarPorProfessor(matrProfessor);
            List<Questao> retorno = new List<Questao>();

            foreach (Questao questao in todas)
            {
                foreach (string palavra in palavraChave)
                {
                    if(questao.Enunciado.ToLower().Contains(palavra))
                    {
                        retorno.Add(questao);
                        break;
                    }
                }
                //todas.Remove(questao);
            }

            return retorno;

        }
    }
}