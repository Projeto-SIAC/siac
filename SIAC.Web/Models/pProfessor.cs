using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Professor
    {
        public Instituicao Instituicao => this.TurmaDiscProfHorario.LastOrDefault()?.Turma.Curso.Diretoria.Campus.Instituicao;

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

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

        public static List<Professor> ListarOrdenadamente()
        {
            return contexto.Professor.OrderBy(p => p.Usuario.PessoaFisica.Nome).ToList();
        }
    }
}