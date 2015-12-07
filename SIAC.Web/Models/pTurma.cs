using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Turma
    {
        public string CodTurma => $"{Periodo}.{CodCurso.ToString("00000")}.{NumTurma}{CodTurno}";

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Turma> ListarOrdenadamente() => contexto.Turma.OrderBy(t => t.Nome).ToList();

        public static void Inserir(Turma turma)
        {
            turma.NumTurma = Turma.ObterNumTurma(turma.CodCurso, turma.CodTurno, turma.Periodo);
            contexto.Turma.Add(turma);
            contexto.SaveChanges();
        }

        public static Turma ListarPorCodigo(string codigo)
        {
            string[] strCodigo = codigo.Split('.');
            int periodo = int.Parse(strCodigo[0]);
            int codCurso = int.Parse(strCodigo[1]);
            string codTurno = strCodigo[2][strCodigo[2].Length - 1].ToString();
            strCodigo[2] = strCodigo[2].Remove(strCodigo[2].Length - 1);
            int numTurma = int.Parse(strCodigo[2]);

            return contexto.Turma
                .SingleOrDefault(t =>
                    t.Periodo == periodo &&
                    t.CodCurso == codCurso &&
                    t.NumTurma == numTurma &&
                    t.CodTurno == codTurno
                );
        }

        private static int ObterNumTurma(int codCurso, string codTurno, int periodo)
        {
            int codNumTurma = 1;

            List<Turma> turmas = contexto.Turma.Where(t => t.CodCurso == codCurso && t.CodTurno == codTurno && t.Periodo == periodo).ToList();
            if (turmas.Count() > 0)
            {
                codNumTurma = turmas.Max(t => t.NumTurma) + 1;
            }
            return codNumTurma;
        }
    }
}