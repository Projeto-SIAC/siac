using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Curso
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<Curso> ListarOrdenadamente()
        {
            return contexto.Curso.OrderBy(c => c.Descricao).ToList();
        }
    }
}