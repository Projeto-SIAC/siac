using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Disciplina
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Disciplina> ListarOrdenadamente() => contexto.Disciplina.OrderBy(d => d.Descricao).ToList();

        public static Disciplina ListarPorCodigo(int codDisciplina) => contexto.Disciplina.FirstOrDefault(d => d.CodDisciplina == codDisciplina);

        public static int Inserir(Disciplina disciplina)
        {
            contexto.Disciplina.Add(disciplina);
            contexto.SaveChanges();
            return disciplina.CodDisciplina;
        }

        public static List<Disciplina> ListarTemQuestoes() =>
            (from qt in contexto.QuestaoTema
             select qt.Tema.Disciplina)
            .Distinct()
            .OrderBy(d => d.Descricao)
            .ToList();

        public static List<Disciplina> ListarPorProfessor(string matrProfessor) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matrProfessor).Disciplina.ToList();
    }
}