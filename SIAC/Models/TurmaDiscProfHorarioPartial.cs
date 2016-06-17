using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class TurmaDiscProfHorario
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<TurmaDiscProfHorario> ListarPorUsuario(Usuario usuario)
        {
            int ano = DateTime.Now.Year;
            int semestre = DateTime.Now.SemestreAtual();

            List<TurmaDiscProfHorario> retorno = new List<TurmaDiscProfHorario>();

            switch (usuario.CodCategoria)
            {
                case Categoria.ALUNO:
                    int codAluno = usuario.Aluno.Last().CodAluno;
                    retorno = contexto.TurmaDiscProfHorario
                        .Where(h => h.Turma.TurmaDiscAluno.FirstOrDefault(t => t.CodAluno == codAluno) != null
                            && h.AnoLetivo == ano
                            && h.SemestreLetivo == semestre)
                        .ToList();
                    break;

                case Categoria.PROFESSOR:
                    int codProfessor = usuario.Professor.Last().CodProfessor;
                    retorno = contexto.TurmaDiscProfHorario
                        .Where(h => h.CodProfessor == codProfessor
                            && h.AnoLetivo == ano
                            && h.SemestreLetivo == semestre)
                        .ToList();
                    break;

                default:
                    break;
            }
            return retorno;
        }
    }
}