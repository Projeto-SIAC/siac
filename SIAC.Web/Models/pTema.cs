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
            return contexto.Tema.Where(t => t.CodDisciplina == CodDisciplina).ToList();
        }
    }
}