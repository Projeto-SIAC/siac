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
            qTemp.Comentario = questao.Comentario;
            qTemp.ChaveDeResposta = questao.ChaveDeResposta;

            contexto.SaveChanges();
        }

        public static List<Questao> ListarPorProfessor(string matricula)
        {
            int codProfessor = contexto.Professor.SingleOrDefault(p => p.MatrProfessor == matricula).CodProfessor;

            return contexto.Questao.Where(q => q.CodProfessor == codProfessor).ToList();
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

            foreach (string tema in Temas)
            {
                int codTema = int.Parse(tema);
                List<QuestaoTema> temp = (from qt in contexto.QuestaoTema
                                          where qt.CodDisciplina == codDisciplina && qt.CodTema == codTema
                                          select qt).ToList();

                QuestoesTotal.AddRange(temp);
            }


            if (qteObj > 0)
            {

                for (int i = dificulDisc; i >= 1 && QuestoesTemas.Count < qteObj; i--)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.CodTema == temaAtual && qt.Questao.CodDificuldade == i && qt.Questao.CodTipoQuestao == 1
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
                        QuestoesTotal.RemoveAt(random);
                    }
                }


                if (qteObj > QuestoesTemas.Count)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.CodTema == temaAtual && qt.Questao.CodDificuldade == dificulDisc && qt.Questao.CodTipoQuestao == 1
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
                            temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(Temas[temaContador]);
                        }
                    }
                    else
                    {
                        QuestoesTemas.AddRange(QuestoesAtual);

                        for (int i = dificulDisc - 1; i >= 1; i--)
                        {
                            QuestoesAtual = (from qt in QuestoesTotal
                                             where qt.CodTema == temaAtual && qt.Questao.CodDificuldade == i && qt.Questao.CodTipoQuestao == 1
                                             select qt).ToList();

                            if (QuestoesAtual.Count != 0)
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

                                        QuestoesTotal.RemoveAt(random);
                                        QuestoesAtual.RemoveAt(random);

                                    }

                                } while (QuestoesTemas.Count != qteObj);
                            }

                            if (QuestoesTemas.Count == qteObj)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            temaContador = 0;
            if (qteDiscu > 0)
            {
                for (int i = 1; i < dificulDisc && i <= qteDiscu; i++)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.CodTema == temaAtual && qt.Questao.CodDificuldade == i && qt.Questao.CodTipoQuestao == 2
                                     select qt).ToList();
                    if (QuestoesAtual.Count != 0)
                    {
                        if (QuestoesAtual.Count > 1)
                        {
                            QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(new Random().Next(0, QuestoesAtual.Count)));
                            temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                            temaAtual = int.Parse(Temas[temaContador]);
                        }
                        else QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());
                    }
                }


                if (qteDiscu > QuestoesTemas.Count)
                {
                    QuestoesAtual = (from qt in QuestoesTotal
                                     where qt.CodTema == temaAtual && qt.Questao.CodDificuldade == dificulDisc && qt.Questao.CodTipoQuestao == 1
                                     select qt).ToList();

                    for (int i = 0; i < qteDiscu; i++)
                    {
                        int random = 0;
                        if (QuestoesAtual.Count > 1)
                        {
                            random = new Random().Next(0, QuestoesAtual.Count);
                            QuestoesTemas.Add(QuestoesAtual.ElementAtOrDefault(random));
                        }
                        else QuestoesTemas.Add(QuestoesAtual.FirstOrDefault());

                        QuestoesAtual.RemoveAt(random);
                        temaContador = (Temas.Length >= temaContador) ? 0 : temaContador++;
                        temaAtual = int.Parse(Temas[temaContador]);
                    }
                }
            }
            return QuestoesTemas;
        }
    }
}