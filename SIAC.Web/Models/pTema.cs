using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Tema
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static Tema ListarPorCodigo(int CodDisciplina, int CodTema) => contexto.Tema.SingleOrDefault(t => t.CodDisciplina == CodDisciplina && t.CodTema == CodTema);

        public static List<Tema> ListarPorDisciplina(int CodDisciplina) => contexto.Tema.Where(t => t.CodDisciplina == CodDisciplina).OrderBy(t => t.Descricao).ToList();

        public static int Inserir(Tema tema)
        {
            //Realizando um "IDENTITY Manual"
            Disciplina disciplina = tema.Disciplina;
            List<Tema> temas = contexto.Tema.Where(t => t.CodDisciplina == tema.CodDisciplina).ToList();
            int id = temas.Count > 0 ? temas.Max(t => t.CodTema) + 1 : 1;

            tema.CodTema = id;

            contexto.Tema.Add(tema);
            contexto.SaveChanges();
            return tema.CodTema;
        }

        public static List<Tema> ListarPorDisciplinaTemQuestao(int codDisciplina) => contexto.QuestaoTema.Where(qt => qt.CodDisciplina == codDisciplina).Select(qt => qt.Tema).Distinct().OrderBy(t => t.Descricao).ToList();

        public static List<Tema> ListarOrdenadamenteComDisciplina() => contexto.Tema.OrderBy(t => t.Disciplina.Descricao).OrderBy(t => t.Descricao).ToList();
    }
}