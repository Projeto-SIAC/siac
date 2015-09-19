using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Professor
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static Professor ListarPorMatricula(string matricula)
        {
            return contexto.Professor.SingleOrDefault(p => p.MatrProfessor == matricula);
        }

        public static void Inserir(Professor professor)
        {
            contexto.Professor.Add(professor);
            contexto.SaveChanges();
        }

        public static List<Disciplina> ObterDisciplinas(int codProfessor)
        {
            return contexto.Professor.Single(p=>p.CodProfessor == codProfessor).Disciplina.OrderBy(d => d.Descricao).ToList();
        }


        public static List<Disciplina> ObterDisciplinas(string matrProfessor)
        {
            return contexto.Professor.Single(p => p.MatrProfessor == matrProfessor).Disciplina.OrderBy(d=>d.Descricao).ToList();
        }
    }
}