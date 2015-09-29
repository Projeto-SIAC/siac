using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Turma
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<Turma> ListarOrdenadamente()
        {
            return contexto.Turma.OrderBy(t => t.Nome).ToList();
        }

        public static void Inserir(Turma turma)
        {
            turma.NumTurma = Turma.ObterNumTurma(turma.CodCurso, turma.CodTurno, turma.Periodo);
            contexto.Turma.Add(turma);
            contexto.SaveChanges();
        }

        private static int ObterNumTurma(int codCurso, string codTurno, int periodo)
        {
            int codNumTurma = 1;

            List<Turma> turmas = contexto.Turma.Where(t => t.CodCurso == codCurso && t.CodTurno == codTurno && t.Periodo == periodo).ToList();
            if (turmas.Count() > 0)
            {
                codNumTurma = turmas.Max(t => t.NumTurma)+1;
            }
            return codNumTurma;
        }
    }
}