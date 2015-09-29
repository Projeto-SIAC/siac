using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class MatrizCurricular
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<MatrizCurricular> ListarOrdenadamente()
        {
            return contexto.MatrizCurricular.OrderBy(m=>m.Curso.Descricao).ToList();
        }

        public static void Inserir(MatrizCurricular matrizCurricular)
        {
            contexto.MatrizCurricular.Add(matrizCurricular);
            contexto.SaveChanges();
        }
    }
}