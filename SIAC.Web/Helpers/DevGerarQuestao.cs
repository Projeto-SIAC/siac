using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Web.Models;

namespace SIAC.Web.Helpers
{
    public class DevGerarQuestao
    {    
        public static List<Questao> GerarQuestao(int qte)
        {
            List<Questao> lstQuestao = new List<Questao>();
            LeroLero leroLero = new LeroLero();
            Random r = new Random();
            List<Professor> lstProfessor = Professor.ListarOrdenadamente();
            List<TipoQuestao> lstTipoQuestao = TipoQuestao.ListarOrdenadamente();
            List<Dificuldade> lstDificuldade = Dificuldade.ListarOrdenadamente();
            for (int i = 0; i < qte; i++)
            {
                Questao questao = new Questao();
                questao.DtCadastro = DateTime.Now;
                questao.Professor = lstProfessor.ElementAt(r.Next(lstProfessor.Count));
                questao.Dificuldade = lstDificuldade.ElementAt(r.Next(lstDificuldade.Count));
                questao.TipoQuestao = lstTipoQuestao.ElementAt(r.Next(lstTipoQuestao.Count));
                questao.Enunciado = leroLero.Paragrafo();
                if (r.Next(2) == 1)
                {
                    questao.Objetivo = leroLero.Paragrafo();
                }
                List<Disciplina> lstDisciplina = Professor.ObterDisciplinas(questao.Professor.CodProfessor);
                if (lstDisciplina.Count == 0)
                {
                    qte++;
                    continue;
                }
                Disciplina disciplina = lstDisciplina.ElementAt(r.Next(lstDisciplina.Count));
                List<Tema> lstTema = Tema.ListarPorDisciplina(disciplina.CodDisciplina);
                if (lstTema.Count == 0)
                {
                    qte++;
                    continue;
                }
                int qteTema = lstTema.Count > 4 ? r.Next(1, 5) : r.Next(1, lstTema.Count);
                for (int j = 0; j < qteTema; j++)
                {
                    var index = r.Next(lstTema.Count);
                    questao.QuestaoTema.Add(
                        new QuestaoTema
                        {
                            Tema = lstTema.ElementAt(index)                            
                        }    
                    );
                    lstTema.Remove(lstTema.ElementAt(index));
                }
                if (questao.TipoQuestao.CodTipoQuestao == 1)
                {
                    int qteAlternativa = r.Next(3, 6);
                    for (int j = 0; j < qteAlternativa; j++)
                    {
                        questao.Alternativa.Add(
                            new Alternativa
                            {
                                CodOrdem = j,
                                Enunciado = leroLero.Paragrafo(),
                                Comentario = r.Next(2) == 1 ? leroLero.Paragrafo() : null                                
                            }    
                        );
                    }
                    questao.Alternativa.ElementAt(r.Next(questao.Alternativa.Count)).FlagGabarito = true;
                }
                else
                {
                    questao.ChaveDeResposta = leroLero.Paragrafo();
                    if (r.Next(2) == 1)
                    {
                        questao.Comentario = leroLero.Paragrafo();
                    }
                }
                lstQuestao.Add(questao);
            }
            return lstQuestao;
        }
    }
}