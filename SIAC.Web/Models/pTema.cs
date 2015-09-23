using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Tema
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static Tema ListarPorCodigo(int CodDisciplina, int CodTema)
        {
            return contexto.Tema.SingleOrDefault(t => t.CodDisciplina == CodDisciplina && t.CodTema == CodTema);
        }

        public static List<Tema> ListarPorDisciplina(int CodDisciplina)
        {
            return contexto.Tema.Where(t => t.CodDisciplina == CodDisciplina).OrderBy(t => t.Descricao).ToList();
        }

        public static int Inserir(Tema tema)
        {
            contexto.Tema.Add(tema);
            contexto.SaveChanges();
            return tema.CodTema;
        }

        public static List<Tema> ListarPorDisciplinaTemQuestao(int codDisciplina)
        {
            return contexto.QuestaoTema.Where(qt => qt.CodDisciplina == codDisciplina).OrderBy(qt => qt.Tema.Descricao).Select(qt=>qt.Tema).Distinct().ToList();
        }
    }
}