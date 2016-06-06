using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class SimProva
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<SimProva> ListarPorProfessor(int codProfessor) =>
            contexto.SimProva.Where(sp => sp.CodProfessor == codProfessor)
                .OrderBy(p => p.SimDiaRealizacao.DtRealizacao)
                .ToList();

        public static List<SimProva> ListarPorProfessor(string matricula)
        {
            Professor professor = Professor.ListarPorMatricula(matricula);
            if (professor != null)
                return contexto.SimProva.Where(sp => sp.CodProfessor == professor.CodProfessor)
                    .OrderBy(p => p.SimDiaRealizacao.DtRealizacao)
                    .ToList();
            else
                return new List<SimProva>();
        }
    }
}