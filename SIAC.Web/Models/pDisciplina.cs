using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Disciplina
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static List<Disciplina> ListarOrdenadamente()
        {
            return contexto.Disciplina.OrderBy(d => d.Descricao).ToList();
        }

        public static Disciplina ListarPorCodigo(int codDisciplina)
        {
            return contexto.Disciplina.FirstOrDefault(d => d.CodDisciplina == codDisciplina);
        }

        public static int Inserir(Disciplina disciplina)
        {
            contexto.Disciplina.Add(disciplina);
            contexto.SaveChanges();
            return disciplina.CodDisciplina;
        }

        public static List<Disciplina> ListarTemQuestoes()
        {
            return (from qt in contexto.QuestaoTema
                    select qt.Tema.Disciplina).
                    Distinct().
                    OrderBy(d => d.Descricao).
                    ToList();
        }

        public static List<Disciplina> ListarPorProfessor(string matrProfessor)
        {
            return contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matrProfessor).Disciplina.ToList();
        }
    }
}