using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Questao
    {
        private static DataClassesSIACDataContext contexto = DataContextSIAC.GetInstance();

        public static void Inserir(Questao questao)
        {
            contexto.Questao.InsertOnSubmit(questao);
            contexto.SubmitChanges();
        }

        public static List<Questao> ListarPorProfessor(string matricula)
        {
            int codProfessor = contexto.Professor.SingleOrDefault(p => p.MatrProfessor == matricula).CodProfessor;

            return contexto.Questao.Where(q => q.CodProfessor == codProfessor).ToList();
        }
    }
}