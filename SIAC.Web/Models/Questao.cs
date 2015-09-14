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
    }
}