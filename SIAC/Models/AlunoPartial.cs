using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Aluno
    {
        [NotMapped]
        public Turma Turma => this.TurmaDiscAluno.OrderBy(t => t.AnoLetivo).Last()?.Turma;

        [NotMapped]
        public List<Disciplina> Disciplinas => this.TurmaDiscAluno.Where(t => t.AnoLetivo == DateTime.Today.Year).Select(t => t.Disciplina).Distinct().ToList();

        [NotMapped]
        public List<Professor> Professores => this.TurmaDiscAluno.Where(t => t.AnoLetivo == DateTime.Today.Year).Join(
                contexto.TurmaDiscProfHorario,
                tda => new { tda.CodDisciplina, tda.AnoLetivo, tda.SemestreLetivo, tda.Turma },
                tdph => new { tdph.CodDisciplina, tdph.AnoLetivo, tdph.SemestreLetivo, tdph.Turma },
                (tda, tdph) =>
                    tdph.Professor
            ).Distinct().ToList();

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Aluno aluno)
        {
            contexto.Aluno.Add(aluno);
            contexto.SaveChanges();
        }

        public static List<Aluno> ListarOrdenadamente() => contexto.Aluno.OrderBy(a => a.Usuario.PessoaFisica.Nome).ToList();

        public static Aluno ListarPorCodigo(int codAluno) => contexto.Aluno.Find(codAluno);

        public static Aluno ListarPorMatricula(string strMatricula) => contexto.Aluno.FirstOrDefault(a => a.MatrAluno == strMatricula);
    }
}