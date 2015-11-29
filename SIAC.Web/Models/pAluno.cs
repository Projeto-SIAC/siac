using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Aluno
    {
        public Turma Turma => this.TurmaDiscAluno.OrderBy(t=>t.AnoLetivo).Last().Turma;
        public List<Disciplina> Disciplinas => this.TurmaDiscAluno.Where(t => t.AnoLetivo == DateTime.Today.Year).Select(t => t.Disciplina).Distinct().ToList();
        public List<Professor> Professores => this.TurmaDiscAluno.Where(t => t.AnoLetivo == DateTime.Today.Year).Join(
                contexto.TurmaDiscProfHorario,
                tda => new { tda.CodDisciplina, tda.AnoLetivo, tda.SemestreLetivo, tda.Turma },
                tdph => new { tdph.CodDisciplina, tdph.AnoLetivo, tdph.SemestreLetivo, tdph.Turma },
                (tda, tdph) => 
                    tdph.Professor
            ).Distinct().ToList();

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(Aluno aluno)
        {
            contexto.Aluno.Add(aluno);
            contexto.SaveChanges();
        }

        public static List<Aluno> ListarOrdenadamente()
        {
            return contexto.Aluno.OrderBy(a => a.Usuario.PessoaFisica.Nome).ToList();
        }


        public static Aluno ListarPorCodigo(int codAluno)
        {
            return contexto.Aluno.FirstOrDefault(a => a.CodAluno == codAluno);
        }

        public static Aluno ListarPorMatricula(string strMatricula)
        {
            return contexto.Aluno.FirstOrDefault(a => a.MatrAluno == strMatricula);
        }        
    }
}