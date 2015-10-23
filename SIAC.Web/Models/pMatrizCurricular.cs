using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class MatrizCurricular
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<MatrizCurricular> ListarOrdenadamente()
        {
            return contexto.MatrizCurricular.OrderBy(m=>m.Curso.Descricao).ToList();
        }

        public static void Inserir(MatrizCurricular matrizCurricular)
        {
            contexto.MatrizCurricular.Add(matrizCurricular);
            contexto.SaveChanges();
        }

        public static int ObterCodMatriz(int codCurso)
        {
            int codMatriz = 1;
            int qteMatriz = contexto.MatrizCurricular.Where(m => m.CodCurso == codCurso).Count();
            if (qteMatriz != 0)
            {
                codMatriz = qteMatriz + 1;
            }
            return codMatriz;
        }
    }
}