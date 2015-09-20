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

        public static List<Questao> ListarPorDisciplina(int codDisciplina, List<int> Temas, int dificulDisc, int qteObj, int qteDiscu)
        {
            List<Questao> questoes = new List<Questao>();
            
            int temaContador = 0;
            int temaAtual = Temas.ElementAt(temaContador);

            if (qteObj > 0)
            {
                List<Questao> q = (from qt in contexto.QuestaoTema
                                   where qt.CodDisciplina == codDisciplina && qt.CodTema == temaAtual && qt.Questao.CodDificuldade <= dificulDisc && qt.Questao.CodTipoQuestao == 1
                                   select qt.Questao).ToList();

                for (int i = 0; i < qteObj; i++)
                {
                    questoes.Add(q.ElementAtOrDefault(i));
                    temaContador = (Temas.Count >= temaContador) ? 0 : temaContador++;
                    temaAtual = Temas.ElementAt(temaContador);
                }
            }
            temaContador = 0;
            if (qteDiscu > 0)
            {
                List<Questao> q = (from qt in contexto.QuestaoTema
                                   where qt.CodDisciplina == codDisciplina && qt.CodTema == temaAtual && qt.Questao.CodDificuldade <= dificulDisc && qt.Questao.CodTipoQuestao == 2
                                   select qt.Questao).ToList();

                for (int i = 0; i < qteDiscu; i++)
                {
                    questoes.Add(q.ElementAtOrDefault(i));
                    temaContador = (Temas.Count >= temaContador) ? 0 : temaContador++;
                    temaAtual = Temas.ElementAt(temaContador);
                }
            }

            return questoes;
        }
    }
}