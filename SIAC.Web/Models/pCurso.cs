using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Curso
    {
        private static dbSIACEntities contexto { get { return DataContextSIAC.GetInstance(); } }

        public static List<Curso> ListarOrdenadamente()
        {
            return contexto.Curso.OrderBy(c => c.Descricao).ToList();
        }

        public static void Inserir(Curso curso)
        {
            curso.CodCurso = ObterCodCurso();
            contexto.Curso.Add(curso);
            contexto.SaveChanges();
        }

        public static int ObterCodCurso()
        {
            int codCurso = contexto.Curso.Max(c => c.CodCurso);
            return codCurso + 1;
        }
    }
}