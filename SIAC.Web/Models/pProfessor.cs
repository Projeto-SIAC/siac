using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Professor
    {
        public Instituicao Instituicao => this.TurmaDiscProfHorario.OrderBy(t=>t.AnoLetivo).LastOrDefault()?.Turma.Curso.Diretoria.Campus.Instituicao;

        public Campus Campus => this.TurmaDiscProfHorario.OrderBy(t => t.AnoLetivo).LastOrDefault()?.Turma.Curso.Diretoria.Campus;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static Professor ListarPorMatricula(string matricula) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matricula);

        public static void Inserir(Professor professor)
        {
            contexto.Professor.Add(professor);
            contexto.SaveChanges();
        }

        public static List<Disciplina> ObterDisciplinas(int codProfessor) => contexto.Professor.FirstOrDefault(p=>p.CodProfessor == codProfessor)?.Disciplina.OrderBy(d => d.Descricao).ToList();

        public static List<Disciplina> ObterDisciplinas(string matrProfessor) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matrProfessor)?.Disciplina.OrderBy(d=>d.Descricao).ToList();

        public static List<Professor> ListarOrdenadamente() => contexto.Professor.OrderBy(p => p.Usuario.PessoaFisica.Nome).ToList();
    }
}